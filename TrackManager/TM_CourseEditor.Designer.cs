
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
            this.checkAvoidFF = new System.Windows.Forms.CheckBox();
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
            this.tabOverlay = new System.Windows.Forms.TabPage();
            this.labelOverlayCapacity = new System.Windows.Forms.Label();
            this.buttonPlaceOverlay = new System.Windows.Forms.Button();
            this.buttonOverlayChange = new System.Windows.Forms.Button();
            this.buttonResetRotation = new System.Windows.Forms.Button();
            this.buttonRotateCounterClockwise = new System.Windows.Forms.Button();
            this.buttonRotateClockwise = new System.Windows.Forms.Button();
            this.panelOverlayContainer = new System.Windows.Forms.Panel();
            this.pictureOvrTilePreview = new System.Windows.Forms.PictureBox();
            this.numericOverlaySize = new System.Windows.Forms.NumericUpDown();
            this.labelOverlaySize = new System.Windows.Forms.Label();
            this.numericOverlayPattern = new System.Windows.Forms.NumericUpDown();
            this.labelOverlayPattern = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonSaveAdv = new System.Windows.Forms.Button();
            this.panelTrackEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTrackDisplay)).BeginInit();
            this.tabFunctions.SuspendLayout();
            this.tabFullSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTileSelect)).BeginInit();
            this.tabPieces.SuspendLayout();
            this.panelPasteDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePasteDisplay)).BeginInit();
            this.tabOverlay.SuspendLayout();
            this.panelOverlayContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureOvrTilePreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericOverlaySize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericOverlayPattern)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonZoomIn
            // 
            this.buttonZoomIn.Location = new System.Drawing.Point(313, 13);
            this.buttonZoomIn.Name = "buttonZoomIn";
            this.buttonZoomIn.Size = new System.Drawing.Size(75, 23);
            this.buttonZoomIn.TabIndex = 1;
            this.buttonZoomIn.Text = "Zoom In!!";
            this.buttonZoomIn.UseVisualStyleBackColor = true;
            this.buttonZoomIn.Click += new System.EventHandler(this.buttonZoomIn_Click);
            // 
            // buttonZoomOut
            // 
            this.buttonZoomOut.Location = new System.Drawing.Point(395, 13);
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
            this.buttonUndo.Location = new System.Drawing.Point(542, 13);
            this.buttonUndo.Name = "buttonUndo";
            this.buttonUndo.Size = new System.Drawing.Size(75, 23);
            this.buttonUndo.TabIndex = 5;
            this.buttonUndo.Text = "UNDO";
            this.buttonUndo.UseVisualStyleBackColor = true;
            this.buttonUndo.Click += new System.EventHandler(this.buttonUndo_Click);
            // 
            // buttonRedo
            // 
            this.buttonRedo.Location = new System.Drawing.Point(623, 13);
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
            this.tabFunctions.Controls.Add(this.tabOverlay);
            this.tabFunctions.Location = new System.Drawing.Point(847, 43);
            this.tabFunctions.Name = "tabFunctions";
            this.tabFunctions.SelectedIndex = 0;
            this.tabFunctions.Size = new System.Drawing.Size(149, 674);
            this.tabFunctions.TabIndex = 7;
            this.tabFunctions.SelectedIndexChanged += new System.EventHandler(this.tabModeChange);
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
            // tabOverlay
            // 
            this.tabOverlay.Controls.Add(this.labelOverlayCapacity);
            this.tabOverlay.Controls.Add(this.buttonPlaceOverlay);
            this.tabOverlay.Controls.Add(this.buttonOverlayChange);
            this.tabOverlay.Controls.Add(this.buttonResetRotation);
            this.tabOverlay.Controls.Add(this.buttonRotateCounterClockwise);
            this.tabOverlay.Controls.Add(this.buttonRotateClockwise);
            this.tabOverlay.Controls.Add(this.panelOverlayContainer);
            this.tabOverlay.Controls.Add(this.numericOverlaySize);
            this.tabOverlay.Controls.Add(this.labelOverlaySize);
            this.tabOverlay.Controls.Add(this.numericOverlayPattern);
            this.tabOverlay.Controls.Add(this.labelOverlayPattern);
            this.tabOverlay.Location = new System.Drawing.Point(4, 22);
            this.tabOverlay.Name = "tabOverlay";
            this.tabOverlay.Padding = new System.Windows.Forms.Padding(3);
            this.tabOverlay.Size = new System.Drawing.Size(141, 648);
            this.tabOverlay.TabIndex = 2;
            this.tabOverlay.Text = "Toppings";
            this.tabOverlay.UseVisualStyleBackColor = true;
            // 
            // labelOverlayCapacity
            // 
            this.labelOverlayCapacity.AutoSize = true;
            this.labelOverlayCapacity.Location = new System.Drawing.Point(9, 149);
            this.labelOverlayCapacity.Name = "labelOverlayCapacity";
            this.labelOverlayCapacity.Size = new System.Drawing.Size(83, 13);
            this.labelOverlayCapacity.TabIndex = 11;
            this.labelOverlayCapacity.Text = "Capacity: 0 / 41";
            // 
            // buttonPlaceOverlay
            // 
            this.buttonPlaceOverlay.Location = new System.Drawing.Point(6, 119);
            this.buttonPlaceOverlay.Name = "buttonPlaceOverlay";
            this.buttonPlaceOverlay.Size = new System.Drawing.Size(129, 23);
            this.buttonPlaceOverlay.TabIndex = 10;
            this.buttonPlaceOverlay.Text = "Place Piece";
            this.buttonPlaceOverlay.UseVisualStyleBackColor = true;
            this.buttonPlaceOverlay.Click += new System.EventHandler(this.buttonPlaceOverlay_Click);
            // 
            // buttonOverlayChange
            // 
            this.buttonOverlayChange.Location = new System.Drawing.Point(6, 89);
            this.buttonOverlayChange.Name = "buttonOverlayChange";
            this.buttonOverlayChange.Size = new System.Drawing.Size(129, 23);
            this.buttonOverlayChange.TabIndex = 9;
            this.buttonOverlayChange.Text = "Apply Change";
            this.buttonOverlayChange.UseVisualStyleBackColor = true;
            this.buttonOverlayChange.Click += new System.EventHandler(this.buttonOverlayChange_Click);
            // 
            // buttonResetRotation
            // 
            this.buttonResetRotation.Location = new System.Drawing.Point(36, 59);
            this.buttonResetRotation.Name = "buttonResetRotation";
            this.buttonResetRotation.Size = new System.Drawing.Size(70, 23);
            this.buttonResetRotation.TabIndex = 8;
            this.buttonResetRotation.Text = "Center";
            this.buttonResetRotation.UseVisualStyleBackColor = true;
            this.buttonResetRotation.Click += new System.EventHandler(this.buttonResetRotation_Click);
            // 
            // buttonRotateCounterClockwise
            // 
            this.buttonRotateCounterClockwise.Location = new System.Drawing.Point(6, 59);
            this.buttonRotateCounterClockwise.Name = "buttonRotateCounterClockwise";
            this.buttonRotateCounterClockwise.Size = new System.Drawing.Size(23, 23);
            this.buttonRotateCounterClockwise.TabIndex = 7;
            this.buttonRotateCounterClockwise.Text = "L";
            this.buttonRotateCounterClockwise.UseVisualStyleBackColor = true;
            this.buttonRotateCounterClockwise.Click += new System.EventHandler(this.buttonRotateCounterClockwise_Click);
            // 
            // buttonRotateClockwise
            // 
            this.buttonRotateClockwise.Location = new System.Drawing.Point(112, 59);
            this.buttonRotateClockwise.Name = "buttonRotateClockwise";
            this.buttonRotateClockwise.Size = new System.Drawing.Size(23, 23);
            this.buttonRotateClockwise.TabIndex = 6;
            this.buttonRotateClockwise.Text = "R";
            this.buttonRotateClockwise.UseVisualStyleBackColor = true;
            this.buttonRotateClockwise.Click += new System.EventHandler(this.buttonRotateClockwise_Click);
            // 
            // panelOverlayContainer
            // 
            this.panelOverlayContainer.AutoScroll = true;
            this.panelOverlayContainer.Controls.Add(this.pictureOvrTilePreview);
            this.panelOverlayContainer.Location = new System.Drawing.Point(7, 165);
            this.panelOverlayContainer.Name = "panelOverlayContainer";
            this.panelOverlayContainer.Size = new System.Drawing.Size(128, 477);
            this.panelOverlayContainer.TabIndex = 5;
            // 
            // pictureOvrTilePreview
            // 
            this.pictureOvrTilePreview.Location = new System.Drawing.Point(4, 4);
            this.pictureOvrTilePreview.Name = "pictureOvrTilePreview";
            this.pictureOvrTilePreview.Size = new System.Drawing.Size(121, 473);
            this.pictureOvrTilePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureOvrTilePreview.TabIndex = 0;
            this.pictureOvrTilePreview.TabStop = false;
            // 
            // numericOverlaySize
            // 
            this.numericOverlaySize.Location = new System.Drawing.Point(53, 33);
            this.numericOverlaySize.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericOverlaySize.Name = "numericOverlaySize";
            this.numericOverlaySize.Size = new System.Drawing.Size(82, 20);
            this.numericOverlaySize.TabIndex = 3;
            this.numericOverlaySize.ValueChanged += new System.EventHandler(this.numericOverlaySize_ValueChanged);
            // 
            // labelOverlaySize
            // 
            this.labelOverlaySize.AutoSize = true;
            this.labelOverlaySize.Location = new System.Drawing.Point(6, 35);
            this.labelOverlaySize.Name = "labelOverlaySize";
            this.labelOverlaySize.Size = new System.Drawing.Size(27, 13);
            this.labelOverlaySize.TabIndex = 2;
            this.labelOverlaySize.Text = "Size";
            // 
            // numericOverlayPattern
            // 
            this.numericOverlayPattern.Location = new System.Drawing.Point(53, 7);
            this.numericOverlayPattern.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericOverlayPattern.Name = "numericOverlayPattern";
            this.numericOverlayPattern.Size = new System.Drawing.Size(82, 20);
            this.numericOverlayPattern.TabIndex = 1;
            this.numericOverlayPattern.ValueChanged += new System.EventHandler(this.numericOverlayPattern_ValueChanged);
            // 
            // labelOverlayPattern
            // 
            this.labelOverlayPattern.AutoSize = true;
            this.labelOverlayPattern.Location = new System.Drawing.Point(6, 9);
            this.labelOverlayPattern.Name = "labelOverlayPattern";
            this.labelOverlayPattern.Size = new System.Drawing.Size(41, 13);
            this.labelOverlayPattern.TabIndex = 0;
            this.labelOverlayPattern.Text = "Pattern";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(13, 13);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 8;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonSaveAdv
            // 
            this.buttonSaveAdv.Location = new System.Drawing.Point(95, 13);
            this.buttonSaveAdv.Name = "buttonSaveAdv";
            this.buttonSaveAdv.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveAdv.TabIndex = 9;
            this.buttonSaveAdv.Text = "Save Adv.";
            this.buttonSaveAdv.UseVisualStyleBackColor = true;
            this.buttonSaveAdv.Click += new System.EventHandler(this.buttonSaveAdv_Click);
            // 
            // TM_CourseEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.buttonSaveAdv);
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
            this.tabOverlay.ResumeLayout(false);
            this.tabOverlay.PerformLayout();
            this.panelOverlayContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureOvrTilePreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericOverlaySize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericOverlayPattern)).EndInit();
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
        private System.Windows.Forms.Button buttonSaveAdv;
        private System.Windows.Forms.TabPage tabOverlay;
        private System.Windows.Forms.Panel panelOverlayContainer;
        private System.Windows.Forms.NumericUpDown numericOverlaySize;
        private System.Windows.Forms.Label labelOverlaySize;
        private System.Windows.Forms.NumericUpDown numericOverlayPattern;
        private System.Windows.Forms.Label labelOverlayPattern;
        private System.Windows.Forms.PictureBox pictureOvrTilePreview;
        private System.Windows.Forms.Button buttonResetRotation;
        private System.Windows.Forms.Button buttonRotateCounterClockwise;
        private System.Windows.Forms.Button buttonRotateClockwise;
        private System.Windows.Forms.Label labelOverlayCapacity;
        private System.Windows.Forms.Button buttonPlaceOverlay;
        private System.Windows.Forms.Button buttonOverlayChange;
    }
}