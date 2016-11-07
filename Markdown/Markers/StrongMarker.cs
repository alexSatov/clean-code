using System.Text;

namespace Markdown.Markers
{
    public class StrongMarker : IMarker
    {
        public string Form { get; }
        public StringBuilder Field { get; }
        public static StrongMarker Marker = new StrongMarker();

        public void ProcessSymbol(char symbol)
        {
            throw new System.NotImplementedException();
        }

        public string GetField()
        {
            throw new System.NotImplementedException();
        }

        private StrongMarker()
        {
            Form = "__";
            Field = new StringBuilder();
        }
    }
}
