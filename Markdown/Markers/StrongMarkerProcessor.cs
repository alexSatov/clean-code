namespace Markdown.Markers
{
    public class StrongMarkerProcessor : MarkerProcessor
    {
        public override string Marker => "__";
        public static StrongMarkerProcessor MarkerProcessor = new StrongMarkerProcessor();

        public override void ProcessSymbol(char symbol)
        {
            return;
        }
    }
}
