using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMKToolbox
{
    public partial class LayoutEditor : Form
    {
        //Layouts
        TableLayoutPanel mainTablePanel;
        Panel panelTileMap;
        TableLayoutPanel tilemapTablePanel;
        TableLayoutPanel panelRight;
        TableLayoutPanel buttonsLayoutPanel;
        Panel panelTilePicker;
        Panel panelLayoutButtons;
        Panel panelTileInfo;
        Panel panelPalettes;
        
        Label displayPaletteNumber;
        Label tileInfo;
        CheckBox buttonY;
        CheckBox buttonX;
        CheckBox buttonP;
        PictureBox thumbnail1;
        PictureBox thumbnail2;
        PictureBox thumbnail3;
        CheckBox buttonFloodFill;
        CheckBox buttonSwap;
        CheckBox buttonUpdate;
        Button buttonZoomIn;
        Button buttonZoomOut;

        //Variables for tile
        int tileIndex = 0;
        int paletteSet = 0;
        bool flipX = false;
        bool flipY = false;
        bool controlBit = false;

        //Arrays for loading raw data

        byte[] rawTiles;
        string tilesetFileName;
        string palettesetFileName;

        int[][] paletteArraySet;
        Bitmap[][] tileSet;
        Bitmap[] paletteRows = new Bitmap[16];

        TileLayout.TileDetails[] tileLayoutDetails;
        TileLayout.TileDetails[] tileLayout1Details;
        TileLayout.TileDetails[] tileLayout2Details;
        TileLayout.TileDetails[] tileLayout3Details;
        int activeTileSet;  //Used to keep track of what TileLayout is being worked on.
        bool floodfill = false;
        bool changeTileProperties = false;
        bool swap = false;
        PictureBox selectedTileDisplay;
        PictureBox tilePreview;
        int zoomSize = 64;

        ToolStripMenuItem menuforegroundSaveAsLayout;
        ToolStripMenuItem menuBackgroundSaveAsLayout;

        public LayoutEditor()
        {
            SetupLayout();
            SetupMenu();
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            this.Size = new Size(1020, 1070);
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            this.ResizeEnd += (sender, e) => { resizeTilemap(sender, e); resizeTilePicker(sender, e); resizePalettes(sender, e); };
        }

        private void SetupLayout()
        {
            //Change this to BorderStyle.FixedSingle to see all layout panels, or BorderStyle.None to hode the layout
            BorderStyle boarderSetting = BorderStyle.None;

            mainTablePanel = new TableLayoutPanel();
            mainTablePanel.Dock = DockStyle.Fill;
            mainTablePanel.RowCount = 4; 
            mainTablePanel.ColumnCount = 2;
            mainTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 75f));
            mainTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 25f));
            mainTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70f));
            mainTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300f));

            tilemapTablePanel = new TableLayoutPanel();

            tilemapTablePanel.SuspendLayout();

            tilemapTablePanel.Dock = DockStyle.Fill;
            tilemapTablePanel.RowCount = 1;
            tilemapTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            

            panelTileMap = new Panel();
            panelTileMap.Dock = DockStyle.Fill;
            panelTileMap.Padding = new Padding(0);
            panelTileMap.BorderStyle = boarderSetting;
            panelTileMap.Width = tilemapTablePanel.Width;
            panelTileMap.Height = tilemapTablePanel.Height;

            int tilemapWidth = mainTablePanel.Width;
            int tilemapHeight = mainTablePanel.Height;
            int tileMapAreaSize = tilemapWidth /9;

            int pictureBoxSize = 64;
            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Size = new Size(pictureBoxSize -1, pictureBoxSize -1),
                        Location = new Point(x * (pictureBoxSize), y * (pictureBoxSize)),
                        BorderStyle = BorderStyle.None,
                        Padding = new Padding(0),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Tag = y * 32 + x
                    };
                    pictureBox.MouseClick += new MouseEventHandler(tileLayout_Click);
                    pictureBox.Paint += new PaintEventHandler(SelectedTileDisplay_Paint);
                    panelTileMap.Controls.Add(pictureBox);
                }
            }

            tilemapTablePanel.Controls.Add(panelTileMap, 0, 0);
            mainTablePanel.Controls.Add(tilemapTablePanel, 0, 0);
            
            tilemapTablePanel.ResumeLayout();

            panelTileInfo = new Panel();
            panelTileInfo.BorderStyle = boarderSetting;
            panelTileInfo.Dock = DockStyle.Fill;

            tilePreview = new PictureBox()
            {
                Size = new Size(96,96),
                Location = new Point (1,1),
                BorderStyle = BorderStyle.None,
                //Image = Image.FromFile("temp.bmp"),
                SizeMode = PictureBoxSizeMode.Zoom,
            };
            tilePreview.Paint += new PaintEventHandler(SelectedTileDisplay_Paint);

            panelTileInfo.Controls.Add(tilePreview);
            tileInfo = new Label();
            tileInfo.Text= "";
            tileInfo.Dock = DockStyle.Right;
            tileInfo.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);
            tileInfo.Size = new Size(200,10);
            tileInfo.Location = new Point(0, tilePreview.Bottom);
            panelTileInfo.Controls.Add(tileInfo);

            panelRight = new TableLayoutPanel();
            panelRight.Dock = DockStyle.Fill;
            panelRight.RowCount = 4;
            panelRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 295f));
            panelRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 100f));
            panelRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 280f));
            panelRight.RowStyles.Add(new RowStyle(SizeType.Absolute, 85f));

            panelTilePicker = new Panel();
            panelTilePicker.Dock = DockStyle.Fill;
            panelTilePicker.BorderStyle = boarderSetting;
            int tilePickerBoxSize = (int)(panelRight.Width / 14);
            
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    PictureBox tilePickerBox = new PictureBox
                    {
                        Size = new Size(tilePickerBoxSize, tilePickerBoxSize),
                        Location = new Point(x * tilePickerBoxSize, y * tilePickerBoxSize),
                        //BorderStyle = BorderStyle.FixedSingle,
                        // Image = Image.FromFile("temp.bmp"),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Tag = y * 16 + x
                    };
                    panelTilePicker.Controls.Add(tilePickerBox);
                    tilePickerBox.Click += new EventHandler(tilePickerBox_Click);
                    tilePickerBox.MouseEnter += new EventHandler(tilePickerBox_MouseEnter);
                    tilePickerBox.MouseLeave += new EventHandler(tilePickerBox_MouseLeave);
                }
            }



            panelPalettes = new Panel();
            panelPalettes.Dock = DockStyle.Fill;
            panelPalettes.BorderStyle = boarderSetting;

            int palettePickerBoxSize = (int)(panelPalettes.Width / 15);
            for (int y = 0; y < 16; y++)
            {

                PictureBox palettePickerBox = new PictureBox
                {
                    Size = new Size(palettePickerBoxSize, palettePickerBoxSize),
                    Location = new Point(0, y * palettePickerBoxSize),
                    BorderStyle = BorderStyle.None,
                    // Image = Image.FromFile("temp.bmp"),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Tag = y
                    };
                palettePickerBox.Click += new EventHandler(palettePickerBox_Click);
                panelPalettes.Controls.Add(palettePickerBox);
     
            }
            
            

            panelLayoutButtons = new Panel();
            panelLayoutButtons.Dock = DockStyle.Fill;
            panelLayoutButtons.BorderStyle = boarderSetting;
            
            buttonsLayoutPanel = new TableLayoutPanel();
            buttonsLayoutPanel.Dock = DockStyle.Fill;
            buttonsLayoutPanel.RowCount = 2;
            buttonsLayoutPanel.ColumnCount = 3;
            buttonsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            buttonsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            buttonsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            buttonsLayoutPanel.Visible = false;

            

            buttonFloodFill = new CheckBox
            {
                Text = "Fill",
                TextAlign = ContentAlignment.MiddleCenter,
                Appearance = System.Windows.Forms.Appearance.Button,
                //Dock = DockStyle.Fill,
                AutoSize = false,
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular)
            };
            buttonFloodFill.Click += buttonFloodFill_CheckedChanged;
            buttonsLayoutPanel.Controls.Add(buttonFloodFill, 1, 1);

            buttonSwap = new CheckBox
            {
                Text = "Swap",
                TextAlign = ContentAlignment.MiddleCenter,
                Appearance = System.Windows.Forms.Appearance.Button,
               // Dock = DockStyle.Fill,
                AutoSize = false,
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular)
            };
            buttonSwap.Click += buttonSwap_CheckedChanged;
            buttonsLayoutPanel.Controls.Add(buttonSwap, 0, 1);

            buttonUpdate = new CheckBox
            {
                Text = "Update",
                TextAlign = ContentAlignment.MiddleCenter,
                Appearance = System.Windows.Forms.Appearance.Button,
              //  Dock = DockStyle.Fill,
                AutoSize = false,
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular)
            };
            buttonUpdate.Click += buttonUpdate_CheckedChanged;
            buttonsLayoutPanel.Controls.Add(buttonUpdate, 2, 1);

            buttonZoomIn = new Button
            {
                Text = "Zoom In",
                TextAlign = ContentAlignment.MiddleCenter,
                //Dock = DockStyle.Fill,
                AutoSize = false,
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular),
                Size = new Size(buttonFloodFill.Width +30, buttonFloodFill.Height),
                //Visible = false,
                //Location = new Point(0, tilePreview.Bottom)
            };
            buttonZoomIn.Click += buttonZoomIn_CheckedChanged;
            buttonsLayoutPanel.Controls.Add(buttonZoomIn, 0, 0);

            buttonZoomOut = new Button
            {
                Text = "Zoom Out",
                TextAlign = ContentAlignment.MiddleCenter,
                //  Dock = DockStyle.Fill,
                Size = new Size(buttonFloodFill.Width + 30, buttonFloodFill.Height),
                AutoSize = false,
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular),
                //Visible = false,
                // Location = new Point(0, 0)
            };
            buttonZoomOut.Click += buttonZoomOut_CheckedChanged;
            buttonsLayoutPanel.Controls.Add(buttonZoomOut, 2, 0);

            

            panelRight.Controls.Add(panelTilePicker, 0, 0);
            panelRight.Controls.Add(panelTileInfo, 0, 1);
            panelRight.Controls.Add(panelPalettes, 0, 2);
            panelRight.Controls.Add(buttonsLayoutPanel, 0, 3);

            mainTablePanel.Controls.Add(panelRight, 1, 0);

            TableLayoutPanel BottomPanel = new TableLayoutPanel();
            BottomPanel.Dock = DockStyle.Fill;
            BottomPanel.ColumnCount = 4;
            BottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            BottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            BottomPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));

            Panel Bottom1 = new Panel();
            Bottom1.Dock = DockStyle.Fill;
            Bottom1.BorderStyle = boarderSetting;

            thumbnail1 = new PictureBox();
            thumbnail1.Dock = DockStyle.Fill;
            thumbnail1.Image = new Bitmap(1, 1);
            thumbnail1.SizeMode = PictureBoxSizeMode.Zoom;
            thumbnail1.BackColor = Color.LightGray;
            thumbnail1.Click += thumbnail1_Clicked;
            Bottom1.Controls.Add(thumbnail1);

            Panel Bottom2 = new Panel();
            Bottom2.Dock = DockStyle.Fill;
            Bottom2.BorderStyle = boarderSetting;

            thumbnail2 = new PictureBox();
            thumbnail2.Dock = DockStyle.Fill;
            thumbnail2.Image = new Bitmap(1, 1);
            thumbnail2.SizeMode = PictureBoxSizeMode.Zoom;
            thumbnail2.BackColor = Color.LightGray;
            thumbnail2.Click += thumbnail2_Clicked;
            Bottom2.Controls.Add(thumbnail2);

            Panel Bottom3 = new Panel();
            Bottom3.Dock = DockStyle.Fill;
            Bottom3.BorderStyle = boarderSetting;

            thumbnail3 = new PictureBox();
            thumbnail3.Dock = DockStyle.Fill;
            thumbnail3.Image = new Bitmap(1, 1);
            thumbnail3.SizeMode = PictureBoxSizeMode.Zoom;
            thumbnail3.BackColor = Color.LightGray;
            thumbnail3.Click += thumbnail3_Clicked;
            Bottom3.Controls.Add(thumbnail3);

            BottomPanel.Controls.Add(Bottom1, 0, 0);
            BottomPanel.Controls.Add(Bottom2, 1, 0);
            BottomPanel.Controls.Add(Bottom3, 2, 0);
            mainTablePanel.Controls.Add(BottomPanel, 0, 1);


            TableLayoutPanel controls = new TableLayoutPanel();
            controls.Dock = DockStyle.Fill;
            controls.BorderStyle = boarderSetting;
            controls.RowCount = 3;
            controls.ColumnCount = 1;
            controls.RowStyles.Add(new RowStyle(SizeType.Percent, 20.0f));
            controls.RowStyles.Add(new RowStyle(SizeType.Percent, 40.0f));
            controls.RowStyles.Add(new RowStyle(SizeType.Percent, 40.0f));

            TableLayoutPanel selectedTilePanel = new TableLayoutPanel();
            selectedTilePanel.Dock = DockStyle.Fill;
            selectedTilePanel.RowCount = 1;
            selectedTilePanel.ColumnCount = 1;
            selectedTilePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            selectedTileDisplay = new PictureBox
            {
                Size = new Size(Math.Min(controls.Width, controls.Height), Math.Min(controls.Width, controls.Height)),
                Location = new Point(0, 0),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };

            Bitmap bmp = new Bitmap(Math.Min(controls.Width, controls.Height), Math.Min(controls.Width, controls.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.LightGray);
            }
            selectedTileDisplay.Image = bmp;
            selectedTilePanel.Controls.Add(selectedTileDisplay, 0, 0);
            selectedTileDisplay.Anchor = AnchorStyles.None;
            selectedTileDisplay.Paint += new PaintEventHandler(SelectedTileDisplay_Paint);

            controls.Controls.Add(selectedTilePanel, 1, 0);


            TableLayoutPanel buttonsPanel = new TableLayoutPanel();
            buttonsPanel.Dock = DockStyle.Fill;
            buttonsPanel.RowCount = 1;
            buttonsPanel.ColumnCount = 3;
            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));

            buttonY = new CheckBox
            {
                Text = "▲\nY\n▼",
                TextAlign = ContentAlignment.MiddleCenter,
                Appearance = System.Windows.Forms.Appearance.Button,
                Dock = DockStyle.Fill,
                AutoSize = false,
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)
            };
        
            buttonY.Click += flipYButton_CheckedChanged;
            buttonsPanel.Controls.Add(buttonY, 0, 0);

            buttonX = new CheckBox
            {
                Text = "◀ X ▶",
                TextAlign = ContentAlignment.MiddleCenter,
                Appearance = System.Windows.Forms.Appearance.Button,
                Dock = DockStyle.Fill,
                AutoSize = false,
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)         
            };
            buttonsPanel.Controls.Add(buttonX, 1, 0);
            buttonX.Click += flipXButton_CheckedChanged;

            buttonP = new CheckBox
            {
                Text = "Priority",
                TextAlign = ContentAlignment.MiddleCenter,
                Appearance = System.Windows.Forms.Appearance.Button,
                Dock = DockStyle.Fill,
                AutoSize = false,
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)
            };

            buttonsPanel.Controls.Add(buttonP, 2, 0);
            buttonP.Click += PriorityButton_CheckedChanged;

            controls.Controls.Add(buttonsPanel, 1, 1);

        //   controls.Controls.Add(buttonsPanel);

            TableLayoutPanel palettePanel = new TableLayoutPanel();
            palettePanel.Dock = DockStyle.Fill;
            palettePanel.RowCount = 1;
            palettePanel.ColumnCount = 3;
            palettePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            palettePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            palettePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));

            Button paletteDownButton = new Button
            {
                Text = "◀",
                Dock = DockStyle.Fill,
                AutoSize = false,
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)
            };

            paletteDownButton.Click += paletteDownButton_Click;
            palettePanel.Controls.Add(paletteDownButton, 0, 0);

            controls.Controls.Add(palettePanel);

            Button paletteUpButton = new Button
            {
                Text = "▶",
                Dock = DockStyle.Fill,
                AutoSize = false,
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)
            };
            paletteUpButton.Click += paletteUpButton_Click;
            palettePanel.Controls.Add(paletteUpButton, 2, 0);

            displayPaletteNumber = new Label
            {
                Text = "0",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                AutoSize = true,
                Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)
            };
            palettePanel.Controls.Add(displayPaletteNumber, 1, 0);

            mainTablePanel.Controls.Add(controls, 1, 1);
            this.Controls.Add(mainTablePanel);
        }

        private void SetupMenu()
        {
            //Create the main menu
            MenuStrip mainMenu = new MenuStrip();
            ToolStripMenuItem menuROM = new ToolStripMenuItem("ROM");
            ToolStripMenuItem menuROMExtract = new ToolStripMenuItem("Extract title screen data pack from ROM");
            ToolStripMenuItem menuROMInsert = new ToolStripMenuItem("Insert title screen data pack to ROM");
            ToolStripMenuItem menuPalettes = new ToolStripMenuItem("Palettes");
            ToolStripMenuItem menuTiles = new ToolStripMenuItem("Tiles");
            ToolStripMenuItem menuLayouts = new ToolStripMenuItem("Layouts");
            ToolStripMenuItem menuLoadLayout = new ToolStripMenuItem("Load Foreground Layout");
            ToolStripMenuItem menuLoadLayout2 = new ToolStripMenuItem("Load Background Layout");
            menuforegroundSaveAsLayout = new ToolStripMenuItem("Save foreground");
            menuBackgroundSaveAsLayout = new ToolStripMenuItem("Save Background");
            ToolStripMenuItem menuHelp = new ToolStripMenuItem("Help");
            ToolStripMenuItem menuAbout = new ToolStripMenuItem("About");
            mainMenu.Items.Add(menuROM);
            menuROM.DropDownItems.Add(menuROMExtract);
            menuROMExtract.Click += new EventHandler(menuROMExtract_Click);
            menuROM.DropDownItems.Add(menuROMInsert);
            menuROMInsert.Click += new EventHandler(menuROMInsert_Click);
            mainMenu.Items.Add(menuPalettes);
            menuPalettes.ShortcutKeys = Keys.Control | Keys.P;
            menuPalettes.Click += new EventHandler(menuPalettes_Click);
            mainMenu.Items.Add(menuTiles);
            menuTiles.Click += new EventHandler(menuTiles_Click);
            mainMenu.Items.Add(menuLayouts);
            menuLayouts.DropDownItems.Add(menuLoadLayout);
            menuLoadLayout.Click += new EventHandler(menuLoadLayout_Click);
            menuLayouts.DropDownItems.Add(menuLoadLayout2);
            menuLoadLayout2.Click += new EventHandler(menuLoadLayout2_Click);
            menuLayouts.DropDownItems.Add(menuforegroundSaveAsLayout);
            menuLayouts.DropDownItems.Add(menuBackgroundSaveAsLayout);
            menuforegroundSaveAsLayout.Enabled = false;
            menuBackgroundSaveAsLayout.Enabled = false;
            menuforegroundSaveAsLayout.Click += new EventHandler(menuforegroundSaveAsLayout_Click);
            menuBackgroundSaveAsLayout.Click += new EventHandler(menuBackgroundSaveAsLayout_Click);
            mainMenu.Items.Add(menuHelp);
            menuHelp.DropDownItems.Add(menuAbout);
            menuAbout.Click += new EventHandler(menuAbout_Click);

            this.Controls.Add(mainMenu);
            this.MainMenuStrip = mainMenu;
            
        }

        private void menuTiles_Click(object sender, EventArgs e)
        {
            if (paletteArraySet == null)
            {
                MessageBox.Show("Load palettes first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Application.StartupPath;
                openFileDialog.Filter = "All files (*.*)|*.*|CGX files(*.CGX)| *.CGX";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    
                    panelTilePicker.SuspendLayout();
                    panelTilePicker.AutoScroll = false;
                    (byte[] TilesRaw, bool status3) = Files.Read(openFileDialog.FileName);
                    rawTiles = TilesRaw;
                    
                    try
                    {
                        tileSet = Tiles.DrawTiles(TilesRaw, paletteArraySet);
                    }
                    catch(Exception Ex)
                    {
                        tileSet = null;
                        MessageBox.Show("Invalid graphics file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    TileLayout.UpdateTilePickerBoxes(panelTilePicker, tileSet, paletteSet);
                    tilesetFileName = openFileDialog.SafeFileName;
                    panelTilePicker.ResumeLayout();
                    
                   //Reset control bits
                    buttonY.Checked = false;
                    buttonX.Checked = false;
                    buttonP.Checked = false;
                    flipX = false;
                    flipY = false;
                    controlBit = false;

                    try
                    {
                        //Update select tile image
                        selectedTileDisplay.Image = tileSet[paletteSet][tileIndex];
                        panelTileInfo.Visible = true;
                        panelTilePicker.ResumeLayout();
                    }
                    catch (Exception Ex)
                    {
                        tileSet = null;
                        MessageBox.Show("Invalid graphics file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (tileLayoutDetails != null)
                    {
                        updateLayoutScreen(true);

                        TileLayout.UpdatePictureBoxes(panelTileMap, tileSet, tileLayoutDetails);

                        changeOfLayout(activeTileSet);
                        
                        if (tileLayout1Details != null)
                        {
                            thumbnail1.Image = TileLayout.DisplayTiles(tileSet, tileLayout1Details); //update thumbnail
                        }
                        if (tileLayout2Details != null)
                        { 
                            thumbnail2.Image = TileLayout.DisplayTiles(tileSet, tileLayout2Details); //update thumbnail
                        }
                        if (tileLayout3Details != null)
                        {
                            thumbnail3.Image = TileLayout.DisplayTiles(tileSet, tileLayout3Details); //update thumbnail
                        }
                        updateLayoutScreen(false);
                    }
                }
               
            }


        }

        private void menuROMExtract_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "All files (*.*)|*.*|ROM files (*.SFC)|*.SFC";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

               (byte[] decompressedTilePack, bool status) = Files.DecompressFromROM(openFileDialog.FileName, 465302);
               if(status)
                {
                    /*
                        .\TITLE-ENG.CGX 	$0000 $2000 $0000
                        .\TITLE-ENG.SCR 	$0000 $0800 $2000
                        .\TITLE2-ENG.SCR	$0000 $1000 $2800
                        .\D-POINT-ENG.SCR	$0000 $0800 $3800
                        .\TITLE.COL 		$0000 $0200 $4000
                        .\REGI.COL	    	$0000 $0200 #4200
                    */
                    
                    byte[] titleTiles = Files.setuparray(8192);
                    byte[] titleLayout1 = Files.setuparray(2048);
                    byte[] titleLayout2 = Files.setuparray(4096);
                    byte[] pointsLayout = Files.setuparray(2048);
                    byte[] titlePalettes = Files.setuparray(512);
                    byte[] pointsPalettes = Files.setuparray(512);

                    int counter = 0;

                    System.Buffer.BlockCopy(decompressedTilePack, 0, titleTiles, 0, titleTiles.Length);
                    counter = counter + titleTiles.Length;
                    System.Buffer.BlockCopy(decompressedTilePack, counter, titleLayout1, 0, titleLayout1.Length);
                    counter = counter + titleLayout1.Length;
                    System.Buffer.BlockCopy(decompressedTilePack, counter, titleLayout2, 0, titleLayout2.Length);
                    counter = counter + titleLayout2.Length;
                    System.Buffer.BlockCopy(decompressedTilePack, counter, pointsLayout, 0, pointsLayout.Length);
                    counter = counter + pointsLayout.Length;
                    System.Buffer.BlockCopy(decompressedTilePack, counter, titlePalettes, 0, titlePalettes.Length);
                    counter = counter + titlePalettes.Length;
                    System.Buffer.BlockCopy(decompressedTilePack, counter, pointsPalettes, 0, pointsPalettes.Length);
                   
                    Files.Write("!titleTiles.CGX", titleTiles, true);
                    Files.Write("!titleLayout1.SCR", titleLayout1, true);
                    Files.Write("!titleLayout2.SCR", titleLayout2, true);
                    Files.Write("!pointsLayout.SCR", pointsLayout, true);
                    Files.Write("!titlePalettes.COL", titlePalettes, true);
                    Files.Write("!pointsPalettes.COL", pointsPalettes, true);
                }
            }
        }

        private void menuROMInsert_Click(object sender, EventArgs e)
        {

            /*
            .\TITLE-ENG.CGX 	$0000 $2000 $0000
            .\TITLE-ENG.SCR 	$0000 $0800 $2000
            .\TITLE2-ENG.SCR	$0000 $1000 $2800
            .\D-POINT-ENG.SCR	$0000 $0800 $3800
            .\TITLE.COL 		$0000 $0200 $4000
            .\REGI.COL	    	$0000 $0200 #4200
            */

            (byte[] titleTiles, bool statusTitleTiles) = Files.Read("!titleTiles.CGX");
            (byte[] titleLayout1, bool statusTitleLayout1) = Files.Read("!titleLayout1.SCR");
            (byte[] titleLayout2, bool statusTitleLayout2) = Files.Read("!titleLayout2.SCR");
            (byte[] pointsLayout, bool statusPointsLayout) = Files.Read("!pointsLayout.SCR");
            (byte[] titlePalettes, bool statusTitlePalettes) = Files.Read("!titlePalettes.COL");
            (byte[] pointsPalettes, bool statuspointsPalettes) = Files.Read("!pointsPalettes.COL");

            if (!statusTitleTiles || !statusTitleLayout1 || !statusTitleLayout2 || !statusPointsLayout || !statusTitlePalettes || !statuspointsPalettes)
            {
                MessageBox.Show("Failed to read all necessary files: \n !titleTiles.CGX\n!titleLayout1.SCR\n!titleLayout2.SCR\n!pointsLayout.SCR\n!titlePalettes.COL\n!pointsPalettes.COL\n\nData not inserted in to ROM", "Error", MessageBoxButtons.OK);
                return;
            }

            byte[] titleDataPack = new byte[17408];//byte[] titleDataPack = Files.setuparray(17408);

            try
            {

                System.Buffer.BlockCopy(titleTiles, 0, titleDataPack, 0, 8192);
                System.Buffer.BlockCopy(titleLayout1, 0, titleDataPack, 8192, 2048);
                System.Buffer.BlockCopy(titleLayout2, 0, titleDataPack, 10240, 4096);
                System.Buffer.BlockCopy(pointsLayout, 0, titleDataPack, 14336, 2048);
                System.Buffer.BlockCopy(titlePalettes, 0, titleDataPack, 16384, 512);
                System.Buffer.BlockCopy(pointsPalettes, 0, titleDataPack, 16896, 512);

            }
            catch (Exception Ex)
            {
                MessageBox.Show("Failed to build data pack", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Console.WriteLine(Ex.ToString());
                return;
            }

            (byte[] compressedTitleDataPack, bool statusCompressedTitleDataPack) = Files.Compress(titleDataPack);

            if (!statusCompressedTitleDataPack)
            {
                MessageBox.Show("Failed to compress data pack", "Error", MessageBoxButtons.OK);
                return;
            }
            if (compressedTitleDataPack.Length > 7748)
            {
                MessageBox.Show("Compressed title screen data pack is too large to reinsert to ROM", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "All files (*.*)|*.*|ROM files (*.SFC)|*.SFC";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Title = "Select ROM to insert title screen data pack";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                if(Files.Insert(openFileDialog.FileName, compressedTitleDataPack, 465302))
                {
                    MessageBox.Show("Title screen data pack inserted in to ROM", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Failed to insert title screen data pack in to ROM", "Error", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
            }
            
        }

        private void menuPalettes_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "All files (*.*)|*.*|Palette files (*.COL)|*.COL";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                panelPalettes.SuspendLayout();
                // Code to load the file into a variable
                //string fileName = openFileDialog.FileName;

                (byte[] RawPalette, bool status2) = Files.Read(openFileDialog.FileName);
                int[] NewPalette = Palettes.Load(RawPalette);
                paletteArraySet = Palettes.ConvertTo2DArray(NewPalette);
                palettesetFileName = openFileDialog.SafeFileName;

                for (int y = 0; y < 16; y++)
                {
                    Bitmap rowImage = new Bitmap(256, 16); //Create a new bitmap for each row
                    for (int x = 0; x < 16; x++)
                    {
                        try
                        {
                            int color = paletteArraySet[y][x];
                            //Set the color of the pixels in the column
                            for (int i = 0; i < 16; i++)
                            {
                                for (int j = 0; j < 16; j++)
                                {
                                    rowImage.SetPixel((x * 16) + i, j, Color.FromArgb(color));
                                }
                            }
                        }
                        catch(Exception Ex)
                        {
                            paletteArraySet = null;
                            MessageBox.Show("Invalid palette file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    //Create a new picture box and set its image to the row bitmap
                    PictureBox palettePickerBox = panelPalettes.Controls[y] as PictureBox;
                    palettePickerBox.Location = new Point(12, y * 16);
                    palettePickerBox.Size = new Size(256, 16);
                    paletteRows[y] = rowImage;
                    palettePickerBox.Image = paletteRows[y];

                    if (y == paletteSet)
                    {
                        Bitmap selected = (Bitmap)rowImage.Clone();
                        //Add red border to the row picture box
                        Graphics g = Graphics.FromImage(selected);
                        g.DrawRectangle(new Pen(Color.Crimson, 3), 0, 0, 255, 15);
                        palettePickerBox.Image = selected;
                    }
                    //Add the row picture box to the panel

                }



                //Check if tiles &/or layoutare already loaded and refresh
                if (tileSet != null)
                {
                    panelTilePicker.SuspendLayout();
                    tileSet = Tiles.DrawTiles(rawTiles, paletteArraySet);
                    TileLayout.UpdateTilePickerBoxes(panelTilePicker, tileSet, paletteSet);
                    panelTilePicker.ResumeLayout();
                    //update selected tile

                    //Reset control bits
                    buttonY.Checked = false;
                    buttonX.Checked = false;
                    buttonP.Checked = false;
                    flipX = false;
                    flipY = false;
                    controlBit = false;

                    //Update select tile image
                    selectedTileDisplay.Image = tileSet[paletteSet][tileIndex];

                }
                //Update layout if needed
                if(tileLayoutDetails != null)
                {
                    updateLayoutScreen(true);

                    TileLayout.UpdatePictureBoxes(panelTileMap, tileSet, tileLayoutDetails);
                    if (tileLayout1Details != null)
                    {
                        thumbnail1.Image = TileLayout.DisplayTiles(tileSet, tileLayout1Details); //update thumbnail
                    }
                    if (tileLayout2Details != null)
                    {
                        thumbnail2.Image = TileLayout.DisplayTiles(tileSet, tileLayout2Details); //update thumbnail
                    }
                    if (tileLayout3Details != null)
                    {
                        thumbnail3.Image = TileLayout.DisplayTiles(tileSet, tileLayout3Details); //update thumbnail
                    }
                    tilemapTablePanel.ResumeLayout();
                    panelTileMap.ResumeLayout();

                }
                updateLayoutScreen(false);
            }
        }

        private void updateLayoutScreen(bool start)
        {
            if(start)
            {
                panelTileMap.AutoScroll = false;
                tilemapTablePanel.SuspendLayout();
                panelTileMap.SuspendLayout();
            }
            else
            {
                panelTilePicker.AutoScroll = true;
                zoomSize = 64;
                tilemapTablePanel.ResumeLayout();
                panelTileMap.ResumeLayout();
            }
        }
        private void FloodFill(int index, int startTileIndex, int startPalette)
        {

            int width = (int)Math.Sqrt(tileLayoutDetails.Length);
            Queue<int> tilesToUpdate = new Queue<int>();
            tilesToUpdate.Enqueue(index);

            while (tilesToUpdate.Count > 0)
            {
                int currentIndex = tilesToUpdate.Dequeue();
                int currentTileIndex = tileLayoutDetails[currentIndex].tileIndex;
                int currentPalette = tileLayoutDetails[currentIndex].palette;

                if (currentTileIndex == startTileIndex && currentPalette == startPalette)
                {
                    tileLayoutDetails[currentIndex].tileIndex = tileIndex;
                    tileLayoutDetails[currentIndex].palette = paletteSet;
                    tileLayoutDetails[currentIndex].hFlip = flipX;
                    tileLayoutDetails[currentIndex].vFlip = flipY;
                    tileLayoutDetails[currentIndex].priority = controlBit;

                    PictureBox pictureBox = panelTileMap.Controls[currentIndex] as PictureBox;
                    pictureBox.Image = tileSet[paletteSet][tileIndex];

                    Bitmap original = new Bitmap(pictureBox.Image);
                    Bitmap flipped = new Bitmap(original);

                    if (flipX)
                    {
                        flipped.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    if (flipY)
                    {
                        flipped.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    }
                    pictureBox.Image = flipped;

                    int[] surroundingIndices = GetTileIndices(currentIndex, width);
                    foreach (int surroundingIndex in surroundingIndices)
                    {
                        tilesToUpdate.Enqueue(surroundingIndex);
                    }
                }
            }
        }

        private int[] GetTileIndices(int index, int width)
        {
            List<int> indices = new List<int>();
            int row = index / width;
            int column = index % width;

            if (row > 0)
            {
                indices.Add(index - width);
            }
            if (row < width - 1)
            {
                indices.Add(index + width);
            }
            if (column > 0)
            {
                indices.Add(index - 1);
            }
            if (column < width - 1)
            {
                indices.Add(index + 1);
            }

            return indices.ToArray();
        }



        private void menuLoadLayout_Click(object sender, EventArgs e)
        {
            if(paletteArraySet == null)
            {
                MessageBox.Show("Load palette first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (tileSet == null)
            {
                MessageBox.Show("Load tiles first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Application.StartupPath;
                openFileDialog.Filter = "All files (*.*)|*.*|SCR files (*.SCR)|*.SCR";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    updateLayoutScreen(true);
                    // Code to load the file into a variable
                    (byte[] LayoutsRaw, bool status4) = Files.Read(openFileDialog.FileName);
                    TileLayout tileLayout = new TileLayout();
                    tileLayoutDetails = TileLayout.GetTileDetails(LayoutsRaw);
                    activeTileSet = 1;
                    TileLayout.UpdatePictureBoxes(panelTileMap, tileSet, tileLayoutDetails);
                    tilemapTablePanel.ResumeLayout();
                    panelTileMap.ResumeLayout();
                    menuforegroundSaveAsLayout.Enabled = true;
                    thumbnail1.Image=TileLayout.DisplayTiles(tileSet, tileLayoutDetails);
                    updateLayoutScreen(false);
                    buttonsLayoutPanel.Visible = true;
                    
                }
            }
        }

        private void menuLoadLayout2_Click(object sender, EventArgs e)
        {
            if (paletteArraySet == null)
            {
                MessageBox.Show("Load palette first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (tileSet == null)
            {
                MessageBox.Show("Load tiles first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Application.StartupPath;
                openFileDialog.Filter = "All files (*.*)|*.*|SCR files (*.SCR)|*.SCR";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    updateLayoutScreen(true);

                    (byte[] layout2Raw, bool status4) = Files.Read(openFileDialog.FileName);




                    TileLayout tileLayout = new TileLayout();
                    tileLayout2Details = TileLayout.GetTileDetails(layout2Raw);
                    if (activeTileSet != 1)
                    {
                        changeOfLayout(1);
                    }
                    TileLayout.UpdatePictureBoxes(panelTileMap, tileSet, tileLayout2Details);
                    if (activeTileSet != 2)
                    {
                        changeOfLayout(2);
                    }
                    else
                    {

                    }
                    byte[] LayoutRawScreen2 = new byte[2048];
                    Array.Copy(layout2Raw, 2048, LayoutRawScreen2, 0, 2048);
                    tileLayout3Details = TileLayout.GetTileDetails(LayoutRawScreen2);
                    activeTileSet = 2;
                    updateLayoutScreen(false);
                    buttonsLayoutPanel.Visible = true;
                    menuBackgroundSaveAsLayout.Enabled = true;
                    

                    thumbnail2.Image = TileLayout.DisplayTiles(tileSet, tileLayout2Details);
                    thumbnail3.Image = TileLayout.DisplayTiles(tileSet, tileLayout3Details);

                }
            }
        }

        private void menuforegroundSaveAsLayout_Click(object sender, EventArgs e)
        {    
            //Make sure tileLayout1 is upto date
            if (activeTileSet == 1)
            {
                tileLayout1Details = tileLayoutDetails;
            }

            if (tileLayout1Details == null)
            {
                MessageBox.Show("No layout to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.Filter = "SCR files (*.SCR)|*.SCR|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;


            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                byte[] saveData = TileLayout.EncodeTileDetails(tileLayout1Details);
                Files.Write(saveFileDialog.FileName, saveData, true);
            }
        }

        private void menuBackgroundSaveAsLayout_Click(object sender, EventArgs e)
        {
            if (tileLayout2Details == null || tileLayout3Details == null)
            {
                MessageBox.Show("No layout to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.Filter = "SCR files (*.SCR)|*.SCR|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 1;
            saveFileDialog.RestoreDirectory = true;

            //Make sure tileLayout2 & 3 is upto date
            if (activeTileSet == 2)
            {
                tileLayout1Details = tileLayoutDetails;
            }
            if (activeTileSet == 3)
            {
                tileLayout3Details = tileLayoutDetails;
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
               
                
                byte[] saveData1 = TileLayout.EncodeTileDetails(tileLayout2Details);
                byte[] saveData2 = TileLayout.EncodeTileDetails(tileLayout3Details);
                byte[] saveDataCombined = new byte[saveData1.Length + saveData2.Length];
                System.Buffer.BlockCopy(saveData1, 0, saveDataCombined, 0, saveData1.Length);
                System.Buffer.BlockCopy(saveData2, 0, saveDataCombined, saveData1.Length, saveData2.Length);
                Files.Write(saveFileDialog.FileName, saveDataCombined, true);
            }
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tile Layer Editor 0.0.3\n\nPart of the SMKTool Box suite of tools by Dirtbag for Project L", "Tile Layer Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonZoomIn_CheckedChanged(object sender, EventArgs e)
        {
            int scrollV = panelTileMap.VerticalScroll.Value;
            int scrollH = panelTileMap.HorizontalScroll.Value;
            panelTileMap.AutoScroll = false;
            tilemapTablePanel.SuspendLayout();
            panelTileMap.SuspendLayout();
            if (zoomSize > 16)
            {
                foreach (Control control in panelTileMap.Controls)
                {
                    if (control is PictureBox pictureBox)
                    {

                        pictureBox.Size = new Size(pictureBox.Height + 8, pictureBox.Width + 8);
                        pictureBox.Location = new Point((int)pictureBox.Tag % 32 * pictureBox.Width, (int)pictureBox.Tag / 32 * pictureBox.Width);
                        zoomSize = pictureBox.Height;
                    }
                }
            }
            panelTileMap.ResumeLayout();
            tilemapTablePanel.ResumeLayout();
            //tilemapTablePanel.AutoScroll = true;
            panelTileMap.VerticalScroll.Value = scrollV;
            panelTileMap.HorizontalScroll.Value = scrollH;
            panelTileMap.AutoScroll = true;

        }



        private void buttonZoomOut_CheckedChanged(object sender, EventArgs e)
        {
            int scrollV = panelTileMap.VerticalScroll.Value;
            int scrollH = panelTileMap.HorizontalScroll.Value;
            panelTileMap.AutoScroll = false;
            tilemapTablePanel.SuspendLayout();
            panelTileMap.SuspendLayout();
            if (zoomSize > 16)
            {
                foreach (Control control in panelTileMap.Controls)
                {
                    if (control is PictureBox pictureBox)
                    {

                        pictureBox.Size = new Size(pictureBox.Height - 8, pictureBox.Width - 8);
                        pictureBox.Location = new Point((int)pictureBox.Tag % 32 * pictureBox.Width, (int)pictureBox.Tag / 32 * pictureBox.Width);
                        zoomSize = pictureBox.Height;
                    }
                }
            }
            panelTileMap.ResumeLayout();
            tilemapTablePanel.ResumeLayout();
            // tilemapTablePanel.AutoScroll = true;
            panelTileMap.VerticalScroll.Value = scrollV;
            panelTileMap.HorizontalScroll.Value = scrollH;
            panelTileMap.AutoScroll = true;

        }


        //Start of resize
        private void resizeTilemap(object sender, EventArgs e)
        {/*
            tilemapTablePanel.SuspendLayout();
            panelTileMap.SuspendLayout();
            

            int tilemapWidth = mainTablePanel.Width;
            int tilemapHeight = mainTablePanel.Height;
            int tileMapAreaSize = tilemapWidth / 44;

            foreach (Control control in panelTileMap.Controls)
            {
                if (control is PictureBox pictureBox)
                {
                    pictureBox.Size = new Size(tileMapAreaSize, tileMapAreaSize);
                    pictureBox.Location = new Point((int)pictureBox.Tag % 32 * tileMapAreaSize, (int)pictureBox.Tag / 32 * tileMapAreaSize);
                }
            }
            panelTileMap.ResumeLayout();
            tilemapTablePanel.ResumeLayout();
            */
                }

                private void resizeTilePicker(object sender, EventArgs e)
        {
            panelTilePicker.SuspendLayout();
            int newTilePickerBoxSize = (int)(panelPalettes.Width / 16);
            foreach (Control c in panelTilePicker.Controls)
            {
                if (c is PictureBox)
                {
                    PictureBox pb = (PictureBox)c;
                    pb.Size = new Size(newTilePickerBoxSize, newTilePickerBoxSize);
                    pb.Location = new Point((int)pb.Tag % 16 * newTilePickerBoxSize, (int)pb.Tag / 16 * newTilePickerBoxSize);
                }
            }
            panelTilePicker.ResumeLayout();
        }
        private void resizePalettes(object sender, EventArgs e)
        {/**
            panelPalettes.SuspendLayout();
            int newPalettePickerBoxSize = (int)(panelPalettes.Width / 16);
            foreach (Control c in panelPalettes.Controls)
            {
                if (c is PictureBox)
                {
                    PictureBox pb = (PictureBox)c;
                    pb.Size = new Size(newPalettePickerBoxSize, newPalettePickerBoxSize);
                    pb.Location = new Point((int)pb.Tag % 16 * newPalettePickerBoxSize, (int)pb.Tag / 16 * newPalettePickerBoxSize);
                }
            }
            panelPalettes.ResumeLayout();**/
        }

        void tileLayout_Click(object sender, MouseEventArgs e)
        {
            if (tileLayoutDetails != null)
            {
                PictureBox pictureBox = (PictureBox)sender;
                int index = (int)pictureBox.Tag;
                Console.WriteLine("Layout Location: " + index.ToString());
                
                if (e.Button == MouseButtons.Left)
                {

                    if (changeTileProperties)
                    {
                        int tileIndex = tileLayoutDetails[index].tileIndex;
                        int palette = tileLayoutDetails[index].palette;
                        bool hFlip = tileLayoutDetails[index].hFlip;
                        bool vFlip = tileLayoutDetails[index].vFlip;
                        bool priority = tileLayoutDetails[index].priority;

                        tileLayoutDetails[index].tileIndex = tileIndex;
                        tileLayoutDetails[index].palette = paletteSet;
                        tileLayoutDetails[index].hFlip = flipX;
                        tileLayoutDetails[index].vFlip = flipY;
                        tileLayoutDetails[index].priority = controlBit;

                        pictureBox.Image = tileSet[paletteSet][tileIndex];

                        Bitmap original = new Bitmap(pictureBox.Image);
                        Bitmap flipped = new Bitmap(original);

                        if (flipX)
                        {
                            flipped.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                        if (flipY)
                        {
                            flipped.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        }
                        pictureBox.Image = flipped;
                        switch (activeTileSet)
                        {
                            case 1:
                                thumbnail1.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails);
                                break;
                            case 2:
                                thumbnail2.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails);
                                break;
                            case 3:
                                thumbnail3.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails);
                                break;
                        }
                        return;
                    }


                    if (swap)
                    {
                        // Loop through all tiles in the layout
                        for (int i = 0; i < tileLayoutDetails.Length; i++)
                        {
                            if (i != index)
                            {
                                // Check if the tile properties match the one that was clicked
                                if (tileLayoutDetails[i].tileIndex == tileLayoutDetails[index].tileIndex &&
                                    tileLayoutDetails[i].palette == tileLayoutDetails[index].palette &&
                                    tileLayoutDetails[i].hFlip == tileLayoutDetails[index].hFlip &&
                                    tileLayoutDetails[i].vFlip == tileLayoutDetails[index].vFlip &&
                                    tileLayoutDetails[i].priority == tileLayoutDetails[index].priority)
                                {
                                    // Update the tile properties
                                    tileLayoutDetails[i].tileIndex = tileIndex;
                                    tileLayoutDetails[i].palette = paletteSet;
                                    tileLayoutDetails[i].hFlip = flipX;
                                    tileLayoutDetails[i].vFlip = flipY;
                                    tileLayoutDetails[i].priority = controlBit;
                                    pictureBox.Image = tileSet[paletteSet][tileIndex];

                                    // Rotate or flip the image before setting it as the PictureBox's Image
                                    Bitmap original = new Bitmap(pictureBox.Image);
                                    Bitmap flipped = new Bitmap(original);

                                    if (flipX)
                                    {
                                        flipped.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                    }
                                    if (flipY)
                                    {
                                        flipped.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                    }
                                    pictureBox.Image = flipped;
                                }
                            }
                        }

                      

                        switch (activeTileSet)
                        {
                            case 1:
                                thumbnail1.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails);
                                break;
                            case 2:
                                thumbnail2.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails);
                                break;
                            case 3:
                                thumbnail3.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails);
                                break;
                        }
                        tilemapTablePanel.SuspendLayout();
                        panelTileMap.SuspendLayout();
                        TileLayout.UpdatePictureBoxes(panelTileMap, tileSet, tileLayoutDetails);
                        tilemapTablePanel.ResumeLayout();
                        panelTileMap.ResumeLayout();
                        
                    }

                    if (floodfill)
                    {
                        if (tileLayoutDetails[index].tileIndex != tileIndex || tileLayoutDetails[index].palette != paletteSet)
                        {
                            FloodFill(index, tileLayoutDetails[index].tileIndex, tileLayoutDetails[index].palette);
                            switch (activeTileSet)
                            {
                                case 1:
                                    thumbnail1.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails); //update thumbnail
                                    break;
                                case 2:
                                    thumbnail2.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails); //update thumbnail
                                    break;
                                case 3:
                                    thumbnail3.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails); //update thumbnail
                                    break;
                            }
                        }
                     
                    }
                    else
                    {

                        tileLayoutDetails[index].tileIndex = tileIndex;
                        tileLayoutDetails[index].palette = paletteSet;
                        tileLayoutDetails[index].hFlip = flipX;
                        tileLayoutDetails[index].vFlip = flipY;
                        tileLayoutDetails[index].priority = controlBit;

                        pictureBox.Image = tileSet[paletteSet][tileIndex];

                        // Rotate or flip the image before setting it as the PictureBox's Image
                        Bitmap original = new Bitmap(pictureBox.Image);
                        Bitmap flipped = new Bitmap(original);

                        if (flipX)
                        {
                            flipped.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                        if (flipY)
                        {
                            flipped.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        }
                        pictureBox.Image = flipped;
                        switch (activeTileSet)
                        {
                            case 1:
                                thumbnail1.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails); //update thumbnail
                                break;
                            case 2:
                                thumbnail2.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails); //update thumbnail
                                break;
                            case 3:
                                thumbnail3.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails); //update thumbnail
                                break;
                        }
                    }
                }

                else if (e.Button == MouseButtons.Right)
                {
                    Console.WriteLine("Layout Location/ Tile update: " + tileIndex.ToString());
                    buttonY.Checked = false;
                    buttonX.Checked = false;
                    buttonP.Checked = false;
                    flipX = false;
                    flipY = false;
                    controlBit = false;

                    selectedTileDisplay.Image = null;
                    selectedTileDisplay.Image = tileSet[tileLayoutDetails[index].palette][tileLayoutDetails[index].tileIndex];
                    displayPaletteNumber.Text = tileLayoutDetails[index].palette.ToString();
                    //update palette colour of tile picker
                    if (paletteSet != tileLayoutDetails[index].palette)
                    {
                        panelTilePicker.SuspendLayout();
                        TileLayout.UpdateTilePickerBoxes(panelTilePicker, tileSet, tileLayoutDetails[index].palette);
                        panelTilePicker.ResumeLayout();
                    }

                    tileIndex = tileLayoutDetails[index].tileIndex;
                    paletteSet = tileLayoutDetails[index].palette;
                    flipX = tileLayoutDetails[index].hFlip;
                    flipY = tileLayoutDetails[index].vFlip;
                    controlBit = tileLayoutDetails[index].priority;

                    buttonY.Checked = flipY;
                    buttonX.Checked = flipX;
                    buttonP.Checked = controlBit;

                    // Create a new Bitmap from the selected tile's image
                    Bitmap original = new Bitmap(selectedTileDisplay.Image);
                    Bitmap flipped = new Bitmap(original);

                    // Rotate or flip the image before setting it as the selected tile display's Image
                    if (flipX)
                    {
                        flipped.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    }
                    if (flipY)
                    {
                        flipped.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    }
                    selectedTileDisplay.Image = flipped;


                }

            }
        }

        void palettePickerBox_Click(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;  

            if ((int)pictureBox.Tag < 8 && tileSet != null)
            {
                paletteSelectUpdate(true);
                paletteSet = (int)pictureBox.Tag;
                paletteSelectUpdate(false);

                displayPaletteNumber.Text = paletteSet.ToString();
                TileLayout.UpdateTilePickerBoxes(panelTilePicker, tileSet, paletteSet);

                // Create a new instance of the original image before flipping it
                Bitmap original = new Bitmap(tileSet[paletteSet][tileIndex]);
                Bitmap flipped = new Bitmap(original);
                if (flipX)
                {
                    flipped.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                if (flipY)
                {
                    flipped.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }
                selectedTileDisplay.Image = flipped;
            }
        }
        void tilePickerBox_Click(object sender, EventArgs e)
        {
            if (tileSet != null)
            {
                //Reset control bits
                buttonY.Checked = false;
                buttonX.Checked = false;
                buttonP.Checked = false;
                flipX = false;
                flipY = false;

                controlBit = false;
                PictureBox pictureBox = (PictureBox)sender;
                tileIndex = (int)pictureBox.Tag;
                selectedTileDisplay.Image = tileSet[paletteSet][tileIndex];
            }
        }

        private void tilePickerBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            if (tileSet != null)
            {
                tilePreview.BorderStyle = BorderStyle.Fixed3D;
                tilePreview.Image = tileSet[paletteSet][(int)pictureBox.Tag];
                tileInfo.Text = "  Index " + ((int)pictureBox.Tag).ToString() + " (" + ((int)pictureBox.Tag).ToString("X2") + ")\n\n" + tilesetFileName + "\n  Palette "+ paletteSet.ToString() +"\n  " + palettesetFileName;
            }

        }

        private void tilePickerBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            tilePreview.Image = null;
            tileInfo.Text = null;
            tilePreview.BorderStyle = BorderStyle.None;
        }


        void SelectedTileDisplay_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                PictureBox TileDisplay = (PictureBox)sender;
                e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                e.Graphics.DrawImage(TileDisplay.Image, 0, 0, TileDisplay.Width - 1, TileDisplay.Height - 1);
            }
            catch
            { }
        }

        void paletteSelectUpdate(bool remove)
        {
            if (remove)
            {
                PictureBox pictureBox = panelPalettes.Controls[paletteSet] as PictureBox;
                pictureBox.Image = paletteRows[paletteSet];
            }
            else
            {
                PictureBox pictureBox = panelPalettes.Controls[paletteSet] as PictureBox;
                Bitmap selected = (Bitmap)paletteRows[paletteSet].Clone();
                //Add red border to the row picture box
                Graphics g = Graphics.FromImage(selected);
                g.DrawRectangle(new Pen(Color.Crimson, 3), 0, 0, 255, 15);
                pictureBox.Image = selected;
            }
        }
        void paletteUpButton_Click (object sender, EventArgs e)
        {
            if (paletteSet < 7 && tileSet != null)
            {

                paletteSelectUpdate(true);
                paletteSet++;
                paletteSelectUpdate(false);

                displayPaletteNumber.Text = paletteSet.ToString();
                TileLayout.UpdateTilePickerBoxes(panelTilePicker, tileSet, paletteSet);

                // Create a new instance of the original image before flipping it
                Bitmap original = new Bitmap(tileSet[paletteSet][tileIndex]);
                Bitmap flipped = new Bitmap(original);
                if (flipX)
                {
                    flipped.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                if (flipY)
                {
                    flipped.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }
                selectedTileDisplay.Image = flipped;
            }
        }
        void paletteDownButton_Click(object sender, EventArgs e)
        {
            if (paletteSet > 0 && tileSet != null)
            {

                paletteSelectUpdate(true);
                paletteSet--;
                paletteSelectUpdate(false);
                
                displayPaletteNumber.Text = paletteSet.ToString();
                TileLayout.UpdateTilePickerBoxes(panelTilePicker, tileSet, paletteSet);

                // Create a new instance of the original image before flipping it
                Bitmap original = new Bitmap(tileSet[paletteSet][tileIndex]);
                Bitmap flipped = new Bitmap(original);
                if (flipX)
                {
                    flipped.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
                if (flipY)
                {
                    flipped.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }
                selectedTileDisplay.Image = flipped;

            }
        }
        

        void flipXButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked == true)
            {
                flipX = true;
            }
            else
            {
                flipX = false;
            }
            Bitmap original = new Bitmap(selectedTileDisplay.Image);
            Bitmap flipped = new Bitmap(original);
            flipped.RotateFlip(RotateFlipType.RotateNoneFlipX);
            selectedTileDisplay.Image = flipped;


        }
        void flipYButton_CheckedChanged(object sender, EventArgs e)
        {

            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked == true)
            {
                flipY = true;
            }
            else
            {
                flipY = false;
            }
            //selectedTileDisplay.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
            Bitmap original = new Bitmap(selectedTileDisplay.Image);
            Bitmap flipped = new Bitmap(original);
            flipped.RotateFlip(RotateFlipType.RotateNoneFlipY);
            selectedTileDisplay.Image = flipped;

        }

        void PriorityButton_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked == true)
            {
                controlBit = true;
            }
            else
            {
                controlBit = false;
            }
        }

        void buttonFloodFill_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked == true)
            {
                floodfill = true;
                swap = false;
                buttonSwap.Checked = false;
                changeTileProperties = false;
                buttonUpdate.Checked = false;
                selectedTileDisplay.Visible = true;
            }
            else
            {
                floodfill = false;
            }
        }

        void buttonSwap_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked == true)
            {
                swap = true;
                floodfill = false;
                buttonFloodFill.Checked = false;
                changeTileProperties = false;
                buttonUpdate.Checked = false;
                selectedTileDisplay.Visible = true;
            }
            else
            {
                swap = false;
            }
        }

        void buttonUpdate_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.Checked == true)
            {
                changeTileProperties = true;
                selectedTileDisplay.Visible = false;
                swap = false;
                buttonSwap.Checked = false;
                buttonFloodFill.Checked = false;
                buttonFloodFill.Checked = false;
            }
            else
            {
                changeTileProperties = false;
                selectedTileDisplay.Visible = true;
            }
        }
        void thumbnail1_Clicked(object sender, EventArgs e)
        {
            if (tileLayout1Details != null)
            {
                changeOfLayout(1);
            }
        }
        void thumbnail2_Clicked(object sender, EventArgs e)
        {
            if (tileLayout2Details != null)
            {
                changeOfLayout(2);
            }
        }
        void thumbnail3_Clicked(object sender, EventArgs e)
        {
            if (tileLayout3Details != null)
            {
                changeOfLayout(3);
            }
        }

        void changeOfLayout(int newLayout)
        {
            //Backup current layout
            switch (activeTileSet)
            {
                case 1:
                    tileLayout1Details = tileLayoutDetails;
                    break;
                case 2:
                    tileLayout2Details = tileLayoutDetails;
                    break;
                case 3:
                    tileLayout3Details = tileLayoutDetails;
                    break;
            }
            //Set new layout as Active
            switch(newLayout)
            {
                case 1:
                    tileLayoutDetails = tileLayout1Details;
                    break;
                case 2:
                    tileLayoutDetails = tileLayout2Details;
                    break;
                case 3:
                    tileLayoutDetails = tileLayout3Details;
                    break;
            }
            activeTileSet = newLayout;
            zoomSize = 64;
            TileLayout.UpdatePictureBoxes(panelTileMap, tileSet, tileLayoutDetails);
        }
    }
}
