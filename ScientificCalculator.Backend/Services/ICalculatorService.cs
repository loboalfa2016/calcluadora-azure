using ScientificCalculator.Backend.Models;

namespace ScientificCalculator.Backend.Services;

public interface ICalculatorService
{
    CalculationResponse Calculate(CalculationRequest request);
    CalculationResponse ConvertUnit(UnitConversionRequest request);
}
