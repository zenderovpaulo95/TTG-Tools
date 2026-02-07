using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Windows.Forms;

namespace TTG_Tools
{
    public static class UiLocalizer
    {
        private const string DefaultLanguageCode = "en";
        private static readonly Dictionary<string, Dictionary<string, string>> _translations =
            new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);

        private static bool _loaded;

        public static IReadOnlyList<string> AvailableLanguages
        {
            get { EnsureLoaded(); return _translations.Keys.OrderBy(x => x).ToList(); }
        }

        public static string CurrentLanguageCode { get; private set; } = DefaultLanguageCode;

        public static void Initialize(string languageCode)
        {
            EnsureLoaded();

            if (!string.IsNullOrWhiteSpace(languageCode) && _translations.ContainsKey(languageCode))
            {
                CurrentLanguageCode = languageCode;
            }
            else
            {
                CurrentLanguageCode = DefaultLanguageCode;
            }
        }

        public static string Get(string key)
        {
            EnsureLoaded();

            if (_translations.TryGetValue(CurrentLanguageCode, out var selectedLanguage) && selectedLanguage.TryGetValue(key, out var text))
            {
                return text;
            }

            if (_translations.TryGetValue(DefaultLanguageCode, out var fallbackLanguage) && fallbackLanguage.TryGetValue(key, out var fallbackText))
            {
                return fallbackText;
            }

            return key;
        }

        public static string GetOrDefault(string key, string defaultText)
        {
            string localized = Get(key);
            return localized == key ? defaultText : localized;
        }

        public static string GetLanguageDisplayName(string code)
        {
            EnsureLoaded();

            if (_translations.TryGetValue(code, out var language) && language.TryGetValue("LanguageName", out var name))
            {
                return name;
            }

            return code;
        }

        public static void ApplyToForm(Form form)
        {
            if (form == null)
            {
                return;
            }

            string formKey = form.Name + ".Text";
            form.Text = GetOrDefault(formKey, form.Text);

            ApplyToControlCollection(form, form.Controls);

            if (form is MainMenu mainMenu)
            {
                mainMenu.ApplyLocalizedNonControlTexts();
            }
        }

        public static void RefreshOpenForms()
        {
            List<Form> forms = new List<Form>();
            foreach (Form form in Application.OpenForms)
            {
                forms.Add(form);
            }

            foreach (Form form in forms)
            {
                if (form is FormSettings settingsForm)
                {
                    ApplyToFormSettings(settingsForm);
                }
                else
                {
                    ApplyToForm(form);
                }
            }
        }

        public static void ApplyToFormSettings(FormSettings form)
        {
            ApplyToForm(form);

            form.SetLocalizedTexts(
                GetOrDefault("FormSettings.label1.Text", form.GetAsciiLabelText()),
                GetOrDefault("FormSettings.buttonApplyAndExitSettings.Text", form.GetApplyAndExitButtonText()),
                GetOrDefault("FormSettings.buttonSaveSettings.Text", form.GetApplyButtonText()),
                GetOrDefault("FormSettings.buttonExitSettingsForm.Text", form.GetExitButtonText()),
                GetOrDefault("FormSettings.buttonInputFolder.Text", form.GetInputFolderButtonText()),
                GetOrDefault("FormSettings.buttonOutputFolder.Text", form.GetOutputFolderButtonText()),
                GetOrDefault("FormSettings.groupBox1.Text", form.GetPathsGroupText()),
                GetOrDefault("FormSettings.checkLanguage.Text", form.GetDetectLanguageCheckboxText()),
                GetOrDefault("FormSettings.labelInterfaceLanguage.Text", form.GetInterfaceLanguageLabelText()),
                GetOrDefault("FormSettings.groupBox2.Text", form.GetUnicodeGroupText()),
                GetOrDefault("FormSettings.rbNormalUnicode.Text", form.GetNormalUnicodeText()),
                GetOrDefault("FormSettings.rbNonNormalUnicode2.Text", form.GetNonNormalUnicodeText()),
                GetOrDefault("FormSettings.rbNewBttF.Text", form.GetNewBttFUnicodeText()),
                GetOrDefault("FormSettings.rbNormalUnicode.ToolTip", form.GetNormalUnicodeTooltipText()),
                GetOrDefault("FormSettings.rbNonNormalUnicode2.ToolTip", form.GetNonNormalUnicodeTooltipText()),
                GetOrDefault("FormSettings.rbNewBttF.ToolTip", form.GetNewBttFUnicodeTooltipText()));
        }

        private static void ApplyToControlCollection(Form form, Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                string key = form.Name + "." + control.Name + ".Text";
                control.Text = GetOrDefault(key, control.Text);

                if (control.HasChildren)
                {
                    ApplyToControlCollection(form, control.Controls);
                }
            }
        }

        private static void EnsureLoaded()
        {
            if (_loaded)
            {
                return;
            }

            string translationPath = Path.Combine(Application.StartupPath, "Translations", "ui-translations.xml");
            if (!File.Exists(translationPath))
            {
                translationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Translations", "ui-translations.xml");
            }

            if (!File.Exists(translationPath))
            {
                _translations[DefaultLanguageCode] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                _loaded = true;
                return;
            }

            XDocument document = XDocument.Load(translationPath);
            foreach (XElement languageElement in document.Root.Elements("language"))
            {
                XAttribute codeAttribute = languageElement.Attribute("code");
                if (codeAttribute == null || string.IsNullOrWhiteSpace(codeAttribute.Value))
                {
                    continue;
                }

                Dictionary<string, string> entries = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (XElement entryElement in languageElement.Elements("entry"))
                {
                    XAttribute keyAttribute = entryElement.Attribute("key");
                    if (keyAttribute == null || string.IsNullOrWhiteSpace(keyAttribute.Value))
                    {
                        continue;
                    }

                    entries[keyAttribute.Value] = entryElement.Value;
                }

                _translations[codeAttribute.Value] = entries;
            }

            if (!_translations.ContainsKey(DefaultLanguageCode))
            {
                _translations[DefaultLanguageCode] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            _loaded = true;
        }
    }
}
