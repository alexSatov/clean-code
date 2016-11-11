using Markdown.MD;
using Markdown.HTML;

namespace Markdown.Markers
{
    public class EmMarkerProcessor : MarkerProcessor
    {
        public override string Marker => "_";

        public EmMarkerProcessor(StringProcessor[] subProcessors = null)
        {
            SubProcessors = subProcessors;
        }

        public override void ProcessSymbol(char symbol)
        {
            FieldBuilder.Append(symbol);
        }

        public override string GetCompletedField()
        {
            return HtmlWrapper.WrapToHtmlTag(base.GetCompletedField(), "<em>");
        }
    }
}
