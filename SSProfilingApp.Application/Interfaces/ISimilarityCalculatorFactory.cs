using SSProfilingApp.Application.Enums;

namespace SSProfilingApp.Application.Interfaces;

public interface ISimilarityCalculatorFactory
{
    ISimilarityCalculator Get(SimilarityAlgorithm algorithm);
}
