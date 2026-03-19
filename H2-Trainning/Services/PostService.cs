using H2_Trainning.Dtos;
using H2_Trainning.Interfaces;
using H2_Trainning.Models;

namespace H2_Trainning.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repo;
        private readonly H2_Trainning.Data.ApplicationDbContext _context;

        public PostService(IPostRepository repo, H2_Trainning.Data.ApplicationDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<List<PostDto>> GetAllAsync()
        {
            var posts = await _repo.GetAllAsync();
            return posts.Select(MapToDto).ToList();
        }

        public async Task<List<PostDto>> GetByCoachIdAsync(string coachId)
        {
            var posts = await _repo.GetByCoachIdAsync(coachId);
            return posts.Select(MapToDto).ToList();
        }

        public async Task<PostDto?> GetByIdAsync(int id)
        {
            var post = await _repo.GetByIdAsync(id);
            return post == null ? null : MapToDto(post);
        }

        public async Task<PostDto> CreateAsync(string coachId, CreatePostDto dto)
        {
            var post = new Post
            {
                CoachId = coachId,
                Title = dto.Title,
                Content = dto.Content,
                ImageUrl = dto.ImageUrl,
                Category = dto.Category,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _repo.CreateAsync(post);
            return MapToDto(created);
        }

        public async Task<PostDto?> UpdateAsync(int id, string coachId, UpdatePostDto dto)
        {
            var post = await _repo.GetByIdAsync(id);
            if (post == null || post.CoachId != coachId) return null;

            post.Title = dto.Title;
            post.Content = dto.Content;
            post.ImageUrl = dto.ImageUrl;
            post.Category = dto.Category;
            post.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(post);
            return MapToDto(post);
        }

        public async Task<bool> DeleteAsync(int id, string coachId)
        {
            var post = await _repo.GetByIdAsync(id);
            if (post == null || post.CoachId != coachId) return false;

            return await _repo.DeleteAsync(id);
        }

        public async Task<PostDto?> ToggleLikeAsync(int postId, string userId)
        {
            var post = await _repo.GetByIdAsync(postId);
            if (post == null) return null;

            var existingLike = post.Likes.FirstOrDefault(l => l.UserId == userId);
            
            if (existingLike != null)
            {
                _context.PostLikes.Remove(existingLike);
            }
            else
            {
                _context.PostLikes.Add(new PostLike { PostId = postId, UserId = userId });
            }

            await _context.SaveChangesAsync();
            
            // Re-fetch to get the full updated object with nested user info
            var updatedPost = await _repo.GetByIdAsync(postId);
            return updatedPost == null ? null : MapToDto(updatedPost);
        }

        public async Task<PostCommentDto?> AddCommentAsync(int postId, string userId, string content)
        {
            var post = await _repo.GetByIdAsync(postId);
            if (post == null) return null;

            var comment = new PostComment
            {
                PostId = postId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.PostComments.Add(comment);
            await _context.SaveChangesAsync();

            // Fetch the user to populate the DTO correctly
            var user = await _context.Users.FindAsync(userId);

            return new PostCommentDto
            {
                Id = comment.Id,
                PostId = comment.PostId,
                UserId = comment.UserId,
                UserName = user?.FullName ?? "",
                Content = comment.Content,
                CreatedAt = comment.CreatedAt
            };
        }

        public async Task<bool> DeleteCommentAsync(int postId, int commentId, string userId)
        {
            var comment = await _context.PostComments.FindAsync(commentId);
            
            // Allow deletion only if the comment belongs to the specified post, and the user owns the comment
            if (comment == null || comment.PostId != postId || comment.UserId != userId) 
                return false;

            _context.PostComments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        private static PostDto MapToDto(Post p) => new()
        {
            Id = p.Id,
            CoachId = p.CoachId,
            CoachName = p.Coach?.FullName ?? "",
            Title = p.Title,
            Content = p.Content,
            ImageUrl = p.ImageUrl,
            Category = p.Category,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            Likes = p.Likes?.Select(l => new PostLikeDto
            {
                Id = l.Id,
                PostId = l.PostId,
                UserId = l.UserId,
                UserName = l.User?.FullName ?? ""
            }).ToList() ?? new List<PostLikeDto>(),
            Comments = p.Comments?.OrderBy(c => c.CreatedAt).Select(c => new PostCommentDto
            {
                Id = c.Id,
                PostId = c.PostId,
                UserId = c.UserId,
                UserName = c.User?.FullName ?? "",
                Content = c.Content,
                CreatedAt = c.CreatedAt
            }).ToList() ?? new List<PostCommentDto>()
        };
    }
}
