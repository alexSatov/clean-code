using System;
using Markdown.HTML;

namespace Markdown.Markers
{
    public class HeaderMarkerProcessor : BaseMarkerProcessor
    {
        public readonly int Size;
        public override string OpenMarker { get; }
        public override string CloseMarker => "\n";

        public HeaderMarkerProcessor(int size)
        {
            if (size < 1 || size > 6)
                throw new ArgumentException("Header size from 1 to 6 only");
            Size = size;
            OpenMarker = new string('#', size);
            OpenMarker += ' ';
        }

        public override bool CheckOnCloseMarker(char symbol, bool isLastSymbol = false)
        {
            if (symbol.ToString() == CloseMarker) return true;
            if (!isLastSymbol) return false;
            FieldBuilder.Append(symbol);
            return true;
        }

        public override string GetCompletedField()
        {
            FieldBuilder.Append(CloseMarker);
            return HtmlWrapper.WrapToHtmlTag(base.GetCompletedField(), $"<h{Size}>", CssClass);
        }
    }
}
