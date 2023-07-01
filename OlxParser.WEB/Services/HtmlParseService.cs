using HtmlAgilityPack;
using OlxParser.WEB.Models;
using OlxParser.WEB.Services.Contracts;
using OlxParser.WEB.StateContainers;
using System.Globalization;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;

namespace OlxParser.WEB.Services
{
    public class HtmlParseService : IHtmlParseService
    {
        private readonly HttpClient _httpClient;
        private readonly FlatStateContainer _flatStateContainer;
        private Timer? timer;
        private bool IsActivated;
        private DateTime? oldDate;

        public HtmlParseService(HttpClient httpClient, FlatStateContainer flatStateContainer)
        {
            _httpClient = httpClient;
            _flatStateContainer = flatStateContainer;
        }

        public void RunOlxListener(string customUrl)
        {
            timer = new Timer(async _ => await GetParseResult(customUrl), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            /*
            IsActivated = true;
            while (IsActivated)
            {
                var parseResult = await GetParseResult();
                await Console.Out.WriteLineAsync("PARSE RESULT FLAT URL: " + parseResult.FlatUrl);
                if (parseResult != null)
                {
                    if (IsNewFlat(parseResult))
                    {
                        IsActivated = false;
                        return parseResult.FlatUrl;
                    }
                }
                Thread.Sleep(10000);
            }

            return string.Empty;*/
        }

        public void StopOlxListener()
        {
            IsActivated = false;
            oldDate = null;
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        private async Task<ParseResult?> GetParseResult(string customUrl)
        {
            var parseResult = await _httpClient.GetFromJsonAsync<ParseResult>("/api/parse" + $"?customUrl={Uri.EscapeDataString(customUrl)}");
            await Console.Out.WriteLineAsync("FLAT URL: " + parseResult.FlatUrl);
            if (IsNewFlat(parseResult))
            {
                _flatStateContainer.ChangeFlatState(parseResult);
            }
            return parseResult;
        }

        public bool IsNewFlat(ParseResult parseResult)
        {
            if (oldDate == null)
            {
                oldDate = parseResult.CreationDateTime;
                return false;
            }

            int compareResult = DateTime.Compare((DateTime)oldDate, parseResult.CreationDateTime);
            if (compareResult == 0) return false;

            if (compareResult < 0)
            {
                Console.WriteLine("NEW FLAT ADDED!");
                oldDate = parseResult.CreationDateTime;
                return true;
            }

            return false;
        }
    }
}
