namespace _Source.Services.Texts.Utility
{
    public static class TagReplacer
    {
        public static string ReplaceTags(this string text)
        {
            return text
                .ReplaceColorTags()
                .ReplaceSpriteTags();
        }
    }
}
