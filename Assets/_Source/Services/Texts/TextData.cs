using System;
using System.Collections.Generic;

namespace _Source.Services.Texts
{
    public static partial class TextService
    {
        private static class TextData
        {
            public static string GetText(TextKey textKey)
            {
                switch (CurrentLanguage)
                {
                    case Language.English:
                        return _textsEN[textKey];

                    case Language.German:
                        return _textsDE[textKey];

                    default:
                        throw new ArgumentOutOfRangeException(nameof(CurrentLanguage), CurrentLanguage, null);
                }
            }

            public static string GetLanguageText(Language languageKey)
            {
                return _textsLANGUAGE[languageKey];
            }

            private static Dictionary<Language, string> _textsLANGUAGE = new Dictionary<Language, string>
            {
                { Language.English, "English"},
                { Language.German, "Deutsch"}
            };

            private static Dictionary<TextKey, string> _textsEN = new Dictionary<TextKey, string>
            {
                { TextKey.StartGame, "Start"},
                { TextKey.Settings, "Settings"},
                { TextKey.HowToPlay, "How to Play"},
                { TextKey.HowToPlayDescription, "#TUTORIAL#"}
            };

            private static Dictionary<TextKey, string> _textsDE = new Dictionary<TextKey, string>
            {
                { TextKey.StartGame, "Start"},
                { TextKey.Settings, "Einstellungen"},
                { TextKey.HowToPlay, "Anleitung"},
                { TextKey.HowToPlayDescription, "#ANLEITUNG#"}
            };
        }
    }
}
