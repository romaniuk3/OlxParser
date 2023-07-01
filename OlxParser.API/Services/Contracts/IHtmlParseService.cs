using OlxParser.API.Models;

namespace OlxParser.API.Services.Contracts
{
    public interface IHtmlParseService
    {
        Task<ParseResultModel> Parse(string url);
    }
}
