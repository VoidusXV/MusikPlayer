using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    /*class HLine : Label
    {
        private int ColorA = 255;

        public HLine()
        {

        }

        protected override void OnHandleCreated(EventArgs e)
        {
            //this.BackColor = Color.Red;
            base.OnHandleCreated(e);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            this.AutoSize = false;
            //this.Height = 100;

            e.Graphics.DrawLine(new Pen(Color.FromArgb(ColorA, Color.White)), -100, 50, 100, 50);
        }
    }*/

    public class HLine : Control
    {
        private int ColorA = 255;
        private Color _LineColor = Color.White;

        public HLine()
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
            e.Graphics.DrawLine(new Pen(Color.FromArgb(ColorA, _LineColor)), -100, this.Height / 2, this.Width, this.Height / 2);
        }

    }
}
