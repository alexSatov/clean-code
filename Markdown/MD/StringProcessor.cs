using System.Linq;
using System.Text;
using Markdown.Markers;

namespace Markdown.MD
{
    public class StringProcessor
    {
        public MarkerProcessor[] MarkerProcessors { get; }

        private string cache { get; set; }
        private StringBuilder result { get; set; }
        private bool isCanReadMarker { get; set; }
        private MarkerProcessor currentMarkerProcessor { get; set; }
        private int currentCharIndex { get; set; }

        public StringProcessor(MarkerProcessor marker)
        {
            MarkerProcessors = new [] { marker };
            Initialization();
        }

        public StringProcessor(MarkerProcessor[] markers)
        {
            MarkerProcessors = markers;
            Initialization();
        }

        public string Process(string inputString)
        {
            for (; currentCharIndex < inputString.Length; currentCharIndex++)
            {
                var symbol = inputString[currentCharIndex];
                bool needToContinue;

                CheckOnEscapeSymbol(symbol, inputString, out needToContinue);
                if (needToContinue) continue;

                if (currentMarkerProcessor == null)
                {
                    if (CheckOnMarkerSymbol(symbol)) continue;
                    ProcessSymbol(symbol);
                }
                else
                {
                    TryCloseMarker(symbol, inputString, out needToContinue);
                    if (needToContinue) continue;
                    currentMarkerProcessor.ProcessSymbol(symbol);
                }
            }
            return GetProcessedString();
        }

        private void Initialization()
        {
            cache = "";
            currentCharIndex = 0;
            isCanReadMarker = true;
            result = new StringBuilder();
            currentMarkerProcessor = null;
        }

        private void CheckOnEscapeSymbol(char symbol, string inputString, out bool needToContinue)
        {
            if (symbol == '\\')
            {
                currentCharIndex++;
                result.Append(inputString[currentCharIndex]);
                needToContinue = true;
            }
            else
                needToContinue = false;
        }

        private void TryCloseMarker(char symbol, string inputString, out bool needToContinue)
        {
            if (currentMarkerProcessor.CheckOnCloseMarker(symbol, currentCharIndex == inputString.Length - 1))
            {
                AddRenderedField(symbol);
                needToContinue = true;
            }
            else
                needToContinue = false;
        }

        private void AddRenderedField(char symbol)
        {
            result.Append(currentMarkerProcessor.GetCompletedField());
            result.Append(symbol == ' ' ? " " : "");
            currentMarkerProcessor = null;
        }

        private void Clear()
        {
            result.Clear();
            currentCharIndex = 0;
            isCanReadMarker = true;
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

        private bool CheckOnMarkerSymbol(char symbol)
        {
            if (!IsMarkerSymbol(symbol)) return false;
            cache += symbol;
            return true;
        }

        private bool IsMarkerSymbol(char symbol)
        {
            var prevSymbol = result.Length > 0 ? result[result.Length - 1] : '-';
            return MarkerProcessors.Any(mp => mp.Marker.StartsWith(symbol.ToString())) &&
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
            currentMarkerProcessor = MarkerProcessors.FirstOrDefault(mp => mp.Marker == cache.ToString());
            cache = currentMarkerProcessor == null ? cache : "";
            currentMarkerProcessor?.ProcessSymbol(symbol);
        }
    }
}
