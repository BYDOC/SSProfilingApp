using Microsoft.AspNetCore.Mvc;
using SSProfilingApp.Application.Interfaces;
using SSProfilingApp.Application.Requests;
using SSProfilingApp.Application.Responses;
using SSProfilingApp.Infrastructure.Helpers;

namespace SSProfilingApp.API.Controllers;

[ApiController]
[Route("api/similarity")]
public class SimilarityController : ControllerBase
{
    private readonly ISimilarityCalculator _jaroWinkler;

    public SimilarityController(JaroWinklerApiClient jaroWinkler)
    {
        _jaroWinkler = jaroWinkler;
    }

    [HttpPost("jarowinkler")]
    public async Task<ActionResult<SimilarityResponse>> Post([FromBody] SimilarityRequest request)
    {
        var score = await _jaroWinkler.CalculateAsync(request.String1, request.String2);
        return Ok(new SimilarityResponse { Score = score });
    }
}
