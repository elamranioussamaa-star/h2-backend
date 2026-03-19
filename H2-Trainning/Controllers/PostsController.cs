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
    public class PostsController : ControllerBase
    {
        private readonly IPostService _service;

        public PostsController(IPostService service)
        {
            _service = service;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        /// <summary>
        /// Get all posts (accessible by both coach and client)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _service.GetAllAsync();
            return Ok(posts);
        }

        /// <summary>
        /// Create a new post (Coach only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Create([FromBody] CreatePostDto dto)
        {
            var result = await _service.CreateAsync(GetUserId(), dto);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update a post (Coach only, must own the post)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePostDto dto)
        {
            var result = await _service.UpdateAsync(id, GetUserId(), dto);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>
        /// Delete a post (Coach only, must own the post)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Coach")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id, GetUserId());
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Toggle like on a post (Coach and Client)
        /// </summary>
        [HttpPost("{id}/like")]
        public async Task<IActionResult> ToggleLike(int id)
        {
            var result = await _service.ToggleLikeAsync(id, GetUserId());
            if (result == null) return NotFound("Post not found");
            return Ok(result);
        }

        /// <summary>
        /// Add a comment to a post (Coach and Client)
        /// </summary>
        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] CreatePostCommentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest("Comment cannot be empty");

            var result = await _service.AddCommentAsync(id, GetUserId(), dto.Content);
            if (result == null) return NotFound("Post not found");
            return Ok(result);
        }

        /// <summary>
        /// Delete a comment (Coach and Client, must own the comment)
        /// </summary>
        [HttpDelete("{postId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComment(int postId, int commentId)
        {
            var success = await _service.DeleteCommentAsync(postId, commentId, GetUserId());
            if (!success) return Forbid("You are not authorized to delete this comment or it does not exist.");
            return NoContent();
        }
    }
}
