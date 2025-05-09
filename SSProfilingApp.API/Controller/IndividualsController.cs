using Microsoft.AspNetCore.Mvc;
using SSProfilingApp.Application.Interfaces;
using SSProfilingApp.Application.Requests;

namespace SSProfilingApp.API.Controllers;

[ApiController]
[Route("[controller]")]
public class IndividualsController : ControllerBase
{
    private readonly IProfilingService _profilingService;
    public IndividualsController(IProfilingService profilingService)
    {
        _profilingService = profilingService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateIndividualRequest request)
    {
        var id = await _profilingService.AddIndividualAsync(request);
        return Ok();
    }

    [HttpGet("with-profiles")]
    public async Task<IActionResult> RecalculateProfiles()
    {
        await _profilingService.GroupIndividualsAsync();
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAll()
    {
        await _profilingService.DeleteAllAsync();
        return Ok("All individuals and related profiles deleted.");
    }

}