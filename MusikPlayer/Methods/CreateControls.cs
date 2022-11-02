using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods
{
    class CreateControls
    {
        public static List<Design.iLabel> iLabels = new List<Design.iLabel>();
        public static int count = 0;

        public static void Create_iLabel(List<Design.iLabel> iLabels, int index, string text, Font font, Point Location, Size MaxSize, bool select, bool HoverAnimation)
        {
            iLabels.Add(new Design.iLabel());
            iLabels[index].Name = $"iLabel_{index}";
            iLabels[index].Location = Location;//new Point(40, 111 + (i * 46));
            iLabels[index].AutoSize = true;
            iLabels[index].MaximumSize = MaxSize;
            iLabels[index].Text = text;
            iLabels[index].Font = font;//new Font("Arial Narrow", 12F, FontStyle.Bold);
            iLabels[index].TextAlign = ContentAlignment.MiddleLeft;
            iLabels[index].AlphaColor = 150;
            iLabels[index].ForeColor = Color.White;
            iLabels[index].Select = select;
            iLabels[index].HoverAnimation = HoverAnimation;
            count++;
        }



        public static Panel Create_SongPlayerPanel(string PanelName, string NumText, string SongName, string Band, string Album, string AddedDate, string SongDuration, Point Location, Size size, Image image)
        {
            Panel SongPlayerPanel = new Panel();
            //SongPlayerPanel.BackColor = Color.Red;
            SongPlayerPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            SongPlayerPanel.Name = PanelName;
            SongPlayerPanel.Location = Location;
            SongPlayerPanel.Size = size;//new Size(1145, 54);
            SongPlayerPanel.Controls.Add(Labels(NumText, new Point(7, 18), AnchorStyles.Left | AnchorStyles.Top, Cursors.Default));
            SongPlayerPanel.Controls.Add(iPictureBoxs(image));
            SongPlayerPanel.Controls.Add(Labels(SongName, new Point(99, 17), AnchorStyles.Left | AnchorStyles.Top, Cursors.Hand));
            SongPlayerPanel.Controls.Add(Labels(Band, new Point(368, 17), AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right, Cursors.Hand));
            SongPlayerPanel.Controls.Add(Labels(Album, new Point(702, 17), AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right, Cursors.Hand));
            SongPlayerPanel.Controls.Add(Labels(AddedDate, new Point(923, 17), AnchorStyles.None, Cursors.Default));
            SongPlayerPanel.Controls.Add(Labels(SongDuration, new Point(1201, 17), (AnchorStyles.Top | AnchorStyles.Right), Cursors.Default));
            return SongPlayerPanel;
        }
        public static Panel Create_SongPlayerPanel(string NumText, Design.iLabel SongName, Design.iLabel Band, Design.iLabel Album, Design.iLabel AddedDate, Design.iLabel SongDuration, Point Location, Size size, Image image)
        {
            Panel SongPlayerPanel = new Panel();
            SongPlayerPanel.Name = "Panel_" + NumText;
            SongPlayerPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            SongPlayerPanel.Location = Location;
            SongPlayerPanel.Size = size;//new Size(1145, 54);
            SongPlayerPanel.Controls.Add(Labels(NumText, new Point(7, 18), AnchorStyles.Left | AnchorStyles.Top, Cursors.Default, "NumLabel_" + NumText));
            SongPlayerPanel.Controls.Add(iPictureBoxs(image, "PictureBox_" + NumText));
            SongPlayerPanel.Controls.Add(SongName);
            SongPlayerPanel.Controls.Add(Band);
            SongPlayerPanel.Controls.Add(Album);
            SongPlayerPanel.Controls.Add(AddedDate);
            SongPlayerPanel.Controls.Add(SongDuration);
            return SongPlayerPanel;
        }
        public static Panel Create_Panel(string PanelName, Point Location, Size size)
        {
            Panel SongPlayerPanel = new Panel();
            SongPlayerPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
            SongPlayerPanel.Name = PanelName;
            SongPlayerPanel.Location = Location;
            SongPlayerPanel.Size = size;
            //SongPlayerPanel.BackColor = Color.Red;
            SongPlayerPanel.BorderStyle = BorderStyle.FixedSingle;
            return SongPlayerPanel;
        }

        public static Design.iLabel Labels(string text, Point Location, AnchorStyles anchorStyles, Cursor cursors, string Name = "")
        {
            Design.iLabel iLabel = new Design.iLabel();
            iLabel.Name = Name;
            iLabel.AlphaColor = 150;
            iLabel.Anchor = anchorStyles;
            iLabel.AutoSize = true;
            iLabel.Font = new Font("Microsoft Sans Serif", 12F);
            iLabel.ForeColor = Color.White;
            iLabel.Location = Location;
            iLabel.Text = text;
            iLabel.Cursor = cursors;
            iLabel.HoverAnimation = false;
            if (cursors == Cursors.Hand)
                iLabel.HoverAnimation = true;
            return iLabel;
        }


        public static Design.iPictureBox iPictureBoxs(Image image, string Name = "")
        {
            Design.iPictureBox iPictureBox = new Design.iPictureBox();
            iPictureBox.Name = Name;
            iPictureBox.Image = image;
            iPictureBox.Size = new Size(50, 30);
            iPictureBox.Location = new Point(33, 12);
            iPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            iPictureBox.BackColor = Color.Red;
            return iPictureBox;
        }
        public static Design.iCheckBox iCheckBox(Point Location, Color FillColor)
        {
            Design.iCheckBox iCheckBox = new Design.iCheckBox();
            iCheckBox.Size = new Size(20, 20);
            iCheckBox.FillColor = FillColor;
            iCheckBox.Location = Location;
            return iCheckBox;
        }
    }
}
