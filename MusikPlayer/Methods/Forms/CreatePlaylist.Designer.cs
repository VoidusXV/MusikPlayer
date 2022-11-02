namespace MusikPlayer.Methods.Forms
{
    partial class CreatePlaylist
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
            this.OpeningTransitionTimer = new System.Windows.Forms.Timer(this.components);
            this.iTextBox1 = new MusikPlayer.Design.iTextBox();
            this.iButton2 = new MusikPlayer.Design.iButton();
            this.iButton1 = new MusikPlayer.Design.iButton();
            this.iPanel21 = new MusikPlayer.Design.iPanel2();
            this.hLine1 = new MusikPlayer.Design.HLine();
            this.iLabel1 = new MusikPlayer.Design.iLabel();
            this.iPanel2 = new MusikPlayer.Design.iPanel2();
            this.iPanel21.SuspendLayout();
            this.iPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // OpeningTransitionTimer
            // 
            this.OpeningTransitionTimer.Interval = 1;
            this.OpeningTransitionTimer.Tick += new System.EventHandler(this.OpeningTransitionTimer_Tick);
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
            this.iTextBox1.Location = new System.Drawing.Point(21, 62);
            this.iTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.iTextBox1.Multiline = false;
            this.iTextBox1.Name = "iTextBox1";
            this.iTextBox1.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.iTextBox1.PasswordChar = false;
            this.iTextBox1.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.iTextBox1.PlaceholderText = "Playlist Name";
            this.iTextBox1.Size = new System.Drawing.Size(418, 31);
            this.iTextBox1.TabIndex = 11;
            this.iTextBox1.Texts = "";
            this.iTextBox1.UnderlinedStyle = true;
            this.iTextBox1._TextChanged += new System.EventHandler(this.iTextBox1__TextChanged);
            // 
            // iButton2
            // 
            this.iButton2.BackColor = System.Drawing.Color.SeaGreen;
            this.iButton2.BackgroundColor = System.Drawing.Color.SeaGreen;
            this.iButton2.BorderColor = System.Drawing.Color.White;
            this.iButton2.BorderRadius = 2;
            this.iButton2.BorderSize = 0;
            this.iButton2.CustomIcon = MusikPlayer.Design.iButton.CollapseCustomIcon.None;
            this.iButton2.FlatAppearance.BorderSize = 0;
            this.iButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iButton2.ForeColor = System.Drawing.Color.White;
            this.iButton2.ImageSize = new System.Drawing.Size(0, 0);
            this.iButton2.Location = new System.Drawing.Point(239, 116);
            this.iButton2.Name = "iButton2";
            this.iButton2.Size = new System.Drawing.Size(200, 40);
            this.iButton2.TabIndex = 1;
            this.iButton2.Text = "Playlist erstellen";
            this.iButton2.TextColor = System.Drawing.Color.White;
            this.iButton2.UseVisualStyleBackColor = false;
            this.iButton2.Click += new System.EventHandler(this.iButton2_Click);
            // 
            // iButton1
            // 
            this.iButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.iButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.iButton1.BorderColor = System.Drawing.Color.White;
            this.iButton1.BorderRadius = 2;
            this.iButton1.BorderSize = 0;
            this.iButton1.CustomIcon = MusikPlayer.Design.iButton.CollapseCustomIcon.None;
            this.iButton1.FlatAppearance.BorderSize = 0;
            this.iButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iButton1.ForeColor = System.Drawing.Color.White;
            this.iButton1.ImageSize = new System.Drawing.Size(0, 0);
            this.iButton1.Location = new System.Drawing.Point(21, 116);
            this.iButton1.Name = "iButton1";
            this.iButton1.Size = new System.Drawing.Size(200, 40);
            this.iButton1.TabIndex = 0;
            this.iButton1.Text = "Abbrechen";
            this.iButton1.TextColor = System.Drawing.Color.White;
            this.iButton1.UseVisualStyleBackColor = false;
            this.iButton1.Click += new System.EventHandler(this.iButton1_Click);
            // 
            // iPanel21
            // 
            this.iPanel21.AlphaColor = 100;
            this.iPanel21.Border = true;
            this.iPanel21.BorderColor = System.Drawing.Color.White;
            this.iPanel21.Controls.Add(this.iPanel2);
            this.iPanel21.Controls.Add(this.iTextBox1);
            this.iPanel21.Controls.Add(this.iButton2);
            this.iPanel21.Controls.Add(this.iButton1);
            this.iPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iPanel21.Location = new System.Drawing.Point(0, 0);
            this.iPanel21.Name = "iPanel21";
            this.iPanel21.Size = new System.Drawing.Size(460, 178);
            this.iPanel21.TabIndex = 13;
            // 
            // hLine1
            // 
            this.hLine1.AlphaColor = 255;
            this.hLine1.BackColor = System.Drawing.Color.Transparent;
            this.hLine1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hLine1.LineColor = System.Drawing.Color.DodgerBlue;
            this.hLine1.Location = new System.Drawing.Point(0, 37);
            this.hLine1.Name = "hLine1";
            this.hLine1.Size = new System.Drawing.Size(460, 1);
            this.hLine1.TabIndex = 12;
            this.hLine1.Text = "hLine1";
            // 
            // iLabel1
            // 
            this.iLabel1.AlphaColor = 150F;
            this.iLabel1.AutoSize = true;
            this.iLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iLabel1.ForeColor = System.Drawing.Color.White;
            this.iLabel1.HoverAnimation = false;
            this.iLabel1.Location = new System.Drawing.Point(146, 8);
            this.iLabel1.Name = "iLabel1";
            this.iLabel1.Select = false;
            this.iLabel1.Size = new System.Drawing.Size(160, 24);
            this.iLabel1.TabIndex = 3;
            this.iLabel1.Text = "Playlist erstellen";
            this.iLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.iLabel1.UseMnemonic = false;
            // 
            // iPanel2
            // 
            this.iPanel2.AlphaColor = 100;
            this.iPanel2.Border = true;
            this.iPanel2.BorderColor = System.Drawing.Color.White;
            this.iPanel2.Controls.Add(this.iLabel1);
            this.iPanel2.Controls.Add(this.hLine1);
            this.iPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.iPanel2.Location = new System.Drawing.Point(0, 0);
            this.iPanel2.Name = "iPanel2";
            this.iPanel2.Size = new System.Drawing.Size(460, 38);
            this.iPanel2.TabIndex = 13;
            this.iPanel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.iPanel2_MouseDown);
            this.iPanel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.iPanel2_MouseMove);
            this.iPanel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.iPanel2_MouseUp);
            // 
            // CreatePlaylist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(460, 178);
            this.Controls.Add(this.iPanel21);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CreatePlaylist";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CreatePlaylist";
            this.Load += new System.EventHandler(this.CreatePlaylist_Load);
            this.iPanel21.ResumeLayout(false);
            this.iPanel2.ResumeLayout(false);
            this.iPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Design.iButton iButton1;
        private Design.iButton iButton2;
        private System.Windows.Forms.Timer OpeningTransitionTimer;
        private Design.iTextBox iTextBox1;
        private Design.iPanel2 iPanel21;
        private Design.iPanel2 iPanel2;
        private Design.iLabel iLabel1;
        private Design.HLine hLine1;
    }
}