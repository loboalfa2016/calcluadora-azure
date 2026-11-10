namespace ScientificCalculator.Backend.Models;

public class CalculationResponse
{
    public bool Success { get; set; }
    public double Result { get; set; }
    public string FormattedResult { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
    public string Expression { get; set; } = string.Empty;

    public static CalculationResponse Ok(double result, string expression) => new()
    {
        Success = true,
        Result = result,
        FormattedResult = result.ToString("G12"),
        Expression = expression
    };

    public static CalculationResponse Fail(string errorMessage) => new()
    {
        Success = false,
        ErrorMessage = errorMessage
    };
}
