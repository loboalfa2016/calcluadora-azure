using Microsoft.AspNetCore.Mvc;
using ScientificCalculator.Backend.Models;
using ScientificCalculator.Backend.Services;

namespace ScientificCalculator.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CalculatorApiController : ControllerBase
{
    private readonly ICalculatorService _calculatorService;

    public CalculatorApiController(ICalculatorService calculatorService)
    {
        _calculatorService = calculatorService;
    }

    [HttpPost("calculate")]
    public IActionResult Calculate([FromBody] CalculationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(CalculationResponse.Fail("Parámetros de solicitud no válidos."));

        var response = _calculatorService.Calculate(request);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpPost("convert")]
    public IActionResult Convert([FromBody] UnitConversionRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(CalculationResponse.Fail("Parámetros de conversión no válidos."));

        var response = _calculatorService.ConvertUnit(request);
        return response.Success ? Ok(response) : BadRequest(response);
    }

    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "Healthy", framework = ".NET 10" });
}
