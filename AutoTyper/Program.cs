using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace AutoTyper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main(string[] args)
        {
            if (args.Length == 1 && args[0] == "INSTALLER") 
            {
                Process.Start(Application.ExecutablePath);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mf = new MainForm();
            Application.Run(mf);
        }
    }
}
