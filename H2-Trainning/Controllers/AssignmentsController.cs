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
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _service;

        public AssignmentsController(IAssignmentService service)
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

            var assignments = role == "Coach"
                ? await _service.GetByCoachIdAsync(userId)
                : await _service.GetByClientIdAsync(userId);

            return Ok(assignments);
        }

        [HttpPost]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Create([FromBody] CreateAssignmentDto dto)
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

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateAssignmentStatusDto dto)
        {
            try
            {
                var result = await _service.UpdateStatusAsync(id, dto);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("{assignmentId}/exercises/{exerciseId}/log")]
        public async Task<IActionResult> UpdateExerciseLog(int assignmentId, int exerciseId, [FromBody] UpdateExerciseLogDto dto)
        {
            try
            {
                var result = await _service.UpdateExerciseLogAsync(assignmentId, exerciseId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("client/{clientId}/exercise/{exerciseId}/weight-history")]
        public async Task<IActionResult> GetExerciseWeightHistory(string clientId, int exerciseId)
        {
            try
            {
                var result = await _service.GetExerciseWeightHistoryAsync(clientId, exerciseId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
