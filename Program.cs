using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace SMKToolbox
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*
            //Testing File Class
            string fileName = "Hello.txt";

            (byte[] fileBytes, bool status) = Files.Read(fileName);
            if (status)
            {
                // Do something with the file bytes
                // for example 
                Console.WriteLine(Encoding.UTF8.GetString(fileBytes));
                Files.Write("Copy.txt", fileBytes, true);
                Files.Append("Copy.txt", fileBytes);
                Files.Insert("Copy.txt", fileBytes, 10);
            }
            else
            {
                // Handle the error
            }


            //Testing Palettes Class
            //Testing Palette Class
            (byte[] RawPalette, bool status2) = Files.Read("title.COL");
            int[] NewPalette = Palettes.Load(RawPalette);
            int[][] PalletSet = Palettes.ConvertTo2DArray(NewPalette);

            if (PalletSet != null)
            {
                // Create a new bitmap to hold the palette colors
                Bitmap bmp = new Bitmap(16, 16);

                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        bmp.SetPixel(j, i, Color.FromArgb(PalletSet[i][j]));
                    }
                }
                bmp.Save("palette.bmp");



            }
            else
            {
                // Handle the error
            }

            Bitmap bmp2 = new Bitmap(16,1);

                for (int j = 0; j < 16; j++)
                {
                    bmp2.SetPixel(j,0, Color.FromArgb(PalletSet[15][j]));
                }
            bmp2.Save("palette2.bmp");

            //Testing Tiles class:
            (byte[] TilesRaw, bool status3) = Files.Read("TITLE-ENG.CGX");
            Bitmap[][] tiles = Tiles.DrawTiles(TilesRaw, PalletSet);

            //save it out
            for (int set = 0; set < 16; set++)
            {
                int numTiles = tiles[set].Length;
                int width = (int)Math.Ceiling(Math.Sqrt(numTiles));
                int height = (int)Math.Ceiling((double)numTiles / width);
                Bitmap result = new Bitmap(width * 8, height * 8);
                using (Graphics g = Graphics.FromImage(result))
                {
                    for (int i = 0; i < numTiles; i++)
                    {
                        int x = i % width;
                        int y = i / width;
                        g.DrawImage(tiles[set][i], x * 8, y * 8);
                    }
                }
                result.Save("set" + set + ".bmp");
            }

            (byte[] LayoutsRaw, bool status4) = Files.Read("TITLE-ENG.SCR");

            TileLayout layout = new TileLayout();
            TileLayout.TileDetails[] tileDetails = TileLayout.GetTileDetails(LayoutsRaw);
            TileLayout.DisplayTiles(tiles, tileDetails);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            */


            Application.Run(new Main());



        }

    }
}
