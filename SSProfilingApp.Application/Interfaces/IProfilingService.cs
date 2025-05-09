using SSProfilingApp.Application.Requests;

namespace SSProfilingApp.Application.Interfaces;
public interface IProfilingService
{
    Task<int> AddIndividualAsync(CreateIndividualRequest request);
    Task GroupIndividualsAsync();
    Task DeleteAllAsync();
}
