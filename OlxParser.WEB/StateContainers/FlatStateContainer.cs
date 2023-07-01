using OlxParser.WEB.Models;

namespace OlxParser.WEB.StateContainers
{
    public class FlatStateContainer
    {
        public ParseResult FlatInfo { get; set; }

        public event Action? FlatStateChanged;
        public void ChangeFlatState(ParseResult flatInfo)
        {
            FlatInfo = flatInfo;
            FlatStateChanged?.Invoke();
        }
    }
}
