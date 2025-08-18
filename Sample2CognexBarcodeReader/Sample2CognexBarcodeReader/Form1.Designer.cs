namespace Sample2CognexBarcodeReader
{
    partial class Form1
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
            this.groupBox3 = new System.Windows.Forms.Panel();
            this.lbReadString = new System.Windows.Forms.Label();
            this.picResultImage = new System.Windows.Forms.PictureBox();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picResultImage)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lbReadString);
            this.groupBox3.Controls.Add(this.picResultImage);
            this.groupBox3.Location = new System.Drawing.Point(4, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(398, 437);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.Text = "Results";
            // 
            // lbReadString
            // 
            this.lbReadString.Location = new System.Drawing.Point(15, 396);
            this.lbReadString.Name = "lbReadString";
            this.lbReadString.Size = new System.Drawing.Size(368, 23);
            this.lbReadString.TabIndex = 16;
            // 
            // picResultImage
            // 
            this.picResultImage.Location = new System.Drawing.Point(15, 55);
            this.picResultImage.Name = "picResultImage";
            this.picResultImage.Size = new System.Drawing.Size(368, 338);
            this.picResultImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picResultImage.TabIndex = 0;
            this.picResultImage.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(413, 468);
            this.Controls.Add(this.groupBox3);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picResultImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel groupBox3;
        private System.Windows.Forms.Label lbReadString;
        private System.Windows.Forms.PictureBox picResultImage;
    }
}

