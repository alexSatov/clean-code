using Markdown.HTML;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class HtmlWrapper_Should
    {
        [TestCase("Курсивный текст", "<em>", ExpectedResult = "<em>Курсивный текст</em>")]
        [TestCase("Жирный текст", "<strong>", ExpectedResult = "<strong>Жирный текст</strong>")]
        public string WrapText_intoHtmlTag(string text, string tag)
        {
            return HtmlWrapper.WrapToHtmlTag(text, tag);
        }
    }
}
