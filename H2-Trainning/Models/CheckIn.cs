using System.ComponentModel.DataAnnotations;

namespace H2_Trainning.Models
{
    public class CheckIn
    {
        public int Id { get; set; }

        [Required]
        public string ClientId { get; set; }
        public AppUser Client { get; set; }

        public decimal? Weight { get; set; }

        [Range(1, 5)]
        public int SleepQuality { get; set; }

        [Range(1, 5)]
        public int EnergyLevel { get; set; }

        [Range(1, 5)]
        public int Mood { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
