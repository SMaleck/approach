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
                { TextKey.TutorialStepOne, "You are the blue cube.\nYou can move around using the arrow keys."},
                { TextKey.TutorialStepTwo, "There are others as well.\nThey might interact when you approach them."},
                { TextKey.Pause, "Pause"},
                { TextKey.Restart, "Restart"},
                { TextKey.ExitToMenu, "Exit to Menu"}
            };

            private static Dictionary<TextKey, string> _textsDE = new Dictionary<TextKey, string>
            {
                { TextKey.StartGame, "Start"},
                { TextKey.Settings, "Einstellungen"},
                { TextKey.HowToPlay, "Anleitung"},
                { TextKey.TutorialStepOne, "Du bist der blaue Würfel.\nMit den Pfeiltasten kannst du dich bewegen."},
                { TextKey.TutorialStepTwo, "Es gibt auch andere Würfel.\nSie werden mit dir interagieren, wenn du dich näherst."},
                { TextKey.Pause, "Pause"},
                { TextKey.Restart, "Neustarten"},
                { TextKey.ExitToMenu, "Zurück zum Menü"}
            };
        }
    }
}
