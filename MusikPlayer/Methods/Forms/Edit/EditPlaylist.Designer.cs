namespace MusikPlayer.Methods.Forms.Edit
{
    partial class EditPlaylist
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
            this.iButton21 = new MusikPlayer.Design.iButton2();
            this.iLabel1 = new MusikPlayer.Design.iLabel();
            this.iPanel21 = new MusikPlayer.Design.iPanel2();
            this.iPanel22 = new MusikPlayer.Design.iPanel2();
            this.iButton1 = new MusikPlayer.Design.iButton();
            this.iLabel2 = new MusikPlayer.Design.iLabel();
            this.iTextBox1 = new MusikPlayer.Design.iTextBox();
            this.OpenFormFadeTimer = new System.Windows.Forms.Timer(this.components);
            this.CloseFormFadeTimer = new System.Windows.Forms.Timer(this.components);
            this.iPanel21.SuspendLayout();
            this.iPanel22.SuspendLayout();
            this.SuspendLayout();
            // 
            // iButton21
            // 
            this.iButton21.AlphaColor = 255F;
            this.iButton21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.iButton21.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.iButton21.BorderColor = System.Drawing.Color.Gray;
            this.iButton21.BorderRadius = 3;
            this.iButton21.BorderSize = 1;
            this.iButton21.ForeColor = System.Drawing.Color.White;
            this.iButton21.HoverBrightnessVal = 0.1F;
            this.iButton21.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.iButton21.Image = null;
            this.iButton21.ImageSize = new System.Drawing.Size(0, 0);
            this.iButton21.Location = new System.Drawing.Point(262, 108);
            this.iButton21.Name = "iButton21";
            this.iButton21.PressAnimimation = true;
            this.iButton21.Size = new System.Drawing.Size(150, 40);
            this.iButton21.TabIndex = 0;
            this.iButton21.Text = "Speichern";
            this.iButton21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.iButton21.TextColor = System.Drawing.Color.White;
            this.iButton21.Click += new System.EventHandler(this.iButton21_Click);
            // 
            // iLabel1
            // 
            this.iLabel1.AlphaColor = 255F;
            this.iLabel1.AutoSize = true;
            this.iLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iLabel1.ForeColor = System.Drawing.Color.White;
            this.iLabel1.HoverAnimation = false;
            this.iLabel1.Location = new System.Drawing.Point(29, 51);
            this.iLabel1.Name = "iLabel1";
            this.iLabel1.Select = false;
            this.iLabel1.Size = new System.Drawing.Size(44, 16);
            this.iLabel1.TabIndex = 2;
            this.iLabel1.Text = "Name";
            this.iLabel1.UseMnemonic = false;
            // 
            // iPanel21
            // 
            this.iPanel21.AlphaColor = 255;
            this.iPanel21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.iPanel21.Border = false;
            this.iPanel21.BorderColor = System.Drawing.Color.White;
            this.iPanel21.Controls.Add(this.iPanel22);
            this.iPanel21.Controls.Add(this.iLabel1);
            this.iPanel21.Controls.Add(this.iTextBox1);
            this.iPanel21.Controls.Add(this.iButton21);
            this.iPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iPanel21.Location = new System.Drawing.Point(0, 0);
            this.iPanel21.Name = "iPanel21";
            this.iPanel21.Size = new System.Drawing.Size(460, 161);
            this.iPanel21.TabIndex = 3;
            // 
            // iPanel22
            // 
            this.iPanel22.AlphaColor = 255;
            this.iPanel22.Border = false;
            this.iPanel22.BorderColor = System.Drawing.Color.White;
            this.iPanel22.Controls.Add(this.iButton1);
            this.iPanel22.Controls.Add(this.iLabel2);
            this.iPanel22.Dock = System.Windows.Forms.DockStyle.Top;
            this.iPanel22.Location = new System.Drawing.Point(0, 0);
            this.iPanel22.Name = "iPanel22";
            this.iPanel22.Size = new System.Drawing.Size(460, 37);
            this.iPanel22.TabIndex = 4;
            // 
            // iButton1
            // 
            this.iButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.iButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.iButton1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.iButton1.BorderRadius = 0;
            this.iButton1.BorderSize = 0;
            this.iButton1.CustomIcon = MusikPlayer.Design.iButton.CollapseCustomIcon.None;
            this.iButton1.FlatAppearance.BorderSize = 0;
            this.iButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iButton1.ForeColor = System.Drawing.Color.White;
            this.iButton1.ImageSize = new System.Drawing.Size(0, 0);
            this.iButton1.Location = new System.Drawing.Point(416, 3);
            this.iButton1.Name = "iButton1";
            this.iButton1.Size = new System.Drawing.Size(40, 31);
            this.iButton1.TabIndex = 5;
            this.iButton1.Text = "X";
            this.iButton1.TextColor = System.Drawing.Color.White;
            this.iButton1.UseVisualStyleBackColor = false;
            this.iButton1.Click += new System.EventHandler(this.iButton1_Click);
            // 
            // iLabel2
            // 
            this.iLabel2.AlphaColor = 255F;
            this.iLabel2.AutoSize = true;
            this.iLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.iLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iLabel2.ForeColor = System.Drawing.Color.White;
            this.iLabel2.HoverAnimation = false;
            this.iLabel2.Location = new System.Drawing.Point(123, 6);
            this.iLabel2.Name = "iLabel2";
            this.iLabel2.Select = false;
            this.iLabel2.Size = new System.Drawing.Size(208, 25);
            this.iLabel2.TabIndex = 3;
            this.iLabel2.Text = "Playlist bearbeiten";
            this.iLabel2.UseMnemonic = false;
            // 
            // iTextBox1
            // 
            this.iTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.iTextBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.iTextBox1.BorderFocusColor = System.Drawing.Color.DodgerBlue;
            this.iTextBox1.BorderRadius = 0;
            this.iTextBox1.BorderSize = 2;
            this.iTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iTextBox1.ForeColor = System.Drawing.Color.White;
            this.iTextBox1.Location = new System.Drawing.Point(32, 71);
            this.iTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.iTextBox1.Multiline = true;
            this.iTextBox1.Name = "iTextBox1";
            this.iTextBox1.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.iTextBox1.PasswordChar = false;
            this.iTextBox1.PlaceholderColor = System.Drawing.Color.White;
            this.iTextBox1.PlaceholderText = "Name";
            this.iTextBox1.Size = new System.Drawing.Size(380, 30);
            this.iTextBox1.TabIndex = 1;
            this.iTextBox1.Texts = "";
            this.iTextBox1.UnderlinedStyle = false;
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
            // EditPlaylist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(460, 161);
            this.Controls.Add(this.iPanel21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "EditPlaylist";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EditPlaylist";
            this.iPanel21.ResumeLayout(false);
            this.iPanel21.PerformLayout();
            this.iPanel22.ResumeLayout(false);
            this.iPanel22.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Design.iButton2 iButton21;
        private Design.iLabel iLabel1;
        private Design.iPanel2 iPanel21;
        private Design.iLabel iLabel2;
        private Design.iTextBox iTextBox1;
        private System.Windows.Forms.Timer OpenFormFadeTimer;
        private System.Windows.Forms.Timer CloseFormFadeTimer;
        private Design.iPanel2 iPanel22;
        private Design.iButton iButton1;
    }
}