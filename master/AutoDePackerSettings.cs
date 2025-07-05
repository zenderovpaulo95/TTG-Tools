using System;
using System.IO;
using System.Windows.Forms;

namespace TTG_Tools
{
    public partial class AutoDePackerSettings : Form
    {
        public AutoDePackerSettings()
        {
            InitializeComponent();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AutoDePackerSettings_Load(object sender, EventArgs e)
        {
            if (MainMenu.settings.tsvFormat)
            {
                tsvFilesRB.Checked = true;
            }
            else
            {
                if (!MainMenu.settings.tsvFormat && MainMenu.settings.newTxtFormat) newTxtFormatRB.Checked = true;
                else txtFilesRB.Checked = true;
            }

            checkBoxChangeLangFlags.Enabled = MainMenu.settings.newTxtFormat;
            checkBoxChangeLangFlags.Visible = MainMenu.settings.newTxtFormat;
            checkBoxChangeLangFlags.Checked = MainMenu.settings.changeLangFlags;

            switch(MainMenu.settings.unicodeSettings)
            {
                case 1:
                    rbNonNormalUnicode2.Checked = true;
                    break;

                case 2:
                    rbNewBttF.Checked = true;
                    break;

                default:
                    rbNormalUnicode.Checked = true;
                    break;
            }

            checkBoxSortStrings.Checked = MainMenu.settings.sortSameString;
            clearMessagesCB.Checked = MainMenu.settings.clearMessages;
            checkBoxD3DTX_after_import.Checked = MainMenu.settings.deleteD3DTXafterImport;
            checkBoxDDS_after_import.Checked = MainMenu.settings.deleteDDSafterImport;
            checkBoxExportRealID.Checked = MainMenu.settings.exportRealID;
            checkBoxImportingOfNames.Checked = MainMenu.settings.importingOfName;
            cbIgnoreEmptyStrings.Checked = MainMenu.settings.ignoreEmptyStrings;

            textBoxInputFolder.Text = MainMenu.settings.pathForInputFolder;
            textBoxOutputFolder.Text = MainMenu.settings.pathForOutputFolder;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(textBoxInputFolder.Text)) MainMenu.settings.pathForInputFolder = textBoxInputFolder.Text;
            if (Directory.Exists(textBoxOutputFolder.Text)) MainMenu.settings.pathForOutputFolder = textBoxOutputFolder.Text;

            MainMenu.settings.clearMessages = clearMessagesCB.Checked;
            MainMenu.settings.sortSameString = checkBoxSortStrings.Checked;
            MainMenu.settings.deleteD3DTXafterImport = checkBoxD3DTX_after_import.Checked;
            MainMenu.settings.deleteDDSafterImport = checkBoxDDS_after_import.Checked;
            MainMenu.settings.exportRealID = checkBoxExportRealID.Checked;
            MainMenu.settings.importingOfName = checkBoxImportingOfNames.Checked;
            MainMenu.settings.changeLangFlags = checkBoxChangeLangFlags.Checked;
            MainMenu.settings.ignoreEmptyStrings = cbIgnoreEmptyStrings.Checked;

            if (rbNormalUnicode.Checked) MainMenu.settings.unicodeSettings = 0;
            else if (rbNonNormalUnicode2.Checked) MainMenu.settings.unicodeSettings = 1;
            else MainMenu.settings.unicodeSettings = 2;

            if (tsvFilesRB.Checked)
            {
                MainMenu.settings.tsvFormat = true;
                MainMenu.settings.newTxtFormat = false;
            }
            else
            {
                MainMenu.settings.newTxtFormat = !txtFilesRB.Checked && newTxtFormatRB.Checked;
                MainMenu.settings.tsvFormat = false;
            }

            Settings.SaveConfig(MainMenu.settings);

            Close();
        }

        private void newTxtFormatRB_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxChangeLangFlags.Enabled = newTxtFormatRB.Checked;
            checkBoxChangeLangFlags.Visible = newTxtFormatRB.Checked;
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
    }
}
