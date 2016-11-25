using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.HTML;

namespace Markdown.Markers
{
    public class UrlMarkerProcessor : BaseMarkerProcessor
    {
        public override string OpenMarker => "[";
        public override string CloseMarker => ")";
        public string BaseUrl;

        private bool canClose;
        private bool linkExpected;

        public UrlMarkerProcessor(string baseUrl = "")
        {
            BaseUrl = baseUrl;
        }

        public override bool CheckOnCloseMarker(ref char symbol, bool isLastSymbol = false)
        {
            if (linkExpected)
                if (!Separators.Contains(symbol) && symbol != '(')
                {
                    FieldBuilder.Append(symbol);
                    return true;
                }

                else if (symbol == '(')
                    linkExpected = false;
            if (canClose && Separators.Contains(symbol) || symbol.ToString() == CloseMarker && isLastSymbol)
                return true;
            canClose = symbol.ToString() == CloseMarker;
            linkExpected = symbol == ']' || linkExpected;
            return false;
        }

        public override string GetCompletedField()
        {
            var field = CurrentField;
            var squareBracketIndex = field.IndexOf(']');
            var circleBracketIndex = field.IndexOf('(');
            var text = squareBracketIndex > 0 ? field.Substring(0, squareBracketIndex) : "";
            var url = circleBracketIndex != -1 ? field.Substring(circleBracketIndex + 1).Replace(")", "") : "";
            try
            {
                if (linkExpected) return OpenMarker + field;
                if (!IsCorrectField(url, text)) return OpenMarker + field + (field[field.Length - 1] == ')' ? "" : ")");

                url = url[0] == '/' ? url.Insert(0, BaseUrl) : url;
                return HtmlWrapper.WrapToUrlTag(url, text, CssClass);
            }
            finally
            {
                Clear();
            }
        }

        public override void Clear()
        {
            canClose = false;
            linkExpected = false;
            base.Clear();
        }

        private static bool IsCorrectField(string url, string text)
        {
            return url != "" && text != "";
        }
    }
}
