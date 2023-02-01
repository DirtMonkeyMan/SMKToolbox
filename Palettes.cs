using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SMKToolbox
{
    public static class Palettes
    {
        //Load: Takes a raw byte array and converts it's contents from RGB555 to 24bit colour values
        public static int[] Load(byte[] input)
        {
            try
            {
                int[] palette;
                using (BinaryReader reader = new BinaryReader(new MemoryStream(input)))
                {
                    palette = new int[input.Length / 2];
                    for (int i = 0; i < palette.Length; i++)
                    {
                        int color = reader.ReadUInt16();
                        // Convert 15-bit RGB to 24-bit RGB
                        int b = (color & 0x7C00) >> 10;
                        int g = (color & 0x03E0) >> 5;
                        int r = color & 0x001F;
                        r = r * 8;
                        g = g * 8;
                        b = b * 8;
                        palette[i] = Color.FromArgb(r, g, b).ToArgb();
                    }
                }
                return palette;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the palette: " + ex.Message);
                return null;
            }
        }

        //ConvertTo2DArray: Takes one long int[] and converts it in to 16x16 int array.
        public static int[][] ConvertTo2DArray(int[] palette)
        {
            try
            {
                int[][] blocks = new int[palette.Length / 16][];
                for (int i = 0; i < blocks.Length; i++)
                {
                    blocks[i] = new int[16];
                    Array.Copy(palette, i * 16, blocks[i], 0, 16);
                }
                return blocks;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while converting the palette to 2D: " + ex.Message);
                return null;
            }
        }

    }
}
