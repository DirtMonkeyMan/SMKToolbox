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
using static SMKToolbox.Overlay;

#pragma warning disable IDE1006

namespace SMKToolbox
{
    public partial class TM_OpenDirect : Form
    {
        TrackManager FormOwner;
        readonly (Label, string)[] tarLabel;
        readonly Color defColor = DefaultForeColor;

        public TM_OpenDirect(TrackManager owner)
        {
            InitializeComponent();

            tarLabel = new (Label, string)[]
            {
                (labelMode7Scr, "Mode 7 Screen (Track Layout)"),
                (labelM7Graphics, "Mode 7 Graphics (Track Tile Graphics)"),
                (labelColorPalette, "Color Palettes"),
                (labelCommon, "Common Tiles"),
                (labelOverlaySource, "Source Code File"),
                (labelPatternReference, "Pattern Reference Label"),
                (labelSizeReference, "Size Reference Label"),
                (labelOverlayDataFile, "Overlay Data")
            };

            FormOwner = owner;
        }

        private (bool, trackManLoadError[]) validatePaths(List<string> paths)
        {
            (bool, trackManLoadError[]) tmle = (true, new trackManLoadError[0]);

            int i;
            int j = paths.Count;

            for (i = 0; i < j; i++)
            {
                if (paths[i] == null) continue;
                if (paths[i].Trim() == "")
                {
                    tmle.Item1 = false;
                    tmle.Item2 = tmle.Item2.Append(new trackManLoadError {
                        module = i,
                        errText = "Please input a valid file name/label.",
                        errColor = Color.Red
                    }).ToArray();
                }
            }

            return tmle;
        }

        private void reportErrors(trackManLoadError[] errList)
        {
            SystemSounds.Asterisk.Play();

            int i;
            int j = tarLabel.Length - 1;

            for (i = 0; i < j; i++)
            {
                tarLabel[i].Item1.Text = tarLabel[i].Item2;
                tarLabel[i].Item1.ForeColor = defColor;
            }

            int k;
            j = errList.Length - 1;

            for (i = 0; i < j; i++)
            {
                k = errList[i].module;
                tarLabel[k].Item1.Text = errList[i].errText;
                tarLabel[k].Item1.ForeColor = errList[i].errColor;
            }
        }

        private void browseGeneralButton(string addfilter, TextBox targetBox)
        {
            string fil;
            int index;
            if (addfilter == null)
            {
                fil = "All Files (*.*)|*.*";
                index = 0;
            }
            else
            {
                fil = "All Files (*.*)|*.*|" + addfilter;
                index = 2;
            }

            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = Application.StartupPath,
                Filter = fil,
                FilterIndex = index,
                RestoreDirectory = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                targetBox.Text = ofd.FileName;
            }
        }

        private void buttonBrowseLayout_Click(object sender, EventArgs e)
        {
            browseGeneralButton("Mode 7 Screens (*.SCR)|*.SCR", textM7ScreenPath);
        }

        private void buttonBrowseGraphics_Click(object sender, EventArgs e)
        {
            browseGeneralButton("Mode 7 Graphics (*.CM7)|*.CM7", textM7Graphics);
        }

        private void buttonBrowsePalette_Click(object sender, EventArgs e)
        {
            browseGeneralButton("Color Palettes (*.COL)|*.COL", textColorPalette);
        }

        private void buttonBrowseCommon_Click(object sender, EventArgs e)
        {
            browseGeneralButton("Mode 7 Graphics (*.CM7)|*.CM7", textCommon);
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            overlaySetup set;

            List<string> paths = new List<string>();
            paths.Add(textM7ScreenPath.Text);
            paths.Add(textM7Graphics.Text);
            paths.Add(textColorPalette.Text);

            if (checkLoadCommon.Checked) paths.Add(textCommon.Text); else paths.Add(null);
            if (checkEnableOverlay.Checked)
            {
                paths.Add(textOverlaySource.Text);
                paths.Add(textPatternReference.Text);
                paths.Add(textSizeStorage.Text);
                paths.Add(textOverlayFile.Text);

                set = new overlaySetup
                {
                    sourceFile = paths[4],
                    patternRef = paths[5],
                    sizeRef = paths[6],
                    dataFile = paths[7],
                    // startsAt = (int)numericCourseNumber.Value
                };

                if (radioCourseNumber.Checked) set.startsAt = (int)numericCourseNumber.Value * 128; else set.startsAt = (int)numericCourseNumber.Value;
            }
            else
            {
                paths.Add(null);
                paths.Add(null);
                paths.Add(null);
                paths.Add(null);

                set = null;
            }

            (bool, trackManLoadError[]) status;

            status = validatePaths(paths);
            if (!status.Item1)
            {
                reportErrors(status.Item2);
                return;
            }

            status = FormOwner.LoadTrack(this.textM7ScreenPath.Text, this.textM7Graphics.Text, this.textColorPalette.Text, paths[3], set);
            if (status.Item1) Close(); else reportErrors(status.Item2);
        }

        private void checkLoadCommon_CheckedChanged(object sender, EventArgs e)
        {
            textCommon.Enabled = checkLoadCommon.Checked;
            buttonCommon.Enabled = checkLoadCommon.Checked;
        }

        private void buttonBrowseOverlaySource_Click(object sender, EventArgs e)
        {
            browseGeneralButton("Assembly Source File (*.asm)|*.asm", textOverlaySource);
        }

        private void buttonBrowseOverlayData_Click(object sender, EventArgs e)
        {
            browseGeneralButton("Binary File (*.bin)|*.bin", textOverlayFile);
        }

        private void checkEnableOverlay_CheckedChanged(object sender, EventArgs e)
        {
            groupOverlay.Enabled = checkEnableOverlay.Checked;
        }
    }
}
