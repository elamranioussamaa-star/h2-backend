using System.ComponentModel.DataAnnotations;

namespace H2_Trainning.Dtos
{
    public class UpdateExerciseLogDto
    {
        [Required]
        public bool IsCompleted { get; set; }
        
        [MaxLength(1000)]
        public string? ClientNotes { get; set; }

        public double? Weight { get; set; }
    }
}
