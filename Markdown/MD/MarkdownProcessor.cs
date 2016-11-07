using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Markers;

namespace Markdown.MD
{
    public class MarkdownProcessor
    {
        public IMarker[] Markers { get; }
        private IMarker currentMarker { get; set; }
        private StringBuilder result { get; }

        public MarkdownProcessor(IEnumerable<IMarker> markers)
        {
            Markers = markers.ToArray();
            currentMarker = null;
            result = new StringBuilder();
        }

        public string Process(string inputString)
        {
            throw new System.NotImplementedException();
        }
    }
}
