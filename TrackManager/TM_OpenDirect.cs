using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using static SMKToolbox.TrackManager;

namespace SMKToolbox
{
    public partial class TM_OpenDirect : Form
    {
        TrackManager FormOwner;

        public TM_OpenDirect(TrackManager owner)
        {
            InitializeComponent();

            FormOwner = owner;
        }

        private void buttonBrowseGeneral_Click(object sender, EventArgs e)
        {
            // MessageBox.Show(sender.ToString());
            string filter;
            Button bsender = (Button)sender;
            System.Windows.Forms.TextBox targetTextBox;

            switch (bsender.Text) {
                case "Browse Screen":
                    filter = "All Files (*.*)|*.*|Mode 7 Screens (*.SCR)|*.SCR";
                    targetTextBox = this.textM7ScreenPath;
                    break;
                case "Browse Graphics":
                    filter = "All Files (*.*)|*.*|Mode 7 Graphics (*.CM7)|*.CM7";
                    targetTextBox = this.textM7Graphics;
                    break;
                case "Browse Palette":
                    filter = "All Files (*.*)|*.*|Color Palettes (*.COL)|*.COL";
                    targetTextBox = this.textColorPalette;
                    break;
                case "Browse Common":
                    filter = "All Files (*.*)|*.*|Mode 7 Graphics (*.CM7)|*.CM7";
                    targetTextBox = this.textCommon;
                    break;
                default:
                    filter = "All Files (*.*)|*.*|Super Family Computer ROMs (*.SFC)|*.SFC";
                    targetTextBox = this.textM7ScreenPath;
                    break;
            }

            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = Application.StartupPath,
                Filter = filter,
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                targetTextBox.Text = ofd.FileName;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            string commonPath;

            if (checkLoadCommon.Checked)
            {
                commonPath = this.textCommon.Text;
            }
            else
            {
                commonPath = null;
            }

            (bool, (string, Color)[]) status = FormOwner.LoadTrack(this.textM7ScreenPath.Text, this.textM7Graphics.Text, this.textColorPalette.Text, commonPath);
            if (status.Item1)
            {
                this.Close();
            }
            else
            {
                this.labelMode7Scr.Text = status.Item2[0].Item1;
                this.labelMode7Scr.ForeColor = status.Item2[0].Item2;

                this.labelM7Graphics.Text = status.Item2[1].Item1;
                this.labelM7Graphics.ForeColor = status.Item2[1].Item2;

                this.labelColorPalette.Text = status.Item2[2].Item1;
                this.labelColorPalette.ForeColor = status.Item2[2].Item2;

                if (commonPath != null)
                {
                    this.labelCommon.Text = status.Item2[3].Item1;
                    this.labelCommon.ForeColor = status.Item2[3].Item2;
                }
            }
        }

        private void checkLoadCommon_CheckedChanged(object sender, EventArgs e)
        {
            textCommon.Enabled = checkLoadCommon.Checked;
            buttonCommon.Enabled = checkLoadCommon.Checked;
        }
    }
}
