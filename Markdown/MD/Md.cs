using System;
using System.Linq;
using System.Text;
using Markdown.HTML;
using Markdown.Markers;

namespace Markdown.MD
{
    public class Md
    {
        private readonly StringProcessor processor;
        public string BaseUrl;
        public string CssClass;

        public Md(string baseUrl = "", string cssClass = "")
        {
            BaseUrl = baseUrl;
            CssClass = cssClass;
            var markerProcessors = GetMarkerProcessors();
            processor = new StringProcessor(markerProcessors);
        }

        public string RenderToHtml(string text)
        {
            text = TryDivideTextOnParagraphs(text);
            text = TryProcessCodeBlocks(text);
            return text;
        }

        public string TryDivideTextOnParagraphs(string text)
        {
            var paragraphs = text.Split(new[] { "\n\n" }, StringSplitOptions.None);
            var textBuilder = new StringBuilder();

            if (paragraphs.Length == 1)
                return processor.Process(text);

            for (var i = 0; i < paragraphs.Length - 1; i++)
                textBuilder.Append(HtmlWrapper.WrapToHtmlTag(processor.Process(paragraphs[i]), "<p>"));

            textBuilder.Append(processor.Process(paragraphs.Last()));
            return textBuilder.ToString();
        }

        public string TryProcessCodeBlocks(string text)
        {
            var textBuilder = new StringBuilder();
            var codeBlockBuilder = new StringBuilder();
            var codeBlockCollecting = false;

            var textLines = text.Split('\n').Where(l => l != "").Select(l => l + "\n").ToArray();
            if (text.Last() != '\n') textLines[textLines.Length - 1] = textLines.Last().Replace("\n", "");

            foreach (var line in textLines)
            {
                if (line[0] == '\t')
                    AppendLineToCodeBlock(codeBlockBuilder, line.Substring(1), ref codeBlockCollecting);

                else if (line.Substring(0, 4) == "    ")
                    AppendLineToCodeBlock(codeBlockBuilder, line.Substring(4), ref codeBlockCollecting);

                else if (codeBlockCollecting)
                {
                    codeBlockCollecting = false;
                    SetCodeBlockToText(codeBlockBuilder, textBuilder);
                    textBuilder.Append(line);
                }

                else textBuilder.Append(line);
            }

            if (codeBlockCollecting) SetCodeBlockToText(codeBlockBuilder, textBuilder);

            return textBuilder.ToString();
        }

        public BaseMarkerProcessor[] GetMarkerProcessors()
        {
            return new BaseMarkerProcessor[] {
                new BulletedListMarkerProcessor('*'), 
                new BulletedListMarkerProcessor('+'), 
                new BulletedListMarkerProcessor('-'), 
                new HeaderMarkerProcessor(1), 
                new HeaderMarkerProcessor(2), 
                new HeaderMarkerProcessor(3), 
                new HeaderMarkerProcessor(4), 
                new HeaderMarkerProcessor(5), 
                new HeaderMarkerProcessor(6), 
                new UrlMarkerProcessor(BaseUrl) {CssClass = CssClass},
                new EmMarkerProcessor {CssClass = CssClass},
                new StrongMarkerProcessor(new StringProcessor(new EmMarkerProcessor {CssClass = CssClass})) {CssClass = CssClass}
            };
        }

        private static void AppendLineToCodeBlock(StringBuilder codeBlockBuilder, string line, ref bool codeBlockCollecting )
        {
            codeBlockCollecting = true;
            codeBlockBuilder.Append(line);
        }

        private void SetCodeBlockToText(StringBuilder codeBlockBuilder, StringBuilder textBuilder)
        {
            textBuilder.Append(HtmlWrapper.WrapToHtmlTag(
                        HtmlWrapper.WrapToHtmlTag(codeBlockBuilder.ToString(), "<code>", CssClass),
                        "<pre>", CssClass));
            textBuilder.Append("\n");
            codeBlockBuilder.Clear();
        }
    }
}