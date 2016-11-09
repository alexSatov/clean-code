using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Markers;

namespace Markdown.MD
{
    public class MarkdownProcessor
    {
        public MarkerProcessor[] MarkerProcessors { get; }
        private MarkerProcessor currentMarkerProcessor { get; set; }
        private StringBuilder result { get; }

        public MarkdownProcessor(IEnumerable<MarkerProcessor> markers)
        {
            MarkerProcessors = markers.ToArray();
            currentMarkerProcessor = null;
            result = new StringBuilder();
        }

        public string Process(string inputString)
        {
            throw new System.NotImplementedException();
        }
    }
}
