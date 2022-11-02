namespace MusikPlayer.Methods.Updater
{
    partial class UpdateWarning
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
            this.iLabel1 = new MusikPlayer.Design.iLabel();
            this.gradientBackground1 = new MusikPlayer.Design.GradientBackground();
            this.SuspendLayout();
            // 
            // iLabel1
            // 
            this.iLabel1.AlphaColor = 255F;
            this.iLabel1.AutoSize = true;
            this.iLabel1.BackColor = System.Drawing.Color.Black;
            this.iLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iLabel1.ForeColor = System.Drawing.Color.White;
            this.iLabel1.HoverAnimation = false;
            this.iLabel1.Location = new System.Drawing.Point(111, 58);
            this.iLabel1.Name = "iLabel1";
            this.iLabel1.Select = false;
            this.iLabel1.Size = new System.Drawing.Size(140, 20);
            this.iLabel1.TabIndex = 1;
            this.iLabel1.Text = "Chill mal do HOND";
            this.iLabel1.UseMnemonic = false;
            // 
            // gradientBackground1
            // 
            this.gradientBackground1.BackColor = System.Drawing.Color.White;
            this.gradientBackground1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gradientBackground1.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(59)))), ((int)(((byte)(85)))));
            this.gradientBackground1.Location = new System.Drawing.Point(0, 0);
            this.gradientBackground1.Name = "gradientBackground1";
            this.gradientBackground1.Size = new System.Drawing.Size(403, 129);
            this.gradientBackground1.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(30)))), ((int)(((byte)(48)))));
            this.gradientBackground1.TabIndex = 0;
            this.gradientBackground1.Text = "gradientBackground1";
            // 
            // UpdateWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 129);
            this.Controls.Add(this.iLabel1);
            this.Controls.Add(this.gradientBackground1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "UpdateWarning";
            this.Text = "UpdateWarning";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Design.GradientBackground gradientBackground1;
        private Design.iLabel iLabel1;
    }
}