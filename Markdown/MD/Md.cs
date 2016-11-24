using Markdown.Markers;

namespace Markdown.MD
{
	public class Md
	{
	    private readonly StringProcessor processor;
	    public string BaseUrl;
	    public string CssClass;

        public Md(string baseUrl = "", string cssClass = "")
        {
            BaseUrl = baseUrl;
            CssClass = cssClass;
            var markerProcessors = GetMarkerProcessors();
            processor = new StringProcessor(markerProcessors);
        }

        public string RenderToHtml(string text)
		{
            return processor.Process(text);
		}

	    public BaseMarkerProcessor[] GetMarkerProcessors()
	    {
            return new BaseMarkerProcessor[] {
                new UrlMarkerProcessor(BaseUrl) {CssClass = CssClass},
                new EmMarkerProcessor {CssClass = CssClass},
                new StrongMarkerProcessor(new StringProcessor(new EmMarkerProcessor {CssClass = CssClass})) {CssClass = CssClass}
            };
        }
	}
}