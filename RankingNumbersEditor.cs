using System;
using System.Drawing;
using System.Windows.Forms;

namespace SMKToolbox
{
    public partial class RankingNumbersEditor : Form
    {
        //Update file \Source\kimura\bin\windeco1.bin

        private string fileName = "";
        private byte[] numbersMain;
        private int GridRows = 64;
        private const int GridColumns = 256;
        private int GridSquareSize = 16;
        private const int RadioButtonGroupWidth = 40;
        private TableLayoutPanel mainLayout;
        private Panel gridPanel;

        private bool[,] grid;

        private int activeNumber = 0;

        private RadioButton large1RadioButton;
        private RadioButton large2RadioButton;
        private RadioButton large3RadioButton;
        private RadioButton large4RadioButton;
        private RadioButton large5RadioButton;
        private RadioButton large6RadioButton;
        private RadioButton large7RadioButton;
        private RadioButton large8RadioButton;

        private RadioButton small1RadioButton;
        private RadioButton small2RadioButton;
        private RadioButton small3RadioButton;
        private RadioButton small4RadioButton;
        private RadioButton small5RadioButton;
        private RadioButton small6RadioButton;
        private RadioButton small7RadioButton;
        private RadioButton small8RadioButton;

        ToolStripMenuItem menuSave;
        ToolStripMenuItem menuSaveAs;
        ToolStripMenuItem menuSaveToROM;

        private int gridRowLimit = 64;
        private Panel radioButtonPanel;
        private TableLayoutPanel gridTable;

        byte[] S1 = new byte[0x50];
        byte[] S2 = new byte[0x50];
        byte[] S3 = new byte[0x50];
        byte[] S4 = new byte[0x50];
        byte[] S5 = new byte[0x50];
        byte[] S6 = new byte[0x50];
        byte[] S7 = new byte[0x50];
        byte[] S8 = new byte[0x50];
        byte[] L1 = new byte[0x100];
        byte[] L2 = new byte[0x100];
        byte[] L3 = new byte[0x100];
        byte[] L4 = new byte[0x100];
        byte[] L5 = new byte[0x100];
        byte[] L6 = new byte[0x100];
        byte[] L7 = new byte[0x100];
        byte[] L8 = new byte[0x100];


        public RankingNumbersEditor()
        {



            InitializeComponent();

            grid = new bool[GridRows, GridColumns];

            mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                ColumnStyles = {
                new ColumnStyle(SizeType.Percent, 95),
                new ColumnStyle(SizeType.AutoSize, 40)
                 }
            };
            Controls.Add(mainLayout);

            gridTable = new TableLayoutPanel();
            gridTable.Dock = DockStyle.Fill;

            gridPanel = new Panel
            {
                
                Dock = DockStyle.Fill,
                BackColor = Color.LightGray,
                
                BorderStyle = BorderStyle.FixedSingle
            };
            gridPanel.Size = new Size(GridColumns * GridSquareSize, gridRowLimit * GridSquareSize);

            gridPanel.MouseDown += new MouseEventHandler(GridPanel_MouseDown);
            gridPanel.Paint += new PaintEventHandler(Grid_Paint);

            gridTable.Controls.Add(gridPanel);
            mainLayout.Controls.Add(gridTable);

            radioButtonPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle
            };
            mainLayout.Controls.Add(radioButtonPanel);

            large1RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(19, 10),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "1st",
                Checked = true
            };

            large1RadioButton.CheckedChanged += new EventHandler(Large1RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(large1RadioButton);

            large2RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(19, large1RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "2nd",
                Checked = false
            };

            large2RadioButton.CheckedChanged += new EventHandler(Large2RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(large2RadioButton);


            large3RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(19, large2RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "3rd",
                Checked = false
            };

            large3RadioButton.CheckedChanged += new EventHandler(Large3RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(large3RadioButton);


            large4RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(19, large3RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "4th",
                Checked = false
            };

            large4RadioButton.CheckedChanged += new EventHandler(Large4RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(large4RadioButton);


            large5RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(19, large4RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "5th",
                Checked = false
            };

            large5RadioButton.CheckedChanged += new EventHandler(Large5RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(large5RadioButton);

            large6RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(19, large5RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "6th",
                Checked = false
            };

            large6RadioButton.CheckedChanged += new EventHandler(Large6RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(large6RadioButton);

            large7RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(19, large6RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "7th",
                Checked = false
            };

            large7RadioButton.CheckedChanged += new EventHandler(Large7RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(large7RadioButton);

            large8RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(19, large7RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "8th",
                Checked = false
            };

            large8RadioButton.CheckedChanged += new EventHandler(Large8RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(large8RadioButton);


            small1RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomRight,
                Location = new Point(large1RadioButton.Right, 10),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "1st",
                Checked = false
            };

            small1RadioButton.CheckedChanged += new EventHandler(small1RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(small1RadioButton);

            small2RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomRight,
                Location = new Point(large2RadioButton.Right, small1RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "2nd",
                Checked = false
            };

            small2RadioButton.CheckedChanged += new EventHandler(small2RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(small2RadioButton);


            small3RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomRight,
                Location = new Point(large3RadioButton.Right, small2RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "3rd",
                Checked = false
            };

            small3RadioButton.CheckedChanged += new EventHandler(small3RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(small3RadioButton);



            small4RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomRight,
                Location = new Point(large4RadioButton.Right, small3RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "4th",
                Checked = false
            };

            small4RadioButton.CheckedChanged += new EventHandler(small4RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(small4RadioButton);

            small5RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomRight,
                Location = new Point(large5RadioButton.Right, small4RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "5th",
                Checked = false
            };

            small5RadioButton.CheckedChanged += new EventHandler(small5RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(small5RadioButton);

            small6RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomRight,
                Location = new Point(large6RadioButton.Right, small5RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "6th",
                Checked = false
            };

            small6RadioButton.CheckedChanged += new EventHandler(small6RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(small6RadioButton);

            small7RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomRight,
                Location = new Point(large7RadioButton.Right, small6RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "7th",
                Checked = false
            };

            small7RadioButton.CheckedChanged += new EventHandler(small7RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(small7RadioButton);

            small8RadioButton = new RadioButton
            {
                Appearance = Appearance.Button,
                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomRight,
                Location = new Point(large7RadioButton.Right, small7RadioButton.Bottom),
                Height = RadioButtonGroupWidth,
                Width = RadioButtonGroupWidth * 2,
                Text = "8th",
                Checked = false
            };

            small8RadioButton.CheckedChanged += new EventHandler(small8RadioButton_CheckedChanged);
            radioButtonPanel.Controls.Add(small8RadioButton);

            Button zoomInButton = new Button
            {

                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(19, large8RadioButton.Bottom),
                Height = large7RadioButton.Height,
                Width = large7RadioButton.Width,
                Text = "Zoom In"
                
            };

            zoomInButton.Click += new EventHandler(zoomInButton_Clicked);
            radioButtonPanel.Controls.Add(zoomInButton);

            Button zoomOutButton = new Button
            {

                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(zoomInButton.Right, large8RadioButton.Bottom),
                Height = large7RadioButton.Height,
                Width = large7RadioButton.Width,
                Text = "Zoom Out"

            };

            zoomOutButton.Click += new EventHandler(zoomOutButton_Clicked);
            radioButtonPanel.Controls.Add(zoomOutButton);



            int gridSize = Math.Min(gridPanel.Height, gridPanel.Width);
            GridSquareSize = gridSize / GridRows;



            gridPanel.Invalidate();
            if (GridColumns * GridSquareSize > gridTable.Width)
            {
                gridTable.AutoScroll = true;
            }
            else
            {
                gridTable.AutoScroll = false;
            }
            ProcessArray(L1, true);

            MenuStrip mainMenu = new MenuStrip();
            ToolStripMenuItem menuFile = new ToolStripMenuItem("File");
            ToolStripMenuItem menuROM = new ToolStripMenuItem("ROM");
            ToolStripMenuItem menuOpen = new ToolStripMenuItem("Open");
            ToolStripMenuItem menuLoadFromROM = new ToolStripMenuItem("Load from ROM");
            menuSave = new ToolStripMenuItem("Save");
            menuSave.Enabled = false;
            menuSaveAs = new ToolStripMenuItem("Save As");
            menuSaveAs.Enabled = false;
            menuSaveToROM = new ToolStripMenuItem("Save to ROM");
            menuSaveToROM.Enabled = false;
            ToolStripMenuItem menuHelp = new ToolStripMenuItem("Help");
            ToolStripMenuItem menuAbout = new ToolStripMenuItem("About");


            mainMenu.Items.Add(menuFile);
            menuFile.ShortcutKeys = Keys.Control | Keys.F;
            menuSave.ShortcutKeys = Keys.Control | Keys.S;
            menuSaveAs.ShortcutKeys = Keys.Control | Keys.A;
            menuSave.Click += new EventHandler(menuSave_Click);
            menuSaveAs.Click += new EventHandler(menuSave_Click);
            menuOpen.Click += new EventHandler(menuOpen_Click);
            menuOpen.ShortcutKeys = Keys.Control | Keys.O;
            
            mainMenu.Items.Add(menuROM);
            menuLoadFromROM.Click += new EventHandler(menuLoadFromROM_Click);
            menuSaveToROM.Click += new EventHandler(menuSave_Click);


            mainMenu.Items.Add(menuHelp);
            menuAbout.Click += new EventHandler(menuAbout_Click);



            menuFile.DropDownItems.Add(menuOpen);
            menuFile.DropDownItems.Add(menuSave);
            menuFile.DropDownItems.Add(menuSaveAs);
            menuROM.DropDownItems.Add(menuLoadFromROM);
            menuROM.DropDownItems.Add(menuSaveToROM);
            menuHelp.DropDownItems.Add(menuAbout);

            //menuHelp.Click += new EventHandler(menuHelp_Click);

            mainLayout.Visible = false;
            this.Controls.Add(mainMenu);
            this.MainMenuStrip = mainMenu;
            
        }

        private void Grid_Paint(object sender, PaintEventArgs e)
        {
            mainLayout.SuspendLayout();
            gridPanel.SuspendLayout();
            // Use a BufferedGraphics object for double buffering
            BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
            //BufferedGraphics myBuffer = currentContext.Allocate(e.Graphics, e.ClipRectangle);
            Rectangle newSize = new Rectangle(0, 0, GridColumns * GridSquareSize, GridRows * GridSquareSize);

            BufferedGraphics myBuffer = currentContext.Allocate(e.Graphics, newSize);
            myBuffer.Graphics.Clear(Color.White);

            using (var pen = new Pen(Color.LightGray))
            {
                for (int i = 0; i < GridRows; i++)
                {
                    int y = i * GridSquareSize;
                    myBuffer.Graphics.DrawLine(pen, 0, y, GridColumns * GridSquareSize, y);
                }
                for (int i = 0; i < GridColumns; i++)
                {
                    int x = i * GridSquareSize;
                    myBuffer.Graphics.DrawLine(pen, x, 0, x, GridRows * GridSquareSize);
                }
            }
            for (int row = 0; row < GridRows; row++)
            {
                for (int col = 0; col < GridColumns; col++)
                {
                    if (grid[row, col] == true)
                    {
                        myBuffer.Graphics.FillRectangle(Brushes.Black, col * GridSquareSize, row * GridSquareSize, GridSquareSize, GridSquareSize);
                    }
                }
            }


            // Render the double buffer to the screen
            myBuffer.Render(e.Graphics);
            myBuffer.Dispose();
            gridPanel.ResumeLayout();
            mainLayout.ResumeLayout();
        }



        private void Large1RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            
            if (large1RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 64;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(L1, true);
                resize();

                gridPanel.Invalidate();
            }
        }

        private void Large2RadioButton_CheckedChanged(object sender, EventArgs e)
        {
 
            if (large2RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 64;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(L2, true);
                resize();
                gridPanel.Invalidate();
            }
        }
        private void Large3RadioButton_CheckedChanged(object sender, EventArgs e)
        {

            if (large3RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 64;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(L3, true);
                resize();
                gridPanel.Invalidate();
            }
        }
        private void Large4RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (large4RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 64;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(L4, true);
                resize();
                gridPanel.Invalidate();
            }
        }
        private void Large5RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (large5RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 64;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(L5, true);
                resize();
                gridPanel.Invalidate();
            }
        }
        private void Large6RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (large6RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 64;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(L6, true);
                resize();
                gridPanel.Invalidate();
            }
        }
        private void Large7RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (large7RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 64;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(L7, true);
                resize();
                gridPanel.Invalidate();
            }
        }
        private void Large8RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (large8RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 64;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(L8, true);
                resize();
                gridPanel.Invalidate();
            }
        }

        private void small1RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (small1RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 20;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(S1, false);
                resize();
                gridPanel.Invalidate();
            }
        }
        private void small2RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (small2RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 20;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(S2, false);
                resize();
                gridPanel.Invalidate();
            }
        }
        private void small3RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (small3RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 20;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(S3, false);
                resize();
                gridPanel.Invalidate();
            }
        }
        private void small4RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (small4RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 20;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(S4, false);
                resize();
                gridPanel.Invalidate();
            }
        }
        private void small5RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (small5RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 20;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(S5, false);
                resize();
                gridPanel.Invalidate();
            }
        }
        private void small6RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (small6RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 20;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(S6, false);
                resize();
                gridPanel.Invalidate();
            }
        }

        private void small7RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (small7RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 20;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(S7, false);
                resize();
                gridPanel.Invalidate();
            }
        }
        private void small8RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (small8RadioButton.Checked)
            {
                saveCurrentEdit();
                GridRows = 20;
                gridRowLimit = GridRows;
                Array.Clear(grid, 0, grid.Length);
                ProcessArray(S8, false);
                resize();
                gridPanel.Invalidate();
            }
        }

        private void zoomInButton_Clicked(object sender, EventArgs e)
        {
            if (GridSquareSize < 40)
            {
                GridSquareSize = GridSquareSize + 1;
                resize();
            }
        }
        private void zoomOutButton_Clicked(object sender, EventArgs e)
        {
            if (GridSquareSize > 1)
            {
                GridSquareSize = GridSquareSize - 1;
                resize();

            }
        }




        public void SetGridSquares(int row, int startCol, int endCol, int startCol2, int endCol2)
        {
            for (int col = startCol; col <= endCol; col++)
            {
                grid[row, col] = true;
            }
            for (int col = startCol2; col <= endCol2; col++)
            {
                grid[row, col] = true;
            }
            gridPanel.Invalidate();
        }


            private void resize()
            {
                if (gridPanel != null)
                {
                int scrollV = gridTable.VerticalScroll.Value;
                int scrollH = gridTable.HorizontalScroll.Value;
                
                gridTable.AutoScroll = false;
                gridPanel.Size = new Size(GridColumns * GridSquareSize, GridRows * GridSquareSize);
                gridTable.VerticalScroll.Value = scrollV;
                gridTable.HorizontalScroll.Value = scrollH;
                gridTable.AutoScroll = true;
                gridPanel.Invalidate();
                }
            }
        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);

            if (gridPanel != null)
            {
                resize();
            }
        }
        /*
                private void GridPanel_MouseDown(object sender, MouseEventArgs e)
                {
                    int x = e.X / GridSquareSize;
                    int y = e.Y / GridSquareSize;

                    if (y >= 0 && y < GridRows && x >= 0 && x < GridColumns)
                    {
                        int countInRow = 0;
                        int rowStart = y;
                        int rowEnd = y;
                        int setCount = 0;
                        bool inSet = false;

                        if (grid[y, x] == false)
                        {
                            grid[y, x] = true;

                            for (int i = 0; i < GridColumns; i++)
                            {
                                if (grid[y, i] == true)
                                {
                                    if (!inSet)
                                    {
                                        setCount++;
                                        inSet = true;
                                    }
                                }
                                else
                                {
                                    inSet = false;
                                }
                            }

                            if (setCount >= 3)
                            {
                                grid[y, x] = false;
                                return;
                            }
                        }
                        else
                        {
                            grid[y, x] = false;
                            for (int i = 0; i < GridColumns; i++)
                            {
                                if (grid[y, i] == true)
                                {
                                    if (!inSet)
                                    {
                                        setCount++;
                                        inSet = true;
                                    }
                                }
                                else
                                {
                                    inSet = false;
                                }
                            }

                            if (setCount >= 3)
                            {
                                grid[y, x] = true;
                                return;
                            }
                        }

                        gridPanel.Invalidate();
                    }
                }

                */

        private void GridPanel_MouseDown(object sender, MouseEventArgs e)
        {
            int x = e.X / GridSquareSize;
            int y = e.Y / GridSquareSize;

            if (y >= 0 && y < GridRows && x >= 0 && x < GridColumns)
            {
                int countInRow = 0;
                int rowStart = y;
                int rowEnd = y;
                int setCount = 0;
                bool inSet = false;

                if (e.Button == MouseButtons.Left)
                {
                    if (grid[y, x] == false)
                    {
                        grid[y, x] = true;

                        for (int i = 0; i < GridColumns; i++)
                        {
                            if (grid[y, i] == true)
                            {
                                if (!inSet)
                                {
                                    setCount++;
                                    inSet = true;
                                }
                            }
                            else
                            {
                                inSet = false;
                            }
                        }

                        if (setCount >= 3)
                        {
                            grid[y, x] = false;
                            return;
                        }
                    }
                    else
                    {
                        grid[y, x] = false;
                        for (int i = 0; i < GridColumns; i++)
                        {
                            if (grid[y, i] == true)
                            {
                                if (!inSet)
                                {
                                    setCount++;
                                    inSet = true;
                                }
                            }
                            else
                            {
                                inSet = false;
                            }
                        }

                        if (setCount >= 3)
                        {
                            grid[y, x] = true;
                            return;
                        }
                    }

                    gridPanel.Invalidate();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    int squareBefore = 0;
                    if (x != 0)
                    {
                        squareBefore = x - 1;
                    }    
                    bool currentValue = grid[y, squareBefore];

                    for (int i = x; i < GridColumns; i++)
                    {
                        if (grid[y, i] == currentValue)
                        {
                            break;
                        }
                        else
                        {
                            grid[y, i] = !grid[y, i];
                        }
                    }

                    gridPanel.Invalidate();
                }
            }
        }

        private void setupNumbers(byte[] numbersMain)
        {
            try
            {
                Array.Copy(numbersMain, 0x0000, S1, 0, 0x50);
                Array.Copy(numbersMain, 0x0050, S2, 0, 0x50);
                Array.Copy(numbersMain, 0x00A0, S3, 0, 0x50);
                Array.Copy(numbersMain, 0x00F0, S4, 0, 0x50);
                Array.Copy(numbersMain, 0x0140, S5, 0, 0x50);
                Array.Copy(numbersMain, 0x0190, S6, 0, 0x50);
                Array.Copy(numbersMain, 0x01E0, S7, 0, 0x50);
                Array.Copy(numbersMain, 0x0230, S8, 0, 0x50);
                Array.Copy(numbersMain, 0x0280, L1, 0, 0x100);
                Array.Copy(numbersMain, 0x0380, L2, 0, 0x100);
                Array.Copy(numbersMain, 0x0480, L3, 0, 0x100);
                Array.Copy(numbersMain, 0x0580, L4, 0, 0x100);
                Array.Copy(numbersMain, 0x0680, L5, 0, 0x100);
                Array.Copy(numbersMain, 0x0780, L6, 0, 0x100);
                Array.Copy(numbersMain, 0x0880, L7, 0, 0x100);
                Array.Copy(numbersMain, 0x0980, L8, 0, 0x100);
            }
            catch(Exception Ex)
            {
                MessageBox.Show("Failed to process file, are you sure it's decompressed?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

        }

        private void ProcessArray(byte[] S, bool Large)
        {
            int size = 20;
            if(Large)
            {
                size = 64;
            }    

            for (int i = 0; i < size; i++)
            {
                int end1 = S[i*4+1];
                int start1 = S[i*4];
                int end2 = S[i*4+ 3];
                int start2 = S[i*4+ 2];

                    SetGridSquares(i, start1, end1, start2, end2);

             
            }
        }

        private void saveCurrentEdit()
        {
            //Save the current edit
            switch (activeNumber)
            {
                case 1:
                    UpdateArray(grid, S1);
                    break;
                case 2:
                    UpdateArray(grid, S2);
                    break;
                case 3:
                    UpdateArray(grid, S3);
                    break;
                case 4:
                    UpdateArray(grid, S4);
                    break;
                case 5:
                    UpdateArray(grid, S5);
                    break;
                case 6:
                    UpdateArray(grid, S6);
                    break;
                case 7:
                    UpdateArray(grid, S7);
                    break;
                case 8:
                    UpdateArray(grid, S8);
                    break;
                case 9:
                    UpdateArray(grid, L1);
                    break;
                case 10:
                    UpdateArray(grid, L2);
                    break;
                case 11:
                    UpdateArray(grid, L3);
                    break;
                case 12:
                    UpdateArray(grid, L4);
                    break;
                case 13:
                    UpdateArray(grid, L5);
                    break;
                case 14:
                    UpdateArray(grid, L6);
                    break;
                case 15:
                    UpdateArray(grid, L7);
                    break;
                case 16:
                    UpdateArray(grid, L8);
                    break;
            }
            Console.WriteLine("Saving number " + activeNumber);
            //Set the new activeNumber so next save will work
            if (small1RadioButton.Checked)
                activeNumber = 1;
            else if (small2RadioButton.Checked)
                activeNumber = 2;
            else if (small3RadioButton.Checked)
                activeNumber = 3;
            else if (small4RadioButton.Checked)
                activeNumber = 4;
            else if (small5RadioButton.Checked)
                activeNumber = 5;
            else if (small6RadioButton.Checked)
                activeNumber = 6;
            else if (small7RadioButton.Checked)
                activeNumber = 7;
            else if (small8RadioButton.Checked)
                activeNumber = 8;
            else if (large1RadioButton.Checked)
                activeNumber = 9;
            else if (large2RadioButton.Checked)
                activeNumber = 10;
            else if (large3RadioButton.Checked)
                activeNumber = 11;
            else if (large4RadioButton.Checked)
                activeNumber = 12;
            else if (large5RadioButton.Checked)
                activeNumber = 13;
            else if (large6RadioButton.Checked)
                activeNumber = 14;
            else if (large7RadioButton.Checked)
                activeNumber = 15;
            else if (large8RadioButton.Checked)
                activeNumber = 16;
            

        }

        private byte[] UpdateArray(bool[,] grid, byte[] updatedArray)
        {
            int size = updatedArray.Length;

            for (int i = 0; i < GridRows; i++)
            {
                int startCol = -1, endCol = -1, startCol2 = -1, endCol2 = -1;
                bool rangeStart = false;
                bool rangeEnd = false;
                bool rangeStart2 = false;
                bool rangeEnd2 = false;


                for (int j = 0; j < GridColumns; j++)
                {
                    if (grid[i, j] == true)
                    {
                        if (!rangeStart && !rangeEnd)
                        {
                            startCol = j;
                            rangeStart = true;
                        }
                        if (!rangeEnd)
                        { endCol = j; }

                        if(rangeEnd)
                        {
                            if (!rangeStart2 && !rangeEnd2)
                            {
                                startCol2 = j;
                                rangeStart2 = true;
                            }
                            if (!rangeEnd2)
                            { endCol2 = j; }
                        }

                    }


                    else if (grid[i, j] == false && rangeStart)
                    {
                        rangeEnd = true;
                    }


                    if (startCol == -1)
                    {
                        updatedArray[i * 4] = 1;
                        updatedArray[i * 4 + 1] = 0;

                    }
                    else
                    {
                        updatedArray[i * 4] = (byte)startCol;
                        updatedArray[i * 4 + 1] = (byte)endCol;
                    }
                    if (startCol2 == -1)
                    {
                        updatedArray[i * 4 + 2] = 1;
                        updatedArray[i * 4 + 3] = 0;
                    }
                    else
                    {
                        updatedArray[i * 4 + 2] = (byte)startCol2;
                        updatedArray[i * 4 + 3] = (byte)endCol2;
                    }
                }
               
            }
            return (updatedArray);
        }

        public void menuLoadFromROM_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            //openFileDialog.FileName = "windeco1.bin";  //Maybe default to this if we decompress in the future
            openFileDialog.Filter = "All files (*.*)|*.*|SFC files (*.SFC)|*.SFC";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                (byte[] numbers, bool status) = Files.DecompressFromROM(openFileDialog.FileName, 386928);
                if (status)
                {
                    fileName = null; //Update filename for saving later
                    numbersMain = numbers;
                    setupNumbers(numbers);
                    GridRows = 64;
                    gridRowLimit = GridRows;
                    Array.Clear(grid, 0, grid.Length);
                    ProcessArray(L1, true);
                    resize();
                    gridPanel.Invalidate();
                    ProcessArray(L1, false);
                    large1RadioButton.Checked = true;
                    activeNumber = 9;
                    mainLayout.Visible = true;
                    menuSave.Enabled = true;
                    menuSaveAs.Enabled = true;
                    menuSaveToROM.Enabled = true;
                }
            }
        }

        public void menuOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            //openFileDialog.FileName = "windeco1.bin";  //Maybe default to this if we decompress in the future
            openFileDialog.Filter = "All files (*.*)|*.*|BIN files (*.BIN)|*.BIN";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                (byte[] numbers, bool status) = Files.Read(openFileDialog.FileName);
                if (status)
                {
                    fileName = openFileDialog.FileName; //Update filename for saving later
                    numbersMain = numbers;
                    setupNumbers(numbers);
                    GridRows = 64;
                    gridRowLimit = GridRows;
                    Array.Clear(grid, 0, grid.Length);
                    ProcessArray(L1, true);
                    resize();
                    gridPanel.Invalidate();
                    ProcessArray(L1, false);
                    large1RadioButton.Checked = true;
                    activeNumber = 9;
                    mainLayout.Visible = true;
                    menuSave.Enabled = true;
                    menuSaveAs.Enabled = true;
                    menuSaveToROM.Enabled = true;
                }
            }
        }


        public void menuSave_Click(object sender, EventArgs e)
        {
            //Make sure tileLayout1 is upto date
            saveCurrentEdit();

            byte[] combinedArray = new byte[2688];

            int currentIndex = 0;
            Array.Copy(S1, 0, combinedArray, currentIndex, S1.Length);
            currentIndex += S1.Length;
            Array.Copy(S2, 0, combinedArray, currentIndex, S2.Length);
            currentIndex += S2.Length;
            Array.Copy(S3, 0, combinedArray, currentIndex, S3.Length);
            currentIndex += S3.Length;
            Array.Copy(S4, 0, combinedArray, currentIndex, S4.Length);
            currentIndex += S4.Length;
            Array.Copy(S5, 0, combinedArray, currentIndex, S5.Length);
            currentIndex += S5.Length;
            Array.Copy(S6, 0, combinedArray, currentIndex, S6.Length);
            currentIndex += S6.Length;
            Array.Copy(S7, 0, combinedArray, currentIndex, S7.Length);
            currentIndex += S7.Length;
            Array.Copy(S8, 0, combinedArray, currentIndex, S8.Length);
            currentIndex += S8.Length;
            Array.Copy(L1, 0, combinedArray, currentIndex, L1.Length);
            currentIndex += L1.Length;
            Array.Copy(L2, 0, combinedArray, currentIndex, L2.Length);
            currentIndex += L2.Length;
            Array.Copy(L3, 0, combinedArray, currentIndex, L3.Length);
            currentIndex += L3.Length;
            Array.Copy(L4, 0, combinedArray, currentIndex, L4.Length);
            currentIndex += L4.Length;
            Array.Copy(L5, 0, combinedArray, currentIndex, L5.Length);
            currentIndex += L5.Length;
            Array.Copy(L6, 0, combinedArray, currentIndex, L6.Length);
            currentIndex += L6.Length;
            Array.Copy(L7, 0, combinedArray, currentIndex, L7.Length);
            currentIndex += L7.Length;
            Array.Copy(L8, 0, combinedArray, currentIndex, L8.Length);
            currentIndex += L8.Length;

            if (sender.ToString() == "Save" && fileName != null)
            {
                Files.Write(fileName, combinedArray, true);
            }
            if (sender.ToString() == "Save As" || (sender.ToString() == "Save" && fileName == null))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Application.StartupPath;
                saveFileDialog.FileName = fileName;
                saveFileDialog.Filter = "BIN files (*.BIN)|*.BIN|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;


                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                { return; }
                else
                {
                    Files.Write(saveFileDialog.FileName, combinedArray, true);
                    fileName = saveFileDialog.FileName; //update filename for next save.
                }    
            }
            if (sender.ToString() == "Save to ROM")
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = Application.StartupPath;
                //openFileDialog.FileName = "windeco1.bin";  //Maybe default to this if we decompress in the future
                openFileDialog.Filter = "All files (*.*)|*.*|SFC files (*.SFC)|*.SFC";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bool status = Files.CompressAndInsert(combinedArray, 386928, 1890, openFileDialog.FileName);
                    if(status)
                    {
                        MessageBox.Show("Compressed and insert into ROM", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

        }

        public void menuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Writen by Dirtbag, with thanks to ice3 and MrL314 for reverse engineering the format.\n\nInstructions:\n\n - To draw a number, simply left click on the desired square.\n\n - To remove a filled square, left click on the square again.\n\n - It is important to note that a row is limited to no more than two lines\n\n - Using the Right will click to Fill or Remove between two points on the same Row\n\n - To fill or delete between two points, right click on the square located next to the desired starting point.\n\n - The value of the square will be replicate to its right until it reaches a square of the same value or the end of the row.\n\n - If the square to the right of your starting point is filled, the application will delete the lines between the two points.\n\n - If the square to the right of your starting point is empty, the application will fill the squares between the two points with lines.\n\n - Please note that the application will still enforce the two - line limit per row, even when using the right-click fill / delete feature.", "SMK Ranking Number Editor");
        }

    }
}