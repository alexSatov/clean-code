using Markdown.Markers;

namespace Markdown.MD
{
	public class Md
	{
		public string RenderToHtml(string markdown)
		{
		    var markerProcessors = GetMarkerProcessors();
            var processor = new StringProcessor(markerProcessors);
            return processor.Process(markdown);
		}

	    public MarkerProcessor[] GetMarkerProcessors()
	    {
            return new MarkerProcessor[] { new EmMarkerProcessor(), new StrongMarkerProcessor()};
        }
	}
}