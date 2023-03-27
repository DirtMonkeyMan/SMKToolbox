
namespace SMKToolbox
{
    partial class Main
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
            this.buttonLayoutEditor = new System.Windows.Forms.Button();
            this.buttonRankingNumbers = new System.Windows.Forms.Button();
            this.buttonDemoEditor = new System.Windows.Forms.Button();
            this.buttonAssetManager = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonLayoutEditor
            // 
            this.buttonLayoutEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLayoutEditor.Location = new System.Drawing.Point(12, 12);
            this.buttonLayoutEditor.Name = "buttonLayoutEditor";
            this.buttonLayoutEditor.Size = new System.Drawing.Size(439, 23);
            this.buttonLayoutEditor.TabIndex = 0;
            this.buttonLayoutEditor.Text = "Title Screen Layout Editor";
            this.buttonLayoutEditor.UseVisualStyleBackColor = true;
            this.buttonLayoutEditor.Click += new System.EventHandler(this.buttonLayoutEditor_Click);
            // 
            // buttonRankingNumbers
            // 
            this.buttonRankingNumbers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRankingNumbers.Location = new System.Drawing.Point(13, 42);
            this.buttonRankingNumbers.Name = "buttonRankingNumbers";
            this.buttonRankingNumbers.Size = new System.Drawing.Size(438, 23);
            this.buttonRankingNumbers.TabIndex = 1;
            this.buttonRankingNumbers.Text = "Ranking Numbers Editor";
            this.buttonRankingNumbers.UseVisualStyleBackColor = true;
            this.buttonRankingNumbers.Click += new System.EventHandler(this.buttonRankingNumbers_Click);
            // 
            // buttonDemoEditor
            // 
            this.buttonDemoEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDemoEditor.Location = new System.Drawing.Point(13, 72);
            this.buttonDemoEditor.Name = "buttonDemoEditor";
            this.buttonDemoEditor.Size = new System.Drawing.Size(438, 23);
            this.buttonDemoEditor.TabIndex = 2;
            this.buttonDemoEditor.Text = "Demo Editor";
            this.buttonDemoEditor.UseVisualStyleBackColor = true;
            this.buttonDemoEditor.Click += new System.EventHandler(this.buttonDemoEditor_Click);
            // 
            // buttonAssetManager
            // 
            this.buttonAssetManager.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAssetManager.Location = new System.Drawing.Point(12, 101);
            this.buttonAssetManager.Name = "buttonAssetManager";
            this.buttonAssetManager.Size = new System.Drawing.Size(438, 23);
            this.buttonAssetManager.TabIndex = 3;
            this.buttonAssetManager.Text = "Asset Manager";
            this.buttonAssetManager.UseVisualStyleBackColor = true;
            this.buttonAssetManager.Click += new System.EventHandler(this.buttonAssetManager_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 268);
            this.Controls.Add(this.buttonAssetManager);
            this.Controls.Add(this.buttonDemoEditor);
            this.Controls.Add(this.buttonRankingNumbers);
            this.Controls.Add(this.buttonLayoutEditor);
            this.Name = "Main";
            this.Text = "SMK Tool Box 0.0.2";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonLayoutEditor;
        private System.Windows.Forms.Button buttonRankingNumbers;
        private System.Windows.Forms.Button buttonDemoEditor;
        private System.Windows.Forms.Button buttonAssetManager;
    }
}

