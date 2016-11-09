namespace Markdown.Markers
{
    public class EmMarkerProcessor : MarkerProcessor
    {
        public override string Marker => "_";
        public static EmMarkerProcessor MarkerProcessor = new EmMarkerProcessor();

        public override void ProcessSymbol(char symbol)
        {
            return;
        }
    }
}
