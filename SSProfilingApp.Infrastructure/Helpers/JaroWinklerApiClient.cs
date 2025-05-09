using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using SSProfilingApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace SSProfilingApp.Infrastructure.Helpers;

public class JaroWinklerApiClient : ISimilarityCalculator
{
    private readonly HttpClient _httpClient;

    public JaroWinklerApiClient(HttpClient httpClient, IConfiguration configuration)
    {
        var baseUrl = configuration["SimilarityApi:BaseUrl"];
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new ArgumentException("SimilarityApi:BaseUrl is not configured in appsettings.json");

        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.tilotech.io/");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    public async Task<double> CalculateAsync(string a, string b)
    {
        try
        {
            var tiloPayload = new
            {
                query = @"query ($comparerConfigurations: [AWSJSON!]!, $token1: String!, $token2: String!) {
                        tiloRes {
                          debugETM(debugETMInput: {
                            comparers: {
                              configurations: $comparerConfigurations
                              token1: $token1
                              token2: $token2
                            }
                          }) {
                            comparers {
                              id
                              details
                            }
                          }
                        }
                      }",
                variables = new
                {
                    comparerConfigurations = new[]
                    {
                    "{\"id\":\"Jaro Winkler Similarity\",\"type\":\"similarity\",\"attributes\":{\"algorithm\":\"jaroWinkler\",\"threshold\":0.8,\"shingleSize\":2}}"
                },
                    token1 = a,
                    token2 = b
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(tiloPayload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("", content);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            var json = await JsonDocument.ParseAsync(stream);

            var detailsText = json
                .RootElement
                .GetProperty("data")
                .GetProperty("tiloRes")
                .GetProperty("debugETM")
                .GetProperty("comparers")[0]
                .GetProperty("details")[0]
                .GetString();

            // Extract number from string like "similarity is 0.855"
            if (detailsText != null && detailsText.Contains("similarity is "))
            {
                var numberText = detailsText.Replace("similarity is ", "").Trim();
                if (double.TryParse(numberText, out double score))
                {
                    return score;
                }
            }

            return 0.0; 
        }
        catch
        {
            throw; 
        }
    }

}

