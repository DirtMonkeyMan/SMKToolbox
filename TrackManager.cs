using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static SMKToolbox.TileDrawing;
using static SMKToolbox.Overlay;
using static SMKToolbox.ASMReading;

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

        public struct trackManLoadError
        {
            public int module;
            public string errText;
            public Color errColor;
        }

        private void addError(trackManLoadError[] errorContainer, int module, string message)
        {
            errorContainer = errorContainer.Append(new trackManLoadError { module = module, errText = message, errColor = Color.Red }).ToArray();
        }

        // ------------------------------------------------
        // LOAD FUNCTION
        // ------------------------------------------------
        public (bool, trackManLoadError[]) LoadTrack(string screenPath, string graphPath, string palPath, string comPath, overlaySetup ovs)
        {
            bool pass = true;

            trackManLoadError[] errinfo = { };

            int tileLimit;



            #region Load Mode 7 Screen
            // --------------------------------------------
            // LoadTrack(): Load Mode 7 Screen
            // --------------------------------------------
            // (byte[], bool) m7scr = Files.Read(screenPath);
            byte[] m7scr = null;

            try
            {
                m7scr = File.ReadAllBytes(screenPath);

                // File load OK.
                if (m7scr.Length != 16384)
                {
                    // File verify error.
                    pass = false;
                    addError(errinfo, 0, "Mode 7 screen is of incorrect length. Must be 16,384 bytes long, and must not be compressed.");
                }
            }
            catch (Exception ex)
            {
                // File load error.
                pass = false;
                addError(errinfo, 0, "System error: " + ex.Message);
            }
            #endregion

            #region Load Mode 7 Graphics
            // --------------------------------------------
            // LoadTrack(): Load Mode 7 Graphics
            // --------------------------------------------
            // (byte[], bool) m7grp = Files.Read(graphPath);
            byte[] m7grp = null;

            try
            {
                m7grp = File.ReadAllBytes(graphPath);

                // File load OK.
                if ((m7grp.Length < 256) | ((m7grp.Length % 32) != 0))
                {
                    // File verify error.
                    pass = false;
                    addError(errinfo, 1, "Mode 7 graphics is of incorrect length. Look for the CM7 file associated with the track's theme.");
                }
            }
            catch (Exception ex)
            {
                // File load error.
                pass = false;
                addError(errinfo, 1, "System error: " + ex.Message);
            }
            #endregion

            #region Load Color Palette
            // --------------------------------------------
            // LoadTrack(): Load Color Palette
            // --------------------------------------------
            byte[] colpal = null; // Files.Read(palPath);

            try
            {
                colpal = File.ReadAllBytes(palPath);

                // File load OK.
                if (colpal.Length != 512)
                {
                    // File verify error.
                    pass = false;
                    addError(errinfo, 2, "Color palette file is of incorrect length. This file must be 512 bytes long.");
                }

            }
            catch (Exception ex)
            {
                // File load error.
                pass = false;
                addError(errinfo, 2, "System error: " + ex.Message);
            }
            #endregion

            #region Load Common Tile Graphics
            // --------------------------------------------
            // LoadTrack(): Load Common Tile Graphics
            // --------------------------------------------
            byte[] commonGrp = new byte[0];

            if (comPath != null)
            {
                // Load the file.
                tileLimit = 0xC0;

                // commonGrp = Files.Read(comPath);

                try
                {
                    commonGrp = File.ReadAllBytes(comPath);

                    if ((commonGrp.Length < 256) | ((commonGrp.Length % 32) != 0))
                    {
                        pass = false;
                        addError(errinfo, 3, "Common Tile graphics is of incorrect length. Look for the CM7 file associated with the graphics.");
                    }
                }
                catch (Exception ex)
                {
                    pass = false;
                    addError(errinfo, 3, "System error: " + ex.Message);
                }
            }
            else
            {
                tileLimit = 0x100;
            }
            #endregion

            #region Load Overlay Patterns
            // --------------------------------------------
            // LoadTrack(): Load Overlay Patterns
            // --------------------------------------------
            overlayState ovstate = null;
            bool ovpass = true;

            if (ovs != null)
            {
                string ovSourceFile = null;
                byte[] ovDataFile = null;

                try
                {
                    ovSourceFile = File.ReadAllText(ovs.sourceFile);
                }
                catch (Exception ex)
                {
                    pass = false;
                    ovpass = false;
                    addError(errinfo, 4, ex.Message);
                }

                try
                {
                    ovDataFile = File.ReadAllBytes(ovs.dataFile);
                }
                catch (Exception ex)
                {
                    pass = false;
                    ovpass = false;
                    addError(errinfo, 7, ex.Message);
                }

                if (!ovpass) goto skipOverlay;

                int a = findLabel(ovSourceFile, ovs.patternRef);
                labeledData[] patLabels;
                if (a != -1)
                {
                    a += ovs.patternRef.Length;
                    patLabels = getLabels(ovSourceFile.ToCharArray(), a);
                    try
                    {
                        patLabels = convertData(ovSourceFile, patLabels);
                    }
                    catch (Exception ex)
                    {
                        pass = false;
                        addError(errinfo, 5, ex.Message);
                        goto skipOverlay;
                    }
                }
                else
                {
                    pass = false;
                    addError(errinfo, 5, "Unable to find label.");
                    goto skipOverlay;
                }

                a = findLabel(ovSourceFile, ovs.sizeRef);
                byte[] sizeData;
                if (a != -1)
                {
                    a += ovs.sizeRef.Length;
                    sizeData = convertData(ovSourceFile.ToCharArray(), a);
                }
                else
                {
                    pass = false;
                    addError(errinfo, 6, "Unable to find label.");
                    goto skipOverlay;
                }

                ovstate = new overlayState
                {
                    pieces = new List<byte[]>(0),
                    sizes = new byte[sizeData.Length / 2, 2],
                    rawData = new byte[128],
                    writePath = ovs.dataFile,
                    writeAt = ovs.startsAt
                };

                int i;
                for (i = 0; i < patLabels.Length; i++)
                {
                    ovstate.pieces.Add(patLabels[i].data);
                }

                int j = 0;
                for (i = 0; i < ovstate.sizes.GetLength(0); i++)
                {
                    ovstate.sizes[i, 0] = sizeData[j];
                    j++;
                    ovstate.sizes[i, 1] = sizeData[j];
                    j++;
                }

                try
                {
                    j = ovs.startsAt;

                    for (i = 0; i < 128; i++)
                    {
                        ovstate.rawData[i] = ovDataFile[j];
                        j++;
                    }
                }
                catch (Exception ex)
                {
                    addError(errinfo, 8, ex.Message);
                }
            }
        #endregion

        skipOverlay:;


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
                        m72d[i, j] = m7scr[k];
                        k++;
                    }
                }

                k = 0x100;

                graphicTile4bpp tile4bpp;
                graphicTile8bpp2d tile8bpp;
                trackTheme2d theme;

                tile4bpp = make4bppTile(0, true);
                theme.tile = new graphicTile8bpp2d[256];

                #region Decode Common Graphics
                // Common Graphics
                if (commonGrp.Length > 0)
                {
                    for (i = 0; i < 0x40; i++)
                    {
                        tile4bpp.palette = commonGrp[i];

                        for (j = 0; j < 32; j++)
                        {
                            tile4bpp.graphics[j] = commonGrp[k];
                            k++;
                        }

                        tile8bpp = convert4bppTo8bpp2d(tile4bpp);
                        theme.tile[i + 0xC0] = tile8bpp;

                        if (k >= commonGrp.Length)
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
                #endregion

                #region Decode Track Graphics
                k = 0x100;

                // Rare Graphics
                for (i = 0; i < tileLimit; i++)
                {
                    tile4bpp.palette = m7grp[i];

                    for (j = 0; j < 32; j++)
                    {
                        tile4bpp.graphics[j] = m7grp[k];
                        k++;
                    }

                    tile8bpp = convert4bppTo8bpp2d(tile4bpp);
                    theme.tile[i] = tile8bpp;

                    if (k >= m7grp.Length)
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
                #endregion

                Color[] pal = convertAllARGB(colpal);

                TM_CourseEditor ce = new TM_CourseEditor(m72d, theme, pal, 128, 128, (TM_CourseEditor.trackEditorParams.AllowCopying | TM_CourseEditor.trackEditorParams.AllowPasteStashing | TM_CourseEditor.trackEditorParams.ReEnableOwner), this, screenPath, ovstate);
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
            MessageBox.Show("Awesome Import\n" +
                "\"Part of an SMKToolbox breakfast!\"\n\n" +
                "Code by [REDACTED].\n" +
                "SMK Toolbox base by Dirtbag.\n",
                "About Awesome Import",MessageBoxButtons.OK);
        }

        private void openIndividualTrackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TM_OpenDirect openDir = new TM_OpenDirect(this);
            openDir.Show();
        }

        private void openMarioCircuit3BecauseIcesanIsTooDamnLazyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadTrack(
                "C:\\ProjectL_ToolboxTest\\Source\\Assets\\mode7\\scr\\circuit\\circuit01.SCR",
                "C:\\ProjectL_ToolboxTest\\Source\\Assets\\mode7\\chr\\circuit.CM7",
                "C:\\ProjectL_ToolboxTest\\Source\\Assets\\color\\circuit.COL",
                "C:\\ProjectL_ToolboxTest\\Source\\Assets\\mode7\\chr\\item.CM7",
                new overlaySetup
                {
                    dataFile = "C:\\ProjectL_ToolboxTest\\Source\\kimura\\bin\\BG_unit.bin",
                    patternRef = "YAKI_NO",
                    sizeRef = "PTADAT",
                    sourceFile = "C:\\ProjectL_ToolboxTest\\Source\\kimura\\kart\\join\\BGunit_set.asm",
                    startsAt = 0
                }
                );
        }
    }
}
