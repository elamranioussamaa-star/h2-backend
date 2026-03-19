using H2_Trainning.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace H2_Trainning.Models
{
    public class AppUser : IdentityUser
    {
        [Required, MaxLength(100)]
        public string FullName { get; set; }
        [Required]
        public Role Role { get; set; }       // enum: Coach, Client
        [MaxLength(200)]
        public string? Goal { get; set; }    // clients only
        public string? CoachId { get; set; } // FK → coach that owns this client
        public AppUser? Coach { get; set; }

        public ICollection<AppUser> Clients { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<AvailabilitySlot> AvailabilitySlots { get; set; }
        public ICollection<Reservation> ClientReservations { get; set; }
        public ICollection<Reservation> CoachReservations { get; set; }
    }
}
