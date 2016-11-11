using Markdown.MD;
using Markdown.HTML;

namespace Markdown.Markers
{
    public class StrongMarkerProcessor : MarkerProcessor
    {
        public override string Marker => "__";

        public StrongMarkerProcessor(StringProcessor[] subProcessors = null)
        {
            SubProcessors = subProcessors;
        }

        public override void ProcessSymbol(char symbol)
        {
            FieldBuilder.Append(symbol);
        }

        public override string GetCompletedField()
        {
            return HtmlWrapper.WrapToHtmlTag(base.GetCompletedField(), "<strong>");
        }
    }
}
