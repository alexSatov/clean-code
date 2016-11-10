using System.Text;

namespace Markdown.Markers
{
    public abstract class MarkerProcessor
    {
        public abstract string Marker { get; }
        public string CurrentField => FieldBuilder.ToString();

        protected readonly StringBuilder FieldBuilder = new StringBuilder();
        private string cache = "";

        public abstract void ProcessSymbol(char symbol);

        public virtual string GetCompletedField()
        {
            FieldBuilder.Remove(FieldBuilder.Length - Marker.Length, Marker.Length);
            var field = CurrentField;
            FieldBuilder.Clear();
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
