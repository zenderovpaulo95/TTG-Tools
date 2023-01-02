using System;
using System.Windows.Forms;
using System.IO;

namespace TTG_Tools
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        public string SetFolder(string inputPath)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            if (Directory.Exists(inputPath))
            {
                fbd.SelectedPath = inputPath;
            }
            else
            {
                fbd.SelectedPath = Application.StartupPath;
            }
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                return fbd.SelectedPath;
            }
            else { return inputPath; }
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            MainMenu.settings.ASCII_N = (int)numericUpDownASCII.Value;
            MainMenu.settings.pathForInputFolder = textBoxInputFolder.Text;
            MainMenu.settings.pathForOutputFolder = textBoxOutputFolder.Text;
            if (rbNormalUnicode.Checked == true) MainMenu.settings.unicodeSettings = 0;
            else if (rbNonNormalUnicode2.Checked == true) MainMenu.settings.unicodeSettings = 1;

            if (((MainMenu.settings.pathForInputFolder != "") && (Directory.Exists(MainMenu.settings.pathForInputFolder)))
                && ((MainMenu.settings.pathForOutputFolder != "") && (Directory.Exists(MainMenu.settings.pathForOutputFolder))))
            {
                Settings.SaveConfig(MainMenu.settings);

                if (Program.FirstTime)
                {
                    MessageBox.Show("Please restart application to confirm settings");
                }
            }
            else
            {
                MessageBox.Show("Please set a correct paths for input and output folders!");
            }
        }

        private void buttonOkSettings_Click(object sender, EventArgs e)
        {
            buttonSaveSettings_Click(sender, e);
            this.Close();
        }

        private void buttonCloseSettingsForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numericUpDownASCII_ValueChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(numericUpDownASCII.Value) < 874)
            {
                numericUpDownASCII.Value = 874;
            }
            if (Convert.ToInt32(numericUpDownASCII.Value) > 1258)
            {
                numericUpDownASCII.Value = 1258;
            }
            switch (Convert.ToInt32(numericUpDownASCII.Value.ToString()))
            {
                case 1249:
                    numericUpDownASCII.Value = 874;
                    break;
                case 875:
                    numericUpDownASCII.Value = 1250;
                    break;
            }
            //Terrible fix for users windows-1252 encoding
            if (Convert.ToInt32(numericUpDownASCII.Value.ToString()) == 1252)
            {
                rbNormalUnicode.Checked = true;
                MainMenu.settings.unicodeSettings = 0;
                rbNonNormalUnicode2.Enabled = false;
                rbNormalUnicode.Enabled = false;
            }
            else
            {
                MainMenu.settings.unicodeSettings = 1;
                rbNonNormalUnicode2.Enabled = true;
                rbNormalUnicode.Enabled = true;
            }
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            numericUpDownASCII.Value = MainMenu.settings.ASCII_N;
            textBoxInputFolder.Text = MainMenu.settings.pathForInputFolder;
            textBoxOutputFolder.Text = MainMenu.settings.pathForOutputFolder;

            switch (MainMenu.settings.unicodeSettings)
            {
                case 0:
                    rbNormalUnicode.Checked = true;
                    break;
                case 1:
                case 2:
                    rbNonNormalUnicode2.Checked = true;
                    break;
                default:
                    rbNormalUnicode.Checked = true;
                    break;
            }
        }

        private void buttonInputFolder_Click(object sender, EventArgs e)
        {
            textBoxInputFolder.Text = SetFolder(textBoxInputFolder.Text);
        }

        private void buttonOutputFolder_Click(object sender, EventArgs e)
        {
            textBoxOutputFolder.Text = SetFolder(textBoxOutputFolder.Text);
        }
    }
}
