using MusikPlayer.Methods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    public class iSwitchBox : Control
    {
        private Color _SwitchBoxColorTemp;
        private Color _SwitchBoxColor;
        private Color _SwitchBoxColorBg;

        private Rectangle _SwichtBoxRectangle = new Rectangle();
        private Rectangle _SwichtBoxRectangleBg = new Rectangle();

        private bool _checked = false;

        System.Timers.Timer switchOnTimer = new System.Timers.Timer();
        System.Timers.Timer switchOffTimer = new System.Timers.Timer();

        public iSwitchBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            _SwitchBoxColor = Color.MediumSlateBlue;
            _SwitchBoxColorBg = Color.Red;

            this.BackColor = Color.FromArgb(17, 17, 23);

            _SwichtBoxRectangle.Y = 0;

            _SwichtBoxRectangleBg.Y = 0;

            switchOnTimer.Elapsed += new ElapsedEventHandler(SwitchMoveOnTimer_Elapsed);
            switchOnTimer.Interval = 1;

            switchOffTimer.Elapsed += new ElapsedEventHandler(SwitchMoveOffTimer_Elapsed);
            switchOffTimer.Interval = 1;
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            _SwitchBoxColorTemp = _SwitchBoxColor;
            CheckedProperties();
        }

        private void CheckedProperties()
        {
            _SwichtBoxRectangle.Width = (int)(this.Width / 3);

            if (Checked == false)
                _SwichtBoxRectangle.X = 0;
            else
                _SwichtBoxRectangle.X = this.Width - _SwichtBoxRectangle.Width;
        }


        public Color SwitchBoxColor
        {
            get
            {
                return _SwitchBoxColor;
            }
            set
            {
                _SwitchBoxColor = value;
                this.Invalidate();
            }
        }
        public Color SwitchBoxBackColor
        {
            get
            {
                return _SwitchBoxColorBg;
            }
            set
            {
                _SwitchBoxColorBg = value;
                this.Invalidate();

            }
        }
        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {

                _checked = value;
                _SwichtBoxRectangle.X = 0;

                if (_checked == true)
                    _SwichtBoxRectangle.X = this.Width - _SwichtBoxRectangle.Width;

                this.Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _SwichtBoxRectangle.Width = (int)(this.Width / 3);
            _SwichtBoxRectangle.Height = this.Height;

            _SwichtBoxRectangleBg.Width = this.Width - _SwichtBoxRectangle.Width;
            _SwichtBoxRectangleBg.Height = _SwichtBoxRectangle.Height;
            _SwichtBoxRectangleBg.X = (_SwichtBoxRectangle.Width - this.Width) + _SwichtBoxRectangle.X;

            e.Graphics.FillRectangle(new SolidBrush(_SwitchBoxColor), _SwichtBoxRectangle);
            e.Graphics.FillRectangle(new SolidBrush(_SwitchBoxColorBg), _SwichtBoxRectangleBg);

            //e.Graphics.DrawRectangle(new Pen(Color.FromArgb(150, Color.Black)), _SwichtBoxRectangle.X, _SwichtBoxRectangle.Y, _SwichtBoxRectangle.Width - 1, _SwichtBoxRectangle.Height - 1); // Border
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(100, Color.Black)), 0, 0, Width - 1, Height - 1); // Border

        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (_checked == false)
            {
                _checked = true;
                switchOnTimer.Start();
            }
            else if (_checked == true)
            {
                _checked = false;
                switchOffTimer.Start();
            }
        }

        private int moveSpeed = 5;
        private void SwitchMoveOnTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _SwichtBoxRectangle.X += moveSpeed;

            if (_SwichtBoxRectangle.X >= (this.Width - _SwichtBoxRectangle.Width) - moveSpeed)
            {
                _SwichtBoxRectangle.X = this.Width - _SwichtBoxRectangle.Width;
                switchOnTimer.Stop();
            }
            this.Invalidate();
        }

        private void SwitchMoveOffTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_SwichtBoxRectangle.X <= moveSpeed)
                switchOffTimer.Stop();

            _SwichtBoxRectangle.X -= moveSpeed;
            this.Invalidate();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _SwitchBoxColor = ControlPaint.Light(_SwitchBoxColor, 0.3f);
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _SwitchBoxColor = _SwitchBoxColorTemp;
            this.Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (Checked == false)
                _SwichtBoxRectangle.X = 0;
            else
                _SwichtBoxRectangle.X = this.Width - _SwichtBoxRectangle.Width;

            this.Invalidate();
        }
    }
}
