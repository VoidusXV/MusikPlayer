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
    public class iButton2 : Control
    {
        private int borderSize = 0;
        private int borderRadius = 0;
        private Size _ImageSize;
        private Image _Image;

        private float ColorA = 255;
        private Color borderColor = Color.White;
        private Color _HoverColorVal;
        private float _HoverBrightnessVal;
        private bool _HoverColorEnabled = true;
        private Color _backColor;

        private bool PressAnimVal = true;

        private ContentAlignment _TextAlign = ContentAlignment.MiddleCenter;

        public iButton2()
        {
            this.Size = new Size(150, 40);
            this.ForeColor = Color.White;
            this.BackColor = Color.MediumSlateBlue;

            _backColor = this.BackColor;
            _HoverColorVal = this.BackColor;
            _HoverBrightnessVal = 0.1f;

            this.MouseEnter += IButton2_MouseEnter;
            this.MouseLeave += IButton2_MouseLeave;
            this.MouseDown += IButton2_MouseDown;
            this.MouseUp += IButton2_MouseUp;
            this.DoubleBuffered = true;
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            _backColor = this.BackColor;
        }
        private void IButton2_MouseUp(object sender, MouseEventArgs e)
        {
            if (PressAnimimation == true)
            {
                this.Location = new Point(this.Location.X - 2, this.Location.Y - 2);
                this.Size = new Size(this.Size.Width + 4, this.Size.Height + 4);
            }

            if (!HoverColorEnabled)
                return;

            this.BackColor = ControlPaint.Light(_backColor, _HoverBrightnessVal);

        }

        private void IButton2_MouseDown(object sender, MouseEventArgs e)
        {
            if (PressAnimimation == true)
            {
                this.Location = new Point(this.Location.X + 2, this.Location.Y + 2);
                this.Size = new Size(this.Size.Width - 4, this.Size.Height - 4);
            }

            if (!HoverColorEnabled)
                return;

            Color lightColor = ControlPaint.Light(this.HoverColor, _HoverBrightnessVal + 0.1f);
            this.BackColor = lightColor;

        }

        private void IButton2_MouseLeave(object sender, EventArgs e)
        {
            if (!HoverColorEnabled)
                return;

            this.BackColor = _backColor;
        }

        private void IButton2_MouseEnter(object sender, EventArgs e)
        {
            if (!HoverColorEnabled)
                return;

            if (HoverColor == this.BackColor)
                this.BackColor = ControlPaint.Light(this.BackColor, _HoverBrightnessVal);//HoverColor;
            else
                this.BackColor = HoverColor;
        }
        public void ResetButtonColor()
        {
            this.BackColor = _backColor;
        }

        public ContentAlignment TextAlign
        {
            get
            {
                return _TextAlign;
            }
            set
            {
                _TextAlign = value;
            }
        }
        public bool PressAnimimation
        {
            get { return PressAnimVal; }
            set { PressAnimVal = value; }
        }
        public float HoverBrightnessVal
        {
            get { return _HoverBrightnessVal; }
            set { _HoverBrightnessVal = value; }
        }

        public Color HoverColor
        {
            get
            {
                return _HoverColorVal;
            }
            set
            {
                _HoverColorVal = value;
            }
        }

        public bool HoverColorEnabled
        {
            get
            {
                return _HoverColorEnabled;
            }
            set
            {
                _HoverColorEnabled = value;
            }
        }

        public float AlphaColor
        {
            get => ColorA;
            set
            {
                ColorA = Convert.ToInt32(value);

                if (ColorA >= 255)
                    ColorA = 255;
                else if (ColorA <= 0)
                    ColorA = 0;
                this.Invalidate();
            }
        }



        private static Image resizeImage(Image imgToResize, Size size)
        {
            //Get the image current width  
            int sourceWidth = imgToResize.Width;
            //Get the image current height  
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            //Calulate  width with new desired size  
            nPercentW = ((float)size.Width / (float)sourceWidth);
            //Calculate height with new desired size  
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            //New Width  
            int destWidth = (int)(sourceWidth * nPercent);
            //New Height  
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            // Draw image with new width and height  
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (System.Drawing.Image)b;
        }

        private void Button_Resize(object sender, EventArgs e)
        {
            if (borderRadius > this.Height)
                borderRadius = this.Height;
        }

        //Properties
        [Category("iDesign")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }

        [Category("iDesign")]
        public int BorderRadius
        {
            get
            {
                return borderRadius;
            }
            set
            {
                borderRadius = value;
                this.Invalidate();
            }
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
        public Color BackgroundColor
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

        public Image Image
        {
            get
            {
                return _Image;
            }
            set
            {
                _Image = value;
                this.Invalidate();
            }
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
                    this.Image = resizeImage(_Image, _ImageSize);

                this.Invalidate();
            }
        }

        private ImageAlignEnum customImageAlign = ImageAlignEnum.MiddleCenter;

        public ImageAlignEnum ImageAlign
        {
            get
            {
                return customImageAlign;
            }
            set
            {
                customImageAlign = value;
                this.Invalidate();
            }
        }

        private void DisplayImageAlign(PaintEventArgs e)
        {
            if (this.Image == null)
            {
                DisplayText(e);
                return;
            }
            if (customImageAlign == ImageAlignEnum.MiddleCenter)
            {
                int x_Image = (this.Width - (int)Image.Width) / 2;
                int y_Image = (this.Height - (int)Image.Height) / 2;
                e.Graphics.DrawImage(Image, new PointF(x_Image, y_Image));
                DisplayText(e);

            }
            else if (customImageAlign == ImageAlignEnum.MiddleLeft)
            {
                int y_Image = (this.Height - (int)Image.Height) / 2;
                e.Graphics.DrawImage(Image, new PointF(5, y_Image));

                SizeF stringSize = e.Graphics.MeasureString(this.Text, this.Font, this.Width);
                int y = (this.Height - (int)stringSize.Height) / 2;

                DisplayText(e, new Point(Image.Width + 10, y));

            }
        }

        public enum ImageAlignEnum
        {
            Empty,
            MiddleLeft,
            MiddleCenter,
        };

        private void DisplayText(PaintEventArgs e, Point? location = null)
        {
            SizeF stringSize = new SizeF();

            if (location != null)
            {
                Rectangle rectangle = new Rectangle(location.Value.X, location.Value.Y, Width, Height);
                e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(Color.FromArgb((int)AlphaColor, ForeColor)), rectangle);
                return;
            }

            stringSize = e.Graphics.MeasureString(this.Text, this.Font, this.Width);

            int x = (this.Width - (int)stringSize.Width) / 2;
            int y = (this.Height - (int)stringSize.Height) / 2;
            if (_TextAlign == ContentAlignment.MiddleLeft)
            {
                x = 5;
            }
            Rectangle rc1 = new Rectangle(x, y, Width, Height);
            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(Color.FromArgb((int)AlphaColor, ForeColor)), rc1);
        }

        private static StringFormat ContentAlignmentToStringAlignment(ContentAlignment ca)
        {
            var format = new StringFormat();
            var l2 = (int)Math.Log((double)ca, 2);
            format.LineAlignment = (StringAlignment)(l2 / 4);
            format.Alignment = (StringAlignment)(l2 % 4);
            return format;
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
            pevent.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            pevent.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            pevent.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            DisplayImageAlign(pevent);

            //Rectangle rectSurface = this.ClientRectangle;
            //Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);
            //int smoothSize = 2;

            #region Border
            /*    if (borderSize > 0)
                   smoothSize = borderSize;

               if (borderRadius > 2) //Rounded button
               {
                   using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                   using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                   using (Pen penSurface = new Pen(this.Parent.BackColor, smoothSize))
                   using (Pen penBorder = new Pen(borderColor, borderSize))
                   {
                       pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                       //Button surface
                       this.Region = new Region(pathSurface);
                       //Draw surface border for HD result
                       pevent.Graphics.DrawPath(penSurface, pathSurface);

                       //Button border                    
                       if (borderSize >= 1)
                           //Draw control border
                           pevent.Graphics.DrawPath(penBorder, pathBorder);
                   }
               }
               else //Normal button
               {
                   pevent.Graphics.SmoothingMode = SmoothingMode.None;
                   //Button surface
                   this.Region = new Region(rectSurface);
                   //Button border
                   if (borderSize >= 1)
                   {
                       using (Pen penBorder = new Pen(borderColor, borderSize))
                       {
                           penBorder.Alignment = PenAlignment.Inset;
                           pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                       }
                   }
               }
            */
            #endregion


        }

    }
}
