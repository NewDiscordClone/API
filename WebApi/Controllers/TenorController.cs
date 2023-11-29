using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Sparkle.WebApi.Common.Options;
using Sparkle.WebApi.Models;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/tenor")]
    [Authorize]
    [ApiController]
    public class TenorController : ControllerBase
    {
        private readonly string _apiKey;
        private readonly int _limit;
        private readonly string _baseUrl;

        public TenorController(IOptions<TenorOptions> options)
        {
            TenorOptions tenor = options.Value;
            _apiKey = tenor.Key;
            _limit = tenor.Limit;
            _baseUrl = tenor.BaseUrl;
        }

        [HttpGet("categories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<TenorCategory>> GetCategories()
        {
            List<TenorCategory> categories = [];
            using HttpClient httpClient = new();

            HttpResponseMessage response = await httpClient.GetAsync($"{_baseUrl}/featured?limit=1&key={_apiKey}");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject json = JObject.Parse(responseBody);
                string url = (json["results"]?[0]?["media_formats"]?["tinygif"]?["url"]?.Value<string>())
                    ?? throw new InvalidDataException("url is null");

                categories.Add(new TenorCategory() { SearchTerm = "Featured", Image = url, IsFeatured = true });
            }
            response = await httpClient.GetAsync($"{_baseUrl}/categories?key={_apiKey}");
            if (response.IsSuccessStatusCode)
            {
                // Parse the response content to get the category data
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject json = JObject.Parse(responseBody);
                JToken? tags = json["tags"];
                if (tags == null)
                    throw new InvalidDataException("tags is null");

                foreach (JToken tag in tags)
                {
                    string searchterm = tag["searchterm"]?.Value<string>();
                    string image = tag["image"]?.Value<string>();

                    if (searchterm != null && image != null)
                    {
                        categories.Add(new TenorCategory
                        {
                            Image = image,
                            SearchTerm = searchterm,
                            IsFeatured = false
                        });
                    }
                }
            }

            return Ok(categories);
        }

        [HttpGet("featured")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult> GetFeatured(string? pos)
        {
            using HttpClient httpClient = new();
            HttpResponseMessage response;
            if (pos != null)
                response = await httpClient.GetAsync($"{_baseUrl}/featured?limit={_limit}&pos={pos}&key={_apiKey}");
            else
                response = await httpClient.GetAsync($"{_baseUrl}/featured?limit={_limit}&key={_apiKey}");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return Ok(responseBody);
            }

            throw new InvalidOperationException($"HTTP request failed with status code: {response.StatusCode}");
        }
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult> Search(string q, string? pos)
        {
            using HttpClient httpClient = new();
            HttpResponseMessage response;
            if (pos != null)
                response = await httpClient.GetAsync($"{_baseUrl}/search?q={q}&limit={_limit}&pos={pos}&key={_apiKey}");
            else
                response = await httpClient.GetAsync($"{_baseUrl}/search?q={q}&limit={_limit}&key={_apiKey}");
            if (response.IsSuccessStatusCode)
            {
                // Console.WriteLine(response.RequestMessage?.RequestUri);
                string responseBody = await response.Content.ReadAsStringAsync();
                return Ok(responseBody);
            }

            throw new InvalidOperationException($"HTTP request failed with status code: {response.StatusCode}");
        }
    }
}