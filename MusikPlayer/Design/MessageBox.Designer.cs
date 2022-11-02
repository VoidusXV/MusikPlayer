namespace MusikPlayer.Design
{
    partial class MessageBox
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
            this.panel0 = new MusikPlayer.Design.iPanel2();
            this.panel3 = new MusikPlayer.Design.iPanel2();
            this.iButton2 = new MusikPlayer.Design.iButton();
            this.iLabel2 = new System.Windows.Forms.Label();
            this.panel1 = new MusikPlayer.Design.iPanel2();
            this.iButton1 = new MusikPlayer.Design.iButton();
            this.iLabel1 = new MusikPlayer.Design.iLabel();
            this.panel0.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel0
            // 
            this.panel0.AlphaColor = 255;
            this.panel0.Border = true;
            this.panel0.BorderColor = System.Drawing.Color.White;
            this.panel0.Controls.Add(this.panel3);
            this.panel0.Controls.Add(this.panel1);
            this.panel0.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel0.Location = new System.Drawing.Point(0, 0);
            this.panel0.Name = "panel0";
            this.panel0.Size = new System.Drawing.Size(393, 169);
            this.panel0.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.AlphaColor = 255;
            this.panel3.Border = true;
            this.panel3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(112)))));
            this.panel3.Controls.Add(this.iButton2);
            this.panel3.Controls.Add(this.iLabel2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 47);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(393, 122);
            this.panel3.TabIndex = 0;
            // 
            // iButton2
            // 
            this.iButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.iButton2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.iButton2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.iButton2.BorderRadius = 0;
            this.iButton2.BorderSize = 1;
            this.iButton2.CustomIcon = MusikPlayer.Design.iButton.CollapseCustomIcon.None;
            this.iButton2.FlatAppearance.BorderSize = 0;
            this.iButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iButton2.ForeColor = System.Drawing.Color.White;
            this.iButton2.ImageSize = new System.Drawing.Size(0, 0);
            this.iButton2.Location = new System.Drawing.Point(137, 85);
            this.iButton2.Name = "iButton2";
            this.iButton2.Size = new System.Drawing.Size(94, 27);
            this.iButton2.TabIndex = 2;
            this.iButton2.Text = "OK";
            this.iButton2.TextColor = System.Drawing.Color.White;
            this.iButton2.UseVisualStyleBackColor = false;
            this.iButton2.Click += new System.EventHandler(this.iButton2_Click);
            // 
            // iLabel2
            // 
            this.iLabel2.AutoSize = true;
            this.iLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.iLabel2.ForeColor = System.Drawing.Color.White;
            this.iLabel2.Location = new System.Drawing.Point(12, 34);
            this.iLabel2.Name = "iLabel2";
            this.iLabel2.Size = new System.Drawing.Size(51, 20);
            this.iLabel2.TabIndex = 4;
            this.iLabel2.Text = "label1";
            // 
            // panel1
            // 
            this.panel1.AlphaColor = 255;
            this.panel1.Border = true;
            this.panel1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(112)))));
            this.panel1.Controls.Add(this.iButton1);
            this.panel1.Controls.Add(this.iLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(393, 47);
            this.panel1.TabIndex = 5;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // iButton1
            // 
            this.iButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.iButton1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.iButton1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.iButton1.BorderRadius = 0;
            this.iButton1.BorderSize = 1;
            this.iButton1.CustomIcon = MusikPlayer.Design.iButton.CollapseCustomIcon.None;
            this.iButton1.FlatAppearance.BorderSize = 0;
            this.iButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iButton1.ForeColor = System.Drawing.Color.White;
            this.iButton1.ImageSize = new System.Drawing.Size(0, 0);
            this.iButton1.Location = new System.Drawing.Point(340, 5);
            this.iButton1.Name = "iButton1";
            this.iButton1.Size = new System.Drawing.Size(49, 38);
            this.iButton1.TabIndex = 4;
            this.iButton1.Text = "X";
            this.iButton1.TextColor = System.Drawing.Color.White;
            this.iButton1.UseVisualStyleBackColor = false;
            this.iButton1.Click += new System.EventHandler(this.iButton1_Click);
            // 
            // iLabel1
            // 
            this.iLabel1.AlphaColor = 255F;
            this.iLabel1.AutoSize = true;
            this.iLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iLabel1.ForeColor = System.Drawing.Color.White;
            this.iLabel1.HoverAnimation = false;
            this.iLabel1.Location = new System.Drawing.Point(12, 12);
            this.iLabel1.Name = "iLabel1";
            this.iLabel1.Select = false;
            this.iLabel1.Size = new System.Drawing.Size(60, 20);
            this.iLabel1.TabIndex = 2;
            this.iLabel1.Text = "iLabel1";
            this.iLabel1.UseMnemonic = false;
            // 
            // MessageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(393, 169);
            this.Controls.Add(this.panel0);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MessageBox";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MessageBox_KeyUp);
            this.panel0.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private iLabel iLabel1;
        private iButton iButton2;
        private iButton iButton1;
        private System.Windows.Forms.Label iLabel2;
        private iPanel2 panel0;
        private iPanel2 panel3;
        private iPanel2 panel1;
    }
}