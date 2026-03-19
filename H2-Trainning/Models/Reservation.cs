using H2_Trainning.Enums;
using System.ComponentModel.DataAnnotations;

namespace H2_Trainning.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        [Required]
        public int SlotId { get; set; }        // FK
        public AvailabilitySlot Slot { get; set; }
        [Required]
        public string ClientId { get; set; }    // FK
        public AppUser Client { get; set; }
        [Required]
        public string CoachId { get; set; }     // FK
        public AppUser Coach { get; set; }
        [Required]
        public ReservationStatus Status { get; set; } = ReservationStatus.Confirmed;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

   
}
