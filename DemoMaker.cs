using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SMKToolbox
{
    public partial class DemoMaker : Form
    {
        

        public static class Header
        {
            public static byte[] array = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }

        public static class Payload
        {
            public static byte[] body;
            public static byte[] full;
            public static int offset = 0;
        }

        public static class Filename
        {
            public static string inputfile = "";
        }


        public DemoMaker()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                openFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    Filename.inputfile = openFileDialog.FileName;
                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                    geneate();
                }
            }




        }

        private void geneate()
        {
            Payload.body = setuparray(Decimal.ToInt32(BufferSize.Value));
            Payload.body = readline(Filename.inputfile, Payload.body, "NTSC", dataGridView1, rtb);

            DataArea.Text = "Data Area: " + String.Format("{0:X2}", Header.array[0]) + String.Format("{0:X2}", Header.array[1]);
            Checksum.Text = "Checksum: " + String.Format("{0:X2}", Header.array[2]) + String.Format("{0:X2}", Header.array[3]);
            CameraMode.Text = "Camera Mode: " + String.Format("{0:X2}", Header.array[4]) + String.Format("{0:X2}", Header.array[5]);
            SaveMode.Text = "Save Mode: " + String.Format("{0:X2}", Header.array[6]) + String.Format("{0:X2}", Header.array[7]);
            RNGSeed.Text = "RNG Seed: " + String.Format("{0:X2}", Header.array[8]) + String.Format("{0:X2}", Header.array[9]);
            FLap.Text = "Fastest Lap: " + String.Format("{0:X2}", Header.array[10]) + String.Format("{0:X2}", Header.array[11]) + String.Format("{0:X2}", Header.array[12]);
            KartType.Text = "Kart Type: " + String.Format("{0:X2}", Header.array[13]);
            Cup.Text = "Cup: " + String.Format("{0:X2}", Header.array[14]);
            Map.Text = "Map: " + String.Format("{0:X2}", Header.array[15]);
            RacerName.Text = "Name: " + String.Format("{0:X2}", Header.array[16]) + String.Format("{0:X2}", Header.array[17]) + String.Format("{0:X2}", Header.array[18]) + String.Format("{0:X2}", Header.array[19]) + String.Format("{0:X2}", Header.array[20]) + String.Format("{0:X2}", Header.array[21]) + String.Format("{0:X2}", Header.array[22]) + String.Format("{0:X2}", Header.array[23]);
            // byte[] checksum = calculateChecksum(Header.array, Payload.body);
            Payload.full = calculateChecksum(Header.array, Payload.body);
            Checksum.Text = "Checksum: " + String.Format("{0:X2}", Payload.full[2]) + String.Format("{0:X2}", Payload.full[3]);

            return;
        }

        static byte[] setuparray(int size)
        {
            byte[] buffer = new byte[size];
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0xFF;
            }
            return (buffer);
        }

        static byte[] readline(string filename, byte[] buffer, string mode, DataGridView dataGridView1, RichTextBox Info)
        {
            int linecount = 0;
            int counter = 0;
            int buffercounter = 0;
            string line;
            int RowCounter = 0;

            //clear down rows from before in case buffer is now smaller.
            dataGridView1.Rows.Clear();

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(@filename);
            while ((line = file.ReadLine()) != null)
            {

                linecount++;
                if (line.StartsWith("#") || line == "")
                {
                    //skip
                }
                else
                {
                    string text = "Line " + linecount.ToString() + ":";
                    //System.Console.Write("Line {0}: ", linecount);
                    Console.Write(text.PadRight(10, ' '));

                    string[] value2 = System.Text.RegularExpressions.Regex.Split(line, @",");
                    int incount = 0;
                    counter++;
                    byte frames = 0;
                    byte input = 0;
                    string NumberOfFrames = "";
                    bool syntaxerror = false;
                    bool headerValue = false;


                    foreach (string value in value2)
                    {


                        if (incount == 0)
                        {
                            int i = 0;
                            if (int.TryParse(value, out i)) //check if input recording data or header data
                            {
                                try
                                {

                                    int number = int.Parse(value);
                                    headerValue = false;

                                    // if > 255 set input byte to 0000 0001 then -256 from the value and set to frames.
                                    frames = 0;


                                    if (number > 511) //Max value = 511
                                    {
                                        number = 511;
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Frame value higher than allowed capping at 511, consider split the command over two or more lines.");
                                        AppendText(Info, "Line " + linecount.ToString() + ": Frame value higher than allowed capping at 511, consider split the command over two or more lines.", Color.Red, true);
                                        Console.ForegroundColor = ConsoleColor.Gray;
                                        // System.Console.Write("Line {0}: ", linecount);
                                        text = "Line " + linecount.ToString() + ":";
                                        Console.Write(text.PadRight(10, ' '));
                                        dataGridView1[0, RowCounter - 1].Value = linecount.ToString();
                                    }
                                    dataGridView1.Rows.Add();
                                    RowCounter++;
                                    NumberOfFrames = number.ToString();

                                    decimal seconds;
                                    if (mode == "PAL") //PAL mode 50Hz
                                    {
                                        seconds = ((decimal)number) / 50;
                                    }
                                    else  //NTSC mode 60Hz
                                    {
                                        seconds = ((decimal)number) / 60;
                                    }
                                    if (number > 255)
                                    {
                                        input = addbit(input, 0b00000001);
                                        number = number - 256;
                                    }

                                    text = NumberOfFrames + " Frames ";
                                    //System.Console.Write("Line {0}: ", linecount);
                                    Console.Write(text.PadLeft(11, ' '));
                                    dataGridView1[1, RowCounter - 1].Value = NumberOfFrames;

                                    Console.Write("/ {0} Seconds\t", seconds.ToString("N4"));
                                    incount++;
                                    dataGridView1[2, RowCounter - 1].Value = (((decimal)number) / 60).ToString("N4");
                                    dataGridView1[3, RowCounter - 1].Value = (((decimal)number) / 50).ToString("N4");

                                    frames = Convert.ToByte(number);
                                }
                                catch (Exception)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    if (syntaxerror == false) // only once per line.
                                    {
                                        Console.WriteLine("Syntax error on line skipping line entry");
                                        AppendText(Info, "Line " + linecount.ToString() + ": Syntax error on line skipping line entry", Color.Red, true);
                                    }
                                    syntaxerror = true;
                                }
                            }
                            else  //header value to be processed
                            {
                                headerValue = true;
                                getHeaderValue(value2, Info);

                                break;
                            }
                        }
                        else
                        {
                            //Payload
                            dataGridView1[0, RowCounter - 1].Value = linecount.ToString();

                            if (value.ToUpper() == "RIGHT" && syntaxerror == false)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(" >\t");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                input = addbit(input, 0b00001000);
                                dataGridView1[4, RowCounter - 1].Value = "RIGHT";

                            }
                            if (value.ToUpper() == "LEFT" && syntaxerror == false)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(" <\t");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                input = addbit(input, 0b00010000);
                                dataGridView1[5, RowCounter - 1].Value = "LEFT";
                            }
                            if (value.ToUpper() == "A" && syntaxerror == false)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write("(A)\t");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                input = addbit(input, 0b00000100);
                                dataGridView1[6, RowCounter - 1].Value = "A";
                            }
                            if (value.ToUpper() == "B" && syntaxerror == false)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Write("(B)\t");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                input = addbit(input, 0b01000000);
                                dataGridView1[7, RowCounter - 1].Value = "B";

                            }
                            if (value.ToUpper() == "X" && syntaxerror == false) //Unused in demo
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.Write("(X)\t");
                                Console.ForegroundColor = ConsoleColor.Gray;
                            }
                            if (value.ToUpper() == "Y" && syntaxerror == false)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write("(Y)\t");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                input = addbit(input, 0b00100000);
                                dataGridView1[8, RowCounter - 1].Value = "Y";
                            }
                            if (value.ToUpper() == "RT" && syntaxerror == false) //RTrigger 10000000 for LTrigger
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("T-R\t");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                input = addbit(input, 0b00000010);
                                dataGridView1[9, RowCounter - 1].Value = "RT";
                            }

                            if (value.ToUpper() == "TRIGGER" && syntaxerror == false) //defaulting to RTrigger 10000000 for LTrigger
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("T-R\t");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                input = addbit(input, 0b00000010);
                                dataGridView1[9, RowCounter - 1].Value = "RT";
                            }

                            if (value.ToUpper() == "LT" && syntaxerror == false) //LTrigger
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("T-L\t");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                input = addbit(input, 0b10000000);
                                dataGridView1[10, RowCounter - 1].Value = "LT";
                            }

                            if (value.ToUpper() == "Start" && syntaxerror == false) // Unused in demo
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write("Start\t");
                                Console.ForegroundColor = ConsoleColor.Gray;
                                dataGridView1[11, RowCounter - 1].Value = "START";

                            }


                        }


                    } //End of reading entries in line

                    if (syntaxerror == false && headerValue == false)
                    {
                        //write Frame and Controller bytes
                        //                        Console.CursorLeft = Console.BufferWidth - 19;
                        Console.Write(" {1} {0}", Convert.ToString(input, 2).PadLeft(8, '0'), Convert.ToString(frames, 2).PadLeft(8, '0'));
                        dataGridView1[12, RowCounter - 1].Value = Convert.ToString(input, 2).PadLeft(8, '0');
                        Console.Write("\n");
                        //Update Buffer
                        if (buffer.Length > buffercounter + 1)
                        {
                            buffer[buffercounter] = frames;
                            buffer[buffercounter + 1] = input;
                            buffercounter = buffercounter + 2;
                        }
                        else
                        {

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("INFO: Buffer size Limit reached, stopping processing file on line {0}", linecount);
                            AppendText(Info, "INFO: Buffer size Limit reached, stopping processing file on line " + linecount, Color.Black, true);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            file.Close();
                            return (buffer);
                        }

                    }
                    else
                    {
                        syntaxerror = true; // resetting for next line.
                    }

                }//End of reading all the file
                Console.ResetColor();


            }



            file.Close();
            System.Console.WriteLine("There were {0} lines in total, with {1} line of instructions.", linecount, counter);
            return (buffer);
        }
        static byte addbit(byte array, int mask)
        {

            int insert = array;

            int Masked_value = (insert | mask);


            return Convert.ToByte(Masked_value);
        }

        static void getHeaderValue(string[] line, RichTextBox Info)
        {
            switch (line[0])
            {
                /*
                 * data_area: 2 bytes
                   checksum: 2 bytes
                   camera_mode: 2 bytes
                   save_code: 2 bytes
                   rng_seed: 2 bytes
                   flap: 3 bytes
                   kart_type: 1 byte
                   cup: 1 byte
                   map: 1 byte
                   name: 8 bytes
                */
                case "data_area":
                    try
                    {
                        Header.array[0] = Convert.ToByte(int.Parse(line[1]));
                        Header.array[1] = Convert.ToByte(int.Parse(line[2]));
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("ERROR: Check header value for data_area.\n\n" + e.Message);
                        AppendText(Info, "ERROR: Check header value for data_area.\n\n" + e.Message, Color.Red, true);
                    }
                    Console.WriteLine("data_area: " + Header.array[0].ToString());

                    break;

                case "checksum":

                    //Header.array[2]; To be calculated at the end
                    //Header.array[3];To be calculated at the end

                    Console.WriteLine("checksum: " + Header.array[2].ToString() + Header.array[3].ToString());


                    break;


                case "camera_mode":
                    try
                    {
                        Header.array[4] = Convert.ToByte(int.Parse(line[1]));
                        Header.array[5] = Convert.ToByte(int.Parse(line[2]));
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("ERROR: Check header value for camera_mode.\n\n" + e.Message);
                        AppendText(Info, "ERROR: Check header value for camera_mode.\n\n" + e.Message, Color.Red, true);
                    }
                    Console.WriteLine("camera_mode: " + Header.array[4].ToString() + Header.array[5].ToString());
                    break;

                case "save_code":

                    try
                    {
                        Header.array[6] = Convert.ToByte(int.Parse(line[1]));
                        Header.array[7] = Convert.ToByte(int.Parse(line[2]));
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("ERROR: Check header value for save_code.\n\n" + e.Message);
                        AppendText(Info, "ERROR: Check header value for save_code.\n\n" + e.Message, Color.Red, true);
                    }
                    Console.WriteLine("save_code: " + Header.array[6].ToString() + Header.array[7].ToString());
                    break;

                case "rng_seed":
                    try
                    {
                        Header.array[8] = Convert.ToByte(int.Parse(line[1]));
                        Header.array[9] = Convert.ToByte(int.Parse(line[2]));
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("ERROR: Check header value for rng_seed.\n\n" + e.Message);
                        AppendText(Info, "ERROR: Check header value for rng_seed.\n\n" + e.Message, Color.Red, true);
                    }
                    Console.WriteLine("rng_seed: " + Header.array[8].ToString() + Header.array[9].ToString());
                    break;

                case "time":

                    try
                    {
                        Header.array[10] = Convert.ToByte(int.Parse(line[1]));
                        Header.array[11] = Convert.ToByte(int.Parse(line[2]));
                        Header.array[12] = Convert.ToByte(int.Parse(line[3]));
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("ERROR: Check header value for time.\n\n" + e.Message);
                        AppendText(Info, "ERROR: Check header value for time.\n\n" + e.Message, Color.Red, true);
                    }
                    Console.WriteLine("time: " + Header.array[10].ToString() + Header.array[11].ToString() + Header.array[12].ToString());
                    break;

                case "kart_type":
                    Console.Write("kart_type");
                    try
                    {
                        Header.array[13] = Convert.ToByte(int.Parse(line[1]));
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("ERROR: Check header value for kart_type.\n\n" + e.Message);
                        AppendText(Info, "ERROR: Check header value for kart_type.\n\n" + e.Message, Color.Red, true);
                    }
                    Console.WriteLine("kart_type: " + Header.array[13].ToString());
                    break;

                case "cup":
                    try
                    {
                        Header.array[14] = Convert.ToByte(int.Parse(line[1]));
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("ERROR: Check header value for cup.\n\n" + e.Message);
                        AppendText(Info, "ERROR: Check header value for cup.\n\n" + e.Message, Color.Red, true);
                    }
                    Console.WriteLine("cup: " + Header.array[14].ToString());
                    break;

                case "map":
                    try
                    {
                        Header.array[15] = Convert.ToByte(int.Parse(line[1]));
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("ERROR: Check header value for map.\n\n" + e.Message);
                        AppendText(Info, "ERROR: Check header value for map.\n\n" + e.Message, Color.Red, true);
                    }
                    Console.WriteLine("map: " + Header.array[15].ToString());
                    break;

                case "name":
                    try
                    {
                        Header.array[16] = Convert.ToByte(int.Parse(line[1]));
                        Header.array[17] = Convert.ToByte(int.Parse(line[2]));
                        Header.array[18] = Convert.ToByte(int.Parse(line[3]));
                        Header.array[19] = Convert.ToByte(int.Parse(line[4]));
                        Header.array[20] = Convert.ToByte(int.Parse(line[5]));
                        Header.array[21] = Convert.ToByte(int.Parse(line[6]));
                        Header.array[22] = Convert.ToByte(int.Parse(line[7]));
                        Header.array[23] = Convert.ToByte(int.Parse(line[8]));
                    }
                    catch (FormatException e)
                    {
                        Console.Write("ERROR: Check header value for name.\n\n" + e.Message);
                        AppendText(Info, "ERROR: Check header value for name.\n\n." + e.Message, Color.Red, true);
                    }
                    Console.WriteLine("name: " + Header.array[16].ToString() + Header.array[17].ToString() + Header.array[18].ToString() + Header.array[19].ToString() + Header.array[20].ToString() + Header.array[21].ToString() + Header.array[22].ToString() + Header.array[23].ToString());
                    break;

                default:
                    Console.WriteLine("Invalid header data");
                    break;
            }



        }


        static byte[] calculateChecksum(byte[] header, byte[] input)
        {
            byte[] newarray = new byte[header.Length + input.Length];
            System.Buffer.BlockCopy(header, 0, newarray, 0, header.Length);
            System.Buffer.BlockCopy(input, 0, newarray, header.Length, input.Length);

            //newarray[2]; To be calculated at the end
            //Header.array[3];To be calculated at the end

            int checksum = 0;
            for (int i = 4; i < newarray.Length; i = i + 2)
            {
                checksum = (checksum + newarray[i] + newarray[i + 1] * 256) & 0xFFFF;
            }
            /*  Peudo code for Checksum
            int calculate_checksum(data_buffer)
            {
            checksum = 0
            for (i = 0; i < data_buffer.length; i += 2)
            {
            checksum = (checksum + data_buffer[i] + data_buffer[i+1]*256) & 0xFFFF;

            }
            return checksum;*/
            int a = checksum;
            Console.WriteLine("Checksum calulated: " + checksum.ToString("X4"));
            string whole = checksum.ToString("X4");
            string part1 = whole.Substring(0, 2);
            string part2 = whole.Substring(2, 2);
            Console.WriteLine("Checksum converted to little-endian: " + part2 + part1);
            newarray[2] = Convert.ToByte(int.Parse(part2, System.Globalization.NumberStyles.HexNumber));
            newarray[3] = Convert.ToByte(int.Parse(part1, System.Globalization.NumberStyles.HexNumber));
            return newarray;
        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Demo Maker Version 1.0\n\nBy Dirtbag & MrL314\nUsed for the creation of demo recordings for SMK.\n\nUse with LakiView Demo Recorder", "Demo Maker Version 1.0", MessageBoxButtons.OK);
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            BufferSize.Enabled = false;
            BufferSize.Value = 162;
            Offset.Value = 524101;
            Offset.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            BufferSize.Enabled = false;
            BufferSize.Value = 156;
            Offset.Value = 8012;
            Offset.Enabled = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            BufferSize.Enabled = false;
            BufferSize.Value = 144;
            Offset.Value = 138387;
            Offset.Enabled = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            BufferSize.Enabled = false;
            BufferSize.Value = 156;
            Offset.Value = 1302968;
            Offset.Enabled = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            BufferSize.Enabled = false;
            BufferSize.Value = 208;
            Offset.Value = 302736;
            Offset.Enabled = false;
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            BufferSize.Enabled = false;
            BufferSize.Value = 684;
            Offset.Value = 138555;
            Offset.Enabled = false;
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            BufferSize.Enabled = true;
            Offset.Enabled = true;
        }

        private void BufferSize_ValueChanged(object sender, EventArgs e)
        {
            if (BufferSize.Value % 2 != 0)
            {
                BufferSize.Value++;
            }
            if (Filename.inputfile != "")
            {
                geneate();
            }

        }

        private void HexMode_CheckedChanged(object sender, EventArgs e)
        {
            if (HexMode.Checked == true)
                BufferSize.Hexadecimal = true;
            else
                BufferSize.Hexadecimal = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].Visible = true;
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.Columns[2].Visible = true;
            dataGridView1.Columns[3].Visible = false;
        }

        public static void AppendText(RichTextBox rtb, string text, Color color, bool isNewLine)
        {
            rtb.SuspendLayout();
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;

            rtb.SelectionColor = color;
            rtb.AppendText(isNewLine ? $"{text}{ Environment.NewLine}" : text);
            rtb.SelectionColor = rtb.ForeColor;
            rtb.ScrollToCaret();
            rtb.ResumeLayout();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            if (Payload.full != null)
            {

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                    saveFileDialog.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
                    saveFileDialog.FilterIndex = 1;
                    saveFileDialog.RestoreDirectory = true;

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            BinaryWriter Writer = null;
                            // Create a new stream to write to the file
                            // Writer = new BinaryWriter(File.OpenWrite(FileName));
                            Writer = new BinaryWriter(File.Create(saveFileDialog.FileName));

                            // Writer raw data    
                            Writer.Write(Payload.full);
                            Writer.Flush();
                            Writer.Close();
                        }
                        catch
                        {
                            Console.WriteLine("Error saving");
                            AppendText(rtb, "Error saving demo to ROM", Color.Red, true);
                        }
                    }
                }
            }
            else
            {
                //no data to save 
                AppendText(rtb, "No data to save, load CSV file first", Color.Black, true);
            }
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            if (Payload.full != null)
            {

                var fileContent = string.Empty;
                var filePath = string.Empty;

                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
                    openFileDialog.Filter = "ROM file (*.sfc)|*.sfc|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {

                        try
                        {
                            byte[] ROM = File.ReadAllBytes(openFileDialog.FileName);

                            BinaryWriter Writer = null;
                            Payload.full.CopyTo(ROM, Payload.offset);
                            Writer = new BinaryWriter(File.Create(openFileDialog.FileName));

                            // Writer raw data    
                            Writer.Write(ROM);
                            Writer.Flush();
                            Writer.Close();
                        }
                        catch
                        {
                            Console.WriteLine("Error saving");
                            AppendText(rtb, "Error saving demo to ROM", Color.Red, true);
                        }

                    }
                }

            }
            else
            {
                //no data to save nothing to do
                AppendText(rtb, "No data to save, load CSV file first", Color.Black, true);
            }

        }

        private void HexMode2_CheckedChanged(object sender, EventArgs e)
        {
            if (HexMode2.Checked == true)
                Offset.Hexadecimal = true;
            else
                Offset.Hexadecimal = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                File.WriteAllText(".\\demomaker.lua",
    "function read_byte(addr)\n" +
    "	if (addr >= 0x7e0000) and (addr <= 0x7fffff) then \n" +
    "		local return_domain = memory.getcurrentmemorydomain()\n" +
    "		memory.usememorydomain(\"WRAM\")\n" +
    "		local b = memory.readbyte(addr - 0x7e0000)\n" +
    "		memory.usememorydomain(return_domain)\n" +
    "		return b\n" +
    "	elseif (addr <= 0x1fff) then\n" +
    "		local return_domain = memory.getcurrentmemorydomain()\n" +
    "		memory.usememorydomain(\"WRAM\")\n" +
    "		local b = memory.readbyte(addr)\n" +
    "		memory.usememorydomain(return_domain)\n" +
    "		return b\n" +
    "	else\n" +
    "		return memory.readbyte(addr)\n" +
    "	end\n" +
    "end\n" +
    "\n" +
    "\n" +
    "function read_word(addr)\n" +
    "	return read_byte(addr+1)*256 + read_byte(addr)\n" +
    "end\n" +
    "\n" +
    "function read_long(addr)\n" +
    "	return read_byte(addr+2)*256*256 + read_byte(addr+1)*256 + read_byte(addr)\n" +
    "end\n" +
    "\n" +
    "\n" +
    "function inRace()\n" +
    "	local gm = read_byte(0x7e0036) -- gamemode\n" +
    "	if (gm == 0x2) or (gm == 0xe) then\n" +
    "		return true\n" +
    "	end\n" +
    "	return false\n" +
    "end\n" +
    "\n" +
    "function raceStatus()\n" +
    "\n" +
    "	local rs = read_byte(0x7e003a) -- race status\n" +
    "\n" +
    "	--console.write(tostring(gm) .. \" \" .. tostring(rs) .. \"\\n\")\n" +
    "\n" +
    "	if inRace() then\n" +
    "		if (rs == 0x4) or (rs == 0x6) then\n" +
    "			return true\n" +
    "		end\n" +
    "	end\n" +
    "	return false\n" +
    "end\n" +
    "\n" +
    "function check_kart_active(v)\n" +
    "	if v == 0 then return false end\n" +
    "\n" +
    "	--if bit.band(v, 0x8000) == 0 then return false end\n" +
    "\n" +
    "	----if bit.band(v, 0x4000) ~= 0 then return false end\n" +
    "\n" +
    "	if bit.band(v, 0x0020) ~= 0 then return false end\n" +
    "\n" +
    "	return true\n" +
    "\n" +
    "end\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "function get_joypad(jpnum)\n" +
    "	local offs = (jpnum-1)*2\n" +
    "\n" +
    "	local ctrl_l_a = read_byte(0x20 + offs + 0) \n" +
    "	local ctrl_h_a = read_byte(0x20 + offs + 1)\n" +
    "\n" +
    "	local ctrl_l_b = read_byte(0x28 + offs + 0) \n" +
    "	local ctrl_h_b = read_byte(0x28 + offs + 1)\n" +
    "\n" +
        "\n" +
    "	local hld = {}\n" +
    "\n" +
    "	hld.A      = bit.check(ctrl_l_a, 7)\n" +
    "	hld.X      = bit.check(ctrl_l_a, 6)\n" +
    "	hld.L      = bit.check(ctrl_l_a, 5)\n" +
    "	hld.R      = bit.check(ctrl_l_a, 4)\n" +
    "\n" +
    "	hld.B      = bit.check(ctrl_h_a, 7)\n" +
    "	hld.Y      = bit.check(ctrl_h_a, 6)\n" +
    "	hld.Select = bit.check(ctrl_h_a, 5)\n" +
    "	hld.Start  = bit.check(ctrl_h_a, 4)\n" +
    "	hld.Up     = bit.check(ctrl_h_a, 3)\n" +
    "	hld.Down   = bit.check(ctrl_h_a, 2)\n" +
    "	hld.Left   = bit.check(ctrl_h_a, 1)\n" +
    "	hld.Right  = bit.check(ctrl_h_a, 0)\n" +
    "\n" +
    "	local tap = {}\n" +
    "\n" +
    "	tap.A      = bit.check(ctrl_l_b, 7)\n" +
    "	tap.X      = bit.check(ctrl_l_b, 6)\n" +
    "	tap.L      = bit.check(ctrl_l_b, 5)\n" +
    "	tap.R      = bit.check(ctrl_l_b, 4)\n" +
    "\n" +
    "	tap.B      = bit.check(ctrl_h_b, 7)\n" +
    "	tap.Y      = bit.check(ctrl_h_b, 6)\n" +
    "	tap.Select = bit.check(ctrl_h_b, 5)\n" +
    "	tap.Start  = bit.check(ctrl_h_b, 4)\n" +
    "	tap.Up     = bit.check(ctrl_h_b, 3)\n" +
    "	tap.Down   = bit.check(ctrl_h_b, 2)\n" +
    "	tap.Left   = bit.check(ctrl_h_b, 1)\n" +
    "	tap.Right  = bit.check(ctrl_h_b, 0)\n" +
    "\n" +
    "	local jp = {}\n" +
    "\n" +
    "	jp.hld = hld\n" +
    "	jp.tap = tap\n" +
    "\n" +
    "	return jp\n" +
    "\n" +
    "end\n" +
    "\n" +
    "\n" +
    "\n" +
    "B_c      = \"B\"\n" +
    "Y_c      = \"Y\"\n" +
    "START_c  = \"+\"\n" +
    "SELECT_c = \"-\"\n" +
    "\n" +
    "UP_c    = \"^\"\n" +
    "DOWN_c  = \"v\"\n" +
    "LEFT_c  = \"<\"\n" +
    "RIGHT_c = \">\"\n" +
    "\n" +
    "A_c     = \"A\"\n" +
    "X_c     = \"X\"\n" +
    "L_c     = \"L\"\n" +
    "R_c     = \"R\"\n" +
    "\n" +
    "\n" +
    "\n" +
    "tw = 10\n" +
    "\n" +
    "\n" +
    "\n" +
    "B_d      = 0\n" +
    "Y_d      = B_d      + tw*(string.len(B_c)+1)\n" +
    "SELECT_d = Y_d      + tw*(string.len(Y_c)+1)\n" +
    "START_d  = SELECT_d + tw*(string.len(SELECT_c)+1)\n" +
    "UP_d     = START_d  + tw*(string.len(START_c)+1)\n" +
    "DOWN_d   = UP_d     + tw*(string.len(UP_c)+1)\n" +
    "LEFT_d   = DOWN_d   + tw*(string.len(DOWN_c)+1)\n" +
    "RIGHT_d  = LEFT_d   + tw*(string.len(LEFT_c)+1)\n" +
    "A_d      = RIGHT_d  + tw*(string.len(RIGHT_c)+1)\n" +
    "X_d      = A_d      + tw*(string.len(A_c)+1)\n" +
    "L_d      = X_d      + tw*(string.len(X_c)+1)\n" +
    "R_d      = L_d      + tw*(string.len(L_c)+1)\n" +
    "\n" +
    "\n" +
    "\n" +
    "function inp_text(display_text, display_offset, csv_text)\n" +
    "	local t = {}\n" +
    "	t.d_txt = display_text\n" +
    "	t.d_off = display_offset\n" +
    "	t.c_txt = csv_text\n" +
    "	return t\n" +
    "end\n" +
    "\n" +
    "\n" +
    "local input_texts\n" +
    "function processInput(input_check, input_c, x, y)\n" +
    "\n" +
    "	input_texts = input_texts or {\n" +
    "		[B_c]      = function() return inp_text(B_c,      B_d,      \"B\") end,\n" +
    "		[Y_c]      = function() return inp_text(Y_c,      Y_d,      \"Y\") end,\n" +
    "		[SELECT_c] = function() return inp_text(SELECT_c, SELECT_d, \"SELECT\") end,\n" +
    "		[START_c]  = function() return inp_text(START_c,  START_d,  \"START\") end,\n" +
    "		[UP_c]     = function() return inp_text(UP_c,     UP_d,     \"UP\") end,\n" +
    "		[DOWN_c]   = function() return inp_text(DOWN_c,   DOWN_d,   \"DOWN\") end,\n" +
    "		[LEFT_c]   = function() return inp_text(LEFT_c,   LEFT_d,   \"LEFT\") end,\n" +
    "		[RIGHT_c]  = function() return inp_text(RIGHT_c,  RIGHT_d,  \"RIGHT\") end,\n" +
    "		[A_c]      = function() return inp_text(A_c,      A_d,      \"A\") end,\n" +
    "		[X_c]      = function() return inp_text(X_c,      X_d,      \"X\") end,\n" +
    "		[L_c]      = function() return inp_text(L_c,      L_d,      \"LT\") end,\n" +
    "		[R_c]      = function() return inp_text(R_c,      R_d,      \"RT\") end\n" +
    "	}\n" +
    "\n" +
    "	if input_check then\n" +
    "		local t = input_texts[input_c]()\n" +
    "		gui.text(x + t.d_off, y, t.d_txt)\n" +
    "		return t.c_txt .. \",\"\n" +
    "	end\n" +
    "\n" +
    "	return \"\"\n" +
    "end\n" +
    "\n" +
    "\n" +
    "local process_input\n" +
    "function captureInput(JOYPAD, X_offset, Y_offset)\n" +
        "\n" +
    "	currentFrame=\",\"\n" +
    "\n" +
    "	--process_input[B_c](JOYPAD)\n" +
    "\n" +
    "	process_input = process_input or {\n" +
    "		[B_c]      = function(JYPD, x, y) return processInput(JYPD.hld.B,      B_c,      x, y) end,\n" +
    "		[Y_c]      = function(JYPD, x, y) return processInput(JYPD.hld.Y,      Y_c,      x, y) end,\n" +
    "		[SELECT_c] = function(JYPD, x, y) return processInput(JYPD.tap.Select, SELECT_c, x, y) end,\n" +
    "		[START_c]  = function(JYPD, x, y) return processInput(JYPD.tap.Start,  START_c,  x, y) end,\n" +
    "		[UP_c]     = function(JYPD, x, y) return processInput(JYPD.hld.Up,     UP_c,     x, y) end,\n" +
    "		[DOWN_c]   = function(JYPD, x, y) return processInput(JYPD.hld.Down,   DOWN_c,   x, y) end,\n" +
    "		[LEFT_c]   = function(JYPD, x, y) return processInput(JYPD.hld.Left,   LEFT_c,   x, y) end,\n" +
    "		[RIGHT_c]  = function(JYPD, x, y) return processInput(JYPD.hld.Right,  RIGHT_c,  x, y) end,\n" +
    "		[A_c]      = function(JYPD, x, y) return processInput(JYPD.tap.A,      A_c,      x, y) end,\n" +
    "		[X_c]      = function(JYPD, x, y) return processInput(JYPD.hld.X,      X_c,      x, y) end,\n" +
    "		[L_c]      = function(JYPD, x, y) return processInput(JYPD.hld.L,      L_c,      x, y) end,\n" +
    "		[R_c]      = function(JYPD, x, y) return processInput(JYPD.hld.R,      R_c,      x, y) end\n" +
    "	}\n" +
    "\n" +
    "\n" +
    "	-- MrL's Note: Not all of the inputs are tested for changes.\n" +
    "	currentFrame = currentFrame .. process_input[B_c](JOYPAD, X_offset, Y_offset)\n" +
    "	currentFrame = currentFrame .. process_input[Y_c](JOYPAD, X_offset, Y_offset)\n" +
    "	--currentFrame = currentFrame .. process_input[SELECT_c](JOYPAD, X_offset, Y_offset)\n" +
    "	--currentFrame = currentFrame .. process_input[START_c](JOYPAD, X_offset, Y_offset)\n" +
    "	--currentFrame = currentFrame .. process_input[UP_c](JOYPAD, X_offset, Y_offset)\n" +
    "	--currentFrame = currentFrame .. process_input[DOWN_c](JOYPAD, X_offset, Y_offset)\n" +
    "	currentFrame = currentFrame .. process_input[LEFT_c](JOYPAD, X_offset, Y_offset)\n" +
    "	currentFrame = currentFrame .. process_input[RIGHT_c](JOYPAD, X_offset, Y_offset)\n" +
    "	currentFrame = currentFrame .. process_input[A_c](JOYPAD, X_offset, Y_offset)\n" +
    "	--currentFrame = currentFrame .. process_input[X_c](JOYPAD, X_offset, Y_offset)\n" +
    "	currentFrame = currentFrame .. process_input[L_c](JOYPAD, X_offset, Y_offset)\n" +
    "	currentFrame = currentFrame .. process_input[R_c](JOYPAD, X_offset, Y_offset)\n" +
    "\n" +
    "\n" +
    "	return currentFrame\n" +
    "\n" +
    "end\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "function newDemoData(output_file, player_number)\n" +
    "	local d = {}\n" +
    "\n" +
    "	d.inputBuffer = \"\"\n" +
    "	d.currInput = \",\"\n" +
    "	d.previousFrame = \",\"\n" +
    "	d.buffer_size = 0\n" +
    "	d.fcnt = 0\n" +
    "	d.first = true\n" +
    "	d.active = false\n" +
    "	d.ready = false\n" +
    "\n" +
    "	d.b_first = false\n" +
    "\n" +
    "	if output_file == nil then\n" +
    "		d.file = nil\n" +
    "	else\n" +
    "		d.file = io.open(output_file, \"w+\")\n" +
    "	end\n" +
    "\n" +
    "\n" +
    "\n" +
    "	if d.file ~= nil then\n" +
    "		d.file:write(\"#Demo Recording Capture Tool 0.1 By Dirtbag and MrL314\\n\")\n" +
    "		d.file:write(\"#-----------------------------------------------------\\n\")\n" +
    "		d.active = true\n" +
    "	end\n" +
    "\n" +
    "\n" +
    "\n" +
    "	return d\n" +
    "end\n" +
    "\n" +
    "\n" +
    "function newHeader()\n" +
    "	local h = {}\n" +
    "\n" +
    "	h.data_area = 0 -- not used in save data\n" +
    "	h.checksum = 0\n" +
    "	h.camera_mode = 0\n" +
    "	h.save_code = 0\n" +
    "	h.rng_seed = 0\n" +
    "	h.time = 0\n" +
    "	h.kart_type = 0\n" +
    "	h.cup = 0\n" +
    "	h.map = 0\n" +
    "	h.name = 0  -- not used in save data\n" +
    "\n" +
    "	return h\n" +
    "end\n" +
    "\n" +
    "function to_n_bytes(val, n)\n" +
    "\n" +
    "	local s = \"\"\n" +
    "	local v = val\n" +
    "	local b = 0\n" +
    "\n" +
    "	for i=1,n do\n" +
    "		b = bit.band(v, 0xFF)\n" +
    "		v = bit.rshift(v, 8)\n" +
    "\n" +
    "		s = s .. tostring(b)\n" +
    "\n" +
    "		if i < n then\n" +
    "			s = s .. \",\"\n" +
    "		end\n" +
    "	end\n" +
    "\n" +
    "	return s\n" +
    "end\n" +
    "\n" +
    "\n" +
    "function headerToString(h)\n" +
    "	local s = \"\"\n" +
    "\n" +
    "	s = s .. \"data_area,\" .. to_n_bytes(h.data_area, 2) .. \"\\n\"\n" +
    "	s = s .. \"checksum,\" .. to_n_bytes(h.checksum, 2) .. \"\\n\"\n" +
    "	s = s .. \"camera_mode,\" .. to_n_bytes(h.camera_mode, 2) .. \"\\n\"\n" +
    "	s = s .. \"save_code,\" .. to_n_bytes(h.save_code, 2) .. \"\\n\"\n" +
    "	s = s .. \"rng_seed,\" .. to_n_bytes(h.rng_seed, 2) .. \"\\n\"\n" +
    "	s = s .. \"time,\" .. tostring(h.time[1]) .. \",\" .. tostring(h.time[2]) .. \",\" .. tostring(h.time[3]) .. \"\\n\"\n" +
    "	s = s .. \"kart_type,\" .. to_n_bytes(h.kart_type, 1) .. \"\\n\"\n" +
    "	s = s .. \"cup,\" .. to_n_bytes(h.cup, 1) .. \"\\n\"\n" +
    "	s = s .. \"map,\" .. to_n_bytes(h.map, 1) .. \"\\n\"\n" +
        "\n" +
    "	-- handle name specially\n" +
    "	s = s .. \"name,\"\n" +
    "	-- for now, just write arbitrary value\n" +
    "	s = s .. \"0,0,0,0,0,0,0,0\"\n" +
    "	s = s .. \"\\n\"\n" +
    "\n" +
    "	return s\n" +
    "\n" +
    "end\n" +
    "\n" +
    "\n" +
    "DEMO = {}\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "function get_core()\n" +
    "	local return_domain = memory.getcurrentmemorydomain()\n" +
    "\n" +
    "	local s = \"\"\n" +
        "\n" +
    "	if memory.usememorydomain(\"OAM\") then\n" +
    "		s = \"BSNES\"\n" +
    "	else\n" +
    "		s = \"Snes9x\"\n" +
    "	end\n" +
    "\n" +
    "	memory.usememorydomain(return_domain)\n" +
    "	return s\n" +
    "end\n" +
    "\n" +
    "\n" +
    "CURR_CORE = get_core()\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "console.clear()\n" +
    "\n" +
    "console.log('Demo Recoding Capture Tool 0.1 By Dirtbag and MrL314')\n" +
    "console.log('Lua advance kart interactive Vehicle interface emulating wheels')\n" +
    "console.log('Using core: ' .. CURR_CORE)\n" +
    "\n" +
    "--demomaker CSV order: RIGHT, LEFT, A, B, X, Y, RIGHTTRIGGER, LEFTTRIGGER, START\n" +
    "--Start of race 0x3f0000 is 240 (on TT and Vs selection screen also 240 but goes to 32 then 240 again when race starts)\n" +
    "\n" +
    "local P1 = newDemoData(nil, 1)\n" +
    "local P2 = newDemoData(nil, 2)\n" +
    "\n" +
    "READY = false\n" +
    "QUIT = false\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "function ReadyButtonClick()\n" +
    "	if READY == false then\n" +
    "		READY = true\n" +
    "	end\n" +
    "end\n" +
    "\n" +
    "function FILE1_SET()\n" +
    "	local floc = forms.openfile()\n" +
    "	if floc ~= \"\" then\n" +
    "		forms.settext(file1_tbox, floc)\n" +
    "	end\n" +
    "end\n" +
    "\n" +
    "function FILE2_SET()\n" +
    "	local floc = forms.openfile()\n" +
    "	if floc ~= \"\" then\n" +
    "		forms.settext(file2_tbox, floc)\n" +
    "	end\n" +
    "end\n" +
    "\n" +
    "\n" +
    "\n" +
    "FORM = forms.newform(250,180,\"LakiView 0.1\")\n" +
    "\n" +
    "\n" +
    "file1_label = forms.label(FORM, \"P1 file:\", 0, 2, 40, 18)\n" +
    "file1_tbox  = forms.textbox(FORM, \"demo_p1.csv\", 140, 20, nil, 40, 0)\n" +
    "file1_setb  = forms.button(FORM, \"Select\", FILE1_SET, 182, 0, 50, 20)\n" +
    "\n" +
    "file2_label = forms.label(FORM, \"P2 file:\", 0, 24, 40, 18)\n" +
    "file2_tbox  = forms.textbox(FORM, \"demo_p2.csv\", 140, 20, nil, 40, 20)\n" +
    "file2_setb  = forms.button(FORM, \"Select\", FILE2_SET, 182, 20, 50, 20)\n" +
    "\n" +
    "\n" +
    "ready_button = forms.button(FORM, \"RECORD\", ReadyButtonClick, 10, 90, 150, 40)\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "function UPDATE()\n" +
    "\n" +
    "	if READY == false then\n" +
    "		forms.settext(ready_button, \"RECORD\")\n" +
    "	else\n" +
    "		if (P1.active or P2.active) then\n" +
    "			forms.settext(ready_button, \"-- RECORDING --\")\n" +
    "		else\n" +
    "			forms.settext(ready_button, \"WAITING TO BEGIN\")\n" +
    "		end\n" +
    "	end\n" +
    "\n" +
    "	emu.frameadvance()\n" +
    "end\n" +
    "\n" +
    "\n" +
    "\n" +
    "header = newHeader()\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "while true do\n" +
    "\n" +
    "	-- wait for signal \"ready to record\"\n" +
    "	while not READY do UPDATE() end\n" +
    "\n" +
    "	-- wait to finish current race (if in the middle of one)\n" +
    "	while raceStatus() do UPDATE() end\n" +
    "\n" +
    "	-- wait for race to start\n" +
    "	while (not raceStatus()) do UPDATE() end\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "	-- Set which screens are active in order to record\n" +
    "	local dm = read_byte(0x7e002e) -- display mode, select top or bottom active\n" +
    "\n" +
    "	P1.ready = true\n" +
    "	P2.ready = true\n" +
    "\n" +
    "	if dm == 2 then\n" +
    "		P2.ready = false\n" +
    "	elseif dm == 4 then\n" +
    "		P1.ready = false\n" +
    "	end\n" +
    "\n" +
    "	-- TODO: re-record ghost maybe? not needed but check just in case\n" +
    "	P1.file = nil\n" +
    "	P2.file = nil\n" +
    "\n" +
    "	if P1.ready then P1 = newDemoData(forms.gettext(file1_tbox)) end\n" +
    "	if P2.ready then P2 = newDemoData(forms.gettext(file2_tbox)) end\n" +
    "\n" +
    "	if CURR_CORE == \"BSNES\" then\n" +
    "		P1.b_first = true\n" +
    "		P2.b_first = true\n" +
    "	end\n" +
    "\n" +
    "	-- Header data\n" +
    "	header = newHeader()\n" +
    "\n" +
    "	header.data_area = 0xFFFF -- not used in save data (2 bytes)\n" +
    "	header.checksum = 0xFFFF -- calculate checksum in demomaker script, not here (2 bytes)\n" +
    "	header.camera_mode = read_word(0x7f6004)\n" +
    "	header.save_code = read_word(0x7f6006)\n" +
    "	header.rng_seed = read_word(0x7f6008)\n" +
    "	header.time = {0xFF, 0xFF, 0xFF} -- calculate later at end of demo (3 bytes)\n" +
    "	header.kart_type = 0xFF --read_byte(0x7f600d)\n" +
    "	header.cup = read_byte(0x7f600e)\n" +
    "	header.map = read_byte(0x7f600f)\n" +
    "	header.name = {} -- not used in save data (8 bytes)\n" +
    "\n" +
    "	P1_kart = 0xFF\n" +
    "	P2_kart = 0xFF\n" +
    "	P1_time_d = 0xFF\n" +
    "	P1_time_s = 0xFF\n" +
    "	P1_time_m = 0xFF\n" +
    "	P2_time_d = 0xFF\n" +
    "	P2_time_s = 0xFF\n" +
    "	P2_time_m = 0xFF\n" +
    "\n" +
    "\n" +
    "	p1_active = P1.active\n" +
    "	p2_active = P2.active\n" +
        "\n" +
    "\n" +
    "\n" +
        "\n" +
    "	while p1_active or p2_active do\n" +
    "\n" +
    "		p1_active = P1.active\n" +
    "		p2_active = P2.active\n" +
    "\n" +
    "		if not raceStatus() then\n" +
    "			P1.active = false\n" +
    "			P2.active = false\n" +
    "		end\n" +
    "\n" +
    "\n" +
    "		-- Press \"Start\" on joypad 1 or joypad 2 to end the respective recording\n" +
    "		if get_joypad(1).tap.Start == true and P1.active then P1.active = false end\n" +
    "		if get_joypad(2).tap.Start == true and P2.active then P2.active = false end\n" +
    "\n" +
    "		if P1.active and (not check_kart_active(read_word(0x7e1010))) then P1.active = false end\n" +
    "		if P2.active and (not check_kart_active(read_word(0x7e1110))) then P2.active = false end\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "		if P1.active then\n" +
    "\n" +
    "			P1_kart = read_byte(0x7e1012)\n" +
    "\n" +
    "			gui.text(10, 8, \"Buffer Size: \" .. tostring(P1.buffer_size) .. \"/1512\")\n" +
    "			gui.text(10,48, \"Frames Held: \" .. tostring(P1.fcnt + 1) .. \"/512\")\n" +
    "			P1.currInput = captureInput(get_joypad(1), 10, 28)\n" +
    "\n" +
    "			\n" +
    "			if (not P1.first) then\n" +
    "\n" +
    "				local inc_cnt = false\n" +
    "				if P1.currInput == P1.previousFrame and P1.fcnt < 0x1FF then\n" +
    "					inc_cnt = true\n" +
    "				end\n" +
    "\n" +
    "\n" +
    "				if inc_cnt then\n" +
    "					P1.fcnt = P1.fcnt + 1\n" +
    "				else\n" +
    "					--Different input, update previous frame, save to buffer and reset frame counter\n" +
    "					P1.inputBuffer = P1.inputBuffer .. tostring(P1.fcnt) .. P1.previousFrame .. \"\\n\"\n" +
    "					P1.fcnt = 0\n" +
    "					P1.buffer_size = P1.buffer_size + 2\n" +
    "				end\n" +
    "\n" +
    "				\n" +
    "\n" +
    "				if P1.buffer_size >= 0x5e8 then\n" +
    "					P1.active = false\n" +
    "				end\n" +
    "			else\n" +
    "				if P1.b_first then\n" +
    "					P1.b_first = false\n" +
    "				else\n" +
    "					P1.first = false\n" +
    "				end\n" +
    "			end\n" +
    "\n" +
    "			P1.previousFrame = P1.currInput\n" +
    "		else\n" +
    "			if p1_active then\n" +
    "				P1_time_d = read_byte(0x7e0100 + 1)\n" +
    "				P1_time_s = read_byte(0x7e0100 + 2)\n" +
    "				P1_time_m = read_byte(0x7e0100 + 4)\n" +
    "			end\n" +
    "		end\n" +
    "\n" +
    "\n" +
    "\n" +
    "		if P2.active then\n" +
    "\n" +
    "			P2_kart = read_byte(0x7e1112)\n" +
    "\n" +
    "			gui.text(10,232, \"Buffer Size: \" .. tostring(P2.buffer_size) .. \"/1512\")\n" +
    "			gui.text(10,272, \"Frames Held: \" .. tostring(P2.fcnt + 1) .. \"/512\")\n" +
    "			P2.currInput = captureInput(get_joypad(2), 10, 252)\n" +
    "\n" +
    "			\n" +
    "			if (not P2.first) then\n" +
    "\n" +
    "				local inc_cnt = false\n" +
    "				if P2.currInput == P2.previousFrame and P2.fcnt < 0x1FF then\n" +
    "					inc_cnt = true\n" +
    "				end\n" +
    "\n" +
    "\n" +
    "				if inc_cnt then\n" +
    "					P2.fcnt = P2.fcnt + 1\n" +
    "				else\n" +
    "					--Different input, update previous frame, save to buffer and reset frame counter\n" +
    "					P2.inputBuffer = P2.inputBuffer .. tostring(P2.fcnt) .. P2.previousFrame .. \"\\n\"\n" +
    "					P2.fcnt = 0\n" +
    "					P2.buffer_size = P2.buffer_size + 2\n" +
    "				end\n" +
    "\n" +
    "				\n" +
    "\n" +
    "				if P2.buffer_size >= 0x5e8 then\n" +
    "					P2.active = false\n" +
    "				end\n" +
    "			else\n" +
    "				if P2.b_first then\n" +
    "					P2.b_first = false\n" +
    "				else\n" +
    "					P2.first = false\n" +
    "				end\n" +
    "			end\n" +
    "\n" +
    "			P2.previousFrame = P2.currInput\n" +
    "		else\n" +
    "			if p2_active then\n" +
    "				P2_time_d = read_byte(0x7e0100 + 1)\n" +
    "				P2_time_s = read_byte(0x7e0100 + 2)\n" +
    "				P2_time_m = read_byte(0x7e0100 + 4)\n" +
    "			end\n" +
    "		end\n" +
    "\n" +
    "		UPDATE()\n" +
            "\n" +
    "	end\n" +
    "\n" +
    "\n" +
        "\n" +
    "	--capture final input\n" +
    "	P1.inputBuffer = P1.inputBuffer .. tostring(P1.fcnt) .. P1.currInput ..\"\\n\"\n" +
    "	P2.inputBuffer = P2.inputBuffer .. tostring(P2.fcnt) .. P2.currInput ..\"\\n\"\n" +
    "\n" +
    "\n" +
    "	--P1_time_d = read_byte(0x7e10f2 + 1)\n" +
    "	--P1_time_s = read_byte(0x7e10f2 + 2)\n" +
    "	--P1_time_m = read_byte(0x7e10f2 + 4)\n" +
    "\n" +
    "\n" +
    "	--P2_time_d = read_byte(0x7e11f2 + 1)\n" +
    "	--P2_time_s = read_byte(0x7e11f2 + 2)\n" +
    "	--P2_time_m = read_byte(0x7e11f2 + 4)\n" +
    "\n" +
    "\n" +
    "\n" +
    "\n" +
    "	if P1.file ~= nil then\n" +
    "		header.kart_type = P1_kart\n" +
    "		header.time = {P1_time_d, P1_time_s, P1_time_m} \n" +
    "\n" +
    "		P1.file:write(headerToString(header))\n" +
    "		P1.file:write(P1.inputBuffer)\n" +
    "		P1.file:close()\n" +
    "	end\n" +
    "\n" +
    "	if P2.file ~= nil then\n" +
    "		header.kart_type = P2_kart\n" +
    "		header.time = {P2_time_d, P2_time_s, P2_time_m}\n" +
    "\n" +
    "		P2.file:write(headerToString(header))\n" +
    "		P2.file:write(P2.inputBuffer)\n" +
    "		P2.file:close()\n" +
    "	end\n" +
    "\n" +
    "	READY = false\n" +
    "	P1.active = false\n" +
    "	P2.active = false\n" +
    "	P1.ready = false\n" +
    "	P2.ready = false\n" +
    "\n" +
    "end");
                string strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                //This will strip just the working path name:
                //C:\Program Files\MyApplication
                string strWorkPath = System.IO.Path.GetDirectoryName(strExeFilePath);
                string strSettingsXmlFilePath = System.IO.Path.Combine(strWorkPath, "demomaker.lua");
                rtb.AppendText("Generated LUA scipt, "+ strSettingsXmlFilePath +", use with BizHawk 2.3 BSNES or SNES9x cores\n");
            }
            catch(Exception)
            { rtb.AppendText("Failed to generate LUA scipt"); }

        }
    }
}
