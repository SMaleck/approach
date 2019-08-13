using System;

namespace _Source.Services.Texts
{
    public static class TextService
    {
        public static string TimeFromSeconds(double seconds)
        {
            var flooredSeconds = Math.Floor(seconds);
            return TimeSpan.FromSeconds(flooredSeconds).ToString("c");
        }

        public static string HealthAmount(double amount)
        {
            return Math.Floor(amount).ToString();
        }
    }
}
