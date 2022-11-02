using System;
using System.Drawing;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    public class VLine : Control
    {
        private int ColorA = 255;
        private Color _LineColor = Color.White;

        public VLine()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        public int AlphaColor
        {
            get => ColorA;
            set
            {
                ColorA = value;

                if (ColorA >= 255)
                    ColorA = 255;
                else if (ColorA <= 0)
                    ColorA = 0;
                Invalidate();
            }
        }
        public Color LineColor
        {
            get => _LineColor;
            set
            {
                _LineColor = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.AutoSize = false;
            e.Graphics.DrawLine(new Pen(Color.FromArgb(ColorA, _LineColor)), this.Width / 2, this.Height, this.Width / 2, 0);

        }
    }
}
