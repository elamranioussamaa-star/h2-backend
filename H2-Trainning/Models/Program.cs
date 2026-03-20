using System.ComponentModel.DataAnnotations;

namespace H2_Trainning.Models
{
    public class Program
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string? NutritionalBases { get; set; }

        [Required]
        public string CoachId { get; set; }
        public AppUser Coach { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<ProgramDay> Days { get; set; } = new List<ProgramDay>();
        public ICollection<Assignment> Assignments { get; set; }
    }
}