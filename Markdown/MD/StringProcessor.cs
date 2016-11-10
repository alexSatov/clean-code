using System.Linq;
using System.Text;
using Markdown.Markers;
using System.Collections.Generic;

namespace Markdown.MD
{
    public class StringProcessor
    {
        public MarkerProcessor[] MarkerProcessors { get; }

        private string cache { get; set; }
        private StringBuilder result { get; }
        private bool isCanReadMarker { get; set; }
        private MarkerProcessor currentMarkerProcessor { get; set; }

        public StringProcessor(IEnumerable<MarkerProcessor> markers)
        {
            cache = "";
            isCanReadMarker = true;
            result = new StringBuilder();
            currentMarkerProcessor = null;
            MarkerProcessors = markers.ToArray();
        }

        public string Process(string inputString)
        {
            for (var i = 0; i < inputString.Length; i++)
            {
                var symbol = inputString[i];

                if (symbol == '\\')
                {
                    i++;
                    result.Append(inputString[i]);
                    continue;
                }

                if (currentMarkerProcessor == null)
                {
                    if (CheckOnMarkerSymbol(symbol)) continue;
                    ProcessSymbol(symbol);
                }
                else
                {
                    if (currentMarkerProcessor.CheckOnCloseMarker(symbol, i == inputString.Length - 1))
                    {
                        AddRenderedField(symbol);
                        continue;
                    }
                    currentMarkerProcessor.ProcessSymbol(symbol);
                }
            }
            return GetProcessedString();
        }

        private void AddRenderedField(char symbol)
        {
            result.Append(currentMarkerProcessor.GetCompletedField());
            result.Append(symbol == ' ' ? " " : "");
            currentMarkerProcessor = null;
        }

        private string GetProcessedString()
        {
            result.Append(currentMarkerProcessor?.Marker + currentMarkerProcessor?.CurrentField);
            currentMarkerProcessor?.GetCompletedField();
            var processedString = result.ToString();
            result.Clear();
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
