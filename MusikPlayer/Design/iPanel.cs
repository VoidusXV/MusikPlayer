using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    public class iPanel : Panel
    {
        private bool _Border = false;
        private int _AlphaColor = 255;
        private Color _BorderColor = Color.White;

        private bool _Bool_CreateParams = false;
        public iPanel()
        {
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            this.UpdateStyles();
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

        public int AlphaColor
        {
            get => _AlphaColor;
            set { _AlphaColor = value; }
        }

        public bool Bool_CreateParams
        {
            get { return _Bool_CreateParams; }
            set { _Bool_CreateParams = value; }
        }
        protected override CreateParams CreateParams
        {
            get
            {

                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (Border == true)
            {
                using (SolidBrush brush = new SolidBrush(BackColor))
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(AlphaColor, BorderColor)), 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
            }

        }
    }
}
