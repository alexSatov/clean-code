using Markdown.MD;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Markdown_Should
    {
        private readonly Md markdownProcessor = new Md();

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

        public string ProcessText(string text)
        {
            return markdownProcessor.RenderToHtml(text);
        }
    }
}
