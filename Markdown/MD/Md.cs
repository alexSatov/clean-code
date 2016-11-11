using Markdown.Markers;

namespace Markdown.MD
{
	public class Md
	{
        private StringProcessor processor { get; }

	    public Md()
	    {
            var markerProcessors = GetMarkerProcessors();
            processor = new StringProcessor(markerProcessors);
        }

        public string RenderToHtml(string text)
		{
            return processor.Process(text);
		}

	    public MarkerProcessor[] GetMarkerProcessors()
	    {
            return new MarkerProcessor[] { new EmMarkerProcessor(),
                new StrongMarkerProcessor(new [] { new StringProcessor(new EmMarkerProcessor()) }) };
        }
	}
}