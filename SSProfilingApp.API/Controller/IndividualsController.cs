using Azure.Core;
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
    public async Task<IActionResult> Create([FromBody] List<CreateIndividualRequest> request)
    {
        if (request == null || request.Count == 0)
            return BadRequest("At least one individual must be provided.");

        var id = await _profilingService.AddIndividualAsync(request);
        return Ok(id);
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