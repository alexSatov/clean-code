using System.Text;

namespace Markdown.Markers
{
    public interface IMarker
    {
        string Form { get; }
        StringBuilder Field { get; }
        void ProcessSymbol(char symbol);
        string GetField();
    }
}
