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

        private void buttonInputFolder_Click(object sender, EventArgs e)
        {
            textBoxInputFolder.Text = SetFolder(textBoxInputFolder.Text);
        }

        private void buttonOutputFolder_Click(object sender, EventArgs e)
        {
            textBoxOutputFolder.Text = SetFolder(textBoxOutputFolder.Text);
        }

        private void buttonTempFolder_Click(object sender, EventArgs e)
        {
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            MainMenu.settings.ASCII_N = (int)numericUpDownASCII.Value;
            MainMenu.settings.deleteD3DTXafterImport = checkBoxD3DTX_after_import.Checked;
            MainMenu.settings.deleteDDSafterImport = checkBoxDDS_after_import.Checked;
            MainMenu.settings.pathForInputFolder = textBoxInputFolder.Text;
            MainMenu.settings.pathForOutputFolder = textBoxOutputFolder.Text;
            MainMenu.settings.importingOfName = checkBoxImportingOfNames.Checked;
            MainMenu.settings.sortSameString = checkBoxSortStrings.Checked;
            MainMenu.settings.exportRealID = checkBoxExportRealID.Checked;
            MainMenu.settings.clearMessages = clearMessagesCB.Checked;
            if (rbNormalUnicode.Checked == true) MainMenu.settings.unicodeSettings = 0;
            else if (rbNonNormalUnicode2.Checked == true) MainMenu.settings.unicodeSettings = 2;

            //MainMenu.settings.unicodeSupport = checkUnicode.Checked;
            Settings.SaveConfig(MainMenu.settings);
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
            if (Convert.ToInt32(numericUpDownASCII.Value.ToString()) >= 1250 && Convert.ToInt32(numericUpDownASCII.Value.ToString()) <= 1258)
            {
                //ASCII_N = Convert.ToInt32(numericUpDownASCII.Value.ToString());
            }
            else
            {
                numericUpDownASCII.Value = 1250;
            }

            //Terrible fix for users windows-1252 encoding
            if(Convert.ToInt32(numericUpDownASCII.Value.ToString()) == 1252)
            {
                rbNormalUnicode.Checked = true;
                MainMenu.settings.unicodeSettings = 0;
                rbNonNormalUnicode2.Enabled = false;
                rbNormalUnicode.Enabled = false;
            }
            else
            {
                rbNonNormalUnicode2.Enabled = true;
                rbNormalUnicode.Enabled = true;
            }
        }

        private void buttonPathForTtarchext_Click(object sender, EventArgs e)
        {

        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            numericUpDownASCII.Value = MainMenu.settings.ASCII_N;
            checkBoxD3DTX_after_import.Checked = MainMenu.settings.deleteD3DTXafterImport;
            checkBoxDDS_after_import.Checked = MainMenu.settings.deleteDDSafterImport;
            checkBoxImportingOfNames.Checked = MainMenu.settings.importingOfName;
            checkBoxSortStrings.Checked = MainMenu.settings.sortSameString;
            checkBoxExportRealID.Checked = MainMenu.settings.exportRealID;
            clearMessagesCB.Checked = MainMenu.settings.clearMessages;
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
    }
}
