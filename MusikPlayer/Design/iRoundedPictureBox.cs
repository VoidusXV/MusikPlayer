using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    public class iRoundedPictureBox : PictureBox
    {
        private Size _ImageSize;
        private Image _Image;

        public iRoundedPictureBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

       /* public Image Image
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


        private void DisplayImageAlign(PaintEventArgs e)
        {
            if (this.Image == null)
                return;

            int x_Image = (this.Width - (int)Image.Width) / 2;
            int y_Image = (this.Height - (int)Image.Height) / 2;
            e.Graphics.DrawImage(Image, new PointF(x_Image, y_Image));
        }
       */
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.CompositingMode = CompositingMode.SourceCopy;
            /*float x = (this.Width - this.Width - 1) / 2;
            float y = (this.Height - this.Height - 1) / 2;
            e.Graphics.FillEllipse(new SolidBrush(Color.Red), new RectangleF(x, 0 / 2, this.Width - 1, this.Width - 1));
            DisplayImageAlign(e);*/
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            using (var gp = new GraphicsPath())
            {
                gp.FillMode = FillMode.Winding;
                gp.AddEllipse(new Rectangle(0, 0, this.Width - 1, this.Height - 1));
                this.Region = new Region(gp);
                gp.CloseFigure();

            }
        }
    }

}
