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

    }
}
