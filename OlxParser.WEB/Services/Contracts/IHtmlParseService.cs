namespace OlxParser.WEB.Services.Contracts
{
    public interface IHtmlParseService
    {
        void RunOlxListener(string customUrl);
        void StopOlxListener();
    }
}
