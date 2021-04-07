using System.Collections.Generic;

namespace _Source.Services.Texts.Utility
{
    public static class InlineSprites
    {
        public static readonly string Key_Arrow_Up = Build("Key_Arrow_Up");
        public static readonly string Key_Arrow_Down = Build("Key_Arrow_Down");
        public static readonly string Key_Arrow_Left = Build("Key_Arrow_Left");
        public static readonly string Key_Arrow_Right = Build("Key_Arrow_Right");
        public static readonly string Mouse_Left = Build("Mouse_Left");

        private const string InlineSpriteSheetName = "InlineSprites";

        private static readonly IReadOnlyDictionary<string, string> SpriteTags = new Dictionary<string, string>()
        {
            {"<S_K_UP>", Key_Arrow_Up},
            {"<S_K_DOWN>", Key_Arrow_Down},
            {"<S_K_LEFT>", Key_Arrow_Left},
            {"<S_K_RIGHT>", Key_Arrow_Right},
            {"<S_MOUSE_LEFT>", Mouse_Left},
        };

        public static string ReplaceSpriteTags(this string text)
        {
            foreach (var key in SpriteTags.Keys)
            {
                text = text.Replace(key, SpriteTags[key]);
            }

            return text;
        }

        private static string Build(string name)
        {
            return $"<sprite name=\"{name}\">";
        }
    }
}
