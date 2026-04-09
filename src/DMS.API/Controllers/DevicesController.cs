using DMS.BLL.DTOs.AI;
using DMS.BLL.DTOs.Assignment;
using DMS.BLL.DTOs.Device;
using DMS.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DMS.API.Controllers;

[ApiController]
[Route("api/devices")]
[Authorize]
public class DevicesController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly IDeviceDescriptionService _deviceDescriptionService;

    public DevicesController(IDeviceService deviceService, IDeviceDescriptionService deviceDescriptionService)
    {
        _deviceService = deviceService;
        _deviceDescriptionService = deviceDescriptionService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(PagedResponseDto<DeviceResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaged([FromQuery] DeviceListRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _deviceService.GetPagedAsync(request, cancellationToken);
        Response.Headers["X-Total-Count"] = result.TotalCount.ToString();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DeviceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _deviceService.GetByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DeviceResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateDeviceRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _deviceService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(DeviceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDeviceRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _deviceService.UpdateAsync(id, request, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _deviceService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/assign")]
    [ProducesResponseType(typeof(DeviceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AssignToSelf(Guid id, [FromBody] SelfAssignDeviceRequestDto request, CancellationToken cancellationToken)
    {
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _deviceService.AssignToSelfAsync(id, currentUserId, request.Notes, cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/unassign")]
    [ProducesResponseType(typeof(DeviceResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UnassignFromSelf(Guid id, CancellationToken cancellationToken)
    {
        var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await _deviceService.UnassignFromSelfAsync(id, currentUserId, cancellationToken);
        return Ok(result);
    }

    [HttpPost("generate-description")]
    [ProducesResponseType(typeof(GenerateDescriptionResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GenerateDescription([FromBody] GenerateDescriptionRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _deviceDescriptionService.GenerateAsync(request, cancellationToken);
        return Ok(result);
    }
}