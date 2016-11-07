using Markdown.Markers;

namespace Markdown.MD
{
	public class Md
	{
		public string RenderToHtml(string markdown)
		{
		    var markers = GetMarkers();
            var processor = new MarkdownProcessor(markers);
            return processor.Process(markdown);
		}

	    public IMarker[] GetMarkers()
	    {
            return new IMarker[] { EmMarker.Marker, StrongMarker.Marker };
        }
	}
}