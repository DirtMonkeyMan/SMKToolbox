
namespace SMKToolbox
{
    partial class TM_OpenDirect
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
            this.labelMode7Scr = new System.Windows.Forms.Label();
            this.textM7ScreenPath = new System.Windows.Forms.TextBox();
            this.buttonBrowseM7Screen = new System.Windows.Forms.Button();
            this.buttonM7GraphicsBrowse = new System.Windows.Forms.Button();
            this.textM7Graphics = new System.Windows.Forms.TextBox();
            this.labelM7Graphics = new System.Windows.Forms.Label();
            this.buttonColorPaletteBrowse = new System.Windows.Forms.Button();
            this.textColorPalette = new System.Windows.Forms.TextBox();
            this.labelColorPalette = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.checkLoadCommon = new System.Windows.Forms.CheckBox();
            this.buttonCommon = new System.Windows.Forms.Button();
            this.textCommon = new System.Windows.Forms.TextBox();
            this.labelCommon = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabTileEditor = new System.Windows.Forms.TabPage();
            this.tabOverlay = new System.Windows.Forms.TabPage();
            this.groupOverlay = new System.Windows.Forms.GroupBox();
            this.labelCourseOffset = new System.Windows.Forms.Label();
            this.radioDirectOffset = new System.Windows.Forms.RadioButton();
            this.radioCourseNumber = new System.Windows.Forms.RadioButton();
            this.numericCourseNumber = new System.Windows.Forms.NumericUpDown();
            this.buttonBrowseOverlayData = new System.Windows.Forms.Button();
            this.textOverlayFile = new System.Windows.Forms.TextBox();
            this.labelOverlayDataFile = new System.Windows.Forms.Label();
            this.buttonUseDefaultLabels = new System.Windows.Forms.Button();
            this.labelSizeReference = new System.Windows.Forms.Label();
            this.textSizeStorage = new System.Windows.Forms.TextBox();
            this.labelPatternReference = new System.Windows.Forms.Label();
            this.textPatternReference = new System.Windows.Forms.TextBox();
            this.labelOverlaySource = new System.Windows.Forms.Label();
            this.textOverlaySource = new System.Windows.Forms.TextBox();
            this.buttonBrowseOverlaySource = new System.Windows.Forms.Button();
            this.checkEnableOverlay = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.tabTileEditor.SuspendLayout();
            this.tabOverlay.SuspendLayout();
            this.groupOverlay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericCourseNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // labelMode7Scr
            // 
            this.labelMode7Scr.AutoSize = true;
            this.labelMode7Scr.Location = new System.Drawing.Point(6, 3);
            this.labelMode7Scr.Name = "labelMode7Scr";
            this.labelMode7Scr.Size = new System.Drawing.Size(152, 13);
            this.labelMode7Scr.TabIndex = 0;
            this.labelMode7Scr.Text = "Mode 7 Screen (Track Layout)";
            // 
            // textM7ScreenPath
            // 
            this.textM7ScreenPath.Location = new System.Drawing.Point(6, 20);
            this.textM7ScreenPath.MaxLength = 260;
            this.textM7ScreenPath.Name = "textM7ScreenPath";
            this.textM7ScreenPath.Size = new System.Drawing.Size(258, 20);
            this.textM7ScreenPath.TabIndex = 1;
            // 
            // buttonBrowseM7Screen
            // 
            this.buttonBrowseM7Screen.Location = new System.Drawing.Point(270, 19);
            this.buttonBrowseM7Screen.Name = "buttonBrowseM7Screen";
            this.buttonBrowseM7Screen.Size = new System.Drawing.Size(75, 22);
            this.buttonBrowseM7Screen.TabIndex = 2;
            this.buttonBrowseM7Screen.Text = "Browse";
            this.buttonBrowseM7Screen.UseVisualStyleBackColor = true;
            this.buttonBrowseM7Screen.Click += new System.EventHandler(this.buttonBrowseLayout_Click);
            // 
            // buttonM7GraphicsBrowse
            // 
            this.buttonM7GraphicsBrowse.Location = new System.Drawing.Point(270, 59);
            this.buttonM7GraphicsBrowse.Name = "buttonM7GraphicsBrowse";
            this.buttonM7GraphicsBrowse.Size = new System.Drawing.Size(75, 22);
            this.buttonM7GraphicsBrowse.TabIndex = 5;
            this.buttonM7GraphicsBrowse.Text = "Browse Graphics";
            this.buttonM7GraphicsBrowse.UseVisualStyleBackColor = true;
            this.buttonM7GraphicsBrowse.Click += new System.EventHandler(this.buttonBrowseGraphics_Click);
            // 
            // textM7Graphics
            // 
            this.textM7Graphics.Location = new System.Drawing.Point(6, 60);
            this.textM7Graphics.MaxLength = 260;
            this.textM7Graphics.Name = "textM7Graphics";
            this.textM7Graphics.Size = new System.Drawing.Size(258, 20);
            this.textM7Graphics.TabIndex = 4;
            // 
            // labelM7Graphics
            // 
            this.labelM7Graphics.AutoSize = true;
            this.labelM7Graphics.Location = new System.Drawing.Point(6, 43);
            this.labelM7Graphics.Name = "labelM7Graphics";
            this.labelM7Graphics.Size = new System.Drawing.Size(190, 13);
            this.labelM7Graphics.TabIndex = 3;
            this.labelM7Graphics.Text = "Mode 7 Graphics (Track Tile Graphics)";
            // 
            // buttonColorPaletteBrowse
            // 
            this.buttonColorPaletteBrowse.Location = new System.Drawing.Point(270, 99);
            this.buttonColorPaletteBrowse.Name = "buttonColorPaletteBrowse";
            this.buttonColorPaletteBrowse.Size = new System.Drawing.Size(75, 22);
            this.buttonColorPaletteBrowse.TabIndex = 8;
            this.buttonColorPaletteBrowse.Text = "Browse Palette";
            this.buttonColorPaletteBrowse.UseVisualStyleBackColor = true;
            this.buttonColorPaletteBrowse.Click += new System.EventHandler(this.buttonBrowsePalette_Click);
            // 
            // textColorPalette
            // 
            this.textColorPalette.Location = new System.Drawing.Point(6, 100);
            this.textColorPalette.MaxLength = 260;
            this.textColorPalette.Name = "textColorPalette";
            this.textColorPalette.Size = new System.Drawing.Size(258, 20);
            this.textColorPalette.TabIndex = 7;
            // 
            // labelColorPalette
            // 
            this.labelColorPalette.AutoSize = true;
            this.labelColorPalette.Location = new System.Drawing.Point(6, 83);
            this.labelColorPalette.Name = "labelColorPalette";
            this.labelColorPalette.Size = new System.Drawing.Size(72, 13);
            this.labelColorPalette.TabIndex = 6;
            this.labelColorPalette.Text = "Color Palettes";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(297, 298);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(216, 298);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 10;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // checkLoadCommon
            // 
            this.checkLoadCommon.AutoSize = true;
            this.checkLoadCommon.Checked = true;
            this.checkLoadCommon.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkLoadCommon.Location = new System.Drawing.Point(6, 127);
            this.checkLoadCommon.Name = "checkLoadCommon";
            this.checkLoadCommon.Size = new System.Drawing.Size(133, 17);
            this.checkLoadCommon.TabIndex = 12;
            this.checkLoadCommon.Text = "Load Common Tile File";
            this.checkLoadCommon.UseVisualStyleBackColor = true;
            this.checkLoadCommon.CheckedChanged += new System.EventHandler(this.checkLoadCommon_CheckedChanged);
            // 
            // buttonCommon
            // 
            this.buttonCommon.Location = new System.Drawing.Point(270, 162);
            this.buttonCommon.Name = "buttonCommon";
            this.buttonCommon.Size = new System.Drawing.Size(75, 22);
            this.buttonCommon.TabIndex = 15;
            this.buttonCommon.Text = "Browse Common";
            this.buttonCommon.UseVisualStyleBackColor = true;
            this.buttonCommon.Click += new System.EventHandler(this.buttonBrowseCommon_Click);
            // 
            // textCommon
            // 
            this.textCommon.Location = new System.Drawing.Point(6, 163);
            this.textCommon.MaxLength = 260;
            this.textCommon.Name = "textCommon";
            this.textCommon.Size = new System.Drawing.Size(258, 20);
            this.textCommon.TabIndex = 14;
            // 
            // labelCommon
            // 
            this.labelCommon.AutoSize = true;
            this.labelCommon.Location = new System.Drawing.Point(5, 147);
            this.labelCommon.Name = "labelCommon";
            this.labelCommon.Size = new System.Drawing.Size(73, 13);
            this.labelCommon.TabIndex = 13;
            this.labelCommon.Text = "Common Tiles";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabTileEditor);
            this.tabControl.Controls.Add(this.tabOverlay);
            this.tabControl.Location = new System.Drawing.Point(13, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(359, 280);
            this.tabControl.TabIndex = 16;
            // 
            // tabTileEditor
            // 
            this.tabTileEditor.Controls.Add(this.labelMode7Scr);
            this.tabTileEditor.Controls.Add(this.buttonCommon);
            this.tabTileEditor.Controls.Add(this.textM7ScreenPath);
            this.tabTileEditor.Controls.Add(this.textCommon);
            this.tabTileEditor.Controls.Add(this.buttonBrowseM7Screen);
            this.tabTileEditor.Controls.Add(this.labelCommon);
            this.tabTileEditor.Controls.Add(this.labelM7Graphics);
            this.tabTileEditor.Controls.Add(this.checkLoadCommon);
            this.tabTileEditor.Controls.Add(this.textM7Graphics);
            this.tabTileEditor.Controls.Add(this.buttonM7GraphicsBrowse);
            this.tabTileEditor.Controls.Add(this.labelColorPalette);
            this.tabTileEditor.Controls.Add(this.buttonColorPaletteBrowse);
            this.tabTileEditor.Controls.Add(this.textColorPalette);
            this.tabTileEditor.Location = new System.Drawing.Point(4, 22);
            this.tabTileEditor.Name = "tabTileEditor";
            this.tabTileEditor.Padding = new System.Windows.Forms.Padding(3);
            this.tabTileEditor.Size = new System.Drawing.Size(351, 254);
            this.tabTileEditor.TabIndex = 0;
            this.tabTileEditor.Text = "Tile Editor";
            this.tabTileEditor.UseVisualStyleBackColor = true;
            // 
            // tabOverlay
            // 
            this.tabOverlay.Controls.Add(this.groupOverlay);
            this.tabOverlay.Controls.Add(this.checkEnableOverlay);
            this.tabOverlay.Location = new System.Drawing.Point(4, 22);
            this.tabOverlay.Name = "tabOverlay";
            this.tabOverlay.Padding = new System.Windows.Forms.Padding(3);
            this.tabOverlay.Size = new System.Drawing.Size(351, 254);
            this.tabOverlay.TabIndex = 1;
            this.tabOverlay.Text = "Overlay";
            this.tabOverlay.UseVisualStyleBackColor = true;
            // 
            // groupOverlay
            // 
            this.groupOverlay.Controls.Add(this.labelCourseOffset);
            this.groupOverlay.Controls.Add(this.radioDirectOffset);
            this.groupOverlay.Controls.Add(this.radioCourseNumber);
            this.groupOverlay.Controls.Add(this.numericCourseNumber);
            this.groupOverlay.Controls.Add(this.buttonBrowseOverlayData);
            this.groupOverlay.Controls.Add(this.textOverlayFile);
            this.groupOverlay.Controls.Add(this.labelOverlayDataFile);
            this.groupOverlay.Controls.Add(this.buttonUseDefaultLabels);
            this.groupOverlay.Controls.Add(this.labelSizeReference);
            this.groupOverlay.Controls.Add(this.textSizeStorage);
            this.groupOverlay.Controls.Add(this.labelPatternReference);
            this.groupOverlay.Controls.Add(this.textPatternReference);
            this.groupOverlay.Controls.Add(this.labelOverlaySource);
            this.groupOverlay.Controls.Add(this.textOverlaySource);
            this.groupOverlay.Controls.Add(this.buttonBrowseOverlaySource);
            this.groupOverlay.Enabled = false;
            this.groupOverlay.Location = new System.Drawing.Point(7, 31);
            this.groupOverlay.Name = "groupOverlay";
            this.groupOverlay.Size = new System.Drawing.Size(338, 218);
            this.groupOverlay.TabIndex = 1;
            this.groupOverlay.TabStop = false;
            this.groupOverlay.Text = "Overlay";
            // 
            // labelCourseOffset
            // 
            this.labelCourseOffset.AutoSize = true;
            this.labelCourseOffset.Location = new System.Drawing.Point(129, 172);
            this.labelCourseOffset.Name = "labelCourseOffset";
            this.labelCourseOffset.Size = new System.Drawing.Size(113, 13);
            this.labelCourseOffset.TabIndex = 18;
            this.labelCourseOffset.Text = "Course Number/Offset";
            // 
            // radioDirectOffset
            // 
            this.radioDirectOffset.AutoSize = true;
            this.radioDirectOffset.Location = new System.Drawing.Point(6, 194);
            this.radioDirectOffset.Name = "radioDirectOffset";
            this.radioDirectOffset.Size = new System.Drawing.Size(106, 17);
            this.radioDirectOffset.TabIndex = 17;
            this.radioDirectOffset.TabStop = true;
            this.radioDirectOffset.Text = "Use Direct Offset";
            this.radioDirectOffset.UseVisualStyleBackColor = true;
            // 
            // radioCourseNumber
            // 
            this.radioCourseNumber.AutoSize = true;
            this.radioCourseNumber.Checked = true;
            this.radioCourseNumber.Location = new System.Drawing.Point(6, 170);
            this.radioCourseNumber.Name = "radioCourseNumber";
            this.radioCourseNumber.Size = new System.Drawing.Size(120, 17);
            this.radioCourseNumber.TabIndex = 16;
            this.radioCourseNumber.TabStop = true;
            this.radioCourseNumber.Text = "Use Course Number";
            this.radioCourseNumber.UseVisualStyleBackColor = true;
            // 
            // numericCourseNumber
            // 
            this.numericCourseNumber.Location = new System.Drawing.Point(132, 191);
            this.numericCourseNumber.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numericCourseNumber.Name = "numericCourseNumber";
            this.numericCourseNumber.Size = new System.Drawing.Size(80, 20);
            this.numericCourseNumber.TabIndex = 15;
            // 
            // buttonBrowseOverlayData
            // 
            this.buttonBrowseOverlayData.Location = new System.Drawing.Point(257, 142);
            this.buttonBrowseOverlayData.Name = "buttonBrowseOverlayData";
            this.buttonBrowseOverlayData.Size = new System.Drawing.Size(75, 22);
            this.buttonBrowseOverlayData.TabIndex = 13;
            this.buttonBrowseOverlayData.Text = "Browse";
            this.buttonBrowseOverlayData.UseVisualStyleBackColor = true;
            this.buttonBrowseOverlayData.Click += new System.EventHandler(this.buttonBrowseOverlayData_Click);
            // 
            // textOverlayFile
            // 
            this.textOverlayFile.Location = new System.Drawing.Point(6, 143);
            this.textOverlayFile.Name = "textOverlayFile";
            this.textOverlayFile.Size = new System.Drawing.Size(245, 20);
            this.textOverlayFile.TabIndex = 12;
            // 
            // labelOverlayDataFile
            // 
            this.labelOverlayDataFile.AutoSize = true;
            this.labelOverlayDataFile.Location = new System.Drawing.Point(6, 126);
            this.labelOverlayDataFile.Name = "labelOverlayDataFile";
            this.labelOverlayDataFile.Size = new System.Drawing.Size(67, 13);
            this.labelOverlayDataFile.TabIndex = 11;
            this.labelOverlayDataFile.Text = "Overlay data";
            // 
            // buttonUseDefaultLabels
            // 
            this.buttonUseDefaultLabels.Location = new System.Drawing.Point(6, 60);
            this.buttonUseDefaultLabels.Name = "buttonUseDefaultLabels";
            this.buttonUseDefaultLabels.Size = new System.Drawing.Size(326, 23);
            this.buttonUseDefaultLabels.TabIndex = 10;
            this.buttonUseDefaultLabels.Text = "Use Default Labels";
            this.buttonUseDefaultLabels.UseVisualStyleBackColor = true;
            // 
            // labelSizeReference
            // 
            this.labelSizeReference.AutoSize = true;
            this.labelSizeReference.Location = new System.Drawing.Point(172, 86);
            this.labelSizeReference.Name = "labelSizeReference";
            this.labelSizeReference.Size = new System.Drawing.Size(109, 13);
            this.labelSizeReference.TabIndex = 8;
            this.labelSizeReference.Text = "Size Reference Label";
            // 
            // textSizeStorage
            // 
            this.textSizeStorage.Location = new System.Drawing.Point(172, 103);
            this.textSizeStorage.MaxLength = 260;
            this.textSizeStorage.Name = "textSizeStorage";
            this.textSizeStorage.Size = new System.Drawing.Size(160, 20);
            this.textSizeStorage.TabIndex = 9;
            this.textSizeStorage.Text = "PTADAT";
            // 
            // labelPatternReference
            // 
            this.labelPatternReference.AutoSize = true;
            this.labelPatternReference.Location = new System.Drawing.Point(6, 86);
            this.labelPatternReference.Name = "labelPatternReference";
            this.labelPatternReference.Size = new System.Drawing.Size(123, 13);
            this.labelPatternReference.TabIndex = 6;
            this.labelPatternReference.Text = "Pattern Reference Label";
            // 
            // textPatternReference
            // 
            this.textPatternReference.Location = new System.Drawing.Point(6, 103);
            this.textPatternReference.MaxLength = 260;
            this.textPatternReference.Name = "textPatternReference";
            this.textPatternReference.Size = new System.Drawing.Size(160, 20);
            this.textPatternReference.TabIndex = 7;
            this.textPatternReference.Text = "YAKI_NO";
            // 
            // labelOverlaySource
            // 
            this.labelOverlaySource.AutoSize = true;
            this.labelOverlaySource.Location = new System.Drawing.Point(6, 16);
            this.labelOverlaySource.Name = "labelOverlaySource";
            this.labelOverlaySource.Size = new System.Drawing.Size(88, 13);
            this.labelOverlaySource.TabIndex = 3;
            this.labelOverlaySource.Text = "Source Code File";
            // 
            // textOverlaySource
            // 
            this.textOverlaySource.Location = new System.Drawing.Point(6, 33);
            this.textOverlaySource.MaxLength = 260;
            this.textOverlaySource.Name = "textOverlaySource";
            this.textOverlaySource.Size = new System.Drawing.Size(245, 20);
            this.textOverlaySource.TabIndex = 4;
            // 
            // buttonBrowseOverlaySource
            // 
            this.buttonBrowseOverlaySource.Location = new System.Drawing.Point(257, 32);
            this.buttonBrowseOverlaySource.Name = "buttonBrowseOverlaySource";
            this.buttonBrowseOverlaySource.Size = new System.Drawing.Size(75, 22);
            this.buttonBrowseOverlaySource.TabIndex = 5;
            this.buttonBrowseOverlaySource.Text = "Browse";
            this.buttonBrowseOverlaySource.UseVisualStyleBackColor = true;
            this.buttonBrowseOverlaySource.Click += new System.EventHandler(this.buttonBrowseOverlaySource_Click);
            // 
            // checkEnableOverlay
            // 
            this.checkEnableOverlay.AutoSize = true;
            this.checkEnableOverlay.Location = new System.Drawing.Point(7, 7);
            this.checkEnableOverlay.Name = "checkEnableOverlay";
            this.checkEnableOverlay.Size = new System.Drawing.Size(133, 17);
            this.checkEnableOverlay.TabIndex = 0;
            this.checkEnableOverlay.Text = "Enable Overlay Editing";
            this.checkEnableOverlay.UseVisualStyleBackColor = true;
            this.checkEnableOverlay.CheckedChanged += new System.EventHandler(this.checkEnableOverlay_CheckedChanged);
            // 
            // TM_OpenDirect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 333);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TM_OpenDirect";
            this.Text = "Load Track Assets Directly";
            this.TopMost = true;
            this.tabControl.ResumeLayout(false);
            this.tabTileEditor.ResumeLayout(false);
            this.tabTileEditor.PerformLayout();
            this.tabOverlay.ResumeLayout(false);
            this.tabOverlay.PerformLayout();
            this.groupOverlay.ResumeLayout(false);
            this.groupOverlay.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericCourseNumber)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelMode7Scr;
        private System.Windows.Forms.TextBox textM7ScreenPath;
        private System.Windows.Forms.Button buttonBrowseM7Screen;
        private System.Windows.Forms.Button buttonM7GraphicsBrowse;
        private System.Windows.Forms.TextBox textM7Graphics;
        private System.Windows.Forms.Label labelM7Graphics;
        private System.Windows.Forms.Button buttonColorPaletteBrowse;
        private System.Windows.Forms.TextBox textColorPalette;
        private System.Windows.Forms.Label labelColorPalette;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.CheckBox checkLoadCommon;
        private System.Windows.Forms.Button buttonCommon;
        private System.Windows.Forms.TextBox textCommon;
        private System.Windows.Forms.Label labelCommon;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabTileEditor;
        private System.Windows.Forms.TabPage tabOverlay;
        private System.Windows.Forms.GroupBox groupOverlay;
        private System.Windows.Forms.CheckBox checkEnableOverlay;
        private System.Windows.Forms.Label labelOverlaySource;
        private System.Windows.Forms.TextBox textOverlaySource;
        private System.Windows.Forms.Button buttonBrowseOverlaySource;
        private System.Windows.Forms.Label labelPatternReference;
        private System.Windows.Forms.TextBox textPatternReference;
        private System.Windows.Forms.Label labelSizeReference;
        private System.Windows.Forms.TextBox textSizeStorage;
        private System.Windows.Forms.Button buttonUseDefaultLabels;
        private System.Windows.Forms.RadioButton radioDirectOffset;
        private System.Windows.Forms.RadioButton radioCourseNumber;
        private System.Windows.Forms.NumericUpDown numericCourseNumber;
        private System.Windows.Forms.Button buttonBrowseOverlayData;
        private System.Windows.Forms.TextBox textOverlayFile;
        private System.Windows.Forms.Label labelOverlayDataFile;
        private System.Windows.Forms.Label labelCourseOffset;
    }
}