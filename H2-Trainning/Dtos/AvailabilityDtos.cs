using System.ComponentModel.DataAnnotations;

namespace H2_Trainning.Dtos
{
    public class SlotDto
    {
        public int Id { get; set; }
        public string CoachId { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;       // "yyyy-MM-dd"
        public string StartTime { get; set; } = string.Empty;  // "HH:mm"
        public string EndTime { get; set; } = string.Empty;    // "HH:mm"
        public bool IsBooked { get; set; }
    }

    public class CreateSlotDto
    {
        [Required]
        public string Date { get; set; } = string.Empty; // "yyyy-MM-dd"

        [Required]
        public string StartTime { get; set; } = string.Empty; // "HH:mm"

        [Required]
        public string EndTime { get; set; } = string.Empty; // "HH:mm"
    }
}
