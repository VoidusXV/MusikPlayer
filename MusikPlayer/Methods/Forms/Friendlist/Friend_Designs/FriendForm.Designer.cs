namespace MusikPlayer.Methods.Forms.Friendlist.Friend_Designs
{
    partial class FriendForm
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Status_Label = new MusikPlayer.Design.iLabel();
            this.Username_Label = new MusikPlayer.Design.iLabel();
            this.hLine1 = new MusikPlayer.Design.HLine();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(14)))), ((int)(((byte)(15)))));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(92, 70);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Status_Label
            // 
            this.Status_Label.AlphaColor = 255F;
            this.Status_Label.AutoSize = true;
            this.Status_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status_Label.ForeColor = System.Drawing.Color.White;
            this.Status_Label.HoverAnimation = false;
            this.Status_Label.Location = new System.Drawing.Point(110, 53);
            this.Status_Label.Name = "Status_Label";
            this.Status_Label.Select = false;
            this.Status_Label.Size = new System.Drawing.Size(51, 16);
            this.Status_Label.TabIndex = 2;
            this.Status_Label.Text = "iLabel2";
            this.Status_Label.UseMnemonic = false;
            // 
            // Username_Label
            // 
            this.Username_Label.AlphaColor = 255F;
            this.Username_Label.AutoSize = true;
            this.Username_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Username_Label.ForeColor = System.Drawing.Color.White;
            this.Username_Label.HoverAnimation = false;
            this.Username_Label.Location = new System.Drawing.Point(110, 26);
            this.Username_Label.Name = "Username_Label";
            this.Username_Label.Select = false;
            this.Username_Label.Size = new System.Drawing.Size(54, 18);
            this.Username_Label.TabIndex = 1;
            this.Username_Label.Text = "iLabel1";
            this.Username_Label.UseMnemonic = false;
            // 
            // hLine1
            // 
            this.hLine1.AlphaColor = 100;
            this.hLine1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hLine1.LineColor = System.Drawing.Color.White;
            this.hLine1.Location = new System.Drawing.Point(0, 93);
            this.hLine1.Name = "hLine1";
            this.hLine1.Size = new System.Drawing.Size(380, 2);
            this.hLine1.TabIndex = 3;
            this.hLine1.Text = "hLine1";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(14)))), ((int)(((byte)(15)))));
            this.panel1.Controls.Add(this.hLine1);
            this.panel1.Controls.Add(this.Status_Label);
            this.panel1.Controls.Add(this.Username_Label);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(380, 95);
            this.panel1.TabIndex = 4;
            // 
            // FriendForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(14)))), ((int)(((byte)(15)))));
            this.ClientSize = new System.Drawing.Size(380, 95);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FriendForm";
            this.Text = "FriendForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pictureBox1;
        public Design.iLabel Username_Label;
        public Design.iLabel Status_Label;
        private Design.HLine hLine1;
        public System.Windows.Forms.Panel panel1;
    }
}