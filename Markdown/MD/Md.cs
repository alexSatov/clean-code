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

	    public Marker[] GetMarkers()
	    {
            return new Marker[] { EmMarker.Marker, StrongMarker.Marker };
        }
	}
}