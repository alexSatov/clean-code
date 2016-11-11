using Markdown.Markers;

namespace Markdown.MD
{
	public class Md
	{
	    private readonly StringProcessor processor;

	    public Md()
	    {
            var markerProcessors = GetMarkerProcessors();
            processor = new StringProcessor(markerProcessors);
        }

        public string RenderToHtml(string text)
		{
            return processor.Process(text);
		}

	    public BaseMarkerProcessor[] GetMarkerProcessors()
	    {
            return new BaseMarkerProcessor[] { new EmMarkerProcessor(),
                new StrongMarkerProcessor(new StringProcessor(new EmMarkerProcessor())) };
        }
	}
}