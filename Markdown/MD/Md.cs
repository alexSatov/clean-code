﻿using System;
using System.Text;
using Markdown.HTML;
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
            var paragraphs = text.Split(new []{"\n\n"}, StringSplitOptions.None);
            var htmlTextBuilder = new StringBuilder();

            if (paragraphs.Length == 1)
                return processor.Process(text);

            for (var i = 0; i < paragraphs.Length - 1; i++)
                htmlTextBuilder.Append(HtmlWrapper.WrapToHtmlTag(processor.Process(paragraphs[i]), "<p>"));
            htmlTextBuilder.Append(processor.Process(paragraphs[paragraphs.Length - 1]));
            return htmlTextBuilder.ToString();
		}

	    public BaseMarkerProcessor[] GetMarkerProcessors()
	    {
            return new BaseMarkerProcessor[] {
                new BulletedListMarkerProcessor('*'), 
                new BulletedListMarkerProcessor('+'), 
                new BulletedListMarkerProcessor('-'), 
                new HeaderMarkerProcessor(1), 
                new HeaderMarkerProcessor(2), 
                new HeaderMarkerProcessor(3), 
                new HeaderMarkerProcessor(4), 
                new HeaderMarkerProcessor(5), 
                new HeaderMarkerProcessor(6), 
                new UrlMarkerProcessor(BaseUrl) {CssClass = CssClass},
                new EmMarkerProcessor {CssClass = CssClass},
                new StrongMarkerProcessor(new StringProcessor(new EmMarkerProcessor {CssClass = CssClass})) {CssClass = CssClass}
            };
        }
	}
}