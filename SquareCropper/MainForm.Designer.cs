namespace SquareCropper
{
    partial class MainForm
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
            this.MainPB = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.MainPB)).BeginInit();
            this.SuspendLayout();
            // 
            // MainPB
            // 
            this.MainPB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPB.Location = new System.Drawing.Point(0, 0);
            this.MainPB.Name = "MainPB";
            this.MainPB.Size = new System.Drawing.Size(682, 657);
            this.MainPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.MainPB.TabIndex = 0;
            this.MainPB.TabStop = false;
            this.MainPB.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainPB_DragDrop);
            this.MainPB.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainPB_DragEnter);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 657);
            this.Controls.Add(this.MainPB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SquareCropper";
            ((System.ComponentModel.ISupportInitialize)(this.MainPB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox MainPB;
    }
}

