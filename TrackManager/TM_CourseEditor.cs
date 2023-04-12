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
using System.Media;

#pragma warning disable IDE1006

namespace SMKToolbox
{
    public partial class TM_CourseEditor : Form
    {
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
        private uint mouseMode = 0;
        private int anchorx = 0;
        private int anchory = 0;
        private MouseButtons currentButtons = 0;
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

        /*
        private Color[] convertAllARGB(byte[] pal)
        {
            Color[] col = new Color[256];
            int i;
            int j = 0;
            int colWord;

            int r;
            int g;
            int b;

            for (i = 0; i < 256; i++)
            {
                colWord = pal[j] + (256 * pal[j+1]);

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
        */

        /*
        private byte[,] convert2DMap(byte[] linearStream, int width, int height)
        {
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
        */

        private void setTile(int x, int y, byte tile, bool invalid)
        {
            if ((x >= 0) & (x < dataWidth) & (y >= 0) & (y < dataHeight))
            {
                layout[x, y] = tile;

                drawTileToBuffer(x, y, layout, palette, theme, pictureTrackDisplay, invalid);
            }
        }

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

        private void resizeTrack()
        {
            bool timeEnable = vstimer.Enabled;
            vstimer.Enabled = false;

            pictureTrackDisplay.Width = bufferWidth * scale;
            pictureTrackDisplay.Height = bufferHeight * scale;

            pictureTrackDisplay.Invalidate();

            vstimer.Enabled = timeEnable;
        }

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

        private void resizeHandle(object sender, EventArgs e)
        {
            resizeAssets();
        }

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

        /*
        private Bitmap[] makeBrushMaps()
        {
            Bitmap[] bm = new Bitmap[256];
            Bitmap cbm;
            Color col;
            int i;
            int j;
            int k;

            for (i = 0; i < 256; i++)
            {
                cbm = new Bitmap(8, 8);

                for (k = 0; k < 8; k++)
                {
                    for (j = 0; j < 8; j++)
                    {
                        col = Color.FromArgb(128, palette[theme.tile[i].graphics[j, k]]);
                        cbm.SetPixel(j, k, col);
                    }
                }

                bm[i] = cbm;
            }

            return bm;
        }

        private TextureBrush[] makeBrushes(Bitmap[] bm)
        {
            TextureBrush[] tb = new TextureBrush[256];
            int i;

            for (i = 0; i < 256; i++)
            {
                tb[i] = new TextureBrush(bm[i]);
            }

            return tb;
        }
        */

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

            if ((param & trackEditorParams.TransferPaste) != 0) buttonSave.Text = "Save && Close";

            if (!(((param & trackEditorParams.AllowCopying) != 0) & ((param & trackEditorParams.AllowPasteStashing) != 0)))
            {
                tabPieces.Dispose();
            }

            pictureTrackDisplay.Image = trkBuffer;
            picturePasteDisplay.Size = pasteSize;

            trkBuffer = initBuffer(layout, trkBuffer, palette, theme);

            byte[,] tilesel = new byte[8, 32];
            initSelect(tilesel);
            selBuffer = initBuffer(tilesel, selBuffer, palette, theme);

            pictureTileSelect.Image = selBuffer;
            pictureTileSelect.Invalidate();

            // tileBrushesBM = makeBrushMaps();
            // tileBrushes = makeBrushes(tileBrushesBM);

            vstimer = new Timer
            {
                Interval = (int)((double)100 / 6)   // 50hz countries beware
            };
            vstimer.Tick += new EventHandler(tileScrambler);
            // vstimer.Enabled = true;  // enable at your own risk -- it'll sprinkle your work of art

            resizeAssets();
            resizeTrack();
        }

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

        public TM_CourseEditor(byte[,] useTrack, trackTheme2d useTheme, Color[] usePalette, int width, int height, trackEditorParams parameters, Form owner, string savePath)
        {
            if ((width < 1) | (height < 1))
            {
                throw new Exception("Cannot make an editor with invalid width or height.");
            }

            parentForm = owner;

            realInit(useTrack, useTheme, usePalette, width, height, parameters);
        }

        private void buttonZoomIn_Click(object sender, EventArgs e)
        {
            if (scale < 4)
            {
                scale++;

                resizeAssets();
                resizeTrack();
            }
        }

        private void buttonZoomOut_Click(object sender, EventArgs e)
        {
            if (scale > 1)
            {
                scale--;

                resizeAssets();
                resizeTrack();
            }
        }

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

        private void clickHandle(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            currentButtons |= me.Button;

            switch (me.Button)
            {
                case MouseButtons.Left:
                    if ((mouseMode & 0xFE) == 0)
                    {
                        mouseMode |= 0x01;

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
                        if ((mouseMode & 0xFD) == 0)
                        {
                            mouseMode |= 0x02;

                            anchorx = cursorx;
                            anchory = cursory;
                        }
                    }

                    break;
            }
        }

        private void releaseHandle(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

            switch (me.Button)
            {
                case MouseButtons.Left:
                    if ((mouseMode & 0x01) != 0)
                    {
                        mouseMode &= 0xFE;

                        currentButtons = (MouseButtons)((uint)currentButtons & 0xFFEFFFFF);

                        undoHistory = undoHistory.Append(currentUndo).ToList();
                        if (undoHistory.Count > 10) undoHistory.RemoveAt(0);
                        redoHistory.RemoveRange(0, redoHistory.Count);

                        if (modifyDepth < 11) modifyDepth++;
                    }

                    break;

                case MouseButtons.Right:
                    if (((mouseMode & 0x02) != 0) & ((param & trackEditorParams.AllowCopying) != 0))
                    {
                        mouseMode &= 0xFD;

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
        }

        private void mouseTick(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e; // HOW!?!? HOW DOES THIS WORK!?!?

            int div = 8 * scale;
            cursorx = me.X / div;
            cursory = me.Y / div;

            if (cursorx < 0) cursorx = 0; else if (cursorx >= dataWidth) cursorx = dataWidth - 1;
            if (cursory < 0) cursory = 0; else if (cursory >= dataHeight) cursory = dataHeight - 1;

            if ((cursorx != cursorpx) | (cursory != cursorpy))
            {
                if ((mouseMode & 0x01) != 0) placeTileWUndoAt(cursorx, cursory);
                pictureTrackDisplay.Invalidate();
            }

            cursorpx = cursorx;
            cursorpy = cursory;
        }

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

        private void drawOverlay(object sender, PaintEventArgs e)
        {
            // e.Graphics.DrawImage(ovrBuffer, new PointF(0, 0));
            int mul = 8 * scale;

            int wid;
            int hei;

            int cx;
            int cy;

            if ((mouseMode & 0x02) != 0)
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

            // e.Graphics.FillRectangle(tileBrushes[tileSel], rectCursor);
            e.Graphics.DrawRectangle(penCursor, rectCursor);
        }

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

        private void handleHotkey(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar.ToString()) {
                case "1":
                    if (pasteArea.Length == 1)
                    {
                        pasteArea[0, 0]--;
                    }
                    else
                    {
                        pasteArea = new byte[1, 1];
                        pictureTrackDisplay.Invalidate();
                    }
                    pictureTileSelect.Invalidate();
                    break;

                case "2":
                    if (pasteArea.Length == 1)
                    {
                        pasteArea[0, 0]++;
                    }
                    else
                    {
                        pasteArea = new byte[1, 1];
                        pictureTrackDisplay.Invalidate();
                    }
                    pictureTileSelect.Invalidate();
                    break;
            }
        }

        private void changePreview()
        {
            labelPasteNumber.Text = (pasteIndex + 1).ToString() + " / " + pasteStache.Count.ToString();

            picturePasteDisplay.Image = pasteStache[pasteIndex].preview;
            pasteSize = pasteStache[pasteIndex].preview.Size;

            picturePasteDisplay.Size = pasteSize;
        }

        private void buttonPasteLeft_Click(object sender, EventArgs e)
        {
            pasteIndex--;
            if (pasteIndex < 0) pasteIndex = pasteStache.Count - 1;
            changePreview();
        }

        private void buttonPasteRight_Click(object sender, EventArgs e)
        {
            pasteIndex = (pasteIndex + 1) % pasteStache.Count;
            changePreview();
        }

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

        private void buttonStashPaste_Click(object sender, EventArgs e)
        {
            Paste mypaste = new Paste()
            {
                tile = pasteArea
            };

            mypaste.preview = initBuffer(mypaste.tile, mypaste.preview, palette, theme);

            pasteIndex = pasteStache.Count; // no need to sbc 1 before storing
            pasteStache.Add(mypaste);

            refreshPasteList();
        }

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

        private void buttonUsePaste_Click(object sender, EventArgs e)
        {
            pasteArea = pasteStache[pasteIndex].tile;

            pictureTrackDisplay.Invalidate();
        }

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
                    myPaste.preview = initBuffer(myPaste.tile, myPaste.preview, palette, theme);

                    pasteStache[pasteIndex] = myPaste;
                    refreshPasteList();
                    buttonUsePaste_Click(null, null);

                    break;
            }
        }

        private void saveMenuClick(object sender, EventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            (string, DialogResult) sfr;

            switch (mi.Index)
            {
                case 0:
                    saveLoc = saveChanges(saveLoc, autoCompressSettings.AskBeforeCompress);
                    break;

                case 1:
                    sfr = makeSaveDialog("Mode 7 Screens (*.SCR)|*.SCR");
                    if (sfr.Item2 == DialogResult.OK) saveLoc = saveChanges(sfr.Item1, autoCompressSettings.AskBeforeCompress);
                    break;

                case 2:
                    saveLoc = saveChanges(saveLoc, autoCompressSettings.DontCompress);
                    break;

                case 3:
                    sfr = makeSaveDialog("Mode 7 Screens (*.SCR)|*.SCR");
                    if (sfr.Item2 == DialogResult.OK) saveLoc = saveChanges(sfr.Item1, autoCompressSettings.DontCompress);
                    break;

                case 4:
                    sfr = makeSaveDialog("Compressed M7 Screens (*.sss)|*.sss");
                    if (sfr.Item2 == DialogResult.OK) writeLayoutToFile(sfr.Item1, autoCompressSettings.OnlyCompress);
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
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            switch (me.Button)
            {
                case MouseButtons.Left:
                    saveLoc = saveChanges(saveLoc, autoCompressSettings.AskBeforeCompress);
                    break;
                case MouseButtons.Right:
                    if ((param & trackEditorParams.TransferPaste) == 0)
                    {
                        MenuItem[] mi = new MenuItem[] {
                            new MenuItem("Save and Compress (Default)"),
                            new MenuItem("Save As and Compress..."),
                            new MenuItem("Save Without Compressing"),
                            new MenuItem("Save As Without Compressing..."),
                            new MenuItem("Compress to..."),
                            new MenuItem("Save Track Image As..."),
                            new MenuItem("Copy Track Image to Clipboard")
                        };

                        int i;
                        for (i = 0; i < mi.Length; i++)
                        {
                            mi[i].Click += new EventHandler(saveMenuClick);
                        }

                        ContextMenu scm = new ContextMenu(mi);
                        scm.Show(buttonSave, me.Location);
                    }
                    break;
            }
        }
    }
}
