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

        public static string GetLanguageDisplayName(string code)
        {
            EnsureLoaded();

            if (_translations.TryGetValue(code, out var language) && language.TryGetValue("LanguageName", out var name))
            {
                return name;
            }

            return code;
        }

        public static void ApplyToFormSettings(FormSettings form)
        {
            form.Text = Get("Settings.FormTitle");
            form.SetLocalizedTexts(
                Get("Settings.AsciiLabel"),
                Get("Settings.ApplyAndExitButton"),
                Get("Settings.ApplyButton"),
                Get("Settings.ExitButton"),
                Get("Settings.InputFolderButton"),
                Get("Settings.OutputFolderButton"),
                Get("Settings.PathsGroup"),
                Get("Settings.DetectLanguageCheckbox"),
                Get("Settings.InterfaceLanguageLabel"),
                Get("Settings.UnicodeGroup"),
                Get("Settings.NormalUnicode"),
                Get("Settings.NonNormalUnicode"),
                Get("Settings.NewBttFUnicode"),
                Get("Settings.NormalUnicodeTooltip"),
                Get("Settings.NonNormalUnicodeTooltip"),
                Get("Settings.NewBttFUnicodeTooltip"));
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
