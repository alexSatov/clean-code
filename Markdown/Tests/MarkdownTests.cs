using Markdown.MD;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;

namespace Markdown.Tests
{
    [TestFixture]
    public class Markdown_Should
    {
        private readonly Md markdownProcessor = new Md("http://example.net/");
        private readonly Md mdProcessorWithCssClass = new Md("", "example");

        [TestCase("Текст без маркеров", ExpectedResult = "Текст без маркеров", TestName = "TextWithoutMarkers")]
        [TestCase("Текст с одним _подчерком", ExpectedResult = "Текст с одним _подчерком", TestName = "TextWithOpenMarkerSymbol")]
        [TestCase("Текст с двумя __подчерками", ExpectedResult = "Текст с двумя __подчерками", TestName = "TextWithDoubleOpenMarkerSymbol")]
        [TestCase("Текст с _курсивным маркером_ ", ExpectedResult = "Текст с <em>курсивным маркером</em> ", TestName = "TextWithEmMarker")]
        [TestCase("Текст с __жирным маркером__", ExpectedResult = "Текст с <strong>жирным маркером</strong>", TestName = "TextWithStrongMarker")]
        [TestCase("Маркер в_нутр_и текста не работает", ExpectedResult = "Маркер в_нутр_и текста не работает", TestName = "MarkerInsideText")]
        [TestCase("Маркер внутри текста c цифрами_123_ не работает", ExpectedResult = "Маркер внутри текста c цифрами_123_ не работает", TestName = "MarkerInsideTextWithNumbers")]
        [TestCase("Маркеры __ без _  _ текста __  __ не работают", ExpectedResult = "Маркеры __ без _  _ текста __  __ не работают", TestName = "MarkersWithoutText")]
        [TestCase("__Непарные символы_ не являются маркером", ExpectedResult = "__Непарные символы_ не являются маркером", TestName = "TextWithUnPairSymbols")]
        [TestCase(@"\_Экранирование\_", ExpectedResult = "_Экранирование_", TestName = "TextWithEscapeCharacters")]
        [TestCase("Внутри __двойного выделения _одинарное_ тоже__ работает",
            ExpectedResult = "Внутри <strong>двойного выделения <em>одинарное</em> тоже</strong> работает",
            TestName = "TextWithEmMarkerInsideStrongMarker")]
        [TestCase("Внутри _одинарного выделения __двойное__ не_ работает",
            ExpectedResult = "Внутри <em>одинарного выделения __двойное__ не</em> работает",
            TestName = "TextWithStrongMarkerInsideEmMarker")]
        [TestCase("_ Маркер с пробелом_ после открывающего символа __и до закрывающего __ не работает",
            ExpectedResult = "_ Маркер с пробелом_ после открывающего символа __и до закрывающего __ не работает",
            TestName = "WhitespacesAfterOrBeforeMarkerSymbols")]

        [TestCase("\t_Табуляция_", ExpectedResult = "\t<em>Табуляция</em>", TestName = "TextTabulation")]
        [TestCase("_Верная_\tтабуляция", ExpectedResult = "<em>Верная</em>\tтабуляция", TestName = "TextCorrectTabulation")]
        [TestCase("_\tМаркер с табуляцией внутри\t_ не работает", ExpectedResult = "_\tМаркер с табуляцией внутри\t_ не работает", TestName = "TabulationInsideMarker")]

        [TestCase("Рабочая [ссылка](http://example.net/)", ExpectedResult = "Рабочая <a href=\"http://example.net/\">ссылка</a>", TestName = "CorrectUrl")]
        [TestCase("Рабочая [ссылка  ]\t(http://example.net/)   с разделителями", ExpectedResult = "Рабочая <a href=\"http://example.net/\">ссылка  </a>   с разделителями", TestName = "UrlWithCorrectSeparatorsProcessing")]
        [TestCase("Относительная [ссылка](/about/)", ExpectedResult = "Относительная <a href=\"http://example.net//about/\">ссылка</a>", TestName = "RelativeUrl")]
        [TestCase("Некорректная [ссылка]() _не влияет на дальшейшую обработку_", ExpectedResult = "Некорректная [ссылка]() <em>не влияет на дальшейшую обработку</em>", TestName = "UncorrectUrl1")]
        [TestCase("Некорректная [](http://example.net/) _не влияет на дальшейшую обработку_", ExpectedResult = "Некорректная [](http://example.net/) <em>не влияет на дальшейшую обработку</em>", TestName = "UncorrectUrl2")]
        [TestCase("Некорректная [ссылка] http://example.net/ _не влияет на дальшейшую обработку_", ExpectedResult = "Некорректная [ссылка] http://example.net/ <em>не влияет на дальшейшую обработку</em>", TestName = "UncorrectUrl3")]
        [TestCase("Некорректная [ссылка http://example.net/) _не влияет на дальшейшую обработку_", ExpectedResult = "Некорректная [ссылка http://example.net/) <em>не влияет на дальшейшую обработку</em>", TestName = "UncorrectUrl4")]
        public string ProcessText(string text)
        {
            return markdownProcessor.RenderToHtml(text);
        }

        [TestCase("_Курсив с css классом_", ExpectedResult = "<em class=\"example\">Курсив с css классом</em>", TestName = "CssInEmMarker")]
        [TestCase("__Жирный с css классом__", ExpectedResult = "<strong class=\"example\">Жирный с css классом</strong>", TestName = "CssInStrongMarker")]
        [TestCase("[Ссылка](http://example.net/) с css классом", ExpectedResult = "<a href=\"http://example.net/\" class=\"example\">Ссылка</a> с css классом", TestName = "CssInUrlMarker")]
        public string AddCssClass_IfClassNotEmpty(string text)
        {
            return mdProcessorWithCssClass.RenderToHtml(text);
        }

        [Test]
        public void Render_ForLinearTime()
        {
            var textPart = " _Часть_ __какого-нибудь__ __ _текста_ __";

            var firstText = CreateStringFromPart(textPart, 2500);
            var secondText = CreateStringFromPart(textPart, 10000);

            var firstTime = GetRenderingTimeInMilliseconds(firstText);
            var secondTime = GetRenderingTimeInMilliseconds(secondText);

            Assert.IsTrue(secondTime / firstTime <= 4);
        }

        private static string CreateStringFromPart(string part, int countOfParts)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < countOfParts; i++)
                stringBuilder.Append(part);
            return stringBuilder.ToString();
        }

        private long GetRenderingTimeInMilliseconds(string text)
        {
            var watch = new Stopwatch();
            watch.Start();
            markdownProcessor.RenderToHtml(text);
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }
    }
}