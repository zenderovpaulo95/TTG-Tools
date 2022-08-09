using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace TTG_Tools
{
    static class Program
    {
        public static bool FirstTime = true;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string xmlPath = Application.StartupPath + "\\config.xml";
            if (File.Exists(xmlPath))
            {
                FirstTime = false;

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainMenu());
            }
            else
            {
                MessageBox.Show("Can't find config.xml!\r\nPlease set path for folders, save changes and restart the program!", "Error");
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormSettings());
            }
        }
    }
}
