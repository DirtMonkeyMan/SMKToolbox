
namespace SMKToolbox
{
    partial class TM_CourseEditor
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
            this.buttonZoomIn = new System.Windows.Forms.Button();
            this.buttonZoomOut = new System.Windows.Forms.Button();
            this.panelTrackEditor = new System.Windows.Forms.Panel();
            this.pictureTrackDisplay = new System.Windows.Forms.PictureBox();
            this.buttonUndo = new System.Windows.Forms.Button();
            this.buttonRedo = new System.Windows.Forms.Button();
            this.tabFunctions = new System.Windows.Forms.TabControl();
            this.tabFullSet = new System.Windows.Forms.TabPage();
            this.pictureTileSelect = new System.Windows.Forms.PictureBox();
            this.tabPieces = new System.Windows.Forms.TabPage();
            this.buttonEditPaste = new System.Windows.Forms.Button();
            this.buttonUsePaste = new System.Windows.Forms.Button();
            this.buttonDeletePaste = new System.Windows.Forms.Button();
            this.labelPasteNumber = new System.Windows.Forms.Label();
            this.buttonStashPaste = new System.Windows.Forms.Button();
            this.buttonPasteRight = new System.Windows.Forms.Button();
            this.buttonPasteLeft = new System.Windows.Forms.Button();
            this.panelPasteDisplay = new System.Windows.Forms.Panel();
            this.picturePasteDisplay = new System.Windows.Forms.PictureBox();
            this.labelSelectPaste = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.checkAvoidFF = new System.Windows.Forms.CheckBox();
            this.panelTrackEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTrackDisplay)).BeginInit();
            this.tabFunctions.SuspendLayout();
            this.tabFullSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTileSelect)).BeginInit();
            this.tabPieces.SuspendLayout();
            this.panelPasteDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePasteDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.Location = new System.Drawing.Point(165, 13);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(75, 23);
            this.buttonZoomIn.TabIndex = 1;
            this.buttonZoomIn.Text = "Zoom In!!";
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            this.buttonZoomIn.Click += new System.EventHandler(this.buttonZoomIn_Click);
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.Location = new System.Drawing.Point(247, 13);
            this.buttonZoomOut.Name = "buttonZoomOut";
            this.buttonZoomOut.Size = new System.Drawing.Size(75, 23);
            this.buttonZoomOut.TabIndex = 2;
            this.buttonZoomOut.Text = "Zoom Out!!";
            this.buttonZoomOut.UseVisualStyleBackColor = true;
            this.buttonZoomOut.Click += new System.EventHandler(this.buttonZoomOut_Click);
            // 
            // panelTrackEditor
            // 
            this.panelTrackEditor.AutoScroll = true;
            this.panelTrackEditor.BackColor = System.Drawing.Color.Black;
            this.panelTrackEditor.Controls.Add(this.pictureTrackDisplay);
            this.panelTrackEditor.Location = new System.Drawing.Point(13, 43);
            this.panelTrackEditor.Name = "panelTrackEditor";
            this.panelTrackEditor.Size = new System.Drawing.Size(828, 674);
            this.panelTrackEditor.TabIndex = 3;
            // 
            // pictureTrackDisplay
            // 
            this.pictureTrackDisplay.Location = new System.Drawing.Point(0, 0);
            this.pictureTrackDisplay.Name = "pictureTrackDisplay";
            this.pictureTrackDisplay.Size = new System.Drawing.Size(1024, 1024);
            this.pictureTrackDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureTrackDisplay.TabIndex = 0;
            this.pictureTrackDisplay.TabStop = false;
            this.pictureTrackDisplay.Paint += new System.Windows.Forms.PaintEventHandler(this.drawOverlay);
            this.pictureTrackDisplay.MouseDown += new System.Windows.Forms.MouseEventHandler(this.clickHandle);
            this.pictureTrackDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseTick);
            this.pictureTrackDisplay.MouseUp += new System.Windows.Forms.MouseEventHandler(this.releaseHandle);
            // 
            // buttonUndo
            // 
            this.buttonUndo.Location = new System.Drawing.Point(394, 13);
            this.buttonUndo.Name = "buttonUndo";
            this.buttonUndo.Size = new System.Drawing.Size(75, 23);
            this.buttonUndo.TabIndex = 5;
            this.buttonUndo.Text = "UNDO";
            this.buttonUndo.UseVisualStyleBackColor = true;
            this.buttonUndo.Click += new System.EventHandler(this.buttonUndo_Click);
            // 
            // buttonRedo
            // 
            this.buttonRedo.Location = new System.Drawing.Point(475, 13);
            this.buttonRedo.Name = "buttonRedo";
            this.buttonRedo.Size = new System.Drawing.Size(75, 23);
            this.buttonRedo.TabIndex = 6;
            this.buttonRedo.Text = "REDO";
            this.buttonRedo.UseVisualStyleBackColor = true;
            this.buttonRedo.Click += new System.EventHandler(this.buttonRedo_Click);
            // 
            // tabFunctions
            // 
            this.tabFunctions.Controls.Add(this.tabFullSet);
            this.tabFunctions.Controls.Add(this.tabPieces);
            this.tabFunctions.Location = new System.Drawing.Point(847, 43);
            this.tabFunctions.Name = "tabFunctions";
            this.tabFunctions.SelectedIndex = 0;
            this.tabFunctions.Size = new System.Drawing.Size(149, 674);
            this.tabFunctions.TabIndex = 7;
            // 
            // tabFullSet
            // 
            this.tabFullSet.Controls.Add(this.pictureTileSelect);
            this.tabFullSet.Location = new System.Drawing.Point(4, 22);
            this.tabFullSet.Name = "tabFullSet";
            this.tabFullSet.Padding = new System.Windows.Forms.Padding(3);
            this.tabFullSet.Size = new System.Drawing.Size(141, 648);
            this.tabFullSet.TabIndex = 0;
            this.tabFullSet.Text = "Full Menu";
            this.tabFullSet.ToolTipText = "All 256 available tiles from the theme.";
            this.tabFullSet.UseVisualStyleBackColor = true;
            // 
            // pictureTileSelect
            // 
            this.pictureTileSelect.Location = new System.Drawing.Point(6, 6);
            this.pictureTileSelect.Name = "pictureTileSelect";
            this.pictureTileSelect.Size = new System.Drawing.Size(128, 512);
            this.pictureTileSelect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureTileSelect.TabIndex = 0;
            this.pictureTileSelect.TabStop = false;
            this.pictureTileSelect.Click += new System.EventHandler(this.clickTileSelect);
            this.pictureTileSelect.Paint += new System.Windows.Forms.PaintEventHandler(this.drawTileSelect);
            // 
            // tabPieces
            // 
            this.tabPieces.Controls.Add(this.checkAvoidFF);
            this.tabPieces.Controls.Add(this.buttonEditPaste);
            this.tabPieces.Controls.Add(this.buttonUsePaste);
            this.tabPieces.Controls.Add(this.buttonDeletePaste);
            this.tabPieces.Controls.Add(this.labelPasteNumber);
            this.tabPieces.Controls.Add(this.buttonStashPaste);
            this.tabPieces.Controls.Add(this.buttonPasteRight);
            this.tabPieces.Controls.Add(this.buttonPasteLeft);
            this.tabPieces.Controls.Add(this.panelPasteDisplay);
            this.tabPieces.Controls.Add(this.labelSelectPaste);
            this.tabPieces.Location = new System.Drawing.Point(4, 22);
            this.tabPieces.Name = "tabPieces";
            this.tabPieces.Padding = new System.Windows.Forms.Padding(3);
            this.tabPieces.Size = new System.Drawing.Size(141, 648);
            this.tabPieces.TabIndex = 1;
            this.tabPieces.Text = "Combo Meals";
            this.tabPieces.ToolTipText = "Saved clipboard pieces that you can edit.";
            this.tabPieces.UseVisualStyleBackColor = true;
            // 
            // buttonEditPaste
            // 
            this.buttonEditPaste.Enabled = false;
            this.buttonEditPaste.Location = new System.Drawing.Point(7, 111);
            this.buttonEditPaste.Name = "buttonEditPaste";
            this.buttonEditPaste.Size = new System.Drawing.Size(126, 23);
            this.buttonEditPaste.TabIndex = 8;
            this.buttonEditPaste.Text = "Edit";
            this.buttonEditPaste.UseVisualStyleBackColor = true;
            this.buttonEditPaste.Click += new System.EventHandler(this.buttonEditPaste_Click);
            // 
            // buttonUsePaste
            // 
            this.buttonUsePaste.Enabled = false;
            this.buttonUsePaste.Location = new System.Drawing.Point(7, 82);
            this.buttonUsePaste.Name = "buttonUsePaste";
            this.buttonUsePaste.Size = new System.Drawing.Size(126, 23);
            this.buttonUsePaste.TabIndex = 7;
            this.buttonUsePaste.Text = "Use";
            this.buttonUsePaste.UseVisualStyleBackColor = true;
            this.buttonUsePaste.Click += new System.EventHandler(this.buttonUsePaste_Click);
            // 
            // buttonDeletePaste
            // 
            this.buttonDeletePaste.Enabled = false;
            this.buttonDeletePaste.Location = new System.Drawing.Point(7, 140);
            this.buttonDeletePaste.Name = "buttonDeletePaste";
            this.buttonDeletePaste.Size = new System.Drawing.Size(126, 23);
            this.buttonDeletePaste.TabIndex = 6;
            this.buttonDeletePaste.Text = "Delete";
            this.buttonDeletePaste.UseVisualStyleBackColor = true;
            this.buttonDeletePaste.Click += new System.EventHandler(this.buttonDeletePaste_Click);
            // 
            // labelPasteNumber
            // 
            this.labelPasteNumber.AutoSize = true;
            this.labelPasteNumber.Location = new System.Drawing.Point(7, 190);
            this.labelPasteNumber.Name = "labelPasteNumber";
            this.labelPasteNumber.Size = new System.Drawing.Size(30, 13);
            this.labelPasteNumber.TabIndex = 5;
            this.labelPasteNumber.Text = "0 / 0";
            // 
            // buttonStashPaste
            // 
            this.buttonStashPaste.Location = new System.Drawing.Point(7, 53);
            this.buttonStashPaste.Name = "buttonStashPaste";
            this.buttonStashPaste.Size = new System.Drawing.Size(126, 23);
            this.buttonStashPaste.TabIndex = 4;
            this.buttonStashPaste.Text = "Stashe";
            this.buttonStashPaste.UseVisualStyleBackColor = true;
            this.buttonStashPaste.Click += new System.EventHandler(this.buttonStashPaste_Click);
            // 
            // buttonPasteRight
            // 
            this.buttonPasteRight.Enabled = false;
            this.buttonPasteRight.Location = new System.Drawing.Point(73, 24);
            this.buttonPasteRight.Name = "buttonPasteRight";
            this.buttonPasteRight.Size = new System.Drawing.Size(60, 23);
            this.buttonPasteRight.TabIndex = 3;
            this.buttonPasteRight.Text = "Right";
            this.buttonPasteRight.UseVisualStyleBackColor = true;
            this.buttonPasteRight.Click += new System.EventHandler(this.buttonPasteRight_Click);
            // 
            // buttonPasteLeft
            // 
            this.buttonPasteLeft.Enabled = false;
            this.buttonPasteLeft.Location = new System.Drawing.Point(7, 24);
            this.buttonPasteLeft.Name = "buttonPasteLeft";
            this.buttonPasteLeft.Size = new System.Drawing.Size(60, 23);
            this.buttonPasteLeft.TabIndex = 2;
            this.buttonPasteLeft.Text = "Left";
            this.buttonPasteLeft.UseVisualStyleBackColor = true;
            this.buttonPasteLeft.Click += new System.EventHandler(this.buttonPasteLeft_Click);
            // 
            // panelPasteDisplay
            // 
            this.panelPasteDisplay.AutoScroll = true;
            this.panelPasteDisplay.Controls.Add(this.picturePasteDisplay);
            this.panelPasteDisplay.Location = new System.Drawing.Point(7, 206);
            this.panelPasteDisplay.Name = "panelPasteDisplay";
            this.panelPasteDisplay.Size = new System.Drawing.Size(128, 436);
            this.panelPasteDisplay.TabIndex = 1;
            // 
            // picturePasteDisplay
            // 
            this.picturePasteDisplay.Location = new System.Drawing.Point(4, 4);
            this.picturePasteDisplay.Name = "picturePasteDisplay";
            this.picturePasteDisplay.Size = new System.Drawing.Size(121, 432);
            this.picturePasteDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picturePasteDisplay.TabIndex = 0;
            this.picturePasteDisplay.TabStop = false;
            // 
            // labelSelectPaste
            // 
            this.labelSelectPaste.AutoSize = true;
            this.labelSelectPaste.Location = new System.Drawing.Point(7, 7);
            this.labelSelectPaste.Name = "labelSelectPaste";
            this.labelSelectPaste.Size = new System.Drawing.Size(67, 13);
            this.labelSelectPaste.TabIndex = 0;
            this.labelSelectPaste.Text = "Select Paste";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(13, 13);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 8;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.MouseUp += new System.Windows.Forms.MouseEventHandler(this.buttonSave_Click);
            // 
            // checkAvoidFF
            // 
            this.checkAvoidFF.AutoSize = true;
            this.checkAvoidFF.Location = new System.Drawing.Point(7, 170);
            this.checkAvoidFF.Name = "checkAvoidFF";
            this.checkAvoidFF.Size = new System.Drawing.Size(103, 17);
            this.checkAvoidFF.TabIndex = 9;
            this.checkAvoidFF.Text = "Avoid 0xFF tiles.";
            this.checkAvoidFF.UseVisualStyleBackColor = true;
            // 
            // TM_CourseEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.tabFunctions);
            this.Controls.Add(this.buttonRedo);
            this.Controls.Add(this.buttonUndo);
            this.Controls.Add(this.panelTrackEditor);
            this.Controls.Add(this.buttonZoomOut);
            this.Controls.Add(this.buttonZoomIn);
            this.KeyPreview = true;
            this.Name = "TM_CourseEditor";
            this.Text = "Course Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.disposeHandle);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.handleHotkey);
            this.Resize += new System.EventHandler(this.resizeHandle);
            this.panelTrackEditor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureTrackDisplay)).EndInit();
            this.tabFunctions.ResumeLayout(false);
            this.tabFullSet.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureTileSelect)).EndInit();
            this.tabPieces.ResumeLayout(false);
            this.tabPieces.PerformLayout();
            this.panelPasteDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picturePasteDisplay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonZoomIn;
        private System.Windows.Forms.Button buttonZoomOut;
        private System.Windows.Forms.Panel panelTrackEditor;
        private System.Windows.Forms.PictureBox pictureTrackDisplay;
        private System.Windows.Forms.Button buttonUndo;
        private System.Windows.Forms.Button buttonRedo;
        private System.Windows.Forms.TabControl tabFunctions;
        private System.Windows.Forms.TabPage tabFullSet;
        private System.Windows.Forms.TabPage tabPieces;
        private System.Windows.Forms.PictureBox pictureTileSelect;
        private System.Windows.Forms.Button buttonPasteLeft;
        private System.Windows.Forms.Panel panelPasteDisplay;
        private System.Windows.Forms.Label labelSelectPaste;
        private System.Windows.Forms.Button buttonPasteRight;
        private System.Windows.Forms.Label labelPasteNumber;
        private System.Windows.Forms.Button buttonStashPaste;
        private System.Windows.Forms.Button buttonUsePaste;
        private System.Windows.Forms.Button buttonDeletePaste;
        private System.Windows.Forms.PictureBox picturePasteDisplay;
        private System.Windows.Forms.Button buttonEditPaste;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.CheckBox checkAvoidFF;
    }
}