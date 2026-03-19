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
    public class CheckInsController : ControllerBase
    {
        private readonly ICheckInService _service;

        public CheckInsController(ICheckInService service)
        {
            _service = service;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        /// <summary>
        /// Get own check-ins (Client)
        /// </summary>
        [HttpGet("my")]
        public async Task<IActionResult> GetMy()
        {
            var checkIns = await _service.GetByClientIdAsync(GetUserId());
            return Ok(checkIns);
        }

        /// <summary>
        /// Get check-ins for a specific client (Coach)
        /// </summary>
        [HttpGet("client/{clientId}")]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> GetByClient(string clientId)
        {
            var checkIns = await _service.GetByClientIdAsync(clientId);
            return Ok(checkIns);
        }

        /// <summary>
        /// Submit a check-in (Client only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Create([FromBody] CreateCheckInDto dto)
        {
            var result = await _service.CreateAsync(GetUserId(), dto);
            return CreatedAtAction(nameof(GetMy), new { id = result.Id }, result);
        }
    }
}
