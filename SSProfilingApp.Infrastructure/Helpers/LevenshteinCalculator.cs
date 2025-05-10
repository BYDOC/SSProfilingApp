using SSProfilingApp.Application.Interfaces;

namespace SSProfilingApp.Infrastructure.Helpers;

public class LevenshteinCalculator : ISimilarityCalculator
{
    public Task<double> CalculateAsync(string s1, string s2)
    {
        if (string.IsNullOrWhiteSpace(s1) || string.IsNullOrWhiteSpace(s2))
            return Task.FromResult(0.0);

        int[,] dp = new int[s1.Length + 1, s2.Length + 1];

        for (int i = 0; i <= s1.Length; i++)
            dp[i, 0] = i;
        for (int j = 0; j <= s2.Length; j++)
            dp[0, j] = j;

        for (int i = 1; i <= s1.Length; i++)
        {
            for (int j = 1; j <= s2.Length; j++)
            {
                int cost = s1[i - 1] == s2[j - 1] ? 0 : 1;
                dp[i, j] = Math.Min(
                    Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                    dp[i - 1, j - 1] + cost);
            }
        }

        int distance = dp[s1.Length, s2.Length];
        int maxLength = Math.Max(s1.Length, s2.Length);
        double similarity = maxLength == 0 ? 1.0 : 1.0 - (double)distance / maxLength;

        return Task.FromResult(similarity);
    }
}
