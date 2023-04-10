
namespace SMKToolbox
{
    partial class TM_NewCatalog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fileNames = new System.Windows.Forms.GroupBox();
            this.buttonRepoBrowse = new System.Windows.Forms.Button();
            this.textRepoRoot = new System.Windows.Forms.TextBox();
            this.repoRoot = new System.Windows.Forms.Label();
            this.catalogSettings = new System.Windows.Forms.GroupBox();
            this.checkPerTrack = new System.Windows.Forms.CheckBox();
            this.labelInitSource = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textInitSource = new System.Windows.Forms.TextBox();
            this.labelTrackAssoc = new System.Windows.Forms.Label();
            this.textThemeAssoc = new System.Windows.Forms.TextBox();
            this.labelBackScr = new System.Windows.Forms.Label();
            this.textBackScr = new System.Windows.Forms.TextBox();
            this.textBackGrp = new System.Windows.Forms.TextBox();
            this.labelBackGrp = new System.Windows.Forms.Label();
            this.labelObstruct = new System.Windows.Forms.Label();
            this.textObstruct = new System.Windows.Forms.TextBox();
            this.textColorPalettes = new System.Windows.Forms.TextBox();
            this.labelColor = new System.Windows.Forms.Label();
            this.labelM7Cgx = new System.Windows.Forms.Label();
            this.textM7Cgx = new System.Windows.Forms.TextBox();
            this.textM7Scr = new System.Windows.Forms.TextBox();
            this.labelM7Scr = new System.Windows.Forms.Label();
            this.radioMakeCustom = new System.Windows.Forms.RadioButton();
            this.radioUseOfficial = new System.Windows.Forms.RadioButton();
            this.checkAuto = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.tabSettings = new System.Windows.Forms.TabControl();
            this.pageTracks = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.fileNames.SuspendLayout();
            this.catalogSettings.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.pageTracks.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileNames
            // 
            this.fileNames.Controls.Add(this.buttonRepoBrowse);
            this.fileNames.Controls.Add(this.textRepoRoot);
            this.fileNames.Controls.Add(this.repoRoot);
            this.fileNames.Location = new System.Drawing.Point(13, 13);
            this.fileNames.Name = "fileNames";
            this.fileNames.Size = new System.Drawing.Size(359, 64);
            this.fileNames.TabIndex = 0;
            this.fileNames.TabStop = false;
            this.fileNames.Text = "Step 1: Repository Root";
            // 
            // buttonRepoBrowse
            // 
            this.buttonRepoBrowse.Location = new System.Drawing.Point(278, 36);
            this.buttonRepoBrowse.Name = "buttonRepoBrowse";
            this.buttonRepoBrowse.Size = new System.Drawing.Size(75, 22);
            this.buttonRepoBrowse.TabIndex = 2;
            this.buttonRepoBrowse.Text = "Browse...";
            this.buttonRepoBrowse.UseVisualStyleBackColor = true;
            // 
            // textRepoRoot
            // 
            this.textRepoRoot.Location = new System.Drawing.Point(7, 37);
            this.textRepoRoot.MaxLength = 260;
            this.textRepoRoot.Name = "textRepoRoot";
            this.textRepoRoot.Size = new System.Drawing.Size(265, 20);
            this.textRepoRoot.TabIndex = 1;
            this.textRepoRoot.WordWrap = false;
            // 
            // repoRoot
            // 
            this.repoRoot.AutoSize = true;
            this.repoRoot.Location = new System.Drawing.Point(7, 20);
            this.repoRoot.Name = "repoRoot";
            this.repoRoot.Size = new System.Drawing.Size(128, 13);
            this.repoRoot.TabIndex = 0;
            this.repoRoot.Text = "Project L Repository Root";
            // 
            // catalogSettings
            // 
            this.catalogSettings.Controls.Add(this.tabSettings);
            this.catalogSettings.Controls.Add(this.radioMakeCustom);
            this.catalogSettings.Controls.Add(this.radioUseOfficial);
            this.catalogSettings.Controls.Add(this.checkAuto);
            this.catalogSettings.Location = new System.Drawing.Point(13, 83);
            this.catalogSettings.Name = "catalogSettings";
            this.catalogSettings.Size = new System.Drawing.Size(359, 281);
            this.catalogSettings.TabIndex = 1;
            this.catalogSettings.TabStop = false;
            this.catalogSettings.Text = "Step 2: Import Settings";
            // 
            // checkPerTrack
            // 
            this.checkPerTrack.AutoSize = true;
            this.checkPerTrack.Location = new System.Drawing.Point(6, 183);
            this.checkPerTrack.Name = "checkPerTrack";
            this.checkPerTrack.Size = new System.Drawing.Size(151, 17);
            this.checkPerTrack.TabIndex = 19;
            this.checkPerTrack.Text = "Unique Themes Per Track";
            this.checkPerTrack.UseVisualStyleBackColor = true;
            // 
            // labelInitSource
            // 
            this.labelInitSource.AutoSize = true;
            this.labelInitSource.Location = new System.Drawing.Point(6, 3);
            this.labelInitSource.Name = "labelInitSource";
            this.labelInitSource.Size = new System.Drawing.Size(60, 13);
            this.labelInitSource.TabIndex = 18;
            this.labelInitSource.Text = "Source File";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(257, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 22);
            this.button1.TabIndex = 17;
            this.button1.Text = "Browse...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textInitSource
            // 
            this.textInitSource.Location = new System.Drawing.Point(6, 20);
            this.textInitSource.MaxLength = 260;
            this.textInitSource.Name = "textInitSource";
            this.textInitSource.Size = new System.Drawing.Size(245, 20);
            this.textInitSource.TabIndex = 16;
            this.textInitSource.Text = ".\\Source\\kimura\\kart\\mak\\kart-init-e.asm";
            // 
            // labelTrackAssoc
            // 
            this.labelTrackAssoc.AutoSize = true;
            this.labelTrackAssoc.Location = new System.Drawing.Point(199, 163);
            this.labelTrackAssoc.Name = "labelTrackAssoc";
            this.labelTrackAssoc.Size = new System.Drawing.Size(133, 13);
            this.labelTrackAssoc.TabIndex = 15;
            this.labelTrackAssoc.Text = "Track Theme Associations";
            // 
            // textThemeAssoc
            // 
            this.textThemeAssoc.Location = new System.Drawing.Point(172, 180);
            this.textThemeAssoc.MaxLength = 256;
            this.textThemeAssoc.Name = "textThemeAssoc";
            this.textThemeAssoc.Size = new System.Drawing.Size(160, 20);
            this.textThemeAssoc.TabIndex = 14;
            this.textThemeAssoc.Text = "Map_type_data";
            // 
            // labelBackScr
            // 
            this.labelBackScr.AutoSize = true;
            this.labelBackScr.Location = new System.Drawing.Point(225, 123);
            this.labelBackScr.Name = "labelBackScr";
            this.labelBackScr.Size = new System.Drawing.Size(107, 13);
            this.labelBackScr.TabIndex = 11;
            this.labelBackScr.Text = "Background Screens";
            // 
            // textBackScr
            // 
            this.textBackScr.Location = new System.Drawing.Point(172, 140);
            this.textBackScr.MaxLength = 256;
            this.textBackScr.Name = "textBackScr";
            this.textBackScr.Size = new System.Drawing.Size(160, 20);
            this.textBackScr.TabIndex = 10;
            this.textBackScr.Text = "Back_screen_address";
            // 
            // textBackGrp
            // 
            this.textBackGrp.Location = new System.Drawing.Point(6, 140);
            this.textBackGrp.MaxLength = 256;
            this.textBackGrp.Name = "textBackGrp";
            this.textBackGrp.Size = new System.Drawing.Size(160, 20);
            this.textBackGrp.TabIndex = 9;
            this.textBackGrp.Text = "Back_character_address";
            // 
            // labelBackGrp
            // 
            this.labelBackGrp.AutoSize = true;
            this.labelBackGrp.Location = new System.Drawing.Point(6, 123);
            this.labelBackGrp.Name = "labelBackGrp";
            this.labelBackGrp.Size = new System.Drawing.Size(110, 13);
            this.labelBackGrp.TabIndex = 8;
            this.labelBackGrp.Text = "Background Graphics";
            // 
            // labelObstruct
            // 
            this.labelObstruct.AutoSize = true;
            this.labelObstruct.Location = new System.Drawing.Point(219, 83);
            this.labelObstruct.Name = "labelObstruct";
            this.labelObstruct.Size = new System.Drawing.Size(113, 13);
            this.labelObstruct.TabIndex = 7;
            this.labelObstruct.Text = "On-Track Obstacle Art";
            // 
            // textObstruct
            // 
            this.textObstruct.Location = new System.Drawing.Point(172, 100);
            this.textObstruct.MaxLength = 256;
            this.textObstruct.Name = "textObstruct";
            this.textObstruct.Size = new System.Drawing.Size(160, 20);
            this.textObstruct.TabIndex = 6;
            this.textObstruct.Text = "Obstruct_address";
            // 
            // textColorPalettes
            // 
            this.textColorPalettes.Location = new System.Drawing.Point(6, 100);
            this.textColorPalettes.MaxLength = 256;
            this.textColorPalettes.Name = "textColorPalettes";
            this.textColorPalettes.Size = new System.Drawing.Size(160, 20);
            this.textColorPalettes.TabIndex = 5;
            this.textColorPalettes.Text = "Color_data_address";
            // 
            // labelColor
            // 
            this.labelColor.AutoSize = true;
            this.labelColor.Location = new System.Drawing.Point(6, 83);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(72, 13);
            this.labelColor.TabIndex = 4;
            this.labelColor.Text = "Color Palettes";
            // 
            // labelM7Cgx
            // 
            this.labelM7Cgx.AutoSize = true;
            this.labelM7Cgx.Location = new System.Drawing.Point(202, 43);
            this.labelM7Cgx.Name = "labelM7Cgx";
            this.labelM7Cgx.Size = new System.Drawing.Size(130, 13);
            this.labelM7Cgx.TabIndex = 3;
            this.labelM7Cgx.Text = "Mode 7 Graphics (Tile Art)";
            // 
            // textM7Cgx
            // 
            this.textM7Cgx.Location = new System.Drawing.Point(172, 60);
            this.textM7Cgx.MaxLength = 256;
            this.textM7Cgx.Name = "textM7Cgx";
            this.textM7Cgx.Size = new System.Drawing.Size(160, 20);
            this.textM7Cgx.TabIndex = 2;
            this.textM7Cgx.Text = "Mode7_character_address";
            // 
            // textM7Scr
            // 
            this.textM7Scr.Location = new System.Drawing.Point(6, 60);
            this.textM7Scr.MaxLength = 256;
            this.textM7Scr.Name = "textM7Scr";
            this.textM7Scr.Size = new System.Drawing.Size(160, 20);
            this.textM7Scr.TabIndex = 1;
            this.textM7Scr.Text = "Mode7_screen_address";
            // 
            // labelM7Scr
            // 
            this.labelM7Scr.AutoSize = true;
            this.labelM7Scr.Location = new System.Drawing.Point(6, 43);
            this.labelM7Scr.Name = "labelM7Scr";
            this.labelM7Scr.Size = new System.Drawing.Size(127, 13);
            this.labelM7Scr.TabIndex = 0;
            this.labelM7Scr.Text = "Mode 7 Screens (Tracks)";
            // 
            // radioMakeCustom
            // 
            this.radioMakeCustom.AutoSize = true;
            this.radioMakeCustom.Enabled = false;
            this.radioMakeCustom.Location = new System.Drawing.Point(234, 20);
            this.radioMakeCustom.Name = "radioMakeCustom";
            this.radioMakeCustom.Size = new System.Drawing.Size(119, 17);
            this.radioMakeCustom.TabIndex = 10;
            this.radioMakeCustom.Text = "Create Data Source";
            this.radioMakeCustom.UseVisualStyleBackColor = true;
            // 
            // radioUseOfficial
            // 
            this.radioUseOfficial.AutoSize = true;
            this.radioUseOfficial.Checked = true;
            this.radioUseOfficial.Enabled = false;
            this.radioUseOfficial.Location = new System.Drawing.Point(121, 20);
            this.radioUseOfficial.Name = "radioUseOfficial";
            this.radioUseOfficial.Size = new System.Drawing.Size(107, 17);
            this.radioUseOfficial.TabIndex = 9;
            this.radioUseOfficial.TabStop = true;
            this.radioUseOfficial.Text = "Use Source Data";
            this.radioUseOfficial.UseVisualStyleBackColor = true;
            // 
            // checkAuto
            // 
            this.checkAuto.AutoSize = true;
            this.checkAuto.Checked = true;
            this.checkAuto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkAuto.Location = new System.Drawing.Point(7, 20);
            this.checkAuto.Name = "checkAuto";
            this.checkAuto.Size = new System.Drawing.Size(73, 17);
            this.checkAuto.TabIndex = 0;
            this.checkAuto.Text = "Automatic";
            this.checkAuto.UseVisualStyleBackColor = true;
            this.checkAuto.CheckedChanged += new System.EventHandler(this.checkAuto_CheckedChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(297, 370);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(216, 370);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 3;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.Controls.Add(this.pageTracks);
            this.tabSettings.Controls.Add(this.tabPage2);
            this.tabSettings.Location = new System.Drawing.Point(7, 43);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.SelectedIndex = 0;
            this.tabSettings.Size = new System.Drawing.Size(346, 232);
            this.tabSettings.TabIndex = 4;
            // 
            // pageTracks
            // 
            this.pageTracks.Controls.Add(this.checkPerTrack);
            this.pageTracks.Controls.Add(this.labelInitSource);
            this.pageTracks.Controls.Add(this.labelM7Scr);
            this.pageTracks.Controls.Add(this.button1);
            this.pageTracks.Controls.Add(this.textM7Scr);
            this.pageTracks.Controls.Add(this.textInitSource);
            this.pageTracks.Controls.Add(this.textM7Cgx);
            this.pageTracks.Controls.Add(this.labelTrackAssoc);
            this.pageTracks.Controls.Add(this.labelM7Cgx);
            this.pageTracks.Controls.Add(this.textThemeAssoc);
            this.pageTracks.Controls.Add(this.labelColor);
            this.pageTracks.Controls.Add(this.labelBackScr);
            this.pageTracks.Controls.Add(this.textColorPalettes);
            this.pageTracks.Controls.Add(this.textBackScr);
            this.pageTracks.Controls.Add(this.textObstruct);
            this.pageTracks.Controls.Add(this.textBackGrp);
            this.pageTracks.Controls.Add(this.labelObstruct);
            this.pageTracks.Controls.Add(this.labelBackGrp);
            this.pageTracks.Location = new System.Drawing.Point(4, 22);
            this.pageTracks.Name = "pageTracks";
            this.pageTracks.Padding = new System.Windows.Forms.Padding(3);
            this.pageTracks.Size = new System.Drawing.Size(338, 206);
            this.pageTracks.TabIndex = 0;
            this.pageTracks.Text = "Tracks & Backgrounds";
            this.pageTracks.Enabled = false;
            this.pageTracks.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(338, 206);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // TM_NewCatalog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 404);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.catalogSettings);
            this.Controls.Add(this.fileNames);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.Name = "TM_NewCatalog";
            this.Text = "New Catalog";
            this.TopMost = true;
            this.fileNames.ResumeLayout(false);
            this.fileNames.PerformLayout();
            this.catalogSettings.ResumeLayout(false);
            this.catalogSettings.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.pageTracks.ResumeLayout(false);
            this.pageTracks.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox fileNames;
        private System.Windows.Forms.Button buttonRepoBrowse;
        private System.Windows.Forms.TextBox textRepoRoot;
        private System.Windows.Forms.Label repoRoot;
        private System.Windows.Forms.GroupBox catalogSettings;
        private System.Windows.Forms.CheckBox checkAuto;
        private System.Windows.Forms.RadioButton radioMakeCustom;
        private System.Windows.Forms.RadioButton radioUseOfficial;
        private System.Windows.Forms.Label labelObstruct;
        private System.Windows.Forms.TextBox textObstruct;
        private System.Windows.Forms.TextBox textColorPalettes;
        private System.Windows.Forms.Label labelColor;
        private System.Windows.Forms.Label labelM7Cgx;
        private System.Windows.Forms.TextBox textM7Cgx;
        private System.Windows.Forms.TextBox textM7Scr;
        private System.Windows.Forms.Label labelM7Scr;
        private System.Windows.Forms.Label labelTrackAssoc;
        private System.Windows.Forms.TextBox textThemeAssoc;
        private System.Windows.Forms.Label labelBackScr;
        private System.Windows.Forms.TextBox textBackScr;
        private System.Windows.Forms.TextBox textBackGrp;
        private System.Windows.Forms.Label labelBackGrp;
        private System.Windows.Forms.Label labelInitSource;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textInitSource;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.CheckBox checkPerTrack;
        private System.Windows.Forms.TabControl tabSettings;
        private System.Windows.Forms.TabPage pageTracks;
        private System.Windows.Forms.TabPage tabPage2;
    }
}