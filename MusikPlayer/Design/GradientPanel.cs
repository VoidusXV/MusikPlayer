using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    public class GradientPanel : Panel
    {
        private Color _startColor = Color.FromArgb(255, 31, 28, 43);
        private Color _endColor = Color.FromArgb(255, 20, 20, 20);
        private int _colorAngle = 90;
        private int _GradientAlpha = 255;
        private bool _GradientBool = true;


        private Color _BorderColor = Color.White;
        private bool _Border = false;
        private int _BorderAlphaColor = 255;

        public GradientPanel()
        {
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
        public int ColorAngle
        {
            get => _colorAngle;
            set
            {
                _colorAngle = value;
                this.Invalidate();
            }
        }
        public int GradientAlpha
        {
            get => _GradientAlpha;
            set
            {
                _GradientAlpha = value;
                this.Invalidate();
            }
        }

        public bool GradientBool
        {
            get => _GradientBool;
            set
            {
                _GradientBool = value;
                this.Invalidate();
            }
        }


        public bool Border
        {
            get { return _Border; }
            set
            {
                _Border = value;
                this.Invalidate();
            }
        }
        public Color BorderColor
        {
            get => _BorderColor;
            set
            {
                _BorderColor = value;
                this.Invalidate();
            }
        }
        public int BorderAlphaColor
        {
            get { return _BorderAlphaColor; }
            set
            {
                _BorderAlphaColor = value;
                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (GradientBool)
            {
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

                /*e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                e.Graphics.CompositingMode = CompositingMode.SourceCopy;*/


                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
                float angle = ColorAngle;
                LinearGradientBrush linGrBrush = new LinearGradientBrush(rect, Color.FromArgb(GradientAlpha, StartColor), Color.FromArgb(GradientAlpha, EndColor), angle);

                e.Graphics.FillRectangle(linGrBrush, 0, 0, this.Width, this.Height);
            }
            if (Border == true)
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(_BorderAlphaColor, BorderColor)), 0, 0, Width - 1, Height - 1);
            }
        }
    }
}
