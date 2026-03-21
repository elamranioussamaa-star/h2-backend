using System.Security.Claims;
using H2_Trainning.Dtos;
using H2_Trainning.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace H2_Trainning.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Coach")]
    public class ProgramsController : ControllerBase
    {
        private readonly IProgramService _service;

        public ProgramsController(IProgramService service)
        {
            _service = service;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var programs = await _service.GetByCoachIdAsync(GetUserId());
            return Ok(programs);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummaries()
        {
            var programs = await _service.GetSummariesByCoachIdAsync(GetUserId());
            return Ok(programs);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            try {
                var program = await _service.GetByIdAsync(id);
                if (program == null) return NotFound();
                return Ok(program);
            } catch (Exception ex) {
                return Ok("EXCEPTION_GET: " + ex.ToString());
            }
        }

        [HttpGet("debug-db")]
        [AllowAnonymous]
        public IActionResult DebugDb([FromServices] H2_Trainning.Data.ApplicationDbContext db, [FromServices] IConfiguration config)
        {
            var dbName = db.Database.GetDbConnection().Database;
            var connStr = config.GetConnectionString("DefaultConnection");
            return Ok(new { DbName = dbName, HasConnStr = !string.IsNullOrEmpty(connStr) });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateProgramDto dto)
        {
            try 
            {
                var db = HttpContext.RequestServices.GetRequiredService<H2_Trainning.Data.ApplicationDbContext>();
                var coachId = db.Users.FirstOrDefault()?.Id ?? "no-coach";
                var result = await _service.CreateAsync(coachId, dto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                // Return 200 with error so CORS isn't blocked by the browser
                return Ok("EXCEPTION: " + ex.ToString());
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProgramDto dto)
        {
            var result = await _service.UpdateAsync(id, GetUserId(), dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id, GetUserId());
            if (!deleted) return NotFound();
            return NoContent();
        }

    }
}
