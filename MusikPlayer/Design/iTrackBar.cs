using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace MusikPlayer.Design
{
    public class iTrackBar : Control
    {
        private Color _BarColor = Color.CornflowerBlue;
        private int _Tick = 0;
        private int _TimeEnd = 180; // 3mins
        private RectangleF foreGround_rectangleF = new RectangleF();
        private RectangleF backGround_rectangleF = new RectangleF();

        public iTrackBar()
        {
            //this.SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }


        public Color BarColor
        {
            get => _BarColor;
            set
            {
                _BarColor = value;
                this.Invalidate();
            }
        }

        public int Value
        {
            get => _Tick;
            set
            {
                _Tick = value;
                if (_Tick > _TimeEnd)
                    _Tick = value - (value - _TimeEnd);
                else if (_Tick < 0)
                    _Tick = 0;
                this.Invalidate();
            }
        }

        public int MaxValue
        {
            get => _TimeEnd;
            set
            {
                _TimeEnd = value;
                this.Invalidate();
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //Background bar
            backGround_rectangleF.Width = this.Width - 10;
            backGround_rectangleF.Height = this.Height - 10;
            backGround_rectangleF.Y = 5;
            backGround_rectangleF.X = 5;
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, 69, 69, 69)), backGround_rectangleF);

            
            //Foreground bar
            foreGround_rectangleF.Width = ((backGround_rectangleF.Width - 10f) / _TimeEnd) * _Tick;
            foreGround_rectangleF.Height = backGround_rectangleF.Height;
            foreGround_rectangleF.Y = 5;
            foreGround_rectangleF.X = 5;
            graphics.FillRectangle(new SolidBrush(BarColor), foreGround_rectangleF);

            //Circle
            RectangleF Ellipse_rectangleF = new RectangleF();
            Ellipse_rectangleF.Width = foreGround_rectangleF.Height + 3;
            Ellipse_rectangleF.Height = foreGround_rectangleF.Height + 3;
            Ellipse_rectangleF.Y = foreGround_rectangleF.Y / 2 + 1;
            Ellipse_rectangleF.X = foreGround_rectangleF.Width; //215
            graphics.FillEllipse(new SolidBrush(Color.FromArgb(255, Color.White)), Ellipse_rectangleF);

        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }
        protected async override void OnMouseDown(MouseEventArgs e)
        {
            float calc = (PointToClient(Cursor.Position).X / backGround_rectangleF.Width);
            float bla = _TimeEnd * calc;
            if (bla <= _TimeEnd && bla >= 0)
                _Tick = (int)bla;

            this.Invalidate();
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }
    }
}
