using Microsoft.Extensions.DependencyInjection;
using SSProfilingApp.Application.Enums;
using SSProfilingApp.Application.Interfaces;
using SSProfilingApp.Infrastructure.Helpers;

namespace SSProfilingApp.Infrastructure.Factories;

public class SimilarityCalculatorFactory: ISimilarityCalculatorFactory
{
    private readonly IServiceProvider _provider;

    public SimilarityCalculatorFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public ISimilarityCalculator Get(SimilarityAlgorithm algorithm)
    {
        return algorithm switch
        {
            SimilarityAlgorithm.Levenshtein => _provider.GetRequiredService<LevenshteinCalculator>(),
            SimilarityAlgorithm.JaroWinkler => _provider.GetRequiredService<JaroWinklerApiClient>(),
            _ => throw new NotSupportedException()
        };
    }
}
