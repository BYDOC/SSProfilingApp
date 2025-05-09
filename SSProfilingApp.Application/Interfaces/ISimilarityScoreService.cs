using SSProfilingApp.Domain.Entities;

namespace SSProfilingApp.Application.Interfaces;

public interface ISimilarityScoreService
{
    Task<double> CalculateScoreAsync(IndividualData a, IndividualData b);
}
