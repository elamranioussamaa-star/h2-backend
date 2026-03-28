using System.ComponentModel.DataAnnotations;

namespace H2_Trainning.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int SlotId { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string ClientName { get; set; } = string.Empty;
        public string CoachId { get; set; } = string.Empty;
        public string CoachName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? RejectionReason { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateReservationDto
    {
        [Required]
        public int SlotId { get; set; }
    }
}
