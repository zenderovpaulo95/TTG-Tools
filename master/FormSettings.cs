using System;
using System.Windows.Forms;
using System.IO;

namespace TTG_Tools
{
    public partial class FormSettings : Form
    {
        private bool _loadingUiLanguageList;

        private class LanguageOption
        {
            public string Code { get; set; }
            public string DisplayName { get; set; }

            public override string ToString()
            {
                return DisplayName;
            }
        }

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

            LanguageOption selectedUiLanguage = interfaceLanguageComboBox.SelectedItem as LanguageOption;
            MainMenu.settings.uiLanguageCode = selectedUiLanguage != null ? selectedUiLanguage.Code : "en";

            UiLocalizer.Initialize(MainMenu.settings.uiLanguageCode);
            UiLocalizer.RefreshOpenForms();

            if (rbNormalUnicode.Checked == true) MainMenu.settings.unicodeSettings = 0;
            else if (rbNonNormalUnicode2.Checked == true) MainMenu.settings.unicodeSettings = 1;
            else MainMenu.settings.unicodeSettings = 2;

            MainMenu.settings.languageIndex = -1;
            if (checkLanguage.Checked)
            {
                MainMenu.settings.languageIndex = languageComboBox.SelectedIndex;

                string selectedLanguage = languageComboBox.Text;

                // Check if text contains parentheses to extract ASCII code
                if (selectedLanguage.Contains("(") && selectedLanguage.Contains(")"))
                {
                    int start = selectedLanguage.IndexOf("(") + 1;
                    int end = selectedLanguage.IndexOf(")");

                    if (start < end && start > 0)
                    {
                        string str_num = selectedLanguage.Substring(start, end - start).Trim();

                        if (int.TryParse(str_num, out int asciiValue) && asciiValue > 0)
                        {
                            MainMenu.settings.ASCII_N = asciiValue;
                        }
                        else
                        {
                            // Fallback to language name mapping
                            ApplyLanguageASCIIDefault(selectedLanguage);
                        }
                    }
                    else
                    {
                        ApplyLanguageASCIIDefault(selectedLanguage);
                    }
                }
                else
                {
                    // If no parentheses, use name mapping
                    ApplyLanguageASCIIDefault(selectedLanguage);
                }

                numericUpDownASCII.Value = MainMenu.settings.ASCII_N;
            }

            if (((MainMenu.settings.pathForInputFolder != "") && (Directory.Exists(MainMenu.settings.pathForInputFolder)))
                && ((MainMenu.settings.pathForOutputFolder != "") && (Directory.Exists(MainMenu.settings.pathForOutputFolder))))
            {
                Settings.SaveConfig(MainMenu.settings);

                if (Program.FirstTime)
                {
                    MessageBox.Show(UiLocalizer.Get("Settings.RestartMessage"));
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show(UiLocalizer.Get("Settings.InvalidPathsMessage"));
            }
        }

        private void ApplyLanguageASCIIDefault(string languageName)
        {
            // Remove any content in parentheses to get only the language name
            if (languageName.Contains("("))
            {
                languageName = languageName.Substring(0, languageName.IndexOf("(")).Trim();
            }

            switch (languageName)
            {
                case "Thai":
                    MainMenu.settings.ASCII_N = 874;
                    break;

                case "Czech":
                case "Polish":
                case "Slovak":
                case "Hungarian":
                case "Serbo-Croatian":
                case "Montenegrin":
                case "Gagauz":
                    MainMenu.settings.ASCII_N = 1250;
                    break;

                case "Belarusian":
                case "Bulgarian":
                case "Macedonian":
                case "Russian":
                case "Rusyn":
                case "Ukrainian":
                    MainMenu.settings.ASCII_N = 1251;
                    break;

                case "Basque":
                case "Catalan":
                case "Faroese":
                case "Occitan":
                case "Romansh":
                case "Swahili":
                    MainMenu.settings.ASCII_N = 1252;
                    break;

                case "Dutch":
                case "Greek":
                    MainMenu.settings.ASCII_N = 1253;
                    break;

                case "Turkish":
                    MainMenu.settings.ASCII_N = 1254;
                    break;

                case "Hebrew":
                    MainMenu.settings.ASCII_N = 1255;
                    break;

                case "Arabic":
                case "Persian":
                case "Urdu":
                    MainMenu.settings.ASCII_N = 1256;
                    break;

                case "Latvian":
                case "Lithuanian":
                case "Latgalian":
                case "Icelandic":
                    MainMenu.settings.ASCII_N = 1257;
                    break;

                case "Vietnamese":
                    MainMenu.settings.ASCII_N = 1258;
                    break;

                default:
                    MainMenu.settings.ASCII_N = 1252; // Safe default value
                    break;
            }
        }

        private void buttonOkSettings_Click(object sender, EventArgs e)
        {
            buttonSaveSettings_Click(sender, e);
            if(!Program.FirstTime) this.Close();
        }

        private void buttonCloseSettingsForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numericUpDownASCII_ValueChanged(object sender, EventArgs e)
        {
            int asciiValue = (int)numericUpDownASCII.Value;

            switch (asciiValue)
            {
                case 873:
                    numericUpDownASCII.Value = 874;
                    break;
                case 875:
                    numericUpDownASCII.Value = 1250;
                    break;
                case 1249:
                    numericUpDownASCII.Value = 874;
                    break;
                case 1259:
                    numericUpDownASCII.Value = 1258;
                    break;
            }

            // Terrible fix for users windows-1252 encoding
            if ((int)numericUpDownASCII.Value == 1252)
            {
                if(rbNonNormalUnicode2.Checked) rbNormalUnicode.Checked = true;
                rbNonNormalUnicode2.Enabled = false;
            }
            else
            {
                rbNonNormalUnicode2.Enabled = true;

                switch (MainMenu.settings.unicodeSettings)
                {
                    case 0:
                        rbNormalUnicode.Checked = true;
                        break;

                    case 1:
                        rbNonNormalUnicode2.Checked = true;
                        break;

                    case 2:
                        rbNewBttF.Checked = true;
                        break;
                }
            }
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            numericUpDownASCII.Value = MainMenu.settings.ASCII_N;
            textBoxInputFolder.Text = MainMenu.settings.pathForInputFolder;
            textBoxOutputFolder.Text = MainMenu.settings.pathForOutputFolder;

            buttonSaveSettings.Enabled = !Program.FirstTime;

            foreach(string lang in MainMenu.languagesASCII)
            {
                languageComboBox.Items.Add(lang);
            }

            checkLanguage.Checked = MainMenu.settings.languageIndex != -1;
            languageComboBox.Enabled = MainMenu.settings.languageIndex != -1;
            languageComboBox.SelectedIndex = MainMenu.settings.languageIndex != -1 ? languageComboBox.SelectedIndex = MainMenu.settings.languageIndex : 0;

            _loadingUiLanguageList = true;
            interfaceLanguageComboBox.Items.Clear();
            foreach (string code in UiLocalizer.AvailableLanguages)
            {
                interfaceLanguageComboBox.Items.Add(new LanguageOption
                {
                    Code = code,
                    DisplayName = UiLocalizer.GetLanguageDisplayName(code)
                });
            }

            string currentLanguage = string.IsNullOrWhiteSpace(MainMenu.settings.uiLanguageCode) ? UiLocalizer.CurrentLanguageCode : MainMenu.settings.uiLanguageCode;
            for (int i = 0; i < interfaceLanguageComboBox.Items.Count; i++)
            {
                LanguageOption option = interfaceLanguageComboBox.Items[i] as LanguageOption;
                if (option != null && option.Code.Equals(currentLanguage, StringComparison.OrdinalIgnoreCase))
                {
                    interfaceLanguageComboBox.SelectedIndex = i;
                    break;
                }
            }
            if (interfaceLanguageComboBox.SelectedIndex == -1 && interfaceLanguageComboBox.Items.Count > 0)
            {
                interfaceLanguageComboBox.SelectedIndex = 0;
            }
            _loadingUiLanguageList = false;

            switch (MainMenu.settings.unicodeSettings)
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

            UiLocalizer.ApplyToFormSettings(this);
        }

        private void buttonInputFolder_Click(object sender, EventArgs e)
        {
            textBoxInputFolder.Text = SetFolder(textBoxInputFolder.Text);
        }

        private void buttonOutputFolder_Click(object sender, EventArgs e)
        {
            textBoxOutputFolder.Text = SetFolder(textBoxOutputFolder.Text);
        }

        private void checkLanguage_CheckedChanged(object sender, EventArgs e)
        {
            languageComboBox.SelectedIndex = 0;
            languageComboBox.Enabled = checkLanguage.Checked;
        }


        private void interfaceLanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_loadingUiLanguageList)
            {
                return;
            }

            LanguageOption selectedUiLanguage = interfaceLanguageComboBox.SelectedItem as LanguageOption;
            string selectedCode = selectedUiLanguage != null ? selectedUiLanguage.Code : "en";

            UiLocalizer.Initialize(selectedCode);
            UiLocalizer.RefreshOpenForms();
        }

        public string GetAsciiLabelText() { return "ASCII"; }
        public string GetApplyAndExitButtonText() { return "Apply and Exit"; }
        public string GetApplyButtonText() { return "Apply"; }
        public string GetExitButtonText() { return "Exit"; }
        public string GetInputFolderButtonText() { return "Input Folder"; }
        public string GetOutputFolderButtonText() { return "Output Folder"; }
        public string GetPathsGroupText() { return "Auto(De)Packer file paths:"; }
        public string GetDetectLanguageCheckboxText() { return "I don't know ASCII code for my language!"; }
        public string GetInterfaceLanguageLabelText() { return "Interface language:"; }
        public string GetUnicodeGroupText() { return "Unicode mode:"; }
        public string GetNormalUnicodeText() { return "Normal Unicode"; }
        public string GetNonNormalUnicodeText() { return "Non normal Unicode"; }
        public string GetNewBttFUnicodeText() { return "New BttF Unicode"; }
        public string GetNormalUnicodeTooltipText() { return "Recommend to use in new games (From\r\nMinecraft: Story Mode). This option could help\r\nto export/import text files from a new game and\r\nremake fonts with support of your symbols."; }
        public string GetNonNormalUnicodeTooltipText() { return "Recommend to use in old and some new games (Until\r\nBatman: A Telltale Series). This option could help\r\nto export/import text files from old games and\r\nremake fonts with support of your symbols."; }
        public string GetNewBttFUnicodeTooltipText() { return "Support all symbols from all modern languages.\r\nRecommend for new version TftB."; }

        public void SetLocalizedTexts(
            string asciiLabel,
            string applyAndExit,
            string apply,
            string exit,
            string inputFolder,
            string outputFolder,
            string pathsGroup,
            string detectLanguageCheckbox,
            string interfaceLanguageLabel,
            string unicodeGroup,
            string normalUnicode,
            string nonNormalUnicode,
            string newBttFUnicode,
            string normalUnicodeTooltip,
            string nonNormalUnicodeTooltip,
            string newBttFUnicodeTooltip)
        {
            label1.Text = asciiLabel;
            buttonApplyAndExitSettings.Text = applyAndExit;
            buttonSaveSettings.Text = apply;
            buttonExitSettingsForm.Text = exit;
            buttonInputFolder.Text = inputFolder;
            buttonOutputFolder.Text = outputFolder;
            groupBox1.Text = pathsGroup;
            checkLanguage.Text = detectLanguageCheckbox;
            labelInterfaceLanguage.Text = interfaceLanguageLabel;
            groupBox2.Text = unicodeGroup;
            rbNormalUnicode.Text = normalUnicode;
            rbNonNormalUnicode2.Text = nonNormalUnicode;
            rbNewBttF.Text = newBttFUnicode;
            toolTip1.SetToolTip(rbNormalUnicode, normalUnicodeTooltip);
            toolTip2.SetToolTip(rbNonNormalUnicode2, nonNormalUnicodeTooltip);
            toolTip3.SetToolTip(rbNewBttF, newBttFUnicodeTooltip);
        }
    }
}
