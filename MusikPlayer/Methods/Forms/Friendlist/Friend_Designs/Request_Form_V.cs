using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Forms.Friendlist.Friend_Designs
{
    class Request_Form_V
    {

        public static Panel RequestFormPanel(string LabelText, int Index, EventHandler blockClick = null, EventHandler declineClick = null, EventHandler acceptClick = null)
        {
            Panel MainPanel = new Panel();
            Panel Textpanel1 = new Panel();
            Panel ButtonPanel = new Panel();
            Design.HLine hLine = new Design.HLine();


            MainPanel.Controls.Add(Textpanel1);
            MainPanel.Controls.Add(ButtonPanel);

            MainPanel.Size = new Size(380, 128);
            MainPanel.BackColor = Color.FromArgb(14, 14, 15);

            Textpanel1.Size = new Size(380, 68);
            Textpanel1.Dock = DockStyle.Top;
            Textpanel1.BackColor = Color.FromArgb(14, 14, 15);

            ButtonPanel.Size = new Size(380, 60);
            ButtonPanel.Dock = DockStyle.Bottom;
            ButtonPanel.BackColor = Color.FromArgb(14, 14, 15);


            ButtonPanel.Controls.Add(hLine);
            hLine.Dock = DockStyle.Bottom;
            hLine.Height = 1;
            hLine.AlphaColor = 100;

            Textpanel1.Controls.Add(label(LabelText));

            var blockButton = iButton(Color.Indigo, new Point(12, 8), "Blockieren", blockClick);
            blockButton.Name = $"BlockButton_{Index}";

            var DeclineButton = iButton(Color.Firebrick, new Point(133, 8), "Ablehnen", declineClick);
            DeclineButton.Name = $"DeclineButton_{Index}";

            var AcceptButton = iButton(Color.CornflowerBlue, new Point(255, 8), "Anehmen", acceptClick);
            AcceptButton.Name = $"AcceptButton_{Index}";

            ButtonPanel.Controls.Add(blockButton);
            ButtonPanel.Controls.Add(DeclineButton);
            ButtonPanel.Controls.Add(AcceptButton);

            return MainPanel;
        }

        public static Design.iButton2 iButton(Color color, Point location, string Text, EventHandler eventHandler = null)
        {
            Design.iButton2 iButton = new Design.iButton2();
            iButton.BackColor = color;
            iButton.HoverColor = color;
            iButton.BorderRadius = 3;
            iButton.Location = location;
            iButton.Size = new Size(114, 40);
            iButton.Text = Text;
            iButton.Click += eventHandler;
            return iButton;
        }



        public static Label label(string Text)
        {
            Label label = new Label();
            label.AutoSize = true;
            label.Location = new Point(12, 12);
            label.Font = new Font("Microsoft Sans Serif", 12, FontStyle.Regular);
            label.ForeColor = Color.White;
            label.Text = Text;
            label.MaximumSize = new Size(360, 0);
            return label;
        }
    }
}
