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
        Panel panelPalettes;
        TableLayoutPanel panelRight;
        Panel panelTilePicker;
        Label displayPaletteNumber;
        CheckBox buttonY;
        CheckBox buttonX;
        CheckBox buttonP;
        PictureBox thumbnail1;
        PictureBox thumbnail2;
        PictureBox thumbnail3;

        //Variables for tile
        int tileIndex = 0;
        int paletteSet = 0;
        bool flipX = false;
        bool flipY = false;
        bool controlBit = false;

        bool hasBeenRotated = false;

        //Arrays for loading raw data
        byte[] RawPalette;
        byte[] rawTiles;
        byte[] Layout1Raw;
        byte[] Layout2Raw;
        byte[] Layout3Raw;

        int[][] palletArraySet;
        Bitmap[][] tileSet;
        TileLayout.TileDetails[] tileLayoutDetails;
        PictureBox selectedTileDisplay;

        ToolStripMenuItem menuSaveAsLayout;

        public LayoutEditor()
        {
            SetupLayout();
            SetupMenu();
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            this.Size = new Size(1020, 1070);
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            //this.ResizeEnd += new System.EventHandler(this.resizeTilemap, this.resizePalettes) ;
            // this.ResizeEnd += (sender, e) => { resizeTilemap(sender, e); resizeTilePicker(sender, e); resizePalettes(sender, e); };
           // this.SizeChanged += (sender, e) => { resizeTilemap(sender, e); resizeTilePicker(sender, e); resizePalettes(sender, e); };
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
            mainTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75f));
            mainTablePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            tilemapTablePanel = new TableLayoutPanel();

            tilemapTablePanel.SuspendLayout();

            tilemapTablePanel.Dock = DockStyle.Fill;
            tilemapTablePanel.RowCount = 1;
            tilemapTablePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            

            panelTileMap = new Panel();
            panelTileMap.Dock = DockStyle.Fill;
            panelTileMap.BorderStyle = BorderStyle.FixedSingle;

            panelTileMap.Width = tilemapTablePanel.Width;
            panelTileMap.Height = tilemapTablePanel.Height;

            int tilemapWidth = mainTablePanel.Width;
            int tilemapHeight = mainTablePanel.Height;
            int tileMapAreaSize = tilemapWidth /9;
            

            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Size = new Size(tileMapAreaSize, tileMapAreaSize),
                        Location = new Point(x * tileMapAreaSize, y * tileMapAreaSize),
                       // BorderStyle = BorderStyle.FixedSingle,
                       //Image = Image.FromFile("temp.bmp"),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Tag = y * 32 + x
                    };
                    pictureBox.MouseClick += new MouseEventHandler(tileLayout_Click);
                    panelTileMap.Controls.Add(pictureBox);
                }
            }

            if (tileMapAreaSize * 32 > panelTileMap.Height)
            {
                panelTileMap.AutoScroll = true;
            }
            tilemapTablePanel.Controls.Add(panelTileMap, 0, 0);
            mainTablePanel.Controls.Add(tilemapTablePanel, 0, 0);
            
            tilemapTablePanel.ResumeLayout();


            panelRight = new TableLayoutPanel();
            panelRight.Dock = DockStyle.Fill;
            panelRight.RowCount = 2;
            panelRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            panelRight.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

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
                }
            }

            // Check if the panel is too small to fit all the picture boxes
            // and add the scrollbar if necessary
            if (tilePickerBoxSize * 32 > panelTilePicker.Height)
            {
                panelTilePicker.AutoScroll = true;
            }
            else
            {
                panelTilePicker.AutoScroll = false;
            }

            panelPalettes = new Panel();
            panelPalettes.Dock = DockStyle.Fill;
            panelPalettes.BorderStyle = boarderSetting;
            panelRight.Controls.Add(panelPalettes, 0, 1);
            // Add the panel to the form
            // this.Controls.Add(panelPalettes);
            //Populate with Pictureboxes
            int palettePickerBoxSize = (int)(panelPalettes.Width / 14);
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    PictureBox palettePickerBox = new PictureBox
                    {
                        Size = new Size(palettePickerBoxSize, palettePickerBoxSize),
                        Location = new Point(x * palettePickerBoxSize, y * palettePickerBoxSize),
                        BorderStyle = BorderStyle.None,
                        // Image = Image.FromFile("temp.bmp"),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Tag = y * 16 + x
                    };
                    panelPalettes.Controls.Add(palettePickerBox);
                }
            }
            if (palettePickerBoxSize * 32 > panelPalettes.Height)
            {
                panelPalettes.AutoScroll = true;
            }
            else
            {
                panelPalettes.AutoScroll = false;
            }


            panelRight.Controls.Add(panelTilePicker, 0, 0);
            

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
            thumbnail1.SizeMode = PictureBoxSizeMode.StretchImage;
            thumbnail1.BackColor = Color.Blue;
            Bottom1.Controls.Add(thumbnail1);

            Panel Bottom2 = new Panel();
            Bottom2.Dock = DockStyle.Fill;
            Bottom2.BorderStyle = boarderSetting;

            thumbnail2 = new PictureBox();
            thumbnail2.Dock = DockStyle.Fill;
            thumbnail2.Image = new Bitmap(1, 1);
            thumbnail2.SizeMode = PictureBoxSizeMode.StretchImage;
            thumbnail2.BackColor = Color.Green;
            Bottom2.Controls.Add(thumbnail2);

            Panel Bottom3 = new Panel();
            Bottom3.Dock = DockStyle.Fill;
            Bottom3.BorderStyle = boarderSetting;

            thumbnail3 = new PictureBox();
            thumbnail3.Dock = DockStyle.Fill;
            thumbnail3.Image = new Bitmap(1, 1);
            thumbnail3.SizeMode = PictureBoxSizeMode.StretchImage;
            thumbnail3.BackColor = Color.Red;
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
                BorderStyle = BorderStyle.Fixed3D,
            };

            Bitmap bmp = new Bitmap(Math.Min(controls.Width, controls.Height), Math.Min(controls.Width, controls.Height), System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);
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
                AutoSize = false,
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
            ToolStripMenuItem menuPalettes = new ToolStripMenuItem("Palettes");
            ToolStripMenuItem menuTiles = new ToolStripMenuItem("Tiles");
            ToolStripMenuItem menuLayouts = new ToolStripMenuItem("Layouts");
            ToolStripMenuItem menuLoadLayout = new ToolStripMenuItem("Load Layout");
            menuSaveAsLayout = new ToolStripMenuItem("Save As");
            ToolStripMenuItem menuHelp = new ToolStripMenuItem("Help");
            ToolStripMenuItem menuAbout = new ToolStripMenuItem("About");
            mainMenu.Items.Add(menuPalettes);
            menuPalettes.ShortcutKeys = Keys.Control | Keys.P;
            menuPalettes.Click += new EventHandler(menuPalettes_Click);
            mainMenu.Items.Add(menuTiles);
            menuTiles.Click += new EventHandler(menuTiles_Click);
            mainMenu.Items.Add(menuLayouts);
            menuLayouts.DropDownItems.Add(menuLoadLayout);
            menuLoadLayout.Click += new EventHandler(menuLoadLayout_Click);
            menuLayouts.DropDownItems.Add(menuSaveAsLayout);
            menuSaveAsLayout.Enabled = false;
            menuSaveAsLayout.Click += new EventHandler(menuSaveAsLayout_Click);
            mainMenu.Items.Add(menuHelp);
            menuHelp.DropDownItems.Add(menuAbout);
            menuAbout.Click += new EventHandler(menuAbout_Click);

            this.Controls.Add(mainMenu);
            this.MainMenuStrip = mainMenu;
            
        }

        private void menuTiles_Click(object sender, EventArgs e)
        {
            if (palletArraySet == null)
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
                    (byte[] TilesRaw, bool status3) = Files.Read(openFileDialog.FileName);
                    rawTiles = TilesRaw;
                    tileSet = Tiles.DrawTiles(TilesRaw, palletArraySet);
                    TileLayout.UpdateTilePickerBoxes(panelTilePicker, tileSet, paletteSet);

                    panelTilePicker.SuspendLayout();
                    tileSet = Tiles.DrawTiles(rawTiles, palletArraySet);
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
                    panelTilePicker.ResumeLayout();


                    if (tileLayoutDetails != null)
                    {
                        tilemapTablePanel.SuspendLayout();
                        panelTileMap.SuspendLayout();
                        TileLayout.UpdatePictureBoxes(panelTileMap, tileSet, tileLayoutDetails);
                        thumbnail1.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails); //update thumbnail
                        tilemapTablePanel.ResumeLayout();
                        panelTileMap.ResumeLayout();

                    }
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
                palletArraySet = Palettes.ConvertTo2DArray(NewPalette);

                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        int color = palletArraySet[y][x];
                        PictureBox palettePickerBox = panelPalettes.Controls[x + (y * 16)] as PictureBox;
                        palettePickerBox.BackColor = Color.FromArgb(color);
                    }
                }
                //Check if tiles &/or layoutare already loaded and refresh
                if(tileSet != null)
                {
                    panelTilePicker.SuspendLayout();
                    tileSet = Tiles.DrawTiles(rawTiles, palletArraySet);
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
                    tilemapTablePanel.SuspendLayout();
                    panelTileMap.SuspendLayout();
                    TileLayout.UpdatePictureBoxes(panelTileMap, tileSet, tileLayoutDetails);
                    thumbnail1.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails); //update thumbnail
                    tilemapTablePanel.ResumeLayout();
                    panelTileMap.ResumeLayout();

                }
                panelPalettes.ResumeLayout();
            }
        }

        private void menuLoadLayout_Click(object sender, EventArgs e)
        {
            if(palletArraySet == null)
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
                    tilemapTablePanel.SuspendLayout();
                    panelTileMap.SuspendLayout();
                    // Code to load the file into a variable

                    (byte[] LayoutsRaw, bool status4) = Files.Read(openFileDialog.FileName);
                    TileLayout tileLayout = new TileLayout();
                    tileLayoutDetails = TileLayout.GetTileDetails(LayoutsRaw);
                    TileLayout.UpdatePictureBoxes(panelTileMap, tileSet, tileLayoutDetails);
                    tilemapTablePanel.ResumeLayout();
                    panelTileMap.ResumeLayout();
                    menuSaveAsLayout.Enabled = true;
                    thumbnail1.Image=TileLayout.DisplayTiles(tileSet, tileLayoutDetails);

                }
            }
        }

        private void menuSaveAsLayout_Click(object sender, EventArgs e)
        {
            if (tileLayoutDetails == null)
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
                byte[] saveData = TileLayout.EncodeTileDetails(tileLayoutDetails);
                Files.Write(saveFileDialog.FileName, saveData, true);
            }
        }



        private void menuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tile Layer Editor 0.0.2\n\nPart of the SMKTool Box suite of tools by Dirtbag for Project L", "Tile Layer Editor", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        //Start of resize
        private void resizeTilemap(object sender, EventArgs e)
        {
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
        {
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
            panelPalettes.ResumeLayout();
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
                    thumbnail1.Image = TileLayout.DisplayTiles(tileSet, tileLayoutDetails);
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
                Console.WriteLine("Tile Location: " + tileIndex.ToString());
                selectedTileDisplay.Image = tileSet[paletteSet][tileIndex];
            }
        }

        void SelectedTileDisplay_Paint(object sender, PaintEventArgs e)
        {
            PictureBox TileDisplay = (PictureBox)sender;
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.DrawImage(TileDisplay.Image, 0, 0, TileDisplay.Width -1, TileDisplay.Height -1);
        }
        void paletteUpButton_Click (object sender, EventArgs e)
        {
            Console.WriteLine("Palette Up Clicked");
            if (paletteSet < 7 && tileSet != null)
            {
                paletteSet = paletteSet + 1;
                displayPaletteNumber.Text = paletteSet.ToString();
                TileLayout.UpdateTilePickerBoxes(panelTilePicker, tileSet, paletteSet);
                selectedTileDisplay.Image = tileSet[paletteSet][tileIndex];
            }
        }
        void paletteDownButton_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Palette Down Clicked");
            if (paletteSet > 0 && tileSet != null)
            {
                paletteSet = paletteSet - 1;
                displayPaletteNumber.Text = paletteSet.ToString();
                TileLayout.UpdateTilePickerBoxes(panelTilePicker, tileSet, paletteSet);
                selectedTileDisplay.Image = tileSet[paletteSet][tileIndex];
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
            if(controlBit)
            {
                Console.WriteLine("before");
            }
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

            if (controlBit)
            {
                Console.WriteLine("after");
            }
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
    }
}
