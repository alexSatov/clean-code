namespace Markdown.HTML
{
    public static class HtmlWrapper
    {
        public static string WrapToHtmlTag(string text, string tag)
        {
            return tag + text + GetCloseTag(tag);
        }

        public static string GetCloseTag(string tag)
        {
            return tag.Insert(1, "/");
        }
    }
}
