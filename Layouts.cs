using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace SMKToolbox
{
    class TileLayout
    {
        public struct TileDetails
        {
            public int tileIndex;
            public int palette;
            public bool hFlip;
            public bool vFlip;
            public bool priority;
        }

        public static TileDetails[] GetTileDetails(byte[] layoutData)
        {
            int layoutWidth = 32;
            int layoutHeight = 32;

            TileDetails[] details = new TileDetails[layoutWidth * layoutHeight];

            // Iterate through layout data
            for (int y = 0; y < layoutHeight; y++)
            {
                for (int x = 0; x < layoutWidth; x++)
                {
                    // Get tile data
                    int index = y * layoutWidth + x;
                    int tileData = (layoutData[index * 2 + 1] << 8) | layoutData[index * 2];

                    // Extract properties from tile data
                    int tileIndex = tileData & 0x3FF;
                    int palette = (tileData >> 10) & 0x7;
                    bool hFlip = ((tileData >> 14) & 1) == 1;
                    bool vFlip = ((tileData >> 15) & 1) == 1;
                    bool priority = ((tileData >> 13) & 1) == 1;

                    // Store tile details in the array
                    details[index] = new TileDetails { tileIndex = tileIndex, palette = palette, hFlip = hFlip, vFlip = vFlip, priority = priority };
                }
            }

            return details;
        }

        public static byte[] EncodeTileDetails(TileDetails[] editedLayout)
        {
            byte[] layoutData = new byte[editedLayout.Length * 2];
            for (int i = 0; i < editedLayout.Length; i++)
            {
                // Extract properties from tile details
                int tileIndex = editedLayout[i].tileIndex & 0x3FF;
                int palette = (editedLayout[i].palette & 0x7) << 10;
                int hFlip = (editedLayout[i].hFlip ? 1 : 0) << 14;
                int vFlip = (editedLayout[i].vFlip ? 1 : 0) << 15;
                int priority = (editedLayout[i].priority ? 1 : 0) << 13;

                // Combine properties into a single 16-bit value
                int tileData = tileIndex | palette | hFlip | vFlip | priority;
                layoutData[i * 2] = (byte)(tileData & 0xFF);
                layoutData[i * 2 + 1] = (byte)((tileData >> 8) & 0xFF);
            }

            return layoutData;
        }
            


            public static Bitmap DisplayTiles(Bitmap[][] bitmaps, TileDetails[] tileDetails)
        {
            int layoutWidth = 32;
            int layoutHeight = 32;

            // Create a bitmap to hold the layout
            Bitmap layout = new Bitmap(layoutWidth * 8, layoutHeight * 8);

            // Iterate through tile details
            for (int y = 0; y < layoutHeight; y++)
            {
                for (int x = 0; x < layoutWidth; x++)
                {
                    // Get tile details
                    int index = y * layoutWidth + x;
                    int tileIndex = tileDetails[index].tileIndex;
                    int palette = tileDetails[index].palette;
                    bool hFlip = tileDetails[index].hFlip;
                    bool vFlip = tileDetails[index].vFlip;
                  

                    // Get tile bitmap
                    Bitmap tileTemp = (Bitmap)bitmaps[palette][tileIndex].Clone();

                    // Flip tile if necessary
                    if (hFlip)
                    {
                        tileTemp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    if (vFlip)
                    {
                        tileTemp.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    }

                    // Draw tile on layout bitmap
                    using (Graphics g = Graphics.FromImage(layout))
                    {
                        g.DrawImage(tileTemp, x * 8, y * 8);
                    }
                }
            }
            // Show the layout in a form or picture box

            return (layout);
        }

        public static void UpdatePictureBoxes(Panel tilemapPanel, Bitmap[][] bitmaps, TileDetails[] tileDetails)
        {
            int layoutWidth = 32;
            int layoutHeight = 32;
            int pictureBoxSize = (int)(tilemapPanel.Width / layoutWidth);

            // Iterate through tile details
            for (int y = 0; y < layoutHeight; y++)
            {
                for (int x = 0; x < layoutWidth; x++)
                {
                    // Get tile details
                    int index = y * layoutWidth + x;
                    int tileIndex = tileDetails[index].tileIndex;
                    int palette = tileDetails[index].palette;
                    bool hFlip = tileDetails[index].hFlip;
                    bool vFlip = tileDetails[index].vFlip;

                    // Get tile bitmap
                    Bitmap tile = (Bitmap)bitmaps[palette][tileIndex].Clone();

                    // Flip tile if necessary
                    if (hFlip)
                    {
                        tile.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    if (vFlip)
                    {
                        tile.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    }

                    // Get the corresponding picture box
                    PictureBox pictureBox = (PictureBox)tilemapPanel.Controls[index];
                    pictureBox.Size = new Size(pictureBoxSize, pictureBoxSize);
                    pictureBox.Location = new Point(x * pictureBoxSize, y * pictureBoxSize);
                    pictureBox.Image = tile;
                }
            }
        }


        public static void UpdateTilePickerBoxes(Panel tilePickerPanel, Bitmap[][] bitmaps, int palette)
        {
            int layoutWidth = 16;
            int layoutHeight = 16;
            int pictureBoxSize = (int)(tilePickerPanel.Width / layoutWidth);
            int tileCounter = 0;
            // Iterate through tile details
            for (int y = 0; y < layoutHeight; y++)
            {
                for (int x = 0; x < layoutWidth; x++)
                {
                    int index = y * layoutWidth + x;
                    // Get tile bitmap
                    Bitmap tile = bitmaps[palette][tileCounter];

                    
                    // Get the corresponding picture box
                    PictureBox pictureBox = (PictureBox)tilePickerPanel.Controls[index];
                    pictureBox.Size = new Size(pictureBoxSize, pictureBoxSize);
                    pictureBox.Location = new Point(x * pictureBoxSize, y * pictureBoxSize);
                    pictureBox.Image = tile;
                    tileCounter++;
                }
                
            }
        }

    }
}
