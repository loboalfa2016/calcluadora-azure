using Microsoft.AspNetCore.Mvc;

namespace ScientificCalculator.Frontend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalculatorProxyController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly string _backendBaseUrl;

    public CalculatorProxyController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient("BackendApi");
        _backendBaseUrl = configuration["BackendUrl"]?.TrimEnd('/') ?? "https://app-calculadora-b-c2fqdshzhuguhzbx.brazilsouth-01.azurewebsites.net";
    }

    [HttpPost("calculate")]
    public async Task<IActionResult> Calculate([FromBody] object request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_backendBaseUrl}/api/calculatorapi/calculate", request);
            var content = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, content);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, errorMessage = $"Error comunicando con el Backend: {ex.Message}" });
        }
    }

    [HttpPost("convert")]
    public async Task<IActionResult> Convert([FromBody] object request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{_backendBaseUrl}/api/calculatorapi/convert", request);
            var content = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, content);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, errorMessage = $"Error comunicando con el Backend: {ex.Message}" });
        }
    }
}
