using System.Collections.Generic;

namespace _Source.Services.Texts.Utility
{
    public static class InlineColors
    {
        public const string Blue = "#2EB9F7";
        public const string Red = "#E83B3B";
        public const string Green = "#20C944";

        private const string ClosingTag = "<C>";
        private const string TMPClosingTag = "</color>";

        public static readonly IReadOnlyDictionary<string, string> ColorTags = new Dictionary<string, string>()
        {
            {"<C_BLUE>", Blue},
            {"<C_RED>", Red},
            {"<C_GREEN>", Green}
        };

        public static string ReplaceColorTags(this string text)
        {
            foreach (var key in ColorTags.Keys)
            {
                text = text.Replace(key, TMPColorTag(ColorTags[key]));
            }

            return text.Replace(ClosingTag, TMPClosingTag);
        }

        private static string TMPColorTag(string color)
        {
            return $"<color={color}>";
        }
    }
}
