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
    public class iPanel2 : Panel
    {
        public delegate void ignoreChildrenFunc();
        public event ignoreChildrenFunc MouseDown_IgnoreChildren;
        public event ignoreChildrenFunc MouseUp_IgnoreChildren;


        private bool _Border = false;
        //private bool _RoundedBorder = false;

        private int _AlphaColor = 255;
        private Color _BorderColor = Color.White;


        private int BorderWidth;
        private int BorderHeight;

        private int _BackColorAlphaColor = 255;

        private bool _ignoreChildren = false;

        System.Timers.Timer clickIgnoreChildren = new System.Timers.Timer();

        public iPanel2()
        {
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            this.UpdateStyles();

            BorderWidth = Width - 1;
            BorderHeight = Height - 1;

            clickIgnoreChildren.Elapsed += new ElapsedEventHandler(IgnoreChildren_Timer);
            clickIgnoreChildren.Interval = 1;
        }


        public bool IgnoreChildren
        {
            get
            {
                return _ignoreChildren;
            }
            set
            {
                _ignoreChildren = value;
            }
        }

        public Color BorderColor
        {
            get
            {
                return _BorderColor;
            }
            set
            {
                _BorderColor = value;
            }
        }
        public bool Border
        {
            get { return _Border; }
            set { _Border = value; }
        }
        /*public bool RoundedBorder
        {
            get { return _RoundedBorder; }
            set { _RoundedBorder = value; }
        }*/
        public int BackColorAlphaColor
        {
            get => _BackColorAlphaColor;
            set { _BackColorAlphaColor = value; }
        }
        public int AlphaColor
        {
            get => _AlphaColor;
            set
            {
                _AlphaColor = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (Border == true)
            {
                //Region fillRegion = new Region(ClientRectangle);
                //e.Graphics.FillRegion(new SolidBrush(BackColor), fillRegion);

                //g.Clear(BackColor);
                g.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
                g.DrawRectangle(new Pen(Color.FromArgb(AlphaColor, BorderColor)), 0, 0, BorderWidth, BorderHeight);
            }
        }
        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            BorderWidth = Width - 1;
            BorderHeight = Height - 1;
            this.Invalidate();
        }

        public bool PanelEnter = false;

        protected override void OnMouseEnter(EventArgs e)
        {
            if (_ignoreChildren == false)
            {
                base.OnMouseEnter(e);
                return;
            }

            if (PanelEnter == false)
            {
                base.OnMouseEnter(e);
                PanelEnter = true;
                if (MouseDown_IgnoreChildren != null)
                    clickIgnoreChildren.Start();
            }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (_ignoreChildren == false)
            {
                base.OnMouseLeave(e);
                return;
            }

            if (this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
                return;

            PanelEnter = false;
            panelOnMouse = "";
            if (MouseUp_IgnoreChildren != null)
                clickIgnoreChildren.Stop();

            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_ignoreChildren == false)
            {
                base.OnMouseDown(e);
                return;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_ignoreChildren == false)
            {
                base.OnMouseUp(e);
                return;
            }
            base.OnMouseUp(e);
        }

        private string panelOnMouse = "";

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }



        private void IgnoreChildren_Timer(object source, ElapsedEventArgs e)
        {

            this.Invoke((MethodInvoker)delegate
            {
                if (this.CanFocus == false) //|| this.Parent.ContainsFocus == false)
                {
                    //panelOnMouse = "";
                    // if (panelOnMouse == "Up")
                    //    MouseDown_IgnoreChildren();
                    // else if (panelOnMouse == "Down")
                    //MouseDown_IgnoreChildren();

                    clickIgnoreChildren.Stop();
                    return;
                }

                // Console.WriteLine(this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)));
                //Console.WriteLine($"{this.CanFocus} | {this.Focused} | {this.ContainsFocus} | {this.Parent.ContainsFocus}");
                //Console.WriteLine($"{this.Parent.CanFocus} | {this.Parent.Focused} | {this.Parent.ContainsFocus}");

                if (Control.MouseButtons == MouseButtons.Left && panelOnMouse != "Down")
                {
                    panelOnMouse = "Down";
                    MouseDown_IgnoreChildren();
                }
                if (panelOnMouse == "Down" && Control.MouseButtons == MouseButtons.None)
                {
                    MouseUp_IgnoreChildren();
                    panelOnMouse = "Up";
                }

            });
            this.Invalidate();
        }
    }
}
