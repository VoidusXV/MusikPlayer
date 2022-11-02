using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    public class iScrollBarV : Control
    {
        public delegate void customScroll();//object Sender, ScrollEventArgs e
        public event customScroll Scroll;

        System.Timers.Timer selection_AnimationON = new System.Timers.Timer();
        System.Timers.Timer selection_AnimationOFF = new System.Timers.Timer();

        private Color _scrollerBackColor = Color.CornflowerBlue;
        private int scrollerColorAlpha = 150;

        private int _BackColorAlpha = 255;

        private Rectangle scrollerRectangle = new Rectangle();
        private int _largeChange = 50;
        private bool _movingState = true;

        private int _scrollChange = 5;
        private int _value = 0;
        private int _maximum;

        bool isMouseDown = false;

        private static readonly object EVENT_SCROLL = new object();

        public iScrollBarV()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);

            this.Size = new Size(20, 350);
            this.BackColor = Color.FromArgb(BackColorAlpha, 37, 39, 42);

            scrollerRectangle.Width = this.Width;
            scrollerRectangle.Height = 50;
            scrollerRectangle.X = 0;
            scrollerRectangle.Y = 0;

            selection_AnimationON.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            selection_AnimationON.Interval = 1;

            selection_AnimationOFF.Elapsed += new ElapsedEventHandler(OnTimedEventOFF);
            selection_AnimationOFF.Interval = 1;
        }

        public int BackColorAlpha
        {
            get => _BackColorAlpha;
            set
            {
                _BackColorAlpha = value;
                this.Invalidate();
            }
        }
        public Color ScrollerBackColor
        {
            get => _scrollerBackColor;
            set
            {
                _scrollerBackColor = value;
                this.Invalidate();
            }
        }

        public int LargeChange
        {
            get => _largeChange;
            set
            {
                _largeChange = value;
                scrollerRectangle.Height = value;

                this.Invalidate();
            }
        }

        public int ScrollChange
        {
            get => _scrollChange;
            set
            {
                _scrollChange = value;
                this.Invalidate();
            }
        }
        public int Value
        {
            get => _value;
            set
            {
                _value = value;

                if (value > this.Height - LargeChange)
                    _value = this.Height - LargeChange;
                else if (value < 0)
                    _value = 0;

                scrollerRectangle.Y = _value;
                this.Invalidate();
            }
        }
        public int Maximum
        {
            get => _maximum;
            set
            {
                _maximum = value;
                this.Invalidate();
            }
        }

        public bool MovingState
        {
            get => _movingState;
            set
            {
                _movingState = value;
                if (_movingState == false)
                    this.LargeChange = Parent.Height;
                this.Invalidate();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            this.BackColor = Color.FromArgb(BackColorAlpha, 37, 39, 42);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(scrollerColorAlpha, ScrollerBackColor)), scrollerRectangle);
        }

        protected virtual void OnScroll(ScrollEventArgs se)
        {
            ((ScrollEventHandler)base.Events[EVENT_SCROLL])?.Invoke(this, se);
        }

        void MoveScroller(int Y)
        {
            //if (Value >= Maximum || Maximum == 0)
            //return;

            if (MovingState == false)
                return;

            Value = Y;
            if (Y >= scrollerRectangle.Height + scrollerRectangle.Location.Y)
            {
                Value = Y - scrollerRectangle.Height;
                //Console.WriteLine("kok");
            }

            if (Scroll != null)
                Scroll();
            this.Update();
            //this.Invalidate();
        }


        bool mouseIsOnScroller(MouseEventArgs e)
        {
            int scrollerBoxStart = scrollerRectangle.Y;
            int scrollerBoxEnd = scrollerRectangle.Y + scrollerRectangle.Height;

            if (e.Location.Y >= scrollerBoxStart && e.Location.Y <= scrollerBoxEnd)
            {
                return true;
            }
            return false;
        }

        int offset = 0;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (isMouseDown)
            {
                MoveScroller(e.Location.Y - offset);
            }
            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            isMouseDown = true;

            if (mouseIsOnScroller(e) == false)
            {
                MoveScroller(e.Location.Y);
                this.Invalidate();
            }
            else
            {
                offset = e.Location.Y - Value;
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            isMouseDown = false;
            this.Invalidate();
        }


        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            int MouseWheel_Y = Value;
            int mousedeltaval = e.Delta / 120;


            if (mousedeltaval == 1) //mousewheel up move
            {
                MouseWheel_Y -= ScrollChange;
                MoveScroller(MouseWheel_Y);
            }
            if (mousedeltaval == -1) //mousewheel down move
            {
                MouseWheel_Y += ScrollChange;
                MoveScroller(MouseWheel_Y);
            }

            //Console.WriteLine($"{this.Value}");
            this.Invalidate();

        }

        int FadeSpeed = 10;
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {

            if (scrollerColorAlpha <= 250 && scrollerColorAlpha + FadeSpeed <= 255)
                scrollerColorAlpha += FadeSpeed;
            else
                selection_AnimationON.Stop();

            this.Invalidate();
        }
        private void OnTimedEventOFF(object source, ElapsedEventArgs e)
        {

            if (scrollerColorAlpha >= 150)
                scrollerColorAlpha -= FadeSpeed;
            else
                selection_AnimationOFF.Stop();

            this.Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);


            selection_AnimationOFF.Stop();
            selection_AnimationON.Start();
            this.Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            selection_AnimationON.Stop();
            selection_AnimationOFF.Start();
            this.Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            scrollerRectangle.Width = this.Width;
            this.Invalidate();
        }
    }
}
