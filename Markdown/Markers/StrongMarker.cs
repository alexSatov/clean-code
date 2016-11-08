namespace Markdown.Markers
{
    public class StrongMarker : Marker
    {
        public override string Form => "__";
        public static StrongMarker Marker = new StrongMarker();

        public override void ProcessSymbol(char symbol)
        {
            return;
        }
    }
}
