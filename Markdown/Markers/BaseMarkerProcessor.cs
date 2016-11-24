using System.Linq;
using System.Text;
using Markdown.MD;

namespace Markdown.Markers
{
    public abstract class BaseMarkerProcessor
    {
        public abstract string OpenMarker { get; }
        public abstract string CloseMarker { get; }
        public string CurrentField => FieldBuilder.ToString();

        protected StringBuilder FieldBuilder = new StringBuilder();
        protected StringProcessor[] SubProcessors;
        protected readonly char[] Separators = { ' ', '\t', '\n' };

        private string cache = "";
       

        public virtual void ProcessSymbol(char symbol)
        {
            FieldBuilder.Append(symbol);
        }

        public virtual string GetCompletedField()
        {
            FieldBuilder.Remove(FieldBuilder.Length - CloseMarker.Length, CloseMarker.Length);
            var field = CurrentField;

            if (SubProcessors != null)
                field = SubProcessors.Aggregate(field, (current, subProcessor) => subProcessor.Process(current));

            Clear();
            return field;
        }

        public virtual bool CheckOnCloseMarker(char symbol, bool isLastSymbol = false)
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

        public virtual void Clear()
        {
            FieldBuilder.Clear();
            cache = "";
        }

        private bool IsMarkerSymbol(char symbol)
        {
            var prevSymbol = FieldBuilder.Length > 0 ? FieldBuilder[FieldBuilder.Length - 1] : ' ';
            return CloseMarker.StartsWith(symbol.ToString()) && !Separators.Contains(prevSymbol);
        }

        private bool IsCompleteCloseMarker(char symbol, bool isLastSymbol)
        {
            return (Separators.Contains(symbol) || isLastSymbol) && CloseMarker == cache;
        }
    }
}