using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/proxy")]
    [Authorize]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProxyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> ProxyRequest(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("URL is required.");
            }

            using var httpClient = _httpClientFactory.CreateClient();
            // You can add additional logic to handle query parameters if needed.
            // For simplicity, we're directly using the provided URL.

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
                    
            // Handle non-success status codes if needed.
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
        
        [HttpGet("media")]
        [AllowAnonymous]
        public async Task<ActionResult> ProxyMediaRequest(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest("URL is required.");
            }

            using var httpClient = _httpClientFactory.CreateClient();
            // You can add additional logic to handle query parameters if needed.
            // For simplicity, we're directly using the provided URL.

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                var contentType = response.Content.Headers.ContentType;
                if (contentType != null)
                    return File(content, contentType.ToString());
                return BadRequest("The provided Url is not a media Url");
            }
                    
            // Handle non-success status codes if needed.
            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}