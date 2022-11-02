using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MusikPlayer.Methods.Forms.Friendlist.Friend_Designs
{
    public partial class RequestForm : Form
    {
        public RequestForm()
        {
            
            InitializeComponent();
            label1.Text = "Du hast eine Freundschaftanfrage von ... erhalten";

            this.Size = new Size(panel2.Width, label1.Height + panel1.Height + 20);
            panel2.Size = this.Size;
        }

        private void iButton21_Click(object sender, EventArgs e)
        {
        }

        private void Button_DeclineFR_Click(object sender, EventArgs e)
        {

        }
    }
}
