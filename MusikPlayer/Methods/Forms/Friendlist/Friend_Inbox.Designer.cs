namespace MusikPlayer.Methods.Forms.Friendlist
{
    partial class Friend_Inbox
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
            this.panel1 = new MusikPlayer.Design.iPanel();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AlphaColor = 255;
            this.panel1.Bool_CreateParams = false;
            this.panel1.Border = false;
            this.panel1.BorderColor = System.Drawing.Color.White;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(376, 534);
            this.panel1.TabIndex = 0;
            // 
            // Friend_Inbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(14)))), ((int)(((byte)(15)))));
            this.ClientSize = new System.Drawing.Size(376, 534);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Friend_Inbox";
            this.Text = "FriendList_Settings";
            this.Load += new System.EventHandler(this.Friend_Inbox_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Design.iPanel panel1;
    }
}