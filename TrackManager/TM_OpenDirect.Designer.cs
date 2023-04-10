
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
            this.SuspendLayout();
            // 
            // labelMode7Scr
            // 
            this.labelMode7Scr.AutoSize = true;
            this.labelMode7Scr.Location = new System.Drawing.Point(13, 13);
            this.labelMode7Scr.Name = "labelMode7Scr";
            this.labelMode7Scr.Size = new System.Drawing.Size(152, 13);
            this.labelMode7Scr.TabIndex = 0;
            this.labelMode7Scr.Text = "Mode 7 Screen (Track Layout)";
            // 
            // textM7ScreenPath
            // 
            this.textM7ScreenPath.Location = new System.Drawing.Point(13, 30);
            this.textM7ScreenPath.MaxLength = 260;
            this.textM7ScreenPath.Name = "textM7ScreenPath";
            this.textM7ScreenPath.Size = new System.Drawing.Size(256, 20);
            this.textM7ScreenPath.TabIndex = 1;
            // 
            // buttonBrowseM7Screen
            // 
            this.buttonBrowseM7Screen.Location = new System.Drawing.Point(275, 29);
            this.buttonBrowseM7Screen.Name = "buttonBrowseM7Screen";
            this.buttonBrowseM7Screen.Size = new System.Drawing.Size(97, 22);
            this.buttonBrowseM7Screen.TabIndex = 2;
            this.buttonBrowseM7Screen.Text = "Browse Screen";
            this.buttonBrowseM7Screen.UseVisualStyleBackColor = true;
            this.buttonBrowseM7Screen.Click += new System.EventHandler(this.buttonBrowseGeneral_Click);
            // 
            // buttonM7GraphicsBrowse
            // 
            this.buttonM7GraphicsBrowse.Location = new System.Drawing.Point(275, 69);
            this.buttonM7GraphicsBrowse.Name = "buttonM7GraphicsBrowse";
            this.buttonM7GraphicsBrowse.Size = new System.Drawing.Size(97, 22);
            this.buttonM7GraphicsBrowse.TabIndex = 5;
            this.buttonM7GraphicsBrowse.Text = "Browse Graphics";
            this.buttonM7GraphicsBrowse.UseVisualStyleBackColor = true;
            this.buttonM7GraphicsBrowse.Click += new System.EventHandler(this.buttonBrowseGeneral_Click);
            // 
            // textM7Graphics
            // 
            this.textM7Graphics.Location = new System.Drawing.Point(13, 70);
            this.textM7Graphics.MaxLength = 260;
            this.textM7Graphics.Name = "textM7Graphics";
            this.textM7Graphics.Size = new System.Drawing.Size(256, 20);
            this.textM7Graphics.TabIndex = 4;
            // 
            // labelM7Graphics
            // 
            this.labelM7Graphics.AutoSize = true;
            this.labelM7Graphics.Location = new System.Drawing.Point(13, 53);
            this.labelM7Graphics.Name = "labelM7Graphics";
            this.labelM7Graphics.Size = new System.Drawing.Size(190, 13);
            this.labelM7Graphics.TabIndex = 3;
            this.labelM7Graphics.Text = "Mode 7 Graphics (Track Tile Graphics)";
            // 
            // buttonColorPaletteBrowse
            // 
            this.buttonColorPaletteBrowse.Location = new System.Drawing.Point(275, 109);
            this.buttonColorPaletteBrowse.Name = "buttonColorPaletteBrowse";
            this.buttonColorPaletteBrowse.Size = new System.Drawing.Size(97, 22);
            this.buttonColorPaletteBrowse.TabIndex = 8;
            this.buttonColorPaletteBrowse.Text = "Browse Palette";
            this.buttonColorPaletteBrowse.UseVisualStyleBackColor = true;
            this.buttonColorPaletteBrowse.Click += new System.EventHandler(this.buttonBrowseGeneral_Click);
            // 
            // textColorPalette
            // 
            this.textColorPalette.Location = new System.Drawing.Point(13, 110);
            this.textColorPalette.MaxLength = 260;
            this.textColorPalette.Name = "textColorPalette";
            this.textColorPalette.Size = new System.Drawing.Size(256, 20);
            this.textColorPalette.TabIndex = 7;
            // 
            // labelColorPalette
            // 
            this.labelColorPalette.AutoSize = true;
            this.labelColorPalette.Location = new System.Drawing.Point(13, 93);
            this.labelColorPalette.Name = "labelColorPalette";
            this.labelColorPalette.Size = new System.Drawing.Size(72, 13);
            this.labelColorPalette.TabIndex = 6;
            this.labelColorPalette.Text = "Color Palettes";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(297, 201);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 9;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(216, 201);
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
            this.checkLoadCommon.Location = new System.Drawing.Point(13, 137);
            this.checkLoadCommon.Name = "checkLoadCommon";
            this.checkLoadCommon.Size = new System.Drawing.Size(133, 17);
            this.checkLoadCommon.TabIndex = 12;
            this.checkLoadCommon.Text = "Load Common Tile File";
            this.checkLoadCommon.UseVisualStyleBackColor = true;
            this.checkLoadCommon.CheckedChanged += new System.EventHandler(this.checkLoadCommon_CheckedChanged);
            // 
            // buttonCommon
            // 
            this.buttonCommon.Location = new System.Drawing.Point(275, 172);
            this.buttonCommon.Name = "buttonCommon";
            this.buttonCommon.Size = new System.Drawing.Size(97, 22);
            this.buttonCommon.TabIndex = 15;
            this.buttonCommon.Text = "Browse Common";
            this.buttonCommon.UseVisualStyleBackColor = true;
            this.buttonCommon.Click += new System.EventHandler(this.buttonBrowseGeneral_Click);
            // 
            // textCommon
            // 
            this.textCommon.Location = new System.Drawing.Point(13, 173);
            this.textCommon.MaxLength = 260;
            this.textCommon.Name = "textCommon";
            this.textCommon.Size = new System.Drawing.Size(256, 20);
            this.textCommon.TabIndex = 14;
            // 
            // labelCommon
            // 
            this.labelCommon.AutoSize = true;
            this.labelCommon.Location = new System.Drawing.Point(12, 157);
            this.labelCommon.Name = "labelCommon";
            this.labelCommon.Size = new System.Drawing.Size(73, 13);
            this.labelCommon.TabIndex = 13;
            this.labelCommon.Text = "Common Tiles";
            // 
            // TM_OpenDirect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 236);
            this.Controls.Add(this.buttonCommon);
            this.Controls.Add(this.textCommon);
            this.Controls.Add(this.labelCommon);
            this.Controls.Add(this.checkLoadCommon);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonColorPaletteBrowse);
            this.Controls.Add(this.textColorPalette);
            this.Controls.Add(this.labelColorPalette);
            this.Controls.Add(this.buttonM7GraphicsBrowse);
            this.Controls.Add(this.textM7Graphics);
            this.Controls.Add(this.labelM7Graphics);
            this.Controls.Add(this.buttonBrowseM7Screen);
            this.Controls.Add(this.textM7ScreenPath);
            this.Controls.Add(this.labelMode7Scr);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "TM_OpenDirect";
            this.Text = "Load Track Assets Directly";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}