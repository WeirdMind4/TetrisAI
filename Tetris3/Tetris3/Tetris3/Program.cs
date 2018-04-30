using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Tetris3
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ShowSplash();
            Application.EnableVisualStyles();
            System.Threading.Thread.Sleep(7000);
            Application.Run(new Form1());
        }

        private static void ShowSplash()
        {
            SplashScreenIntro sp = new SplashScreenIntro();
            sp.Show();
            SoundPlayer music = new SoundPlayer(Resources.Resource1.Tetris);
            music.Play();
            Application.DoEvents();

            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer
            {
                Interval = 5000
            };
            t.Tick += new EventHandler((sender, ea) =>
            {
                sp.BeginInvoke(new Action(() =>
                {
                    if (sp != null && Application.OpenForms.Count > 1)
                    {
                        sp.Close();
                        sp.Dispose();
                        sp = null;
                        t.Stop();
                        t.Dispose();
                        t = null;
                    }
                }));
            });
            t.Start();
        }
    }
}
