using Markdown.MD;
using Markdown.HTML;

namespace Markdown.Markers
{
    public class StrongMarkerProcessor : BaseMarkerProcessor
    {
        public override string OpenMarker => "__";
        public override string CloseMarker => "__";

        public StrongMarkerProcessor(StringProcessor[] subProcessors)
        {
            SubProcessors = subProcessors;
        }

        public StrongMarkerProcessor(StringProcessor subProcessor)
        {
            SubProcessors = new[] { subProcessor };
        }

        public StrongMarkerProcessor() { }

        public override string GetCompletedField()
        {
            return HtmlWrapper.WrapToHtmlTag(base.GetCompletedField(), "<strong>", CssClass);
        }
    }
}
