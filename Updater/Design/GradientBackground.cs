using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater.Design
{
    public class GradientBackground : Control
    {
        private Color _startColor = Color.FromArgb(255, 31, 28, 43);
        private Color _endColor = Color.FromArgb(255, 20, 20, 20);

        public GradientBackground()
        {
            this.BackColor = Color.White;
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
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
            //Rectangle rect, Color color1, Color color2, float angle
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);


            float angle = 90;
            LinearGradientBrush linGrBrush = new LinearGradientBrush(rect, StartColor, EndColor, angle);
            e.Graphics.FillRectangle(linGrBrush, 0, 0, this.Width, this.Height);

        }

    }
}
