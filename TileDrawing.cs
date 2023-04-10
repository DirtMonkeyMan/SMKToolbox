using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#pragma warning disable IDE1006

namespace SMKToolbox
{
    public static partial class TileDrawing
    {
        /*
        --------------------------------------------------
        struct graphicTile4bpp
        --------------------------------------------------
        A 1D linear graphical tile measuring 4bpp. When converting to 8bpp mode 7, the palette byte can be used to determine which area of the color palette the tile uses.
        Palettes normally count up in 16s (0x10), but any 8-bit value can be stored in palette.

        The actual graphics data is stored in the linear byte[] array as 4bpp graphics. Whether or not the pixels in each byte are reverse depends on the reverseOrder flag.
        */
        public struct graphicTile4bpp
        {
            public byte palette;        //8-bit byte of the color palette the tile uses. Increments by 0x10 (16).
            public byte[] graphics;     //4bpp graphics data. Must be 32 bytes long.
            public bool reverseOrder;   //boolean to reverse every nybble when converting to 8bpp or drawing
        }

        /*
        --------------------------------------------------
        struct graphicTile8bpp2d
        --------------------------------------------------
        A 2D graphical tile measuring 8bpp. There is only a single 2D byte[,] array in which the graphics are stored.
        */
        public struct graphicTile8bpp2d
        {
            public byte[,] graphics;
        }

        /*
        --------------------------------------------------
        struct trackTheme2d
        --------------------------------------------------
        A collection of 2D 8bpp tiles for use in editing tracks.
        */
        public struct trackTheme2d
        {
            public graphicTile8bpp2d[] tile;
        }

        /*
        --------------------------------------------------
        graphicTile4bpp make4bppTile(byte palette, bool reverse)
        --------------------------------------------------
        Creates a blank 1D 4bpp tile.

        byte palette:
            Appends a palette ID to the tile for use in 8bpp conversion. Increments in 16s (0x10).

        bool reverse:
            Determines whether or not the hex digits in each byte should be switched around when converting to 8bpp. True if working with SMK formatted track graphics.

        Outputs:
            A blank 4bpp tile.
        */
        public static graphicTile4bpp make4bppTile(byte palette, bool reverse)
        {
            graphicTile4bpp tile = new graphicTile4bpp
            {
                palette = palette,
                graphics = new byte[32],
                reverseOrder = reverse
            };

            return tile;
        }

        /*
        --------------------------------------------------
        graphicTile8bpp2d make8bppTile2d()
        --------------------------------------------------
        Creates a blank 2D 8bpp tile.

        Outputs:
            A blank 2D 8bpp tile.
        */
        public static graphicTile8bpp2d make8bppTile2d()
        {
            graphicTile8bpp2d tile = new graphicTile8bpp2d {graphics = new byte[8,8]};

            return tile;
        }

        /*
        --------------------------------------------------
        graphicTile8bpp2d convert4bppTo8bpp2d(graphicTile4bpp tile16r)
        --------------------------------------------------
        Converts a 1D 4bpp tile to a 2D 8bpp tile.

        graphicTile4bpp tile16r
            The input tile to convert to 8bpp. Contrary to its name, the function can process both normal order and reverse order graphics.

        Outputs:
            A 2D 8bpp tile containing the same graphics as the input 4bpp tile.
        */
        public static graphicTile8bpp2d convert4bppTo8bpp2d(graphicTile4bpp tile16r)
        {
            graphicTile8bpp2d tile256 = make8bppTile2d();

            byte pal = tile16r.palette;
            int col;
            int i = 0;
            int j = 0;
            int k;

            if (tile16r.reverseOrder == true)
            {
                for (k = 0; k < 32; k++)
                {
                    col = tile16r.graphics[k];
                    col = (col % 16) + pal;
                    tile256.graphics[i, j] = (byte)col;
                    i++;

                    col = tile16r.graphics[k];
                    col = (col / 16) + pal;
                    tile256.graphics[i, j] = (byte)col;
                    i++;

                    j += i / 8;
                    i %= 8;
                }
            }
            else
            {
                for (k = 0; k < 32; k++)
                {
                    col = tile16r.graphics[k];
                    col = (col / 16) + pal;
                    tile256.graphics[i, j] = (byte)col;
                    i++;

                    col = tile16r.graphics[k];
                    col = (col % 16) + pal;
                    tile256.graphics[i, j] = (byte)col;
                    i++;

                    j += i / 8;
                    i %= 8;
                }
            }

            return tile256;
        }

        /*
        --------------------------------------------------
        Color[] convertAllARGB(byte[] pal)
        --------------------------------------------------
        Converts from 15-bit BGR specification to 32-bit ARGB standard more easily usable by .NET.

        byte[] pal:
            Input 15-bit BGR palette. This palette must be little-endian, meaning its high byte comes after its low byte internally.

        Outputs:
            Color[] array usable by plenty of drawing functions.
        */
        public static Color[] convertAllARGB(byte[] pal)
        {
            if (pal == null) throw new System.Exception("The input palette is null.");
            
            if ((pal.Length % 2) != 0) throw new System.Exception("The input palette is uneven in length.");

            int l = pal.Length / 2;
            Color[] col = new Color[l];

            int i;
            int j = 0;
            int colWord;

            int r;
            int g;
            int b;

            for (i = 0; i < l; i++)
            {
                colWord = pal[j] + (256 * pal[j + 1]);

                // Don't be upsetti!
                // Have some spaghetti!
                r = (int)((double)((double)(colWord & 0x001F) / 31) * 255);             // R: 0000 0000 0001 1111
                g = (int)((double)((double)((colWord & 0x03E0) / 0x0020) / 31) * 255);  // G: 0000 0011 1110 0000
                b = (int)((double)((double)((colWord & 0x7C00) / 0x0400) / 31) * 255);  // B: 0111 1100 0000 0000

                col[i] = Color.FromArgb(r, g, b);

                j += 2;
            }

            return col;
        }

        /*
        --------------------------------------------------
        byte[,] convert2DMap(byte[] linearStream, int width, int height)
        --------------------------------------------------
        Converts from a linear array of bytes to a 2D array

        byte[] linearStream
            1D linear array to convert to 2D.

        int width
            Width of the output buffer.
        
        int height
            Height of the output buffer.

        Outputs:
            2D byte array that can be addressed by array[x, y].
        */
        public static byte[,] convert2DMap(byte[] linearStream, int width, int height)
        {
            if (linearStream == null)
            {
                throw new System.Exception("The input array is null.");
            }
            if ((linearStream.Length) != (width * height))
            {
                throw new System.Exception("The area of the output doesn't match the input array's length.");
            }

            byte[,] newMap = new byte[width, height];

            int i;
            int j;
            int k = 0;

            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    newMap[i, j] = linearStream[k];
                    k++;
                }
            }

            return newMap;
        }

        /*
        --------------------------------------------------
        Bitmap initBuffer(byte[,] tilemap, Bitmap buffer, Color[] palette, trackTheme2d theme)
        --------------------------------------------------
        Initializes a bitmap buffer of an 8bpp 8-bit tilemap.

        byte[,] tilemap
            2D array of an 8-bit tilemap.

        Bitmap buffer
            The buffer to write to.
        
        Color[] palette
            The color palette to use while drawing.

        trackThem2d theme
            The tilemap to use while drawing.

        Outputs:
            A bitmap to be displayed by a Picture Box or a textured brush.
        */
        public static Bitmap initBuffer(byte[,] tilemap, Bitmap buffer, Color[] palette, trackTheme2d theme)
        {
            if (tilemap == null) throw new System.Exception("The tilemap buffer is null.");

            int lx = tilemap.GetLength(0) * 8;
            int ly = tilemap.GetLength(1) * 8;

            if (buffer == null) buffer = new Bitmap(lx, ly);
            else
            {
                if (buffer.Width < lx) lx = buffer.Width;
                if (buffer.Height < ly) ly = buffer.Height;
            }

            int px;
            int py;

            int gx;
            int gy = 0;

            int tx;
            int ty = 0;

            for (py = 0; py < ly; py++)
            {
                gx = 0;
                tx = 0;

                for (px = 0; px < lx; px++)
                {
                    buffer.SetPixel(px, py, palette[theme.tile[tilemap[tx, ty]].graphics[gx, gy]]);

                    gx++;
                    tx += gx / 8;
                    gx %= 8;
                }

                gy++;
                ty += gy / 8;
                gy %= 8;
            }

            return buffer;
        }

        /*
        --------------------------------------------------
        void drawTileToBuffer(int x, int y, byte[,] layout, Color[] palette, trackTheme2d theme, PictureBox picturebox, bool invalid)
        --------------------------------------------------
        Draws a tile to the selected picture box's bitmap buffer.

        int x, y
            Target coordinates to draw the tile.
        
        byte[,] layout
            The array to read from to draw the tile.

        Color[] palette
            The color palette to use while drawing.

        trackTheme2d theme
            The track theme to use while drawing.

        PictureBox picturebox
            The Picture Box whose bitmap will be modified.

        bool invalidate
            Determines whether or not to invalidate the picturebox after drawing the tile. Not recommended when drawing multiple tiles.

        Outputs:
            Void.
        */
        public static void drawTileToBuffer(int x, int y, byte[,] layout, Color[] palette, trackTheme2d theme, PictureBox picturebox, bool invalid)
        {
            int w = layout.GetLength(0);
            int h = layout.GetLength(1);

            Bitmap trkBuffer = (Bitmap)picturebox.Image;
            if (trkBuffer == null) throw new System.Exception("The track buffer bitmap is null.");

            if ((x >= 0) & (x < w) & (y >= 0) & (y < h))
            {
                w *= 8;
                h *= 8;

                if (trkBuffer.Width < w) w = trkBuffer.Width;
                if (trkBuffer.Height < h) h = trkBuffer.Height;

                int px = x * 8;
                int py = y * 8;

                int i;
                int j;

                int a;
                int b = py - 1;

                byte ct = layout[x, y];
                Color col;

                for (j = 0; j < 8; j++)
                {
                    b++;
                    if (b >= h) break;
                    a = px;

                    for (i = 0; i < 8; i++)
                    {
                        col = palette[theme.tile[ct].graphics[i, j]];

                        trkBuffer.SetPixel(a, b, col);

                        a++;
                        if (a >= w) break;
                    }

                }

                if (invalid) picturebox.Invalidate();
            }
        }


    }
}