using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    public class iCircleButton : Button
    {

        private Color borderColor = Color.White;
        private Size _ImageSize;

        private string _superTip = "";
        private bool onMouseEnter = false;
        private Form superTipForm = new Form();
        Label superTiplabel = new Label();

        public iCircleButton()
        {
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(80, 80);
            this.BackColor = Color.MediumSlateBlue;
            this.ForeColor = Color.White;

           /* superTiplabel.ForeColor = Color.White;
            superTiplabel.Font = new Font("Microsoft Sans Serif", 10f, FontStyle.Bold);
            superTiplabel.AutoSize = true;
            superTipForm.Controls.Add(superTiplabel);
            superTipForm.ShowInTaskbar = false;
            superTipForm.TopMost = true;
            superTipForm.FormBorderStyle = FormBorderStyle.None;
            superTipForm.BackColor = Color.FromArgb(20, 20, 20);*/
        }

        #region RoundedBorder
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
     (
         int nLeftRect,     // x-coordinate of upper-left corner
         int nTopRect,      // y-coordinate of upper-left corner
         int nRightRect,    // x-coordinate of lower-right corner
         int nBottomRect,   // y-coordinate of lower-right corner
         int nWidthEllipse, // width of ellipse
         int nHeightEllipse // height of ellipse
     );
        #endregion

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        [Category("iDesign")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }
        [Category("iDesign")]
        public Color FillColor
        {
            get { return this.BackColor; }
            set { this.BackColor = value; }
        }

        [Category("iDesign")]
        public Color TextColor
        {
            get { return this.ForeColor; }
            set { this.ForeColor = value; }
        }
        public Size ImageSize
        {
            get
            {
                if (this.Image != null)
                    return this.Image.Size;
                return _ImageSize;
            }
            set
            {
                _ImageSize = value;
                if (this.Image != null)
                    this.Image = resizeImage(this.Image, _ImageSize);

            }
        }
        public string SuperTip
        {
            get { return _superTip; }
            set
            {
                _superTip = value;
                this.Invalidate();
            }
        }
        private GraphicsPath GetFigurePath(Rectangle rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            //CreateSuperTipForm();

            int borderSize = 0;
            int borderRadius = this.Width / 2;
            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
            int smoothSize = 2;
            if (borderSize > 0)
                smoothSize = borderSize;

            if (borderRadius > 2) //Rounded button
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize)) // Border
                using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    this.Region = new Region(pathSurface);
                    pevent.Graphics.DrawPath(penSurface, pathSurface);

                    if (borderSize >= 1) // Border
                        pevent.Graphics.DrawPath(penBorder, pathBorder);
                }
            }
        }



        protected override void OnHandleCreated(EventArgs e)
        {
            try
            {
                base.OnHandleCreated(e);
                this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
            }
            catch { }
        }

        private void Container_BackColorChanged(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        #region ToolTip
        /*  private void CreateSuperTipForm()
          {

              if (SuperTip != "" && onMouseEnter == true)
              {
                  superTiplabel.Text = SuperTip;

                  var form1 = Parent.Parent.Parent;
                  Point locationOnForm = this.FindForm().PointToClient(Parent.PointToScreen(this.Location));
                  superTipForm.Size = new Size(superTiplabel.Width + 15, superTiplabel.Height + 15);
                  superTiplabel.Location = new Point((superTipForm.Width - superTiplabel.Width) / 2, (superTipForm.Height - superTiplabel.Height) / 2);
                  superTipForm.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, superTipForm.Width, superTipForm.Height, 5, 5));

                  superTipForm.Show();
                  superTipForm.Location = new Point(this.Bounds.X, this.Bounds.Y);
              }
          }

          protected override void OnMouseEnter(EventArgs e)
          {
              base.OnMouseEnter(e);
              onMouseEnter = true;
              CreateSuperTipForm();

          }
          protected override void OnMouseLeave(EventArgs e)
          {
              base.OnMouseLeave(e);
              onMouseEnter = false;
              superTipForm.Hide();
          }

          protected override void OnMouseHover(EventArgs e)
          {
              base.OnMouseHover(e);
              onMouseEnter = true;
          }*/
    }
    #endregion
}
