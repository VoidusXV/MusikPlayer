using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Forms
{
    public partial class FavouriteSongs : Form
    {
        private const int cGripSize = 20;
        private bool mDragging;
        private Point mDragPos;
        public FavouriteSongs()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private const int
           HTLEFT = 10,
           HTRIGHT = 11,
           HTTOP = 12,
           HTTOPLEFT = 13,
           HTTOPRIGHT = 14,
           HTBOTTOM = 15,
           HTBOTTOMLEFT = 16,
           HTBOTTOMRIGHT = 17;


        const int distance = 5;


        Rectangle Top { get { return new Rectangle(0, 0, this.ClientSize.Width, distance); } }
        Rectangle Left { get { return new Rectangle(0, 0, distance, this.ClientSize.Height); } }
        Rectangle Bottom { get { return new Rectangle(0, this.ClientSize.Height - distance, this.ClientSize.Width, distance); } }

 
        Rectangle Right { get { return new Rectangle(this.ClientSize.Width - distance, 0, distance, this.ClientSize.Height); } }

        Rectangle TopLeft { get { return new Rectangle(0, 0, distance, distance); } }
        Rectangle TopRight { get { return new Rectangle(this.ClientSize.Width - distance, 0, distance, distance); } }
        Rectangle BottomLeft { get { return new Rectangle(0, this.ClientSize.Height - distance, distance, distance); } }
        Rectangle BottomRight { get { return new Rectangle(this.ClientSize.Width - distance, this.ClientSize.Height - distance, distance, distance); } }

        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);
            if (message.Msg == 0x84) // WMdistanceNCHITTEST
            {
                var cursor = this.PointToClient(Cursor.Position);
                Console.WriteLine(TopLeft.Contains(cursor));

                if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;

                else if (Top.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (Left.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
        }





        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));
            //sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);

            region.Exclude(Top);
            region.Exclude(Left);
            region.Exclude(Bottom);
            region.Exclude(Right);
            this.panel1.Region = region;
            //this.panel1.BackColor = Color.Red;
            this.Invalidate();
        }

    }
}

