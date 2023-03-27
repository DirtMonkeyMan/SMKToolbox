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
            Payload.body = readline(Filename.inputfile, Payload.body, "NTSC", dataGridView1, Info);

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
                            AppendText(Info, "Error saving demo to ROM", Color.Red, true);
                        }
                    }
                }
            }
            else
            {
                //no data to save 
                AppendText(Info, "No data to save, load CSV file first", Color.Black, true);
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
                            AppendText(Info, "Error saving demo to ROM", Color.Red, true);
                        }

                    }
                }

            }
            else
            {
                //no data to save nothing to do
                AppendText(Info, "No data to save, load CSV file first", Color.Black, true);
            }

        }

        private void HexMode2_CheckedChanged(object sender, EventArgs e)
        {
            if (HexMode2.Checked == true)
                Offset.Hexadecimal = true;
            else
                Offset.Hexadecimal = false;

        }

        private void Offset_ValueChanged(object sender, EventArgs e)
        {
            Payload.offset = Convert.ToInt32(Offset.Value);
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
