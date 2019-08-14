using _Source.App;
using System;
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
            return TimeSpan.FromSeconds(flooredSeconds).ToString("c");
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
    }
}
