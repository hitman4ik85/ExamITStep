using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelRoomReserved.Core.Interfaces;
using HotelRoomReserved.Core.DTOs;
using System.Security.Claims;

namespace HotelRoomReserved.API.Controllers;

[Route("api/reservations")]
[ApiController]
[Authorize]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUserReservations(CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized("Invalid token: User ID is missing.");
        }

        if (!int.TryParse(userIdClaim, out int userId))
        {
            return BadRequest("Invalid token: User ID is not a valid number.");
        }

        return Ok(await _reservationService.GetUserReservationsAsync(userId, cancellationToken));
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllReservations(CancellationToken cancellationToken = default)
    {
        return Ok(await _reservationService.GetAllReservationsAsync(cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDTO dto, CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized("Invalid token: User ID is missing.");
        }

        if (!int.TryParse(userIdClaim, out int userId))
        {
            return BadRequest("Invalid token: User ID is not a valid number.");
        }

        return Ok(await _reservationService.CreateReservationAsync(dto, userId, cancellationToken));
    }

    [HttpPut("{reservationId}")]
    public async Task<IActionResult> CancelReservation(int reservationId, CancellationToken cancellationToken = default)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized("Invalid token: User ID is missing.");
        }

        if (!int.TryParse(userIdClaim, out int userId))
        {
            return BadRequest("Invalid token: User ID is not a valid number.");
        }

        await _reservationService.CancelReservationAsync(reservationId, userId, cancellationToken);
        return NoContent();
    }
}
