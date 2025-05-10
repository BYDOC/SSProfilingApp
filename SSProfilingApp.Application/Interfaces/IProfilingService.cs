using SSProfilingApp.Application.Requests;

namespace SSProfilingApp.Application.Interfaces;
public interface IProfilingService
{
    Task<List<int>> AddIndividualAsync(List<CreateIndividualRequest>request);
    Task GroupIndividualsAsync();
    Task DeleteAllAsync();
}
