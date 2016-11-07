using System.Text;

namespace Markdown.Markers
{
    public class EmMarker : IMarker
    {
        public string Form { get; }
        public StringBuilder Field { get; }
        public static EmMarker Marker = new EmMarker();

        public void ProcessSymbol(char symbol)
        {
            throw new System.NotImplementedException();
        }

        public string GetField()
        {
            throw new System.NotImplementedException();
        }

        private EmMarker()
        {
            Form = "_";
            Field = new StringBuilder();
        }
    }
}
