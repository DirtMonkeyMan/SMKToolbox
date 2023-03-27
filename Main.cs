using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SMKToolbox
{
    public partial class Main : Form
    {


        public Main()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();


        }

        private void buttonLayoutEditor_Click(object sender, EventArgs e)
        {
            LayoutEditor layoutEditor = new LayoutEditor();
            layoutEditor.Show();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void buttonRankingNumbers_Click(object sender, EventArgs e)
        {
            RankingNumbersEditor rankingEditor = new RankingNumbersEditor();
            rankingEditor.Show();
        }

        private void buttonAssetManager_Click(object sender, EventArgs e)
        {
            AssetManager assetManager = new AssetManager();
        }

        private void buttonDemoEditor_Click(object sender, EventArgs e)
        {
            DemoMaker demoMaker = new DemoMaker();
            demoMaker.Show();
        }
    }
}


