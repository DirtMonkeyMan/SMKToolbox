// #define disableDeleteWarning

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SMKToolbox.TrackManager;
using static SMKToolbox.TileDrawing;
using static SMKToolbox.Overlay;
using System.Media;

#pragma warning disable IDE1006

namespace SMKToolbox
{
    public partial class TM_CourseEditor : Form
    {
        // ----------------------------------------------------
        // DATA
        // ----------------------------------------------------
        private Bitmap trkBuffer;
        private Bitmap selBuffer;
        private byte[,] layout;
        private byte[,] pasteArea = new byte[1, 1];
        private trackTheme2d theme;
        private Color[] palette;
        private Size picturesize;
        private Size pasteSize = new Size();
        private Point panelLocation;
        private readonly Random rand = new Random();
        private Timer vstimer;
        private int scale = 1;
        // private int tileSel; OLD VARIABLE
        private Rectangle rectCursor = new Rectangle(new Point(0, 0), new Size(8, 8));
        private Rectangle rectSelect = new Rectangle()
        {
            Width = 16,
            Height = 16
        };
        private readonly Pen penCursor = new Pen(Color.White);
        private int cursorx;
        private int cursory;
        private int cursorpx = 128;
        private int cursorpy = 0;
        private mouseModes mouseMode = 0;
        private int anchorx = 0;
        private int anchory = 0;
        private List<Undo> undoHistory = new List<Undo>();
        private List<Undo> redoHistory = new List<Undo>();
        private Undo currentUndo;
        // private Bitmap[] tileBrushesBM;
        // private TextureBrush[] tileBrushes;
        private List<Paste> pasteStache = new List<Paste>();
        private int pasteIndex = 0;
        private int dataWidth;
        private int dataHeight;
        private int bufferWidth;
        private int bufferHeight;
        private trackEditorParams param;
        private Form parentForm;
        private TM_CourseEditor receiverForm;
        private int modifyDepth = 0;
        private string saveLoc = null;
        private int selectedOvr = -1;
        private int ovrRelX;
        private int ovrRelY;
        private int ovrRot = 0;
        private bool ovrSave = true;
        private bool ovrMod = false;
        private overlayState ovrState = null;
        private overlayPiece[] ovrPieces = null;
        private List<overlayBlock> ovrBlocks = null;
        private editorModes mode = editorModes.TileEditor;
        private Rectangle tempRect = new Rectangle(new Point(0, 0), new Size(16, 16));
        private Brush brushSel = new SolidBrush(Color.FromArgb(0x80, 0xFF, 0xFF, 0xFF));

        private struct UndoCoord
        {
            public int X;
            public int Y;
        }

        private struct Undo
        {
            public byte[] tileNumber;
            public byte[] redoNumber;
            public UndoCoord[] coords;
        }

        private struct Paste
        {
            public byte[,] tile;
            public Bitmap preview;
        }

        public enum trackEditorParams
        {
            AllowCopying = 1,
            AllowPasteStashing = 2,
            AllowOverlay = 4,
            AllowStartLine = 8,
            AllowCPZones = 16,
            AllowObjects = 32,
            ReEnableOwner = 64,
            TransferPaste = 128
        }

        public enum receiveMode
        {
            CopyToPaste = 0
        }

        private enum autoCompressSettings
        {
            DontCompress = 0,
            OnlyCompress = 1,
            AlwaysCompress = 2,
            AskBeforeCompress = 3
        }

        private enum editorModes
        {
            TileEditor = 0,
            OverlayEditor = 1,
            PlaceOverlay = 2
        }

        private enum mouseModes
        {
            NoMouse = 0x00,
            LeftMouse = 0x01,
            RightMouse = 0x02,
            AnythingButLeft = 0xFE,
            AnythingButRight = 0xFD
        }

        // ----------------------------------------------------
        // Tile placement (direct, no undo)
        // ----------------------------------------------------
        private void setTile(int x, int y, byte tile, bool invalid)
        {
            if ((x >= 0) & (x < dataWidth) & (y >= 0) & (y < dataHeight))
            {
                layout[x, y] = tile;

                drawTileToBuffer(x, y, layout, palette, theme, pictureTrackDisplay, invalid);
            }
        }

        // ----------------------------------------------------
        // Tile receiving
        // ----------------------------------------------------
        private byte getTile(int x, int y)
        {
            if ((x >= 0) & (x < dataWidth) & (y >= 0) & (y < dataHeight))
            {
                return layout[x, y];
            }
            else
            {
                return 0;
            }
        }

        // ----------------------------------------------------
        // Track picture scaling
        // ----------------------------------------------------
        private void resizeTrack()
        {
            bool timeEnable = vstimer.Enabled;
            vstimer.Enabled = false;

            pictureTrackDisplay.Width = bufferWidth * scale;
            pictureTrackDisplay.Height = bufferHeight * scale;

            pictureTrackDisplay.Invalidate();

            vstimer.Enabled = timeEnable;
        }

        // ----------------------------------------------------
        // Window control scaling
        // ----------------------------------------------------
        private void resizeAssets()
        {
            bool timeEnable = vstimer.Enabled;
            vstimer.Enabled = false;

            picturesize.Width = this.Size.Width - 196;
            picturesize.Height = this.Size.Height - 94;

            panelLocation.X = picturesize.Width + 19;
            panelLocation.Y = 43;

            // drawBuffer();

            this.panelTrackEditor.Size = picturesize;
            this.tabFunctions.Location = panelLocation;

            pictureTrackDisplay.Invalidate();

            vstimer.Enabled = timeEnable;
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Resize
        // ----------------------------------------------------
        private void resizeHandle(object sender, EventArgs e)
        {
            resizeAssets();
        }

        // ----------------------------------------------------
        // Deprecated function: Tile randomizer.
        // ----------------------------------------------------
        private void tileScrambler(object sender, EventArgs e)
        {
            int x;
            int y;
            int tile;

            x = rand.Next(0x00, bufferWidth);
            y = rand.Next(0x00, bufferHeight);
            tile = rand.Next(0xF0, 0xF4);

            setTile(x, y, (byte)tile, true);

            vstimer.Interval = (int)((double)100 / 6);
            vstimer.Enabled = true;
        }

        // ----------------------------------------------------
        // Whatchamacallit
        // ----------------------------------------------------
        private void whatchamacallit(object sender, EventArgs e)
        {
            pictureTrackDisplay.Invalidate();
            vstimer.Interval = 1;
            vstimer.Enabled = true;
        }

        // ----------------------------------------------------
        // Selection catalog initializer
        // ----------------------------------------------------
        private void initSelect(byte[,] ts)
        {
            int i;
            int j;
            byte k = 0;

            for (j = 0; j < 32; j++)
            {
                for (i = 0; i < 8; i++)
                {
                    ts[i, j] = k;
                    k++;
                }
            }
        }

        // ----------------------------------------------------
        // Array copier
        // ----------------------------------------------------
        private byte[,] copyBytes(byte[,] source)
        {
            int w = source.GetLength(0);
            int h = source.GetLength(1);

            int i;
            int j;

            byte[,] dest = new byte[w, h];

            for (j = 0; j < h; j++)
            {
                for (i = 0; i < w; i++)
                {
                    dest[i, j] = source[i, j];
                }
            }

            return dest;
        }

        // ----------------------------------------------------
        // Encode 1D array into 2D array
        // ----------------------------------------------------
        private byte[,] convert1Dto2D(byte[] arr1d, int width, int height)
        {
            if (arr1d.Length < (width * height)) throw new Exception("Input buffer isn't long enough to convert to 2D array given the width and height.");

            byte[,] arr2d = new byte[width, height];
            int i;
            int j;
            int k = 0;

            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    arr2d[i, j] = arr1d[k];
                    k++;
                }
            }

            return arr2d;
        }

        // ----------------------------------------------------
        // Error Square Generator
        // ----------------------------------------------------
        private Bitmap makeErrorSquare(int width, int height)
        {
            width *= 8;
            height *= 8;

            Bitmap bm = new Bitmap(width, height);

            Color c1 = Color.FromArgb(160, 160, 160);
            Color c2 = Color.FromArgb(96, 96, 96);

            int i;
            int j;
            bool a = false;
            bool b = false;

            for (j = 0; j < height; j++)
            {
                for (i = 0; i < width; i++)
                {
                    if ((a | b) & !(a & b)) bm.SetPixel(i, j, c2); else bm.SetPixel(i, j, c1);

                    a = !a;
                }

                b = !b;
            }

            return bm;
        }

        // ----------------------------------------------------
        // Overlay Setup
        // ----------------------------------------------------
        private overlayPiece[] setUpOverlay(overlayState ovs)
        {
            int i;
            int j;
            int l = ovs.pieces.Count;
            int z = ovs.sizes.GetLength(0);

            overlayPiece[] ovps = new overlayPiece[l];
            byte[,] buffer2d;

            for (i = 0; i < l; i++)
            {
                ovps[i] = new overlayPiece
                {
                    buffers = new Bitmap[z],
                    valid = new bool[z]
                };

                for (j = 0; j < z; j++)
                {
                    try
                    {
                        buffer2d = convert1Dto2D(ovs.pieces[i], ovs.sizes[j, 0], ovs.sizes[j, 1]);
                        ovps[i].buffers[j] = initBuffer(buffer2d, ovps[i].buffers[j], palette, theme, false);
                        ovps[i].valid[j] = true;
                    }
                    catch (Exception)
                    {
                        ovps[i].buffers[j] = makeErrorSquare(ovs.sizes[j, 0], ovs.sizes[j, 1]);
                        ovps[i].valid[j] = false;
                    }
                }
            }

            return ovps;
        }

        // ----------------------------------------------------
        // Overlay Block Setup
        // ----------------------------------------------------
        private List<overlayBlock> setUpBlocks(overlayState ovs)
        {
            List<overlayBlock> ob = new List<overlayBlock>(41);

            int i;
            int j = 0;

            for (i = 0; i < 41; i++)
            {
                if ((ovs.rawData[j] == 0xFF) & (ovs.rawData[j + 1] == 0xFF)) break;

                ob.Add(new overlayBlock
                {
                    enabled = true,

                    entry = ovs.rawData[j] & 0x3F,
                    size = ovs.rawData[j] / 0x40,

                    x = ovs.rawData[j + 1] & 0x7F,
                    y = (ovs.rawData[j + 2] * 2) + (ovs.rawData[j + 1] / 0x80),
                });

                overlayBlock cob = ob[i];

                cob.rect.X = cob.x * 8;
                cob.rect.Y = cob.y * 8;

                cob.rect.Width = ovs.sizes[cob.size, 0] * 8;
                cob.rect.Height = ovs.sizes[cob.size, 1] * 8;

                j += 3;
            }

            return ob;
        }

        // ----------------------------------------------------
        // Overlay Capacity Text
        // ----------------------------------------------------
        private void updateCapacityText()
        {
            labelOverlayCapacity.Text = "Capacity: " + ovrBlocks.Count.ToString() + " / 41";
        }

        // ----------------------------------------
        // INIT CODE
        // ----------------------------------------
        private void realInit(byte[,] useTrack, trackTheme2d useTheme, Color[] usePalette, int width, int height, trackEditorParams parameters)
        {
            param = parameters;

            dataWidth = width;
            dataHeight = height;

            bufferWidth = dataWidth * 8;
            bufferHeight = dataHeight * 8;

            layout = useTrack;
            theme = useTheme;
            palette = usePalette;

            trkBuffer = new Bitmap(bufferWidth, bufferHeight);

            InitializeComponent();

            if ((param & trackEditorParams.TransferPaste) != 0)
            {
                buttonSave.Text = "Save && Close";
                buttonSaveAdv.Dispose();
            }

            if (!(((param & trackEditorParams.AllowCopying) != 0) & ((param & trackEditorParams.AllowPasteStashing) != 0)))
            {
                tabPieces.Dispose();
            }

            pictureTrackDisplay.Image = trkBuffer;
            picturePasteDisplay.Size = pasteSize;

            trkBuffer = initBuffer(layout, trkBuffer, palette, theme, true);

            byte[,] tilesel = new byte[8, 32];
            initSelect(tilesel);
            selBuffer = initBuffer(tilesel, selBuffer, palette, theme, true);

            pictureTileSelect.Image = selBuffer;
            pictureTileSelect.Invalidate();

            // tileBrushesBM = makeBrushMaps();
            // tileBrushes = makeBrushes(tileBrushesBM);

            if (ovrState != null)
            {
                try
                {
                    ovrPieces = setUpOverlay(ovrState);
                    numericOverlayPattern.Maximum = (ovrPieces.Length / 4) - 1;
                    numericOverlaySize.Maximum = ovrState.sizes.GetLength(0) - 1;

                    updateOverlaySelect();

                    ovrBlocks = setUpBlocks(ovrState);

                    updateCapacityText();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while decoding overlay data:\n" + ex.Message);
                    tabOverlay.Dispose();

                    goto skipOverlay;
                }
            }
            else
            {
                tabOverlay.Dispose();
            }
        skipOverlay:;

            vstimer = new Timer
            {
                Interval = (int)((double)100 / 6)   // 50hz countries beware
            };
            vstimer.Tick += new EventHandler(whatchamacallit);
            // vstimer.Enabled = true;  // enable at your own risk -- it'll sprinkle your work of art

            resizeAssets();
            resizeTrack();
        }

        // ----------------------------------------------------
        // Init overload
        // ----------------------------------------------------
        public TM_CourseEditor(byte[,] useTrack, trackTheme2d useTheme, Color[] usePalette, int width, int height, trackEditorParams parameters, TM_CourseEditor owner)
        {
            if ((width < 1) | (height < 1))
            {
                throw new Exception("Cannot make an editor with invalid width or height.");
            }

            receiverForm = owner;
            parentForm = owner;

            realInit(useTrack, useTheme, usePalette, width, height, parameters);
        }

        // ----------------------------------------------------
        // Init overload
        // ----------------------------------------------------
        public TM_CourseEditor(byte[,] useTrack, trackTheme2d useTheme, Color[] usePalette, int width, int height, trackEditorParams parameters, Form owner, string savePath, overlayState overstate)
        {
            if ((width < 1) | (height < 1))
            {
                throw new Exception("Cannot make an editor with invalid width or height.");
            }

            parentForm = owner;
            saveLoc = savePath;
            ovrState = overstate;

            realInit(useTrack, useTheme, usePalette, width, height, parameters);
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Zoom In button
        // ----------------------------------------------------
        private void buttonZoomIn_Click(object sender, EventArgs e)
        {
            if (scale < 4)
            {
                scale++;

                resizeAssets();
                resizeTrack();
            }
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Zoom Out button
        // ----------------------------------------------------
        private void buttonZoomOut_Click(object sender, EventArgs e)
        {
            if (scale > 1)
            {
                scale--;

                resizeAssets();
                resizeTrack();
            }
        }

        // ----------------------------------------------------
        // Tile placement (indirect, w/ Undo functionality)
        // ----------------------------------------------------
        private void placeTileWUndoAt(int tx, int ty)
        {
            UndoCoord uc;
            int i;
            int j;

            int k;
            int l = ty;

            int w = pasteArea.GetLength(0);
            int h = pasteArea.GetLength(1);

            byte tc;
            byte tile;

            bool avoidFF = checkAvoidFF.Checked;

            for (j = 0; j < h; j++)
            {
                if (l >= bufferHeight) break;
                k = tx;

                for (i = 0; i < w; i++)
                {
                    if (k >= bufferWidth) break;

                    tc = getTile(k, l);
                    tile = pasteArea[i, j];

                    if ((tc == tile) | (avoidFF & (tile == 0xFF)))
                    {
                        k++;
                        continue;
                    }

                    uc = new UndoCoord()
                    {
                        X = k,
                        Y = l
                    };

                    currentUndo.coords = currentUndo.coords.Append(uc).ToArray();
                    currentUndo.tileNumber = currentUndo.tileNumber.Append(tc).ToArray();
                    currentUndo.redoNumber = currentUndo.redoNumber.Append(tile).ToArray();

                    setTile(k, l, tile, false);

                    k++;
                }

                l++;
            }

            pictureTrackDisplay.Invalidate();
        }

        // ----------------------------------------------------
        // Detect overlay tile. Returns -1 if no tile selected.
        // ----------------------------------------------------
        private int detectOverlay(int tx, int ty)
        {
            int i;
            overlayBlock ob = null;
            byte[,] sz = ovrState.sizes;

            for (i = ovrBlocks.Count - 1; i >= 0; i--)
            {
                ob = ovrBlocks[i];

                if (ob == null) continue;
                if (ob.enabled == false) continue;

                if ((tx >= ob.x) &
                    (tx < (ob.x + sz[ob.size, 0])) &
                    (ty >= ob.y) &
                    (ty < (ob.y + sz[ob.size, 1])))
                {
                    ovrRelX = tx - ob.x;
                    ovrRelY = ty - ob.y;

                    break;
                }
            }

            return i;
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Track mouse down
        // ----------------------------------------------------
        private void clickHandle(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            switch (mode)
            {
                case editorModes.TileEditor:
                    switch (me.Button)
                    {
                        case MouseButtons.Left:
                            if ((mouseMode & mouseModes.AnythingButLeft) == 0)
                            {
                                mouseMode |= mouseModes.LeftMouse;

                                currentUndo = new Undo
                                {
                                    coords = new UndoCoord[0],
                                    tileNumber = new byte[0],
                                    redoNumber = new byte[0]
                                };

                                placeTileWUndoAt(cursorx, cursory);

                                pictureTrackDisplay.Invalidate();
                            }

                            break;

                        case MouseButtons.Right:
                            if ((param & trackEditorParams.AllowCopying) == 0)
                            {
                                pasteArea[0, 0] = getTile(cursorx, cursory);
                                pictureTileSelect.Invalidate();
                            }
                            else
                            {
                                if ((mouseMode & mouseModes.AnythingButRight) == 0)
                                {
                                    mouseMode |= mouseModes.RightMouse;

                                    anchorx = cursorx;
                                    anchory = cursory;
                                }
                            }

                            break;
                    }
                    break;

                case editorModes.OverlayEditor:
                    switch (me.Button)
                    {
                        case MouseButtons.Left:
                            mouseMode |= mouseModes.LeftMouse;
                            selectedOvr = detectOverlay(cursorx, cursory);
                            if (selectedOvr != -1)
                            {
                                ovrRot = ovrBlocks[selectedOvr].entry % 4;
                                numericOverlayPattern.Value = ovrBlocks[selectedOvr].entry / 4;
                                numericOverlaySize.Value = ovrBlocks[selectedOvr].size;
                                updateOverlaySelect();
                            }
                            break;

                        case MouseButtons.Right:
                            mouseMode |= mouseModes.RightMouse;
                            selectedOvr = -1;
                            break;
                    }

                    pictureTrackDisplay.Invalidate();

                    break;

                case editorModes.PlaceOverlay:
                    switch (me.Button)
                    {
                        case MouseButtons.Left:
                            int entry = ((int)numericOverlayPattern.Value * 4) + ovrRot;
                            int size = (int)numericOverlaySize.Value;

                            if ((ovrBlocks.Count >= 41) | (!ovrPieces[entry].valid[size]))
                            {
                                SystemSounds.Asterisk.Play();
                                break;
                            }

                            overlayBlock ob = new overlayBlock
                            {
                                enabled = true,

                                x = cursorx,
                                y = cursory,
                                entry = entry,
                                size = size
                            };

                            ob.rect.X = ob.x * 8;
                            ob.rect.Y = ob.y * 8;
                            ob.rect.Width = ovrState.sizes[ob.size, 0] * 8;
                            ob.rect.Height = ovrState.sizes[ob.size, 1] * 8;

                            ovrBlocks.Add(ob);

                            ovrMod = true;

                            updateCapacityText();
                            pictureTrackDisplay.Invalidate();

                            break;

                        case MouseButtons.Right:
                            mode = editorModes.OverlayEditor;
                            pictureTrackDisplay.Invalidate();
                            break;
                    }
                    break;
            }
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Track mouse up
        // ----------------------------------------------------
        private void releaseHandle(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            switch (mode)
            {
                case editorModes.TileEditor:
                    switch (me.Button)
                    {
                        case MouseButtons.Left:
                            if ((mouseMode & mouseModes.LeftMouse) != 0)
                            {
                                mouseMode &= mouseModes.AnythingButLeft;

                                undoHistory = undoHistory.Append(currentUndo).ToList();
                                if (undoHistory.Count > 10) undoHistory.RemoveAt(0);
                                redoHistory.RemoveRange(0, redoHistory.Count);

                                if (modifyDepth < 11) modifyDepth++;
                            }

                            break;

                        case MouseButtons.Right:
                            if (((mouseMode & mouseModes.RightMouse) != 0) & ((param & trackEditorParams.AllowCopying) != 0))
                            {
                                mouseMode &= mouseModes.AnythingButRight;

                                int sx;
                                int sy;

                                int wid = Math.Abs(cursorx - anchorx) + 1;
                                int hei = Math.Abs(cursory - anchory) + 1;

                                int i;
                                int j;

                                if (cursorx < anchorx) sx = cursorx; else sx = anchorx;
                                if (cursory < anchory) sy = cursory; else sy = anchory;

                                pasteArea = new byte[wid, hei];

                                for (j = 0; j < hei; j++)
                                {
                                    for (i = 0; i < wid; i++)
                                    {
                                        pasteArea[i, j] = getTile(sx + i, sy + j);
                                    }
                                }

                                pictureTileSelect.Invalidate();
                                pictureTrackDisplay.Invalidate();
                            }

                            break;
                    }
                    break;

                case editorModes.OverlayEditor:
                    switch (me.Button)
                    {
                        case MouseButtons.Left:
                            if ((mouseMode & mouseModes.LeftMouse) != 0)
                            {
                                mouseMode &= mouseModes.AnythingButLeft;
                            }
                            if (selectedOvr != -1) ovrMod = true;
                            break;
                    }
                    break;
            }

        }

        // ----------------------------------------------------
        // EVENT HANDLER: Track mouse hover
        // ----------------------------------------------------
        private void mouseTick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e; // HOW!?!? HOW DOES THIS WORK!?!?

            int div = 8 * scale;
            cursorx = me.X / div;
            cursory = me.Y / div;

            if (cursorx < 0) cursorx = 0; else if (cursorx >= dataWidth) cursorx = dataWidth - 1;
            if (cursory < 0) cursory = 0; else if (cursory >= dataHeight) cursory = dataHeight - 1;

            switch (mode)
            {
                case editorModes.TileEditor:
                    if ((cursorx != cursorpx) | (cursory != cursorpy))
                    {
                        if ((mouseMode & mouseModes.LeftMouse) != 0) placeTileWUndoAt(cursorx, cursory);
                        pictureTrackDisplay.Invalidate();
                    }

                    break;

                case editorModes.OverlayEditor:
                    if ((cursorx != cursorpx) | (cursory != cursorpy))
                    {
                        if (((mouseMode & mouseModes.LeftMouse) != 0) & (selectedOvr != -1))
                        {
                            overlayBlock ob = ovrBlocks[selectedOvr];

                            int x = cursorx - ovrRelX;
                            int y = cursory - ovrRelY;
                            int lx = dataWidth - ovrState.sizes[ob.size, 0];
                            int ly = dataHeight - ovrState.sizes[ob.size, 1];

                            if (x < 0) x = 0; else if (x > lx) x = lx;
                            if (y < 0) y = 0; else if (y > ly) y = ly;

                            ob.x = x;
                            ob.y = y;

                            ob.rect.X = x * 8;
                            ob.rect.Y = y * 8;

                            pictureTrackDisplay.Invalidate();
                        }
                    }
                    break;

                case editorModes.PlaceOverlay:
                    int size = (int)numericOverlaySize.Value;

                    if ((cursorx + ovrState.sizes[size, 0]) >= dataWidth) cursorx = dataWidth - ovrState.sizes[size, 0];
                    if ((cursory + ovrState.sizes[size, 1]) >= dataHeight) cursory = dataHeight - ovrState.sizes[size, 1];

                    pictureTrackDisplay.Invalidate();

                    break;
            }

            cursorpx = cursorx;
            cursorpy = cursory;
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Redraw tile select
        // ----------------------------------------------------
        private void drawTileSelect(object sender, PaintEventArgs e)
        {
            if (pasteArea.Length == 1)
            {
                int tile = pasteArea[0,0];

                rectSelect.X = (tile % 8) * 16;
                rectSelect.Y = (tile / 8) * 16;

                e.Graphics.DrawRectangle(penCursor, rectSelect);
            }
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Click on tile select
        // ----------------------------------------------------
        private void clickTileSelect(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            int sx = me.X / 16;
            int sy = me.Y / 16;

            pasteArea = new byte[1, 1] {
                {
                    (byte)((8 * sy) + sx)
                }
            };

            pictureTileSelect.Invalidate();
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Draws overlying shapes on top of the track buffer
        // ----------------------------------------------------
        private void drawOverlay(object sender, PaintEventArgs e)
        {
            // e.Graphics.DrawImage(ovrBuffer, new PointF(0, 0));

            Graphics g = e.Graphics;

            int mul = 8 * scale;

            int wid;
            int hei;

            int cx;
            int cy;

            switch (mode)
            {
                case editorModes.TileEditor:

                    if ((mouseMode & mouseModes.RightMouse) != 0)
                    {
                        if (cursorx < anchorx) cx = cursorx; else cx = anchorx;
                        if (cursory < anchory) cy = cursory; else cy = anchory;

                        wid = Math.Abs(cursorx - anchorx) + 1;
                        hei = Math.Abs(cursory - anchory) + 1;
                    }
                    else
                    {
                        cx = cursorx;
                        cy = cursory;

                        wid = pasteArea.GetLength(0);
                        hei = pasteArea.GetLength(1);
                    }

                    rectCursor.X = cx * mul;
                    rectCursor.Y = cy * mul;
                    rectCursor.Width = wid * mul;
                    rectCursor.Height = hei * mul;

                    g.DrawRectangle(penCursor, rectCursor);
                    break;

                case editorModes.OverlayEditor:
                    if ((ovrState != null) & (ovrBlocks != null))
                    {
                        int i;
                        int l = ovrBlocks.Count;
                        overlayBlock ob;
                        Rectangle r;

                        for (i = 0; i < l; i++)
                        {
                            ob = ovrBlocks[i];
                            r = ob.rect;

                            tempRect.X = r.X * scale;
                            tempRect.Y = r.Y * scale;
                            tempRect.Width = r.Width * scale;
                            tempRect.Height = r.Height * scale;

                            g.DrawImage(ovrPieces[ob.entry].buffers[ob.size], tempRect);
                            if ((i == selectedOvr) & (mode == editorModes.OverlayEditor)) g.FillRectangle(brushSel, tempRect);
                        }

                        if (mode == editorModes.PlaceOverlay)
                        {
                            int sz = (int)numericOverlaySize.Value;

                            rectCursor.X = cursorx * mul;
                            rectCursor.Y = cursory * mul;
                            rectCursor.Width = ovrState.sizes[sz, 0] * mul;
                            rectCursor.Height = ovrState.sizes[sz, 1] * mul;

                            g.DrawRectangle(penCursor, rectCursor);
                        }
                    }
                    break;

                case editorModes.PlaceOverlay:
                    goto case editorModes.OverlayEditor;
            }
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Undo button clicked
        // ----------------------------------------------------
        private void buttonUndo_Click(object sender, EventArgs e)
        {
            if (undoHistory.Count == 0)
            {
                MessageBox.Show("can't undo :(");
                return;
            }

            Undo ud = undoHistory.Last();
            int i;
            
            for (i = ud.coords.Length - 1; i >= 0; i--)
            {
                setTile(ud.coords[i].X, ud.coords[i].Y, ud.tileNumber[i], false);
            }

            pictureTrackDisplay.Invalidate();

            redoHistory = redoHistory.Append(ud).ToList();
            undoHistory.RemoveAt(undoHistory.Count - 1);
            pictureTrackDisplay.Invalidate();

            modifyDepth--;
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Redo button clicked
        // ----------------------------------------------------
        private void buttonRedo_Click(object sender, EventArgs e)
        {
            if (redoHistory.Count == 0)
            {
                MessageBox.Show("can't redo :)");
                return;
            }

            Undo rd = redoHistory.Last();
            int i;
            int j = rd.coords.Length;

            for (i = 0; i < j; i++)
            {
                setTile(rd.coords[i].X, rd.coords[i].Y, rd.redoNumber[i], false);
            }

            pictureTrackDisplay.Invalidate();

            undoHistory = undoHistory.Append(rd).ToList();
            redoHistory.RemoveAt(redoHistory.Count - 1);
            pictureTrackDisplay.Invalidate();

            if (modifyDepth < 11) modifyDepth++;
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Hotkeys
        // ----------------------------------------------------
        private void handleHotkey(object sender, KeyPressEventArgs e)
        {
            char kc = e.KeyChar;
            // MessageBox.Show(((int)kc).ToString());

            switch (mode)
            {
                case editorModes.TileEditor:
                    switch (kc)
                    {
                        case '1':
                            if (pasteArea.Length == 1) pasteArea[0, 0]--;
                            else
                            {
                                pasteArea = new byte[1, 1];
                                pictureTrackDisplay.Invalidate();
                            }
                            pictureTileSelect.Invalidate();
                            break;

                        case '2':
                            if (pasteArea.Length == 1) pasteArea[0, 0]++;
                            else
                            {
                                pasteArea = new byte[1, 1];
                                pictureTrackDisplay.Invalidate();
                            }
                            pictureTileSelect.Invalidate();
                            break;
                    }
                    break;

                case editorModes.OverlayEditor:
                    switch (kc)
                    {
                        case (char)0x0008:
                            if (selectedOvr == -1) break;

                            ovrBlocks.RemoveAt(selectedOvr);
                            selectedOvr = -1;

                            updateCapacityText();
                            pictureTrackDisplay.Invalidate();

                            break;
                    }
                    break;
            }
        }

        // ----------------------------------------------------
        // Paste preview update
        // ----------------------------------------------------
        private void changePreview()
        {
            labelPasteNumber.Text = (pasteIndex + 1).ToString() + " / " + pasteStache.Count.ToString();

            picturePasteDisplay.Image = pasteStache[pasteIndex].preview;
            pasteSize = pasteStache[pasteIndex].preview.Size;

            picturePasteDisplay.Size = pasteSize;
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Scroll paste left
        // ----------------------------------------------------
        private void buttonPasteLeft_Click(object sender, EventArgs e)
        {
            pasteIndex--;
            if (pasteIndex < 0) pasteIndex = pasteStache.Count - 1;
            changePreview();
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Scroll paste right
        // ----------------------------------------------------
        private void buttonPasteRight_Click(object sender, EventArgs e)
        {
            pasteIndex = (pasteIndex + 1) % pasteStache.Count;
            changePreview();
        }

        // ----------------------------------------------------
        // Tick paste list
        // ----------------------------------------------------
        private void refreshPasteList()
        {
            bool enableFlag;

            if (pasteStache.Count > 0)
            {
                enableFlag = true;

                changePreview();
            }
            else
            {
                enableFlag = false;

                labelPasteNumber.Text = "0 / 0";

                pasteSize.Width = 1;
                pasteSize.Height = 1;

                picturePasteDisplay.Image = null;
                picturePasteDisplay.Size = pasteSize;
            }

            picturePasteDisplay.Invalidate();

            buttonDeletePaste.Enabled = enableFlag;
            buttonPasteLeft.Enabled = enableFlag;
            buttonPasteRight.Enabled = enableFlag;
            buttonUsePaste.Enabled = enableFlag;
            buttonEditPaste.Enabled = enableFlag;
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Stache paste
        // ----------------------------------------------------
        private void buttonStashPaste_Click(object sender, EventArgs e)
        {
            Paste mypaste = new Paste()
            {
                tile = pasteArea
            };

            mypaste.preview = initBuffer(mypaste.tile, mypaste.preview, palette, theme, true);

            pasteIndex = pasteStache.Count; // no need to sbc 1 before storing
            pasteStache.Add(mypaste);

            refreshPasteList();
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Delete paste
        // ----------------------------------------------------
        private void buttonDeletePaste_Click(object sender, EventArgs e)
        {
#if disableDeleteWarning
            pasteStache.RemoveAt(pasteIndex);
            if (pasteIndex >= pasteStache.Count) pasteIndex--;

            refreshPasteList();
#else
            System.Media.SystemSounds.Asterisk.Play();
            if (MessageBox.Show("Are you sure you want to permanently delete this paste?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                pasteStache.RemoveAt(pasteIndex);
                if (pasteIndex >= pasteStache.Count) pasteIndex--;

                refreshPasteList();
            }
#endif
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Use paste
        // ----------------------------------------------------
        private void buttonUsePaste_Click(object sender, EventArgs e)
        {
            pasteArea = pasteStache[pasteIndex].tile;

            pictureTrackDisplay.Invalidate();
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Edit paste
        // ----------------------------------------------------
        private void buttonEditPaste_Click(object sender, EventArgs e)
        {
            /*
            try
            {
                SoundPlayer sp = new SoundPlayer
                {
                    SoundLocation = "C:\\Windows\\Media\\tada.wav"
                };
                sp.Play();
            }
            catch
            {
                SystemSounds.Asterisk.Play();
            }
            MessageBox.Show("POP goes the unimplemented function!", "Icesan Meditation Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            */

            byte[,] tmap = copyBytes(pasteStache[pasteIndex].tile);

            TM_CourseEditor myce = new TM_CourseEditor(tmap, theme, palette, tmap.GetLength(0), tmap.GetLength(1), (trackEditorParams.ReEnableOwner | trackEditorParams.TransferPaste | trackEditorParams.AllowCopying), this);
            myce.Show();
            Enabled = false;
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Close event
        // ----------------------------------------------------
        private void disposeHandle(object sender, EventArgs e)
        {
            SystemSounds.Question.Play();
            DialogResult dr;

            if (modifyDepth == 0)
            {
                dr = DialogResult.No;
            }
            else
            {
                if ((param & trackEditorParams.TransferPaste) != 0)
                    dr = MessageBox.Show("Save changes to paste?", "Close", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                else
                    dr = MessageBox.Show("Save changes to track?", "Close", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            }


            if (dr == DialogResult.Cancel)
            {
                FormClosingEventArgs ce = (FormClosingEventArgs)e;
                ce.Cancel = true;
            }
            else
            {
                if (parentForm != null)
                {
                    if ((param & trackEditorParams.ReEnableOwner) != 0)
                    {
                        parentForm.Enabled = true;
                        parentForm.BringToFront();
                    }
                }

                if (dr == DialogResult.Yes) saveLoc = saveChanges(saveLoc, autoCompressSettings.AskBeforeCompress);
            }
        }

        // ----------------------------------------------------
        // Save dialog creator
        // ----------------------------------------------------
        private (string, DialogResult) makeSaveDialog(string additionalFilters)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                InitialDirectory = Application.StartupPath,
                Filter = "All Files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (additionalFilters != null)
            {
                sfd.Filter += '|' + additionalFilters;
            }

            (string, DialogResult) output;
            output.Item1 = null;
            output.Item2 = sfd.ShowDialog();

            if (output.Item2 == DialogResult.OK)
            {
                output.Item1 = sfd.FileName;
            }

            return output;
        }

        // ----------------------------------------------------
        // Save functionality
        // ----------------------------------------------------
        private string saveChanges(string filename, autoCompressSettings acs)
        {
            if ((param & trackEditorParams.TransferPaste) == 0)
            {
                if (filename == null)
                {
                    (string, DialogResult) sd = makeSaveDialog("Mode 7 Screens (*.SCR)|*.SCR");
                    if (sd.Item2 == DialogResult.Cancel) return filename;
                    filename = sd.Item1;
                }

                modifyDepth = 0;
                if (undoHistory.Count > 0) undoHistory.RemoveRange(0, undoHistory.Count);
                if (redoHistory.Count > 0) redoHistory.RemoveRange(0, redoHistory.Count);

                writeLayoutToFile(filename, acs);
            }
            else
            {
                if (receiverForm != null)
                {
                    receiverForm.receiveTiles(layout, receiveMode.CopyToPaste);
                    modifyDepth = 0;
                    Close();
                }
            }

            return filename;
        }

        // ----------------------------------------------------
        // Encode 2D array into 1D array
        // ----------------------------------------------------
        private byte[] convert2dTo1d(byte[,] input)
        {
            byte[] output = new byte[input.Length];

            int i;
            int j;
            int k = 0;

            int w = input.GetLength(0);
            int h = input.GetLength(1);

            for (j = 0; j < h; j++)
            {
                for (i = 0; i < w; i++)
                {
                    output[k] = input[i, j];
                    k++;
                }
            }

            return output;
        }

        // ----------------------------------------------------
        // Replace extension
        // ----------------------------------------------------
        private string replaceExtension(string instr, string extension)
        {
            int trim;

            for (trim = instr.Length - 1; trim >= 0; trim--)
            {
                if (instr[trim] == '.') break;
            }

            if (trim >= 0) instr = instr.Substring(0, trim) + extension; else instr += '.' + extension;

            return instr;
        }

        // ----------------------------------------------------
        // Directly write 1D layout to file
        // ----------------------------------------------------
        private void writeLayoutToFile(string file, autoCompressSettings settings)
        {
            byte[] trackRaw = convert2dTo1d(layout);
            if (settings != autoCompressSettings.OnlyCompress) Files.Write(file, trackRaw, false);
            if (settings != autoCompressSettings.DontCompress)
            {
                string compExt;

                if (settings == autoCompressSettings.OnlyCompress) compExt = file; else compExt = replaceExtension(file, ".sss"); ;

                if (settings == autoCompressSettings.AskBeforeCompress)
                {
                    if (MessageBox.Show("Compress to "+compExt+"?", "Save", MessageBoxButtons.YesNo) == DialogResult.No) return;
                }

                (byte[], bool) trackComp = Files.DoubleCompress(trackRaw);

                if (trackComp.Item2) Files.Write(compExt, trackComp.Item1, false);
            }
        }

        // ----------------------------------------------------
        // Receiver function for tile layouts from child windows
        // ----------------------------------------------------
        public void receiveTiles(byte[,] tile, receiveMode mode)
        {
            byte[,] myTile = copyBytes(tile);

            switch (mode)
            {
                case receiveMode.CopyToPaste:
                    Paste myPaste = new Paste
                    {
                        tile = myTile
                    };
                    myPaste.preview = initBuffer(myPaste.tile, myPaste.preview, palette, theme, true);

                    pasteStache[pasteIndex] = myPaste;
                    refreshPasteList();
                    buttonUsePaste_Click(null, null);

                    break;
            }
        }

        // ----------------------------------------------------
        // Save Overlay
        // ----------------------------------------------------
        public void saveOverlay()
        {
            byte[] ovrEnc = Files.setuparray(128);
            overlayBlock ob;

            int i;
            int j = 0;

            for (i = 0; i < ovrBlocks.Count; i++)
            {
                ob = ovrBlocks[i];

                ovrEnc[j] = (byte)((ob.entry % 64) + (ob.size * 64));
                ovrEnc[j + 1] = (byte)(ob.x + (128 * (ob.y % 2)));
                ovrEnc[j + 2] = (byte)(ob.y / 2);

                j += 3;
            }

            Files.Insert(ovrState.writePath, ovrEnc, ovrState.writeAt);
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Advance saving menu select
        // ----------------------------------------------------
        private void saveMenuClick(object sender, EventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            (string, DialogResult) sfr;

            switch (mi.Index)
            {
                case 0:
                    saveLoc = saveChanges(saveLoc, autoCompressSettings.AskBeforeCompress);
                    if (ovrSave & ovrMod) saveOverlay();
                    break;

                case 1:
                    sfr = makeSaveDialog("Mode 7 Screens (*.SCR)|*.SCR");
                    if (sfr.Item2 == DialogResult.OK)
                    {
                        saveLoc = saveChanges(sfr.Item1, autoCompressSettings.AskBeforeCompress);
                        if (ovrSave & ovrMod) saveOverlay();
                    }
                    break;

                case 2:
                    saveLoc = saveChanges(saveLoc, autoCompressSettings.DontCompress);
                    if (ovrSave & ovrMod) saveOverlay();
                    break;

                case 3:
                    sfr = makeSaveDialog("Mode 7 Screens (*.SCR)|*.SCR");
                    if (sfr.Item2 == DialogResult.OK)
                    {
                        saveLoc = saveChanges(sfr.Item1, autoCompressSettings.DontCompress);
                        if (ovrSave & ovrMod) saveOverlay();
                    }
                    break;

                case 4:
                    sfr = makeSaveDialog("Compressed M7 Screens (*.sss)|*.sss");
                    if (sfr.Item2 == DialogResult.OK)
                    {
                        writeLayoutToFile(sfr.Item1, autoCompressSettings.OnlyCompress);
                        if (ovrSave & ovrMod) saveOverlay();
                    }
                    break;

                case 5:
                    try
                    {
                        sfr = makeSaveDialog("Portable Network Graphics (*.png)|*.png");

                        if (sfr.Item2 == DialogResult.OK) trkBuffer.Save(sfr.Item1);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while saving track bitmap.\n" + ex.Message);
                    }
                    break;

                case 6:
                    Clipboard.SetDataObject(trkBuffer);
                    break;

                case 7:
                    ovrSave = !ovrSave;
                    break;
            }
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Save button
        // ----------------------------------------------------
        private void buttonSave_Click(object sender, EventArgs e)
        {
            saveLoc = saveChanges(saveLoc, autoCompressSettings.AskBeforeCompress);
            if (ovrSave & ovrMod) saveOverlay();
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Advance saving button
        // ----------------------------------------------------
        private void buttonSaveAdv_Click(object sender, EventArgs e)
        {
            if ((param & trackEditorParams.TransferPaste) == 0)
            {
                MenuItem[] mi = new MenuItem[] {
                            new MenuItem("Save Layout and Compress (Default)", saveMenuClick),
                            new MenuItem("Save Layout As and Compress...", saveMenuClick),
                            new MenuItem("Save Layout Without Compressing", saveMenuClick),
                            new MenuItem("Save Layout As Without Compressing...", saveMenuClick),
                            new MenuItem("Compress Layout to...", saveMenuClick),
                            new MenuItem("Save Track Image As...", saveMenuClick),
                            new MenuItem("Copy Track Image to Clipboard", saveMenuClick),
                            new MenuItem("Save Overlay When Modified", saveMenuClick)
                        };

                mi[7].Checked = ovrSave;

                ContextMenu scm = new ContextMenu(mi);
                scm.Show(buttonSave, ((Button)sender).Location);
            }
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Update Overlay Display
        // ----------------------------------------------------
        private void updateOverlaySelect()
        {
            Bitmap bm = ovrPieces[((int)numericOverlayPattern.Value * 4) + ovrRot].buffers[(int)numericOverlaySize.Value];
            pictureOvrTilePreview.Image = bm;
            pictureOvrTilePreview.Width = bm.Width * 2;
            pictureOvrTilePreview.Height = bm.Height * 2;
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Overlay Change Pattern
        // ----------------------------------------------------
        private void numericOverlayPattern_ValueChanged(object sender, EventArgs e)
        {
            int c = ((int)numericOverlayPattern.Value * 4) + ovrRot;
            int l = ovrState.pieces[c].Length;
            int a;

            int i;

            for (i = 0; i < ovrState.sizes.GetLength(0); i++)
            {
                a = ovrState.sizes[i, 0] * ovrState.sizes[i, 1];
                if (a == l)
                {
                    numericOverlaySize.Value = i;
                    break;
                }
            }
            updateOverlaySelect();
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Overlay Change Size
        // ----------------------------------------------------
        private void numericOverlaySize_ValueChanged(object sender, EventArgs e)
        {
            updateOverlaySelect();
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Tab Change
        // ----------------------------------------------------
        private void tabModeChange(object sender, EventArgs e)
        {
            TabControl tc = (TabControl)sender;

            mouseMode = mouseModes.NoMouse;

            switch (tc.SelectedIndex)
            {
                case 0:
                    mode = editorModes.TileEditor;
                    pictureTrackDisplay.Invalidate();
                    break;
                case 1:
                    mode = editorModes.TileEditor;
                    pictureTrackDisplay.Invalidate();
                    break;
                case 2:
                    mode = editorModes.OverlayEditor;
                    pictureTrackDisplay.Invalidate();
                    break;
            }
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Rotate Overlay Counter Clockwise
        // ----------------------------------------------------
        private void buttonRotateCounterClockwise_Click(object sender, EventArgs e)
        {
            ovrRot--;
            if (ovrRot < 0) ovrRot = 3;
            updateOverlaySelect();
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Rotate Overlay Clockwise
        // ----------------------------------------------------
        private void buttonRotateClockwise_Click(object sender, EventArgs e)
        {
            ovrRot++;
            ovrRot %= 4;
            updateOverlaySelect();
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Reset Overlay Rotation
        // ----------------------------------------------------
        private void buttonResetRotation_Click(object sender, EventArgs e)
        {
            ovrRot = 0;
            updateOverlaySelect();
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Change Overlay Block
        // ----------------------------------------------------
        private void buttonOverlayChange_Click(object sender, EventArgs e)
        {
            if (selectedOvr != -1)
            {
                overlayBlock ob = ovrBlocks[selectedOvr];
                Rectangle or = ob.rect;
                byte[,] sz = ovrState.sizes;

                int a = ((int)numericOverlayPattern.Value * 4) + ovrRot;
                int b = (int)numericOverlaySize.Value;

                if (!ovrPieces[a].valid[b])
                {
                    SystemSounds.Asterisk.Play();
                    return;
                }

                ob.entry = a;
                // ob.size = (int)numericOverlaySize.Value;

                if (ob.size != b)
                {
                    ob.size = b;
                    if ((ob.x + sz[ob.size, 0]) >= dataWidth)
                    {
                        ob.x = dataWidth - sz[ob.size, 0];
                        or.X = ob.x * 8;
                    }
                    if ((ob.y + sz[ob.size, 1]) >= dataHeight)
                    {
                        ob.y = dataHeight - sz[ob.size, 1];
                        or.Y = ob.y * 8;
                    }

                    or.Width = sz[ob.size, 0] * 8;
                    or.Height = sz[ob.size, 1] * 8;

                    ob.rect = or;
                }

                ovrMod = true;

                pictureTrackDisplay.Invalidate();
            }
        }

        // ----------------------------------------------------
        // EVENT HANDLER: Place Overlay Block
        // ----------------------------------------------------
        private void buttonPlaceOverlay_Click(object sender, EventArgs e)
        {
            mouseMode = mouseModes.NoMouse;
            mode = editorModes.PlaceOverlay;
            pictureTrackDisplay.Invalidate();
        }
    }
}
