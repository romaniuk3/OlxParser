using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OlxParser.API.Services.Contracts;

namespace OlxParser.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParseController : ControllerBase
    {
        private readonly IHtmlParseService _htmlParseService;

        public ParseController(IHtmlParseService htmlParseService)
        {
            _htmlParseService = htmlParseService;
        }

        [HttpGet]
        public async Task<IActionResult> ParseOlx([FromQuery] string? customUrl = null)
        {
            string URL = "https://www.olx.ua/d/uk/nedvizhimost/kvartiry/dolgosrochnaya-arenda-kvartir/lvov/?currency=UAH&search%5Border%5D=created_at:desc&search%5Bfilter_float_price:from%5D=11999&search%5Bfilter_float_price:to%5D=16001";
            if (customUrl != null && customUrl.Length > 0) 
            { 
                URL = customUrl;
            }

            var parseResult = await _htmlParseService.Parse(URL);

            return Ok(parseResult);
        }
    }
}
