using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    public class GradientBackground : Control
    {
        private Color _startColor = Color.FromArgb(255, 31, 28, 43);
        private Color _endColor = Color.FromArgb(255, 20, 20, 20);

        public GradientBackground()
        {
            this.BackColor = Color.White;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
        public Color StartColor
        {
            get => _startColor;
            set
            {
                _startColor = value;
                this.Invalidate();
            }
        }
        public Color EndColor
        {
            get => _endColor;
            set
            {
                _endColor = value;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.CompositingMode = CompositingMode.SourceCopy;

            float angle = 90;
            LinearGradientBrush linGrBrush = new LinearGradientBrush(this.ClientRectangle, StartColor, EndColor, angle, true);
            e.Graphics.FillRectangle(linGrBrush, this.ClientRectangle);

            //e.Graphics.FillRectangle(linGrBrush, 0, 0, this.Width, this.Height);

        }

    }
}
