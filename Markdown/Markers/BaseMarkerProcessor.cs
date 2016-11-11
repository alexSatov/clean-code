using System.Linq;
using System.Text;
using Markdown.MD;

namespace Markdown.Markers
{
    public abstract class BaseMarkerProcessor
    {
        public abstract string Marker { get; }
        public string CurrentField => FieldBuilder.ToString();

        protected StringBuilder FieldBuilder = new StringBuilder();
        protected StringProcessor[] SubProcessors;

        private string cache = "";

        public virtual void ProcessSymbol(char symbol)
        {
            FieldBuilder.Append(symbol);
        }

        public virtual string GetCompletedField()
        {
            FieldBuilder.Remove(FieldBuilder.Length - Marker.Length, Marker.Length);
            var field = CurrentField;

            if (SubProcessors != null)
                field = SubProcessors.Aggregate(field, (current, subProcessor) => subProcessor.Process(current));

            Clear();
            return field;
        }

        public bool CheckOnCloseMarker(char symbol, bool isLastSymbol = false)
        {
            if (IsMarkerSymbol(symbol))
            {
                cache += symbol;
                if (!IsCompleteCloseMarker(symbol, isLastSymbol)) return false;
                FieldBuilder.Append(symbol);
                return true;
            }

            if (IsCompleteCloseMarker(symbol, isLastSymbol))
            {
                cache = "";
                return true;
            }

            cache = "";
            return false;
        }

        public void Clear()
        {
            FieldBuilder.Clear();
            cache = "";
        }

        private bool IsMarkerSymbol(char symbol)
        {
            var prevSymbol = FieldBuilder.Length > 0 ? FieldBuilder[FieldBuilder.Length - 1] : '-';
            return Marker.StartsWith(symbol.ToString()) && prevSymbol != ' ';
        }

        private bool IsCompleteCloseMarker(char symbol, bool isLastSymbol)
        {
            return (symbol == ' ' || isLastSymbol) && Marker == cache;
        }
    }
}
