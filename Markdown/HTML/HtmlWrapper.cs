namespace Markdown.HTML
{
    public static class HtmlWrapper
    {
        public static string WrapToHtmlTag(string text, string tag, string cssClass = "")
        {
            var closeTag = GetCloseTag(tag);
            tag = TryAddCssClassToTag(tag, cssClass);
            return tag + text + closeTag;
        }

        public static string GetCloseTag(string tag)
        {
            return tag.Insert(1, "/");
        }

        public static string TryAddCssClassToTag(string tag, string cssClass = "")
        {
            if (cssClass != "")
                tag = tag.Insert(tag.Length - 1, $" class=\"{cssClass}\"");
            return tag;
        }

        public static string WrapToUrlTag(string url, string text, string cssClass = "")
        {
            var tag = TryAddCssClassToTag($"<a href=\"{url}\">", cssClass);
            return $"{tag}{text}</a>";
        }
    }
}
