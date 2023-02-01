using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMKToolbox
{
    class Tiles
    {
        public static Bitmap[][] DrawTiles(byte[] tileData, int[][] palette)
        {
            Bitmap[][] tiles = new Bitmap[16][];
            for (int p = 0; p < 16; p++)
            {
                using (MemoryStream ms = new MemoryStream(tileData))
                using (BinaryReader reader = new BinaryReader(ms))
                {
                    int numTiles = (int)(reader.BaseStream.Length / 32);
                    tiles[p] = new Bitmap[numTiles];
                    int tileCounter = 0;
                    for (int i = 0; i < numTiles; i++)
                    {
                        try
                        {
                            tiles[p][tileCounter] = new Bitmap(8, 8);
                            byte[] tileData1 = reader.ReadBytes(16);
                            byte[] tileData2 = reader.ReadBytes(16);
                            for (int y = 0; y < 8; y++)
                            {
                                for (int x = 0; x < 8; x++)
                                {
                                    int color = (tileData1[y * 2] >> (7 - x) & 1) | ((tileData1[y * 2 + 1] >> (7 - x) & 1) << 1) | ((tileData2[y * 2] >> (7 - x) & 1) << 2) | ((tileData2[y * 2 + 1] >> (7 - x) & 1) << 3);
                                    tiles[p][tileCounter].SetPixel(x, y, Color.FromArgb(palette[p][color]));
                                }
                            }
                            tileCounter++;
                        }
                        catch (EndOfStreamException)
                        {
                            break;
                        }
                    }
                }
            }
            return tiles;
        }
    }

    
}
