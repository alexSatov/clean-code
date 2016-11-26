using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Markdown.HTML;

namespace Markdown.Markers
{
    public class BulletedListMarkerProcessor : BaseMarkerProcessor
    {
        public override string OpenMarker { get; }
        public override string CloseMarker => "";

        private List<string> items = new List<string>();
        private bool newItemExpected;
        private bool textContinue;

        public BulletedListMarkerProcessor(char markerSymbol)
        {
            if (!new []{ '*', '+', '-'}.Contains(markerSymbol))
                throw new ArgumentException("Only \'*\', \'+\', \'-\' symbols can be bulleted list marker");
            OpenMarker = markerSymbol.ToString();
            OpenMarker += ' ';
        }

        public override void ProcessSymbol(char symbol)
        {
            if (newItemExpected)
            {
                if (symbol == ' ') newItemExpected = false;
                return;
            }

            if (symbol == '\n')
            {
                items.Add(CurrentField);
                FieldBuilder.Clear();
                newItemExpected = true;
                return;
            }

            base.ProcessSymbol(symbol);
        }

        public override bool CheckOnCloseMarker(char symbol, bool isLastSymbol = false)
        {
            if (newItemExpected && symbol != OpenMarker[0] && symbol != ' ')
            {
                textContinue = true;
                return true;
            }

            newItemExpected = newItemExpected && (symbol == OpenMarker[0] || symbol == ' ');

            if (!isLastSymbol) return false;

            textContinue = false;
            if (symbol != '\n') FieldBuilder.Append(symbol);
            ProcessSymbol('\n');
            return true;
        }

        public override string GetCompletedField()
        {
            var listBuilder = new StringBuilder();

            if (items.Count == 1)
                return OpenMarker + items[0];

            foreach (var item in items)
                listBuilder.Append(HtmlWrapper.WrapToHtmlTag(item, "<li>", CssClass));

            var endOfList = textContinue ? "\n" : "";
            Clear();
            return HtmlWrapper.WrapToHtmlTag(listBuilder.ToString(), "<ul>", CssClass) + endOfList;
        }

        public override void Clear()
        {
            items = new List<string>();
            newItemExpected = false;
            textContinue = false;
        }
    }
}
