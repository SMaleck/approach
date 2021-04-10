using System;
using System.Collections.Generic;

namespace _Source.Services.Texts
{
    public static partial class TextService
    {
        private const Language FallBackLang = Language.English;

        private static class TextData
        {
            public static string GetText(TextKey textKey, params object[] args)
            {
                if (!TryGetTerm(CurrentLanguage, textKey, out var term))
                {
                    TryGetTerm(FallBackLang, textKey, out term);
                }

                return string.Format(term, args);
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

            private static bool TryGetTerm(Language lang, TextKey key, out string term)
            {
                return GetTerms(lang)
                    .TryGetValue(key, out term);
            }

            private static IReadOnlyDictionary<TextKey, string> GetTerms(Language lang)
            {
                switch (lang)
                {
                    case Language.English:
                        return _textsEN;

                    case Language.German:
                        return _textsDE;

                    default:
                        throw new ArgumentOutOfRangeException($"No TermRepo defined for {lang}");
                }
            }

            private static IReadOnlyDictionary<TextKey, string> _textsEN = new Dictionary<TextKey, string>
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
                { TextKey.End, "The End"},
                { TextKey.TutorialLife, "This is your <C_BLUE>life<C>. It depletes over time and goes down more, whenever you meet <C_RED>enemies<C>."},
                { TextKey.TutorialNovatars, "This is one of many others. You can <C_BLUE>approach<C> them, or not, it is up to you. They might be <C_GREEN>friendly<C>, or <C_RED>maybe not<C>."},
                { TextKey.ResultFriendsAndEnemies, "On your journey you have made <C_GREEN>{0} friends<C> and <C_RED>{1} enemies<C>."},
                { TextKey.ResultOnlyFriends, "You had a lucky journey and made <C_GREEN>{0} friends<C> along the way."},
                { TextKey.ResultOnlyEnemies, "You had little luck with the others and made <C_RED>{0} enemies<C> along the way."},
                { TextKey.ResultOnlyNeutral, "You met some others, but no bond was formed in any way."},
                { TextKey.ResultNobody, "Your were very cautious and avoided everybody, so you made no enemies, but also no friends."},
                { TextKey.ResultFriendsLost, "But you also lost <C_GREEN>{0} of your friends<C> again."},
                { TextKey.ResultNeutral, "There were also some that did not care for you either way."},
            };

            private static IReadOnlyDictionary<TextKey, string> _textsDE = new Dictionary<TextKey, string>
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
                { TextKey.End, "Ende"},
                { TextKey.TutorialLife, "Das ist dein <C_BLUE>Leben<C>. Es vergeht mit der Zeit und sinkt schneller, wenn du auf <C_RED>Feinde<C> triffst."},
                { TextKey.TutorialNovatars, "Dies ist einer von vielen anderen. Du bestimmst ob du dich ihnen <C_BLUE>näherst<C>, oder nicht. Sie können <C_GREEN>freundlich<C> sein, oder auch <C_RED>nicht<C>."},
                { TextKey.ResultFriendsAndEnemies, "Du hast <C_GREEN>{0} Freunde<C> und <C_RED>{1} Feinde<C> auf deinem Weg getroffen."},
                { TextKey.ResultOnlyFriends, "Du hattest Glück und hast <C_GREEN>{0} Freunde<C> auf deinem Weg getroffen."},
                { TextKey.ResultOnlyEnemies, "Du hattest wenig Glück mit den anderen und hast <C_RED>{0} Feinde<C> auf deinem Weg getroffen."},
                { TextKey.ResultOnlyNeutral, "Du bist an andere herangetreten, aber konntest keine Bindung aufbauen."},
                { TextKey.ResultNobody, "Du warst sehr vorsichtig unterwegs und hast alle gemieden. So hast du keine Feinde getroffen, aber auch keine Freunde."},
                { TextKey.ResultFriendsLost, "Aber du hast <C_GREEN>{0} Freunde<C> auch wieder verloren."},
                { TextKey.ResultNeutral, "Es gab auch welche, denen du egal warst."},
            };
        }
    }
}
