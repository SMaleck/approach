using _Source.App;
using _Source.Features.Tutorials;
using _Source.Services.Texts.Utility;
using System;
using System.Globalization;
using UnityEngine;

namespace _Source.Services.Texts
{
    public static partial class TextService
    {
        public static Language CurrentLanguage { get; private set; }

        static TextService()
        {
            var storedLanguage = PlayerPrefs.GetInt(Constants.PrefsKeyLanguage);

            var isStoredLanguageValid = Enum.IsDefined(typeof(Language), storedLanguage);
            if (isStoredLanguageValid)
            {
                CurrentLanguage = (Language)storedLanguage;
            }
            else
            {
                SetLanguage(Language.English);
            }
        }

        public static void SetLanguage(Language language)
        {
            PlayerPrefs.SetInt(Constants.PrefsKeyLanguage, (int)language);
            CurrentLanguage = language;
        }

        public static string TimeFromSeconds(double seconds)
        {
            var flooredSeconds = Math.Floor(seconds);
            return TimeSpan.FromSeconds(flooredSeconds)
                .ToString(@"m\:ss", CultureInfo.InvariantCulture);
        }

        public static string HealthAmount(double amount)
        {
            return Math.Floor(amount).ToString();
        }

        public static string StartGame()
        {
            return TextData.GetText(TextKey.StartGame);
        }

        public static string HowToPlay()
        {
            return TextData.GetText(TextKey.HowToPlay);
        }

        public static string Settings()
        {
            return TextData.GetText(TextKey.Settings);
        }

        public static string LanguageName(Language language)
        {
            return TextData.GetLanguageText(language);
        }

        public static string HowToControls()
        {
            return TextData.GetText(TextKey.HowToPlayControls)
                .ReplaceTags();
        }

        public static string HowToStep1()
        {
            return TextData.GetText(TextKey.HowToPlayStep1)
                .ReplaceTags();
        }

        public static string HowToStep2()
        {
            return TextData.GetText(TextKey.HowToPlayStep2)
                .ReplaceTags();
        }

        public static string HowToStep3()
        {
            return TextData.GetText(TextKey.HowToPlayStep3)
                .ReplaceTags();
        }

        public static string Pause()
        {
            return TextData.GetText(TextKey.Pause);
        }

        public static string Restart()
        {
            return TextData.GetText(TextKey.Restart);
        }

        public static string ExitToMenu()
        {
            return TextData.GetText(TextKey.ExitToMenu);
        }

        public static string End()
        {
            return TextData.GetText(TextKey.End);
        }

        public static string TutorialDescription(TutorialId id)
        {
            switch (id)
            {
                case TutorialId.Controls:
                    return HowToControls();

                case TutorialId.Life:
                    return TextData.GetText(TextKey.TutorialLife)
                        .ReplaceTags();

                case TutorialId.Novatars:
                    return TextData.GetText(TextKey.TutorialNovatars)
                        .ReplaceTags();

                default:
                    throw new ArgumentOutOfRangeException(nameof(id), id, null);
            }
        }

        public static string ResultFriendsAndEnemies(int friendsCount, int enemiesCount)
        {
            return TextData.GetText(TextKey.ResultFriendsAndEnemies, friendsCount, enemiesCount)
                .ReplaceTags();
        }

        public static string ResultOnlyFriends(int count)
        {
            return TextData.GetText(TextKey.ResultOnlyFriends, count)
                .ReplaceTags();
        }

        public static string ResultOnlyEnemies(int count)
        {
            return TextData.GetText(TextKey.ResultOnlyEnemies, count)
                .ReplaceTags();
        }

        public static string ResultFriendsLost(int count)
        {
            return TextData.GetText(TextKey.ResultFriendsLost, count)
                .ReplaceTags();
        }

        public static string ResultOnlyNeutral()
        {
            return TextData.GetText(TextKey.ResultOnlyNeutral)
                .ReplaceTags();
        }

        public static string ResultNobody()
        {
            return TextData.GetText(TextKey.ResultNobody)
                .ReplaceTags();
        }

        public static string ResultNeutral()
        {
            return TextData.GetText(TextKey.ResultNeutral)
                .ReplaceTags();
        }
    }
}
