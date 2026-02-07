using System;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

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

                Settings loadedSettings = null;
                try
                {
                    using (XmlReader reader = new XmlTextReader(xmlPath))
                    {
                        XmlSerializer settingsDeserializer = new XmlSerializer(typeof(Settings));
                        loadedSettings = (Settings)settingsDeserializer.Deserialize(reader);
                    }
                }
                catch
                {
                    loadedSettings = null;
                }

                UiLocalizer.Initialize(loadedSettings != null ? loadedSettings.uiLanguageCode : "en");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainMenu());
            }
            else
            {
                UiLocalizer.Initialize("en");
                MessageBox.Show(UiLocalizer.Get("Program.MissingConfigMessage"), UiLocalizer.Get("Program.MissingConfigTitle"));
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormSettings());
            }
        }
    }
}
