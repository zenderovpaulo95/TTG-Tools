<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TTG_Tools
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());
        }
    }
}
=======
﻿using System;
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
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainMenu());

                FirstTime = false;
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
>>>>>>> cff1486165f08fc8356befbdc6f9e91a39b189db
