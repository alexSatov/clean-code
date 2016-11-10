using Markdown.MD;
using Markdown.HTML;

namespace Markdown.Markers
{
    public class StrongMarkerProcessor : MarkerProcessor
    {
        public override string Marker => "__";
        private StringProcessor subProcessor { get; set; }

        public override void ProcessSymbol(char symbol)
        {
            FieldBuilder.Append(symbol);
            subProcessor = new StringProcessor(new [] { new EmMarkerProcessor() });
        }

        public override string GetCompletedField()
        {
            var initialField = HtmlWrapper.WrapToHtmlTag(base.GetCompletedField(), "<strong>");
            return subProcessor.Process(initialField);
        }
    }
}
