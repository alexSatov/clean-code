using Markdown.HTML;

namespace Markdown.Markers
{
    public class EmMarkerProcessor : MarkerProcessor
    {
        public override string Marker => "_";

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
