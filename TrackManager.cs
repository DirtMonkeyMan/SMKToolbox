using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SMKToolbox.TileDrawing;

#pragma warning disable IDE1006

namespace SMKToolbox
{
    public partial class TrackManager : Form
    {
        public TrackManager()
        {
            InitializeComponent();
        }

        /*
        --------------------------------------------------
        struct course
        --------------------------------------------------
        Temporarily unused.
        */
        public struct course
        {
            public string[] name;
            public trackTheme2d theme;
        }


        /*
        --------------------------------------------------
        struct trackManFile
        --------------------------------------------------
        Temporarily unused.
        */
        public struct trackManFile
        {

        }

        // ------------------------------------------------
        // LOAD FUNCTION
        // ------------------------------------------------
        public (bool, (string, Color)[]) LoadTrack(string screenPath, string graphPath, string palPath, string comPath)
        {
            bool pass = true;

            Color colPass = Color.FromArgb(0, 128, 0);
            Color colErr = Color.FromArgb(255, 0, 0);

            (string, Color)[] errinfo = { };

            int tileLimit;



            // --------------------------------------------
            // LoadTrack(): Load Mode 7 Screen
            // --------------------------------------------
            (byte[], bool) m7scr = Files.Read(screenPath);

            if (m7scr.Item2)
            {
                // File load OK.
                if (m7scr.Item1.Length == 16384)
                {
                    // File verify OK.
                    errinfo = errinfo.Append(("Successfully loaded and verified.", colPass)).ToArray();
                }
                else
                {
                    // File verify error.
                    pass = false;
                    errinfo = errinfo.Append(("Mode 7 screen is of incorrect length. Must be 16,384 bytes long, and must not be compressed.", colErr)).ToArray();
                }
            }
            else
            {
                // File load error.
                pass = false;
                errinfo = errinfo.Append(("Error loading SCR screen.", colErr)).ToArray();
            }



            // --------------------------------------------
            // LoadTrack(): Load Mode 7 Graphics
            // --------------------------------------------
            (byte[], bool) m7grp = Files.Read(graphPath);

            if (m7grp.Item2)
            {
                // File load OK.
                if ((m7grp.Item1.Length < 256) | ((m7grp.Item1.Length % 32) != 0))
                {
                    // File verify error.
                    pass = false;
                    errinfo = errinfo.Append(("Mode 7 graphics is of incorrect length. Look for the CM7 file associated with the track's theme.", colErr)).ToArray();
                }
                else
                {
                    // File verify OK.
                    errinfo = errinfo.Append(("Successfully loaded and verified.", colPass)).ToArray();
                }
            }
            else
            {
                // File load error.
                pass = false;
                errinfo = errinfo.Append(("Error loading CM7 graphics.", colErr)).ToArray();
            }



            // --------------------------------------------
            // LoadTrack(): Load Color Palette
            // --------------------------------------------
            (byte[], bool) colpal = Files.Read(palPath);

            if (colpal.Item2)
            {
                // File load OK.
                if (colpal.Item1.Length == 512)
                {
                    // File verify OK.
                    errinfo = errinfo.Append(("Successfully loaded and verified.", colPass)).ToArray();
                }
                else
                {
                    // File verify error.
                    pass = false;
                    errinfo = errinfo.Append(("Color palette file is of incorrect length. This file must be 512 bytes long.", colErr)).ToArray();
                }
            }
            else
            {
                // File load error.
                pass = false;
                errinfo = errinfo.Append(("Error loading color palettes.", colErr)).ToArray();
            }



            // --------------------------------------------
            // LoadTrack(): Load Common Tile Graphics
            // --------------------------------------------
            (byte[], bool) commonGrp = (new byte[0], false);

            if (comPath != null)
            {
                // Load the file.
                tileLimit = 0xC0;

                commonGrp = Files.Read(comPath);

                if (commonGrp.Item2)
                {
                    if ((commonGrp.Item1.Length < 256) | ((commonGrp.Item1.Length % 32) != 0))
                    {
                        pass = false;
                        errinfo = errinfo.Append(("Common Tile graphics is of incorrect length. Look for the CM7 file associated with the graphics.", colErr)).ToArray();
                    }
                    else
                    {
                        errinfo = errinfo.Append(("Successfully loaded and verified.", colPass)).ToArray();
                    }
                }
                else
                {
                    pass = false;
                    errinfo = errinfo.Append(("Error loading common tile graphics.", colErr)).ToArray();
                }
            }
            else
            {
                // Skip loading file.
                tileLimit = 0x100;

                errinfo = errinfo.Append(("Loading skipped.", Color.FromArgb(0, 128, 255))).ToArray();
            }



            if (pass)
            {
                int i;
                int j;
                int k = 0;

                byte[,] m72d = new byte[128, 128];

                for (j = 0; j < 128; j++)
                {
                    for (i = 0; i < 128; i++)
                    {
                        m72d[i, j] = m7scr.Item1[k];
                        k++;
                    }
                }

                k = 0x100;

                graphicTile4bpp tile4bpp;
                graphicTile8bpp2d tile8bpp;
                trackTheme2d theme;

                tile4bpp = make4bppTile(0, true);
                theme.tile = new graphicTile8bpp2d[256];

                // Common Graphics
                if (commonGrp.Item2)
                {
                    for (i = 0; i < 0x40; i++)
                    {
                        tile4bpp.palette = commonGrp.Item1[i];

                        for (j = 0; j < 32; j++)
                        {
                            tile4bpp.graphics[j] = commonGrp.Item1[k];
                            k++;
                        }

                        tile8bpp = convert4bppTo8bpp2d(tile4bpp);
                        theme.tile[i + 0xC0] = tile8bpp;

                        if (k >= commonGrp.Item1.Length)
                        {
                            i++;
                            break;
                        }
                    }
                    while (i < 0x40)
                    {
                        theme.tile[i + 0xC0] = make8bppTile2d();
                        i++;
                    }
                }

                k = 0x100;

                // Rare Graphics
                for (i = 0; i < tileLimit; i++)
                {
                    tile4bpp.palette = m7grp.Item1[i];

                    for (j = 0; j < 32; j++)
                    {
                        tile4bpp.graphics[j] = m7grp.Item1[k];
                        k++;
                    }

                    tile8bpp = convert4bppTo8bpp2d(tile4bpp);
                    theme.tile[i] = tile8bpp;

                    if (k >= m7grp.Item1.Length)
                    {
                        i++;
                        break;
                    }
                }
                while (i < tileLimit)
                {
                    theme.tile[i] = make8bppTile2d();
                    i++;
                }

                Color[] pal = convertAllARGB(colpal.Item1);

                TM_CourseEditor ce = new TM_CourseEditor(m72d, theme, pal, 128, 128, (TM_CourseEditor.trackEditorParams.AllowCopying | TM_CourseEditor.trackEditorParams.AllowPasteStashing | TM_CourseEditor.trackEditorParams.ReEnableOwner), this);
                ce.Show();

                Enabled = false;

                return (true, errinfo);
            }
            else
            {
                return (false, errinfo);
            }
        }

        // ------------------------------------------------
        // CONTROL INTERACTION
        // ------------------------------------------------
        private void TrackManager_Load(object sender, EventArgs e)
        {
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TM_NewCatalog newCat = new TM_NewCatalog();
            newCat.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Awesome Import\nPart of an SMKToolbox breakfast!\n\nCode by Icesan/ice3\nSMK Toolbox base by Dirtbag\n\nSpecial thanks to:\nR4M0N\nMrL314\nScouB\n\nShoutouts to SMK Workshop!!","About Awesome Import",MessageBoxButtons.OK);
        }

        private void openIndividualTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TM_OpenDirect openDir = new TM_OpenDirect(this);
            openDir.Show();
        }

        private void openMarioCircuit3BecauseIcesanIsTooDamnLazyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadTrack("C:\\ProjectL_Mirror\\Source\\Assets\\mode7\\scr\\circuit\\circuit01.SCR", "C:\\ProjectL_Mirror\\Source\\Assets\\mode7\\chr\\circuit.CM7", "C:\\ProjectL_Mirror\\Source\\Assets\\color\\circuit.COL", "C:\\ProjectL_Mirror\\Source\\Assets\\mode7\\chr\\item.CM7");
        }
    }
}
