using H2_Trainning.Dtos;

namespace H2_Trainning.Interfaces
{
    public interface IPostService
    {
        Task<List<PostDto>> GetAllAsync();
        Task<List<PostDto>> GetByCoachIdAsync(string coachId);
        Task<PostDto?> GetByIdAsync(int id);
        Task<PostDto> CreateAsync(string coachId, CreatePostDto dto);
        Task<PostDto?> UpdateAsync(int id, string coachId, UpdatePostDto dto);
        Task<bool> DeleteAsync(int id, string coachId);
        
        Task<PostDto?> ToggleLikeAsync(int postId, string userId);
        Task<PostCommentDto?> AddCommentAsync(int postId, string userId, string content);
        Task<bool> DeleteCommentAsync(int postId, int commentId, string userId);
    }
}
