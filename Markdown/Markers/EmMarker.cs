namespace Markdown.Markers
{
    public class EmMarker : Marker
    {
        public override string Form => "_";
        public static EmMarker Marker = new EmMarker();

        public override void ProcessSymbol(char symbol)
        {
            return;
        }
    }
}
