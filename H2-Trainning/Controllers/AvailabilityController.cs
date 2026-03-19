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
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService _service;

        public AvailabilityController(IAvailabilityService service)
        {
            _service = service;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        /// <summary>
        /// Get slots for a specific coach. If no coachId is provided, returns the authenticated coach's slots.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSlots([FromQuery] string? coachId = null)
        {
            var targetCoachId = coachId ?? GetUserId();
            var slots = await _service.GetByCoachIdAsync(targetCoachId);
            return Ok(slots);
        }

        /// <summary>
        /// Get all available (un-booked) slots across all coaches.
        /// </summary>
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var slots = await _service.GetAllAvailableAsync();
            return Ok(slots);
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Create([FromBody] CreateSlotDto dto)
        {
            try
            {
                var result = await _service.CreateAsync(GetUserId(), dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id, GetUserId());
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
