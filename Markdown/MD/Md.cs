using Markdown.Markers;

namespace Markdown.MD
{
	public class Md
	{
		public string RenderToHtml(string markdown)
		{
		    var markerProcessors = GetMarkerProcessors();
            var processor = new MarkdownProcessor(markerProcessors);
            return processor.Process(markdown);
		}

	    public MarkerProcessor[] GetMarkerProcessors()
	    {
            return new MarkerProcessor[] { EmMarkerProcessor.MarkerProcessor, StrongMarkerProcessor.MarkerProcessor };
        }
	}
}