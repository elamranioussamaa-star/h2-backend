using System.Security.Claims;
using H2_Trainning.Dtos;
using H2_Trainning.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace H2_Trainning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationsController(IReservationService service)
        {
            _service = service;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        private string GetUserRole() => User.FindFirstValue(ClaimTypes.Role)!;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var role = GetUserRole();
            var userId = GetUserId();

            var reservations = role == "Coach"
                ? await _service.GetByCoachIdAsync(userId)
                : await _service.GetByClientIdAsync(userId);

            return Ok(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> Book([FromBody] CreateReservationDto dto)
        {
            try
            {
                var result = await _service.BookAsync(GetUserId(), dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/confirm")]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Confirm(int id)
        {
            var result = await _service.ConfirmAsync(id, GetUserId());
            if (result == null) return NotFound(new { message = "Reservation not found or not pending." });
            return Ok(result);
        }

        [HttpPatch("{id}/reject")]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Reject(int id)
        {
            var result = await _service.RejectAsync(id, GetUserId());
            if (result == null) return NotFound(new { message = "Reservation not found or not pending." });
            return Ok(result);
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var cancelled = await _service.CancelAsync(id, GetUserId());
            if (!cancelled) return NotFound();
            return Ok(new { message = "Reservation cancelled." });
        }
    }
}
