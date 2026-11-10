namespace ScientificCalculator.Backend.Models;

public class CalculationRequest
{
    public string Operation { get; set; } = string.Empty;
    public double Operand1 { get; set; }
    public double? Operand2 { get; set; }
    public string AngleMode { get; set; } = "DEG";
}

public class UnitConversionRequest
{
    public double Value { get; set; }
    public string Category { get; set; } = string.Empty; // Length, Mass, Temperature
    public string FromUnit { get; set; } = string.Empty;
    public string ToUnit { get; set; } = string.Empty;
}
