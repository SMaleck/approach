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
                { TextKey.HowToPlayControls, "Use the arrow keys <S_K_UP> <S_K_DOWN> <S_K_LEFT> <S_K_RIGHT> or hold left mouse & drag <S_MOUSE_LEFT> to move around"},
                { TextKey.HowToPlayStep1, "You are the one with the <C_BLUE>blue<C> light above its head."},
                { TextKey.HowToPlayStep2, "There are many others, as well. Coming and going as they please. Some might be <C_GREEN>friendly<C>, some <C_RED>less<C> so."},
                { TextKey.HowToPlayStep3, "It is up to you to decide if want to <C_BLUE>approach<C> or avoid them."},
                { TextKey.Pause, "Pause"},
                { TextKey.Restart, "Restart"},
                { TextKey.ExitToMenu, "Exit to Menu"},
                { TextKey.End, "The End"}
            };

            private static Dictionary<TextKey, string> _textsDE = new Dictionary<TextKey, string>
            {
                { TextKey.StartGame, "Start"},
                { TextKey.Settings, "Einstellungen"},
                { TextKey.HowToPlay, "Anleitung"},
                { TextKey.HowToPlayControls, "Benutze die Pfeiltasten <S_K_UP> <S_K_DOWN> <S_K_LEFT> <S_K_RIGHT> or halte die linke Maustatse & ziehe die Maus <S_MOUSE_LEFT>, um dich zu bewegen."},
                { TextKey.HowToPlayStep1, "Du bist der mit dem <C_BLUE>blauen<C> Licht über dem Kopf."},
                { TextKey.HowToPlayStep2, "Es gibt noch viele andere. Sie kommen un gehen, wie es ihnen gefällt. Manche <C_GREEN>freundlich<C> und manche <C_RED>vielleicht nicht<C>."},
                { TextKey.HowToPlayStep3, "Es liegt an dir, ob du dich ihnen <C_BLUE>näherst<C>, oder sie doch eher meidest."},
                { TextKey.Pause, "Pause"},
                { TextKey.Restart, "Neustarten"},
                { TextKey.ExitToMenu, "Zurück zum Menü"},
                { TextKey.End, "Ende"}
            };
        }
    }
}
