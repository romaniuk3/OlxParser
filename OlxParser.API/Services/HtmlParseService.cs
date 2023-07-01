using HtmlAgilityPack;
using OlxParser.API.Models;
using OlxParser.API.Services.Contracts;
using System.Globalization;

namespace OlxParser.API.Services
{
    public class HtmlParseService : IHtmlParseService
    {
        private DateTime? creationDateTime;
        private string? flatUrl;

        public async Task<ParseResultModel> Parse(string url)
        {
            var parseResult = new ParseResultModel();
            var web = new HtmlWeb();
            var document = await web.LoadFromWebAsync(url);
            var divElement = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'css-oukcj3')]");
            if (divElement != null)
            {
                var childElements = divElement.ChildNodes;
                var filteredElements = RemoveInvalidElements(childElements).Skip(3);
                var theFirstFlat = filteredElements.FirstOrDefault();
                if (theFirstFlat != null)
                {
                    var isOperationSuccessfull = ProcessFlat(theFirstFlat);
                    parseResult.CreationDateTime = creationDateTime;
                    parseResult.FlatUrl = "https://olx.ua" + flatUrl;
                }
            }

            return parseResult;
        }

        private bool ProcessFlat(HtmlNode element)
        {
            SetFlatUrl(element);
            var locationDateString = GetLocationDateString(element);
            if (locationDateString != null)
            {
                var date = GetDateFromLocationDateString(locationDateString);

                if (date != null)
                {
                    creationDateTime = date;
                    return true;
                    //return IsNewDate((DateTime)date);
                }
            }

            return false;
        }

        /*
        private bool IsNewDate(DateTime date)
        {
            if (oldDate == null)
            {
                oldDate = date;
                return false;
            }

            int compareResult = DateTime.Compare((DateTime)oldDate, date);
            if (compareResult == 0) return false;

            if (compareResult < 0)
            {
                oldDate = date;
                SendNewFlatNotification();
                return true;
            }

            return false;
        }*/

        private void SetFlatUrl(HtmlNode element)
        {
            var anchorTag = element.SelectSingleNode(".//a");

            if (anchorTag != null)
            {
                flatUrl = anchorTag.GetAttributeValue("href", "");
            }
        }

        private void SendNewFlatNotification()
        {
            Console.WriteLine("NEW FLAT ADDED!");
            Console.WriteLine("New FLAT URL: " + flatUrl);
        }

        private string? GetLocationDateString(HtmlNode element)
        {
            var paragraphElement = element.SelectSingleNode(".//a/div/div/div[2]/div[3]/p");

            if (paragraphElement != null)
            {
                return paragraphElement.InnerText;
            }

            return null;
        }

        private DateTime? GetDateFromLocationDateString(string locationDateString)
        {
            DateTime time;
            var timeString = locationDateString.Substring(locationDateString.Length - 5);
            bool isParsingSuccessfull = DateTime.TryParseExact(timeString, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out time);
            if (isParsingSuccessfull)
            {
                return TimeWithUtcOffset(time);
            }
            return null;
        }

        private IEnumerable<HtmlNode> RemoveInvalidElements(HtmlNodeCollection elements)
        {
            return elements.Where(e => IsValidId(e.Id));
        }

        private DateTime TimeWithUtcOffset(DateTime time)
        {
            return time.AddHours(3);
        }

        private static bool IsValidId(string id)
        {
            int result;
            return int.TryParse(id, out result);
        }
    }
}
