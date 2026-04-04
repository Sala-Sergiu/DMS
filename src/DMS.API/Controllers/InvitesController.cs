using DMS.API.Constants;
using DMS.BLL.DTOs.Invites;
using DMS.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DMS.API.Controllers;

[ApiController]
[Route("api/invites")]
public class InvitesController : ControllerBase
{
    private readonly IInviteService _inviteService;

    public InvitesController(IInviteService inviteService)
    {
        _inviteService = inviteService;
    }

    /// <summary>
    /// Creates an invite for a new user. Admin only.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AdminOnly)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateInvite([FromBody] CreateInviteRequestDto request, CancellationToken cancellationToken)
    {
        var adminId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")!);

        var result = await _inviteService.CreateInviteAsync(request, adminId, cancellationToken);
        return CreatedAtAction(nameof(CreateInvite), new { id = result.Id }, result);
    }

    /// <summary>
    /// Accepts an invite and creates the user account. Public endpoint.
    /// </summary>
    [HttpPost("accept")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AcceptInvite([FromBody] AcceptInviteRequestDto request, CancellationToken cancellationToken)
    {
        var result = await _inviteService.AcceptInviteAsync(request, cancellationToken);
        return CreatedAtAction(nameof(UsersController.GetById), "Users", new { id = result.Id }, result);
    }
}