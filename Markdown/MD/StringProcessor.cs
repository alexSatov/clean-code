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
                ProcessNextSymbol(symbol, inputString);
                currentCharIndex++;
            }
            return GetProcessedString();
        }

        private void ProcessNextSymbol(char symbol, string inputString)
        {
            bool shouldProcessNext;

            TryProcessEscapeSymbol(symbol, inputString, out shouldProcessNext);
            if (shouldProcessNext) return;

            if (currentMarkerProcessor == null)
            {
                if (TryProcessMarkerSymbol(symbol)) return;
                ProcessSymbol(symbol);
            }
            else
            {
                TryCloseMarker(symbol, inputString, out shouldProcessNext);
                if (shouldProcessNext) return;
                currentMarkerProcessor.ProcessSymbol(symbol);
            }
        }

        private void TryProcessEscapeSymbol(char symbol, string inputString, out bool shouldProcessNext)
        {
            if (symbol == '\\')
            {
                currentCharIndex++;
                result.Append(inputString[currentCharIndex]);
                shouldProcessNext = true;
            }
            else
                shouldProcessNext = false;
        }

        private void TryCloseMarker(char symbol, string inputString, out bool shouldProcessNext)
        {
            if (currentMarkerProcessor.CheckOnCloseMarker(symbol, currentCharIndex == inputString.Length - 1))
            {
                AddRenderedField(symbol);
                shouldProcessNext = true;
            }
            else
                shouldProcessNext = false;
        }

        private void AddRenderedField(char symbol)
        {
            result.Append(currentMarkerProcessor.GetCompletedField());
            result.Append(symbol == ' ' ? " " : "");
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

        private bool TryProcessMarkerSymbol(char symbol)
        {
            if (!IsMarkerSymbol(symbol)) return false;
            cache += symbol;
            return true;
        }

        private bool IsMarkerSymbol(char symbol)
        {
            var prevSymbol = result.Length > 0 ? result[result.Length - 1] : '-';
            return markerProcessors.Any(mp => mp.Marker.StartsWith(symbol.ToString())) &&
                   prevSymbol == ' ';
        }

        private void ProcessSymbol(char symbol)
        {
            if (cache == "")
                result.Append(symbol);
            else
            {
                if (symbol == ' ')
                    SetTextFromCacheToResult();
                else
                    TryStartMarkerProcessing(symbol);
            }
        }

        private void SetTextFromCacheToResult()
        {
            result.Append(cache);
            result.Append(" ");
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
