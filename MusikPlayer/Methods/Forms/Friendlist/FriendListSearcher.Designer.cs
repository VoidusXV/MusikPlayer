namespace MusikPlayer.Methods.Forms.Friendlist
{
    partial class FriendListSearcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FriendListSearcher));
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.iTextBox1 = new MusikPlayer.Design.iTextBox();
            this.iButton24 = new MusikPlayer.Design.iButton2();
            this.ContextMenuStrip_FocusCheckTimer = new System.Windows.Forms.Timer(this.components);
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(14)))), ((int)(((byte)(15)))));
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(376, 534);
            this.panel2.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 41);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(376, 493);
            this.panel4.TabIndex = 4;
            this.panel4.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel4_MouseClick);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.iTextBox1);
            this.panel3.Controls.Add(this.iButton24);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(376, 41);
            this.panel3.TabIndex = 3;
            // 
            // iTextBox1
            // 
            this.iTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(14)))), ((int)(((byte)(15)))));
            this.iTextBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.iTextBox1.BorderFocusColor = System.Drawing.Color.DodgerBlue;
            this.iTextBox1.BorderRadius = 0;
            this.iTextBox1.BorderSize = 2;
            this.iTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iTextBox1.ForeColor = System.Drawing.Color.White;
            this.iTextBox1.Location = new System.Drawing.Point(9, 4);
            this.iTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.iTextBox1.Multiline = true;
            this.iTextBox1.Name = "iTextBox1";
            this.iTextBox1.Padding = new System.Windows.Forms.Padding(10, 7, 10, 7);
            this.iTextBox1.PasswordChar = false;
            this.iTextBox1.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.iTextBox1.PlaceholderText = "Freunde in der Liste suchen";
            this.iTextBox1.Size = new System.Drawing.Size(291, 30);
            this.iTextBox1.TabIndex = 0;
            this.iTextBox1.Texts = "";
            this.iTextBox1.UnderlinedStyle = true;
            // 
            // iButton24
            // 
            this.iButton24.AlphaColor = 255F;
            this.iButton24.BackColor = System.Drawing.Color.CornflowerBlue;
            this.iButton24.BackgroundColor = System.Drawing.Color.CornflowerBlue;
            this.iButton24.BorderColor = System.Drawing.Color.White;
            this.iButton24.BorderRadius = 0;
            this.iButton24.BorderSize = 0;
            this.iButton24.ForeColor = System.Drawing.Color.White;
            this.iButton24.HoverBrightnessVal = 0.4F;
            this.iButton24.HoverColor = System.Drawing.Color.CornflowerBlue;
            this.iButton24.Image = ((System.Drawing.Image)(resources.GetObject("iButton24.Image")));
            this.iButton24.ImageSize = new System.Drawing.Size(25, 25);
            this.iButton24.Location = new System.Drawing.Point(307, 4);
            this.iButton24.Name = "iButton24";
            this.iButton24.PressAnimimation = true;
            this.iButton24.Size = new System.Drawing.Size(66, 30);
            this.iButton24.TabIndex = 2;
            this.iButton24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.iButton24.TextColor = System.Drawing.Color.White;
            this.iButton24.Click += new System.EventHandler(this.iButton24_Click);
            // 
            // ContextMenuStrip_FocusCheckTimer
            // 
            this.ContextMenuStrip_FocusCheckTimer.Tick += new System.EventHandler(this.ContextMenuStrip_FocusCheckTimer_Tick);
            // 
            // FriendListSearcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(14)))), ((int)(((byte)(15)))));
            this.ClientSize = new System.Drawing.Size(376, 534);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FriendListSearcher";
            this.Text = "FriendListSearcher";
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private Design.iTextBox iTextBox1;
        private Design.iButton2 iButton24;
        private System.Windows.Forms.Timer ContextMenuStrip_FocusCheckTimer;
    }
}