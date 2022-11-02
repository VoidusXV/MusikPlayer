using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    public class iCheckBox : Control
    {
        bool _checked = false;
        Color _fillColor = Color.Gray;

        Color _pressedColor = Color.CornflowerBlue;
        private int _pressedColorAplha = 0;

        System.Timers.Timer fade_TimerON = new System.Timers.Timer();
        System.Timers.Timer fade_TimerOFF = new System.Timers.Timer();

        private int fill_speed = 15;
        public iCheckBox()
        {
            this.Size = new Size(30, 30);
            SetStyle(ControlStyles.SupportsTransparentBackColor, false);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.Cursor = Cursors.Hand;
            this.BackColor = Color.White;
            this.Font = new Font("Microsoft Sans Serif", 18);
            this.ForeColor = Color.White;

            fade_TimerON.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            fade_TimerON.Interval = 1;

            fade_TimerOFF.Elapsed += new ElapsedEventHandler(OnTimedEventOFF);
            fade_TimerOFF.Interval = 1;
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (_pressedColorAplha >= 255 || _pressedColorAplha + fill_speed >= 255)
            {
                _pressedColorAplha = 255;
                fade_TimerON.Stop();
            }
            else
            {
                _pressedColorAplha += fill_speed;
            }

            this.Invalidate();
        }

        private void OnTimedEventOFF(object source, ElapsedEventArgs e)
        {

            if (_pressedColorAplha <= 0 || _pressedColorAplha - fill_speed <= 0)
            {
                _pressedColorAplha = 0;
                fade_TimerOFF.Stop();
            }
            else
            {
                //Console.WriteLine(_pressedColorAplha);
                _pressedColorAplha -= fill_speed;
            }
            this.Invalidate();
        }

        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
            }
        }

        public Color FillColor
        {
            get { return _fillColor; }
            set { _fillColor = value; }
        }

        public Color PressedColor
        {
            get { return _pressedColor; }
            set { _pressedColor = value; }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (_checked == true)
                _pressedColorAplha = 255;
            else
                _pressedColorAplha = 0;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            RectangleF rectangleBorder = new RectangleF();
            rectangleBorder.Width = this.Size.Width - 4;
            rectangleBorder.Height = this.Size.Height - 4;
            rectangleBorder.Location = new PointF((this.Width - rectangleBorder.Width) / 2, (this.Height - rectangleBorder.Height) / 2);
            e.Graphics.FillRectangle(new SolidBrush(_fillColor), rectangleBorder);

            RectangleF rectangle_Fill = new RectangleF();
            rectangle_Fill.Width = this.Size.Width;
            rectangle_Fill.Height = this.Size.Height;
            Color color = Color.FromArgb(_pressedColorAplha, _pressedColor);
            if (_checked == true && fade_TimerOFF.Enabled == false)
                e.Graphics.FillRectangle(new SolidBrush(color), rectangle_Fill);
            e.Graphics.FillRectangle(new SolidBrush(color), rectangle_Fill);


            //DrawText(e, "✓");
            //DrawTick(e);

            /*Rectangle rectangle = new Rectangle();
            rectangle.Width = 20;
            rectangle.Height = 20;
            rectangle.Location = new Point((this.Width - rectangle.Width) / 2, (this.Height - rectangle.Height) / 2);
            ControlPaint.DrawCheckBox(e.Graphics, rectangle, ButtonState.Checked);*/
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (_pressedColorAplha == 255)
            {
                fade_TimerOFF.Start();
                _checked = false;
            }
            if (_pressedColorAplha == 0)
            {
                _checked = true;
                fade_TimerON.Start();
            }
        }


        void DrawText(PaintEventArgs pevent, string text)
        {

            SizeF stringSize = new SizeF();
            stringSize = pevent.Graphics.MeasureString(text, this.Font, this.Width);

            int x = (this.Width - (int)stringSize.Width) / 2;
            int y = (this.Height - (int)stringSize.Height) / 2;
            Rectangle rc1 = new Rectangle(x, y + 2, Width, Height);
            pevent.Graphics.DrawString(text, this.Font, new SolidBrush(Color.FromArgb(_pressedColorAplha, ForeColor)), rc1);

        }

        void DrawTick(PaintEventArgs paintEventArgs)
        {

            paintEventArgs.Graphics.DrawLine(new Pen(Color.FromArgb(13, 13, 13), 1), 7, 15, 11, 18);

            paintEventArgs.Graphics.DrawLine(new Pen(Color.FromArgb(13, 13, 13), 1), 11, 18, 18, 10);
        }
    }
}
