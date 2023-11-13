using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Interfaces;
using Sparkle.WebApi.Models;
using Newtonsoft.Json.Linq;

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
    }
}