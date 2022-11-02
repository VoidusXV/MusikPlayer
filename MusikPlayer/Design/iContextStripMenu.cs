using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    public partial class iContextStripMenu : Form
    {

        public List<iButton2> iButtons = new List<iButton2>();
        public List<string> iButtons_text = new List<string>();

        public List<HLine> iHLine = new List<HLine>();
        public List<int> H_Line_Locations = new List<int>();

        int ButtonCount = 0;
        public iContextStripMenu()
        {
            InitializeComponent();
        }

        private void iContextStripMenu_Load(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < iHLine.Count; i++)
                {
                    Create_iLine(i);
                    this.Controls.Add(iHLine[i]);
                }
                for (int i = 0; i < iButtons_text.Count; i++)
                    this.Controls.Add(iButtons[i]);


                this.Size = new Size(this.Width, ButtonCount * 40 + 15);
                Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 1, 1));
            }
            catch { }
        }

        private void IContextStripMenu_MouseLeave(object sender, EventArgs e)
        {

        }

        private void IContextStripMenu_MouseEnter(object sender, EventArgs e)
        {

        }

        public int GetLongestLabel()
        {
            int max = 0;
            for (int i = 0; i < iButtons_text.Count; i++)
            {
                if (max < iButtons[i].Size.Width)
                {
                    max = iButtons[i].Size.Width;
                }
            }
            return max;
        }
        public void Create_iButton(string text, int index, Image image = null, iButton2.ImageAlignEnum imageAlignEnum = iButton2.ImageAlignEnum.MiddleCenter, Size? imageSize = null)
        {

            iButtons.Add(new iButton2());
            iButtons[index].Location = new Point(6, 10 + (40 * index));
            iButtons[index].Text = text;
            iButtons[index].TextAlign = ContentAlignment.MiddleLeft;
            iButtons[index].BorderRadius = 2;
            iButtons[index].Font = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            iButtons[index].ForeColor = Color.White;
            iButtons[index].BackColor = Color.FromArgb(255, 41, 42, 45);
            iButtons[index].HoverColor = Color.FromArgb(255, 41, 42, 45);
            iButtons[index].HoverBrightnessVal = 0.2f;
            iButtons[index].PressAnimimation = false;
            iButtons[index].Size = new Size(226, 39);
            if (image != null)
            {
                iButtons[index].Image = image;
                iButtons[index].ImageAlign = imageAlignEnum;
                iButtons[index].ImageSize = imageSize.Value;
            }
            ButtonCount++;
        }

        public void Clear()
        {
            iButtons_text.Clear();
            iHLine.Clear();
        }

        public void AddOption(string Option, EventHandler eventHandler = null)
        {
            iButtons_text.Add(Option);
            Create_iButton(iButtons_text[ButtonCount], ButtonCount);
            if (eventHandler != null)
            {
                iButtons[ButtonCount - 1].Click += eventHandler;
                iButtons[ButtonCount - 1].MouseEnter += IContextStripMenu_MouseEnter;
                iButtons[ButtonCount - 1].MouseLeave += IContextStripMenu_MouseLeave;
            }
        }

        public void AddOption(string Option, EventHandler eventHandler = null, Image image = null, iButton2.ImageAlignEnum imageAlignEnum = iButton2.ImageAlignEnum.MiddleCenter, Size? imageSize = null)
        {
            iButtons_text.Add(Option);
            Create_iButton(iButtons_text[ButtonCount], ButtonCount, image, imageAlignEnum, imageSize);
            if (eventHandler != null)
            {
                iButtons[ButtonCount - 1].Click += eventHandler;
                iButtons[ButtonCount - 1].MouseEnter += IContextStripMenu_MouseEnter;
                iButtons[ButtonCount - 1].MouseLeave += IContextStripMenu_MouseLeave;
            }
        }

        public void AddLine()
        {
            iHLine.Add(new HLine());
            H_Line_Locations.Add(iButtons_text.Count);
        }
        public void Create_iLine(int index)
        {
            iHLine[index].AlphaColor = 50;
            iHLine[index].Size = new Size(226, 39);
            iHLine[index].Width = 226;
            iHLine[index].Height = 1;
            iHLine[index].Location = new Point(6, 8 + (40 * H_Line_Locations[index]));//44 * H_Line_Locations[index]
        }

        #region Round
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
    (
        int nLeftRect,     // x-coordinate of upper-left corner
        int nTopRect,      // y-coordinate of upper-left corner
        int nRightRect,    // x-coordinate of lower-right corner
        int nBottomRect,   // y-coordinate of lower-right corner
        int nWidthEllipse, // width of ellipse
        int nHeightEllipse // height of ellipse
    );

        private void iContextStripMenu_VisibleChanged(object sender, EventArgs e)
        {
            if (iButtons.Count == 0)
                return;

            if(Visible)
            {
                for (int i = 0; i < iButtons.Count; i++)
                {
                    iButtons[i].ResetButtonColor();
                }
            }
        }
    }
    #endregion
}
