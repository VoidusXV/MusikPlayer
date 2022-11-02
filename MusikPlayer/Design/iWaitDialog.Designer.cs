namespace MusikPlayer.Design
{
    partial class iWaitDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(iWaitDialog));
            this.iPanel21 = new MusikPlayer.Design.iPanel2();
            this.iLabel1 = new MusikPlayer.Design.iLabel();
            this.OpenFormFadeTimer = new System.Windows.Forms.Timer(this.components);
            this.CloseFormFadeTimer = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.iLabel2 = new MusikPlayer.Design.iLabel();
            this.iPanel21.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // iPanel21
            // 
            this.iPanel21.AlphaColor = 255;
            this.iPanel21.Border = true;
            this.iPanel21.BorderColor = System.Drawing.Color.White;
            this.iPanel21.Controls.Add(this.iLabel2);
            this.iPanel21.Controls.Add(this.pictureBox1);
            this.iPanel21.Controls.Add(this.iLabel1);
            this.iPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iPanel21.Location = new System.Drawing.Point(0, 0);
            this.iPanel21.Name = "iPanel21";
            this.iPanel21.Size = new System.Drawing.Size(271, 76);
            this.iPanel21.TabIndex = 0;
            // 
            // iLabel1
            // 
            this.iLabel1.AlphaColor = 255F;
            this.iLabel1.AutoSize = true;
            this.iLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iLabel1.ForeColor = System.Drawing.Color.White;
            this.iLabel1.HoverAnimation = false;
            this.iLabel1.Location = new System.Drawing.Point(79, 15);
            this.iLabel1.Name = "iLabel1";
            this.iLabel1.Select = false;
            this.iLabel1.Size = new System.Drawing.Size(39, 20);
            this.iLabel1.TabIndex = 0;
            this.iLabel1.Text = "Text";
            this.iLabel1.UseMnemonic = false;
            this.iLabel1.SizeChanged += new System.EventHandler(this.iLabel1_SizeChanged);
            this.iLabel1.TextChanged += new System.EventHandler(this.iLabel1_TextChanged);
            // 
            // OpenFormFadeTimer
            // 
            this.OpenFormFadeTimer.Interval = 1;
            this.OpenFormFadeTimer.Tick += new System.EventHandler(this.OpenFormFadeTimer_Tick);
            // 
            // CloseFormFadeTimer
            // 
            this.CloseFormFadeTimer.Interval = 1;
            this.CloseFormFadeTimer.Tick += new System.EventHandler(this.CloseFormFadeTimer_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(13, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // iLabel2
            // 
            this.iLabel2.AlphaColor = 255F;
            this.iLabel2.AutoSize = true;
            this.iLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iLabel2.ForeColor = System.Drawing.Color.White;
            this.iLabel2.HoverAnimation = false;
            this.iLabel2.Location = new System.Drawing.Point(81, 43);
            this.iLabel2.Name = "iLabel2";
            this.iLabel2.Select = false;
            this.iLabel2.Size = new System.Drawing.Size(75, 16);
            this.iLabel2.TabIndex = 2;
            this.iLabel2.Text = "Bitte warten";
            this.iLabel2.UseMnemonic = false;
            // 
            // iWaitDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(271, 76);
            this.Controls.Add(this.iPanel21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "iWaitDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "iWaitDialog";
            this.iPanel21.ResumeLayout(false);
            this.iPanel21.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public iPanel2 iPanel21;
        public iLabel iLabel1;
        private System.Windows.Forms.Timer OpenFormFadeTimer;
        private System.Windows.Forms.Timer CloseFormFadeTimer;
        private System.Windows.Forms.PictureBox pictureBox1;
        public iLabel iLabel2;
    }
}