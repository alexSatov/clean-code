using Markdown.HTML;

namespace Markdown.Markers
{
    public class H1MarkerProcessor : BaseMarkerProcessor
    {
        public override string OpenMarker => "#";
        public override string CloseMarker => "\n";

        public override bool CheckOnCloseMarker(ref char symbol, bool isLastSymbol = false)
        {
            if (symbol.ToString() == CloseMarker) return true;
            if (!isLastSymbol) return false;
            FieldBuilder.Append(symbol);
            return true;
        }

        public override string GetCompletedField()
        {
            FieldBuilder.Append(CloseMarker);
            return HtmlWrapper.WrapToHtmlTag(base.GetCompletedField(), "<h1>", CssClass);
        }
    }
}
