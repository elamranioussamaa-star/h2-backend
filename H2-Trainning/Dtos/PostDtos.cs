namespace H2_Trainning.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public string CoachId { get; set; }
        public string CoachName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<PostLikeDto> Likes { get; set; } = new();
        public List<PostCommentDto> Comments { get; set; } = new();
    }

    public class PostLikeDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }

    public class PostCommentDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreatePostCommentDto
    {
        public string Content { get; set; }
    }

    public class CreatePostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string Category { get; set; } = "General";
    }

    public class UpdatePostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public string Category { get; set; }
    }
}
