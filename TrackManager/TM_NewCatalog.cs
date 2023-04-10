using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMKToolbox
{
    public partial class TM_NewCatalog : Form
    {
        public TM_NewCatalog()
        {
            InitializeComponent();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void resetNewCatSettings()
        {
            this.radioUseOfficial.Checked = true;
            this.textInitSource.Text = ".\\Source\\kimura\\kart\\mak\\kart-init-e.asm";
            this.textM7Scr.Text = "Mode7_screen_address";
            this.textM7Cgx.Text = "Mode7_character_address";
            this.textColorPalettes.Text = "Color_data_address";
            this.textObstruct.Text = "Obstruct_address";
            this.textBackGrp.Text = "Back_character_address";
            this.textBackScr.Text = "Back_screen_address";
            this.textThemeAssoc.Text = "Map_type_data";
            this.checkPerTrack.Checked = false;
        }

        private void setSettingsEnable(bool enable)
        {
            this.radioUseOfficial.Enabled = enable;
            this.radioMakeCustom.Enabled = enable;
            this.pageTracks.Enabled = enable;
        }

        private void checkAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkAuto.Checked) {
                setSettingsEnable(false);
                resetNewCatSettings();
            }
            else
            {
                setSettingsEnable(true);
            }
        }

        
    }
}
