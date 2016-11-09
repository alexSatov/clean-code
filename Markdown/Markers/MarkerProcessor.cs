using System.Text;

namespace Markdown.Markers
{
    public abstract class MarkerProcessor
    {
        public abstract string Marker { get; }
        private readonly StringBuilder field = new StringBuilder();

        public abstract void ProcessSymbol(char symbol);

        public string GetField()
        {
            var result = field.ToString();
            field.Clear();
            return result;
        }
    }
}
