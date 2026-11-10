using ScientificCalculator.Backend.Models;

namespace ScientificCalculator.Backend.Services;

public class CalculatorService : ICalculatorService
{
    public CalculationResponse Calculate(CalculationRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Operation))
            return CalculationResponse.Fail("Debe especificar una operación.");

        var op = request.Operation.Trim().ToLowerInvariant();
        double a = request.Operand1;
        double? b = request.Operand2;

        try
        {
            return op switch
            {
                // Operaciones básicas
                "add" => b.HasValue 
                    ? CalculationResponse.Ok(a + b.Value, $"{a} + {b.Value}") 
                    : CalculationResponse.Fail("Falta el segundo operando."),

                "subtract" => b.HasValue 
                    ? CalculationResponse.Ok(a - b.Value, $"{a} - {b.Value}") 
                    : CalculationResponse.Fail("Falta el segundo operando."),

                "multiply" => b.HasValue 
                    ? CalculationResponse.Ok(a * b.Value, $"{a} × {b.Value}") 
                    : CalculationResponse.Fail("Falta el segundo operando."),

                "divide" => ValidateDivision(a, b),

                // Operaciones científicas
                "power" => b.HasValue 
                    ? ValidatePower(a, b.Value) 
                    : CalculationResponse.Fail("Falta el exponente."),

                "sqrt" => a < 0 
                    ? CalculationResponse.Fail("Error: Raíz cuadrada de número negativo.") 
                    : CalculationResponse.Ok(Math.Sqrt(a), $"√({a})"),

                "sin" => CalculationResponse.Ok(Math.Sin(ToRadians(a, request.AngleMode)), $"sin({a}°/{request.AngleMode})"),
                "cos" => CalculationResponse.Ok(Math.Cos(ToRadians(a, request.AngleMode)), $"cos({a}°/{request.AngleMode})"),
                "tan" => ValidateTangent(a, request.AngleMode),

                "ln" => a <= 0 
                    ? CalculationResponse.Fail("Error: El logaritmo natural solo está definido para x > 0.") 
                    : CalculationResponse.Ok(Math.Log(a), $"ln({a})"),

                "log10" => a <= 0 
                    ? CalculationResponse.Fail("Error: El logaritmo base 10 solo está definido para x > 0.") 
                    : CalculationResponse.Ok(Math.Log10(a), $"log10({a})"),

                "factorial" => CalculateFactorial(a),

                _ => CalculationResponse.Fail($"Operación '{request.Operation}' no soportada.")
            };
        }
        catch (Exception ex)
        {
            return CalculationResponse.Fail($"Error de cálculo: {ex.Message}");
        }
    }

    public CalculationResponse ConvertUnit(UnitConversionRequest request)
    {
        try
        {
            var category = request.Category.ToLowerInvariant();
            var from = request.FromUnit.ToLowerInvariant();
            var to = request.ToUnit.ToLowerInvariant();
            double val = request.Value;

            double result = category switch
            {
                "length" => ConvertLength(val, from, to),
                "mass" => ConvertMass(val, from, to),
                "temperature" => ConvertTemperature(val, from, to),
                _ => throw new InvalidOperationException("Categoría de conversión no válida.")
            };

            return CalculationResponse.Ok(result, $"{val} {request.FromUnit} → {request.ToUnit}");
        }
        catch (Exception ex)
        {
            return CalculationResponse.Fail(ex.Message);
        }
    }

    #region Helpers Matemáticos
    private static CalculationResponse ValidateDivision(double a, double? b)
    {
        if (!b.HasValue) return CalculationResponse.Fail("Falta el divisor.");
        if (Math.Abs(b.Value) < double.Epsilon) return CalculationResponse.Fail("Error: División entre cero.");
        return CalculationResponse.Ok(a / b.Value, $"{a} ÷ {b.Value}");
    }

    private static CalculationResponse ValidatePower(double a, double b)
    {
        if (a < 0 && Math.Abs(b % 1) > double.Epsilon)
            return CalculationResponse.Fail("Error: Base negativa con exponente fraccionario produce número complejo.");
        
        double res = Math.Pow(a, b);
        if (double.IsInfinity(res) || double.IsNaN(res))
            return CalculationResponse.Fail("Error: Desbordamiento numérico en potencia.");

        return CalculationResponse.Ok(res, $"{a} ^ {b}");
    }

    private static CalculationResponse ValidateTangent(double angle, string mode)
    {
        double normalized = mode.Equals("DEG", StringComparison.OrdinalIgnoreCase) 
            ? Math.Abs(angle % 180) 
            : Math.Abs(angle % Math.PI);

        if (mode.Equals("DEG", StringComparison.OrdinalIgnoreCase) && Math.Abs(normalized - 90) < 1e-9)
            return CalculationResponse.Fail("Error: Tangente indefinida en 90° + k·180°.");

        return CalculationResponse.Ok(Math.Tan(ToRadians(angle, mode)), $"tan({angle})");
    }

    private static CalculationResponse CalculateFactorial(double n)
    {
        if (n < 0 || Math.Abs(n % 1) > double.Epsilon)
            return CalculationResponse.Fail("Error: El factorial solo aplica para enteros no negativos.");

        if (n > 170)
            return CalculationResponse.Fail("Error: Factorial excede el límite de precisión (desbordamiento).");

        long intN = (long)n;
        double fact = 1;
        for (long i = 2; i <= intN; i++) fact *= i;

        return CalculationResponse.Ok(fact, $"{intN}!");
    }

    private static double ToRadians(double angle, string mode) =>
        mode.Equals("DEG", StringComparison.OrdinalIgnoreCase) ? (angle * Math.PI / 180.0) : angle;

    private static double ConvertLength(double v, string from, string to)
    {
        double inMeters = from switch { "m" => v, "km" => v * 1000, "cm" => v / 100, "in" => v * 0.0254, "ft" => v * 0.3048, _ => throw new ArgumentException("Unidad origen inválida.") };
        return to switch { "m" => inMeters, "km" => inMeters / 1000, "cm" => inMeters * 100, "in" => inMeters / 0.0254, "ft" => inMeters / 0.3048, _ => throw new ArgumentException("Unidad destino inválida.") };
    }

    private static double ConvertMass(double v, string from, string to)
    {
        double inGrams = from switch { "g" => v, "kg" => v * 1000, "lb" => v * 453.59237, "oz" => v * 28.3495, _ => throw new ArgumentException("Unidad origen inválida.") };
        return to switch { "g" => inGrams, "kg" => inGrams / 1000, "lb" => inGrams / 453.59237, "oz" => inGrams / 28.3495, _ => throw new ArgumentException("Unidad destino inválida.") };
    }

    private static double ConvertTemperature(double v, string from, string to)
    {
        if (from == to) return v;
        double celsius = from switch { "c" => v, "f" => (v - 32) * 5 / 9, "k" => v - 273.15, _ => throw new ArgumentException("Unidad origen inválida.") };
        return to switch { "c" => celsius, "f" => (celsius * 9 / 5) + 32, "k" => celsius + 273.15, _ => throw new ArgumentException("Unidad destino inválida.") };
    }
    #endregion
}
