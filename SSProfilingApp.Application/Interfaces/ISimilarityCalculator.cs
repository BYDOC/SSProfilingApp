namespace SSProfilingApp.Application.Interfaces
{
    public interface ISimilarityCalculator
    {
        Task<double> CalculateAsync(string a, string b);
    }
}
