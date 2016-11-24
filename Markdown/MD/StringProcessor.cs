using System.Linq;
using System.Text;
using Markdown.Markers;

namespace Markdown.MD
{
    public class StringProcessor
    {
        private string cache = "";
        private int currentCharIndex;
        private BaseMarkerProcessor currentMarkerProcessor;
        private readonly BaseMarkerProcessor[] markerProcessors;
        private readonly StringBuilder result = new StringBuilder();
        private char[] separators = new[] {' ', '\t'};

        public StringProcessor(BaseMarkerProcessor marker)
        {
            markerProcessors = new [] { marker };
        }

        public StringProcessor(BaseMarkerProcessor[] markers)
        {
            markerProcessors = markers;
        }

        public string Process(string inputString)
        {
            while (currentCharIndex < inputString.Length)
            {
                var symbol = inputString[currentCharIndex];
                ProcessSymbol(symbol, inputString);
                currentCharIndex++;
            }
            return GetProcessedString();
        }

        private void ProcessSymbol(char symbol, string inputString)
        {
            if (symbol == '\\')
                ProcessEscapeSymbol(symbol, inputString);
            else if (currentMarkerProcessor == null)
                SelfProcessing(symbol);
            else
                MarkerProcessing(symbol, inputString);
        }

        private void MarkerProcessing(char symbol, string inputString)
        {
            if (CloseMarkerCollected(symbol, inputString)) return;
            currentMarkerProcessor.ProcessSymbol(symbol);
        }

        private void SelfProcessing(char symbol)
        {
            if (OpenMarkerCollected(symbol)) return;
            ProcessSymbolDependingOnCache(symbol);
        }

        private void ProcessEscapeSymbol(char symbol, string inputString)
        {
            currentCharIndex++;
            result.Append(inputString[currentCharIndex]);
        }

        private bool CloseMarkerCollected(char symbol, string inputString)
        {
            if (currentMarkerProcessor.CheckOnCloseMarker(symbol, currentCharIndex == inputString.Length - 1))
            {
                AddRenderedField(symbol);
                return true;
            }
            return false;
        }

        private void AddRenderedField(char symbol)
        {
            result.Append(currentMarkerProcessor.GetCompletedField());
            result.Append(separators.Contains(symbol) ? symbol.ToString() : "");
            currentMarkerProcessor = null;
        }

        private void Clear()
        {
            cache = "";
            result.Clear();
            currentCharIndex = 0;
            currentMarkerProcessor?.Clear();
            currentMarkerProcessor = null;
        }

        private string GetProcessedString()
        {
            result.Append(currentMarkerProcessor?.Marker + currentMarkerProcessor?.CurrentField);
            var processedString = result.ToString();
            Clear();
            return processedString;
        }

        private bool OpenMarkerCollected(char symbol)
        {
            if (!IsMarkerSymbol(symbol)) return false;
            cache += symbol;
            return true;
        }

        private bool IsMarkerSymbol(char symbol)
        {
            var prevSymbol = result.Length > 0 ? result[result.Length - 1] : ' ';
            return markerProcessors.Any(mp => mp.Marker.StartsWith(symbol.ToString())) &&
                   separators.Contains(prevSymbol);
        }

        private void ProcessSymbolDependingOnCache(char symbol)
        {
            if (cache == "")
                result.Append(symbol);
            else
            {
                if (separators.Contains(symbol))
                    SetTextFromCacheToResult(symbol);
                else
                    TryStartMarkerProcessing(symbol);
            }
        }

        private void SetTextFromCacheToResult(char symbol)
        {
            result.Append(cache);
            result.Append(symbol.ToString());
            cache = "";
        }

        private void TryStartMarkerProcessing(char symbol)
        {
            currentMarkerProcessor = markerProcessors.FirstOrDefault(mp => mp.Marker == cache.ToString());
            cache = currentMarkerProcessor == null ? cache : "";
            currentMarkerProcessor?.ProcessSymbol(symbol);
        }
    }
}
