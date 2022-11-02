using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace MusikPlayer.Methods
{
    public class Animations
    {
        #region Label_CopiedAnimation
        private Thread Thread_CopiedAnimation;
        private bool ThreadsRunning = false;
        private Design.iLabel CopiedLabel;

        private void CopiedAnimation_Properties(Control control)
        {
            CopiedLabel = new Design.iLabel();
            CopiedLabel.Text = "Kopiert";
            CopiedLabel.BackColor = Color.Transparent;
            CopiedLabel.ForeColor = Color.FromArgb(0, 192, 0);
            CopiedLabel.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
            //CopiedLabel.Location = location;
            int X_Middle = control.Location.X + (control.Width - CopiedLabel.Width) / 2;
            CopiedLabel.Location = new Point(X_Middle, control.Location.Y - 20);
        }

        private void CopiedAnimation_Thread(Control control)
        {
            ThreadsRunning = true;
            for (int i = 0; i <= 255; i += 5)
            {
                CopiedLabel.AlphaColor = i;
                Thread.Sleep(10);
            }
            for (int i = 255; i >= 0; i -= 5)
            {
                CopiedLabel.AlphaColor = i;
                Thread.Sleep(10);
            }
            control.Invoke((MethodInvoker)delegate
            {
                control.Parent.Controls.Remove(CopiedLabel);
            });
            ThreadsRunning = false;
        }

        public void CopiedAnimation(Control control)
        {
            CopiedAnimation_Properties(control);
            control.Parent.Controls.Add(CopiedLabel);
            Thread_CopiedAnimation = new Thread(() => CopiedAnimation_Thread(control)); //new Thread(CopiedAnimation_Thread);
            if (ThreadsRunning == false)
            {
                Thread_CopiedAnimation.Start();
            }
        }
        #endregion

        #region FormFade_Animation

        private System.Timers.Timer OpenFormFadeTimer;
        private Form form;

        private void FormFade_Properties(Form form)
        {
            OpenFormFadeTimer = new System.Timers.Timer();
            OpenFormFadeTimer.Interval = 1;
            OpenFormFadeTimer.Elapsed += new ElapsedEventHandler(OpenFormFadeTimer_Elapsed);
            this.form = form;
            this.form.Opacity = 0;
        }

        private void OpenFormFadeTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (this.form.Opacity >= 1)
                OpenFormFadeTimer.Stop();
            this.form.Invoke((MethodInvoker)delegate
            {
                this.form.Opacity += .08;
            });
        }

        public void OpenFormFade_AnimationStart(Form form)
        {
            FormFade_Properties(form);
            OpenFormFadeTimer.Start();
        }
        #endregion
    }
}
