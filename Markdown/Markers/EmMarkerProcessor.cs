using Markdown.MD;
using Markdown.HTML;

namespace Markdown.Markers
{
    public class EmMarkerProcessor : BaseMarkerProcessor
    {
        public override string OpenMarker => "_";
        public override string CloseMarker => "_";

        public EmMarkerProcessor(StringProcessor[] subProcessors)
        {
            SubProcessors = subProcessors;
        }

        public EmMarkerProcessor(StringProcessor subProcessor)
        {
            SubProcessors = new [] { subProcessor };
        }

        public EmMarkerProcessor() { }

        public override string GetCompletedField()
        {
            return HtmlWrapper.WrapToHtmlTag(base.GetCompletedField(), "<em>", CssClass);
        }
    }
}
