using System.ComponentModel.DataAnnotations;

namespace H2_Trainning.Models
{
    public class AvailabilitySlot
    {
        public int Id { get; set; }
        [Required]
        public string CoachId { get; set; }     // FK
        public AppUser Coach { get; set; }
        [Required]
        public DateOnly Date { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        public bool IsBooked { get; set; } = false;
        public Reservation? Reservation { get; set; }
    }
}
