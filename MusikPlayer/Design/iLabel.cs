using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    public class iLabel : Label
    {
        private float ColorA = 255;
        private bool HoverAnim = false;
        private bool _select = false;
        private bool _wrapText = false;

        public iLabel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.UseMnemonic = false;
            this.MaximumSize = new Size(0, 0);

        }

        public float AlphaColor
        {
            get => ColorA;
            set
            {
                ColorA = Convert.ToInt32(value);

                if (ColorA >= 255)
                    ColorA = 255;
                else if (ColorA <= 0)
                    ColorA = 0;
                Invalidate();
            }
        }
        public bool HoverAnimation
        {
            get => HoverAnim;
            set
            {
                HoverAnim = value;
                Invalidate();
            }
        }

        public bool Select
        {
            get => _select;
            set
            {
                _select = value;
                if (_select == true)
                {
                    HoverAnimation = false;
                    ColorA = 255;
                }
                else if (_select == false && HoverAnimation == true)
                {
                    ColorA = 150;
                }
                this.Invalidate();
            }
        }

        public bool WrapText
        {
            get
            {
                return _wrapText;
            }
            set
            {
                _wrapText = value;
            }
        }
        private static StringFormat ContentAlignmentToStringAlignment(ContentAlignment ca)
        {
            var format = new StringFormat();
            var l2 = (int)Math.Log((double)ca, 2);
            format.LineAlignment = (StringAlignment)(l2 / 4);
            format.Alignment = (StringAlignment)(l2 % 4);
            return format;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            SizeF sizeF = e.Graphics.MeasureString(this.Text, this.Font);
            RectangleF rc1 = new RectangleF(0, 0, sizeF.Width, sizeF.Height);

            RectangleF rc2 = new RectangleF(0, 0, this.Width, this.Height);

            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.FitBlackBox;

            if (WrapText == false)
                e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb((int)AlphaColor, ForeColor)), rc1);
            else
                e.Graphics.DrawString(Text, Font, new SolidBrush(Color.FromArgb((int)AlphaColor, ForeColor)), rc2, drawFormat);
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            if (HoverAnimation == true)
            {
                for (float i = 150; i <= 255; i += 0.2f)
                {
                    this.AlphaColor = i;
                }
                this.AlphaColor = 255;
            }
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (HoverAnimation == true)
            {
                for (float i = 255; i >= 150; i -= 0.2f)
                {
                    this.AlphaColor = i;
                }
                this.AlphaColor = 150;
            }
            base.OnMouseLeave(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }
    }
}
