namespace MusikPlayer.Methods.Forms
{
    partial class MainPlayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPlayer));
            this.panel1 = new System.Windows.Forms.Panel();
            this.gradientPanel1 = new MusikPlayer.Design.GradientPanel();
            this.iLabel3 = new MusikPlayer.Design.iLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.iLabel2 = new MusikPlayer.Design.iLabel();
            this.iCircleButton2 = new MusikPlayer.Design.iCircleButton();
            this.iButton3 = new MusikPlayer.Design.iButton();
            this.AddedDate_Label = new MusikPlayer.Design.iLabel();
            this.Title_Label = new MusikPlayer.Design.iLabel();
            this.Album_Label = new MusikPlayer.Design.iLabel();
            this.Band_Label = new MusikPlayer.Design.iLabel();
            this.iTextBox1 = new MusikPlayer.Design.iTextBox();
            this.hLine1 = new MusikPlayer.Design.HLine();
            this.SongDuration_Label = new MusikPlayer.Design.iPictureBox();
            this.iLabel1 = new MusikPlayer.Design.iLabel();
            this.iButton1 = new MusikPlayer.Design.iButton();
            this.iButton2 = new MusikPlayer.Design.iButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.ContextMenuStrip_FocusCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.iToolTip1 = new MusikPlayer.Design.iToolTip();
            this.panel1.SuspendLayout();
            this.gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SongDuration_Label)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.gradientPanel1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1145, 630);
            this.panel1.TabIndex = 2;
            this.panel1.SizeChanged += new System.EventHandler(this.panel1_SizeChanged);
            // 
            // gradientPanel1
            // 
            this.gradientPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gradientPanel1.BackColor = System.Drawing.Color.Transparent;
            this.gradientPanel1.Border = false;
            this.gradientPanel1.BorderAlphaColor = 255;
            this.gradientPanel1.BorderColor = System.Drawing.Color.White;
            this.gradientPanel1.ColorAngle = 90;
            this.gradientPanel1.Controls.Add(this.iLabel3);
            this.gradientPanel1.Controls.Add(this.pictureBox1);
            this.gradientPanel1.Controls.Add(this.iLabel2);
            this.gradientPanel1.Controls.Add(this.iCircleButton2);
            this.gradientPanel1.Controls.Add(this.iButton3);
            this.gradientPanel1.Controls.Add(this.AddedDate_Label);
            this.gradientPanel1.Controls.Add(this.Title_Label);
            this.gradientPanel1.Controls.Add(this.Album_Label);
            this.gradientPanel1.Controls.Add(this.Band_Label);
            this.gradientPanel1.Controls.Add(this.iTextBox1);
            this.gradientPanel1.Controls.Add(this.hLine1);
            this.gradientPanel1.Controls.Add(this.SongDuration_Label);
            this.gradientPanel1.Controls.Add(this.iLabel1);
            this.gradientPanel1.Controls.Add(this.iButton1);
            this.gradientPanel1.Controls.Add(this.iButton2);
            this.gradientPanel1.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.gradientPanel1.GradientAlpha = 255;
            this.gradientPanel1.GradientBool = true;
            this.gradientPanel1.Location = new System.Drawing.Point(0, 0);
            this.gradientPanel1.Name = "gradientPanel1";
            this.gradientPanel1.Size = new System.Drawing.Size(1128, 263);
            this.gradientPanel1.StartColor = System.Drawing.Color.DarkSlateBlue;
            this.gradientPanel1.TabIndex = 27;
            // 
            // iLabel3
            // 
            this.iLabel3.AlphaColor = 255F;
            this.iLabel3.AutoSize = true;
            this.iLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.iLabel3.HoverAnimation = false;
            this.iLabel3.Location = new System.Drawing.Point(78, 178);
            this.iLabel3.Name = "iLabel3";
            this.iLabel3.Select = false;
            this.iLabel3.Size = new System.Drawing.Size(56, 16);
            this.iLabel3.TabIndex = 31;
            this.iLabel3.Text = "1 von 10";
            this.iLabel3.UseMnemonic = false;
            this.iLabel3.Visible = false;
            this.iLabel3.WrapText = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(21, 159);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(51, 51);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 30;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // iLabel2
            // 
            this.iLabel2.AlphaColor = 200F;
            this.iLabel2.AutoSize = true;
            this.iLabel2.BackColor = System.Drawing.Color.Transparent;
            this.iLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iLabel2.ForeColor = System.Drawing.Color.White;
            this.iLabel2.HoverAnimation = false;
            this.iLabel2.Location = new System.Drawing.Point(18, 128);
            this.iLabel2.Name = "iLabel2";
            this.iLabel2.Select = false;
            this.iLabel2.Size = new System.Drawing.Size(72, 20);
            this.iLabel2.TabIndex = 26;
            this.iLabel2.Text = "Songs: 0";
            this.iLabel2.UseMnemonic = false;
            this.iLabel2.WrapText = false;
            // 
            // iCircleButton2
            // 
            this.iCircleButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iCircleButton2.BackColor = System.Drawing.Color.White;
            this.iCircleButton2.BorderColor = System.Drawing.Color.LightCoral;
            this.iCircleButton2.FillColor = System.Drawing.Color.White;
            this.iCircleButton2.FlatAppearance.BorderSize = 0;
            this.iCircleButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iCircleButton2.ForeColor = System.Drawing.Color.White;
            this.iCircleButton2.Image = ((System.Drawing.Image)(resources.GetObject("iCircleButton2.Image")));
            this.iCircleButton2.ImageSize = new System.Drawing.Size(45, 45);
            this.iCircleButton2.Location = new System.Drawing.Point(22, 9);
            this.iCircleButton2.Name = "iCircleButton2";
            this.iCircleButton2.Size = new System.Drawing.Size(50, 50);
            this.iCircleButton2.SuperTip = "";
            this.iCircleButton2.TabIndex = 9;
            this.iCircleButton2.TextColor = System.Drawing.Color.White;
            this.iCircleButton2.UseVisualStyleBackColor = false;
            this.iCircleButton2.Visible = false;
            // 
            // iButton3
            // 
            this.iButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.iButton3.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.iButton3.BorderColor = System.Drawing.Color.Gray;
            this.iButton3.BorderRadius = 0;
            this.iButton3.BorderSize = 1;
            this.iButton3.CustomIcon = MusikPlayer.Design.iButton.CollapseCustomIcon.None;
            this.iButton3.FlatAppearance.BorderSize = 0;
            this.iButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iButton3.ForeColor = System.Drawing.Color.White;
            this.iButton3.ImageSize = new System.Drawing.Size(0, 0);
            this.iButton3.Location = new System.Drawing.Point(841, 142);
            this.iButton3.Name = "iButton3";
            this.iButton3.Size = new System.Drawing.Size(223, 31);
            this.iButton3.TabIndex = 25;
            this.iButton3.Text = "Refresh";
            this.iButton3.TextColor = System.Drawing.Color.White;
            this.iButton3.UseVisualStyleBackColor = false;
            this.iButton3.Visible = false;
            this.iButton3.Click += new System.EventHandler(this.iButton3_Click);
            // 
            // AddedDate_Label
            // 
            this.AddedDate_Label.AlphaColor = 100F;
            this.AddedDate_Label.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.AddedDate_Label.AutoSize = true;
            this.AddedDate_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddedDate_Label.ForeColor = System.Drawing.Color.White;
            this.AddedDate_Label.HoverAnimation = false;
            this.AddedDate_Label.Location = new System.Drawing.Point(838, 224);
            this.AddedDate_Label.Name = "AddedDate_Label";
            this.AddedDate_Label.Select = false;
            this.AddedDate_Label.Size = new System.Drawing.Size(127, 16);
            this.AddedDate_Label.TabIndex = 23;
            this.AddedDate_Label.Text = "HINZUGEFÜGT AM";
            this.AddedDate_Label.UseMnemonic = false;
            this.AddedDate_Label.WrapText = false;
            // 
            // Title_Label
            // 
            this.Title_Label.AlphaColor = 100F;
            this.Title_Label.AutoSize = true;
            this.Title_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title_Label.ForeColor = System.Drawing.Color.White;
            this.Title_Label.HoverAnimation = false;
            this.Title_Label.Location = new System.Drawing.Point(30, 222);
            this.Title_Label.Name = "Title_Label";
            this.Title_Label.Select = false;
            this.Title_Label.Size = new System.Drawing.Size(44, 16);
            this.Title_Label.TabIndex = 10;
            this.Title_Label.Text = "TITEL";
            this.Title_Label.UseMnemonic = false;
            this.Title_Label.WrapText = false;
            // 
            // Album_Label
            // 
            this.Album_Label.AlphaColor = 100F;
            this.Album_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Album_Label.AutoSize = true;
            this.Album_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Album_Label.ForeColor = System.Drawing.Color.White;
            this.Album_Label.HoverAnimation = false;
            this.Album_Label.Location = new System.Drawing.Point(706, 224);
            this.Album_Label.Name = "Album_Label";
            this.Album_Label.Select = false;
            this.Album_Label.Size = new System.Drawing.Size(53, 16);
            this.Album_Label.TabIndex = 12;
            this.Album_Label.Text = "ALBUM";
            this.Album_Label.UseMnemonic = false;
            this.Album_Label.WrapText = false;
            // 
            // Band_Label
            // 
            this.Band_Label.AlphaColor = 100F;
            this.Band_Label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Band_Label.AutoSize = true;
            this.Band_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Band_Label.ForeColor = System.Drawing.Color.White;
            this.Band_Label.HoverAnimation = false;
            this.Band_Label.Location = new System.Drawing.Point(372, 224);
            this.Band_Label.Name = "Band_Label";
            this.Band_Label.Select = false;
            this.Band_Label.Size = new System.Drawing.Size(56, 16);
            this.Band_Label.TabIndex = 11;
            this.Band_Label.Text = "ARTIST";
            this.Band_Label.UseMnemonic = false;
            this.Band_Label.WrapText = false;
            // 
            // iTextBox1
            // 
            this.iTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.iTextBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.iTextBox1.BorderFocusColor = System.Drawing.Color.DodgerBlue;
            this.iTextBox1.BorderRadius = 0;
            this.iTextBox1.BorderSize = 2;
            this.iTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iTextBox1.ForeColor = System.Drawing.Color.White;
            this.iTextBox1.Location = new System.Drawing.Point(834, 105);
            this.iTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.iTextBox1.Multiline = true;
            this.iTextBox1.Name = "iTextBox1";
            this.iTextBox1.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.iTextBox1.PasswordChar = false;
            this.iTextBox1.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.iTextBox1.PlaceholderText = "Nach Songs suchen";
            this.iTextBox1.Size = new System.Drawing.Size(281, 31);
            this.iTextBox1.TabIndex = 6;
            this.iTextBox1.Texts = "";
            this.iTextBox1.UnderlinedStyle = true;
            // 
            // hLine1
            // 
            this.hLine1.AlphaColor = 50;
            this.hLine1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hLine1.BackColor = System.Drawing.Color.Transparent;
            this.hLine1.LineColor = System.Drawing.Color.White;
            this.hLine1.Location = new System.Drawing.Point(12, 241);
            this.hLine1.Name = "hLine1";
            this.hLine1.Size = new System.Drawing.Size(1089, 10);
            this.hLine1.TabIndex = 1;
            this.hLine1.Text = "hLine1";
            // 
            // SongDuration_Label
            // 
            this.SongDuration_Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SongDuration_Label.Image = ((System.Drawing.Image)(resources.GetObject("SongDuration_Label.Image")));
            this.SongDuration_Label.ImageAngle = 0F;
            this.SongDuration_Label.ImageSize = new System.Drawing.Size(48, 48);
            this.SongDuration_Label.Location = new System.Drawing.Point(1030, 218);
            this.SongDuration_Label.Name = "SongDuration_Label";
            this.SongDuration_Label.Size = new System.Drawing.Size(25, 22);
            this.SongDuration_Label.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SongDuration_Label.TabIndex = 15;
            this.SongDuration_Label.TabStop = false;
            // 
            // iLabel1
            // 
            this.iLabel1.AlphaColor = 255F;
            this.iLabel1.AutoSize = true;
            this.iLabel1.BackColor = System.Drawing.Color.Transparent;
            this.iLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iLabel1.ForeColor = System.Drawing.Color.White;
            this.iLabel1.HoverAnimation = false;
            this.iLabel1.Location = new System.Drawing.Point(12, 62);
            this.iLabel1.Name = "iLabel1";
            this.iLabel1.Select = false;
            this.iLabel1.Size = new System.Drawing.Size(342, 55);
            this.iLabel1.TabIndex = 4;
            this.iLabel1.Text = "Lieblingssongs";
            this.iLabel1.UseMnemonic = false;
            this.iLabel1.WrapText = false;
            // 
            // iButton1
            // 
            this.iButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.iButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.iButton1.BorderColor = System.Drawing.Color.Gray;
            this.iButton1.BorderRadius = 0;
            this.iButton1.BorderSize = 1;
            this.iButton1.CustomIcon = MusikPlayer.Design.iButton.CollapseCustomIcon.None;
            this.iButton1.FlatAppearance.BorderSize = 0;
            this.iButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iButton1.ForeColor = System.Drawing.Color.White;
            this.iButton1.ImageSize = new System.Drawing.Size(0, 0);
            this.iButton1.Location = new System.Drawing.Point(604, 105);
            this.iButton1.Name = "iButton1";
            this.iButton1.Size = new System.Drawing.Size(223, 31);
            this.iButton1.TabIndex = 17;
            this.iButton1.Text = "Songs hinzufügen";
            this.iButton1.TextColor = System.Drawing.Color.White;
            this.iButton1.UseVisualStyleBackColor = false;
            this.iButton1.Visible = false;
            this.iButton1.Click += new System.EventHandler(this.iButton1_Click);
            // 
            // iButton2
            // 
            this.iButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.iButton2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.iButton2.BorderColor = System.Drawing.Color.Gray;
            this.iButton2.BorderRadius = 0;
            this.iButton2.BorderSize = 1;
            this.iButton2.CustomIcon = MusikPlayer.Design.iButton.CollapseCustomIcon.None;
            this.iButton2.FlatAppearance.BorderSize = 0;
            this.iButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iButton2.ForeColor = System.Drawing.Color.White;
            this.iButton2.ImageSize = new System.Drawing.Size(0, 0);
            this.iButton2.Location = new System.Drawing.Point(604, 142);
            this.iButton2.Name = "iButton2";
            this.iButton2.Size = new System.Drawing.Size(223, 31);
            this.iButton2.TabIndex = 18;
            this.iButton2.Text = "TrackBar Sync Test";
            this.iButton2.TextColor = System.Drawing.Color.White;
            this.iButton2.UseVisualStyleBackColor = false;
            this.iButton2.Visible = false;
            this.iButton2.Click += new System.EventHandler(this.iButton2_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.Location = new System.Drawing.Point(0, 260);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1122, 370);
            this.panel2.TabIndex = 16;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.Transparent;
            this.panel4.Controls.Add(this.vScrollBar1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1128, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(17, 630);
            this.panel4.TabIndex = 24;
            this.panel4.Visible = false;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vScrollBar1.Location = new System.Drawing.Point(0, 0);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 630);
            this.vScrollBar1.TabIndex = 0;
            // 
            // ContextMenuStrip_FocusCheckTimer
            // 
            this.ContextMenuStrip_FocusCheckTimer.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // iToolTip1
            // 
            this.iToolTip1.AutomaticDelay = 300;
            this.iToolTip1.OwnerDraw = true;
            // 
            // MainPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(1145, 630);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MainPlayer";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MainPlayer";
            this.panel1.ResumeLayout(false);
            this.gradientPanel1.ResumeLayout(false);
            this.gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SongDuration_Label)).EndInit();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Timer ContextMenuStrip_FocusCheckTimer;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private Design.GradientPanel gradientPanel1;
        private Design.iLabel iLabel2;
        private Design.iCircleButton iCircleButton2;
        private Design.iButton iButton3;
        private Design.iLabel AddedDate_Label;
        private Design.iLabel Title_Label;
        private Design.iLabel Album_Label;
        private Design.iLabel Band_Label;
        private Design.iTextBox iTextBox1;
        private Design.HLine hLine1;
        private Design.iPictureBox SongDuration_Label;
        public Design.iLabel iLabel1;
        private Design.iButton iButton1;
        private Design.iButton iButton2;
        private Design.iToolTip iToolTip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Design.iLabel iLabel3;
    }
}