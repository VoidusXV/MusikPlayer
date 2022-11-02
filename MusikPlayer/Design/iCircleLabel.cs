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
    public class iCircleLabel : Control
    {
        private Color _fillColor = Color.Red;

        public iCircleLabel()
        {
            this.Size = new Size(21, 21);
            this.ForeColor = Color.White;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
        public Color FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            SizeF sizeF = e.Graphics.MeasureString(this.Text, this.Font);
            RectangleF rc1 = new RectangleF((this.Width - sizeF.Width) / 2, (this.Height - sizeF.Height) / 2, sizeF.Width, sizeF.Height);

            float x = (this.Width - this.Width - 1) / 2;
            float y = (this.Height - this.Height - 1) / 2;

            //Size Size = new Size((int)sizeF.Width, (int)sizeF.Height);

            //Console.WriteLine(sizeF.Width.ToString());
            e.Graphics.FillEllipse(new SolidBrush(FillColor), new RectangleF(x, 0 / 2, this.Width - 1, this.Width - 1));
            e.Graphics.DrawString(Text, Font, new SolidBrush(this.ForeColor), rc1);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            this.Invalidate();
        }
    }
}
