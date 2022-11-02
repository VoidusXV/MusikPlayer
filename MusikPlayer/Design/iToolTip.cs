using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Design
{
    public class iToolTip : ToolTip
    {
        Font font = new Font("Microsoft Sans Serif", 13f, FontStyle.Regular);

        public iToolTip()
        {
            this.OwnerDraw = true;
            this.Popup += new PopupEventHandler(this.OnPopup);
            this.Draw += new DrawToolTipEventHandler(this.OnDraw);

        }
        private void OnPopup(object sender, PopupEventArgs e)
        {
            string toolTipText = (sender as ToolTip).GetToolTip(e.AssociatedControl);
            var TextSize = TextRenderer.MeasureText(toolTipText, font);
            e.ToolTipSize = new Size(TextSize.Width + 10, TextSize.Height + 10);
        }

        private void OnDraw(object sender, DrawToolTipEventArgs e)
        {
            Graphics g = e.Graphics;
            SizeF fontSize = e.Graphics.MeasureString(e.ToolTipText, font);
            g.FillRectangle(new SolidBrush(Color.FromArgb(25, 25, 25)), e.Bounds);
            g.DrawString(e.ToolTipText, font, new SolidBrush(Color.White), new PointF((e.Bounds.Width - fontSize.Width) / 2, (e.Bounds.Height - fontSize.Height) / 2));

        }
        
    }
}
