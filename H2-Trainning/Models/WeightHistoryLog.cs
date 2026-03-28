using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace H2_Trainning.Models
{
    public class WeightHistoryLog
    {
        public int Id { get; set; }

        [Required]
        public string ClientId { get; set; }
        [JsonIgnore]
        public AppUser Client { get; set; }

        [Required]
        public int ExerciseId { get; set; }
        [JsonIgnore]
        public Exercise Exercise { get; set; }

        public double Weight { get; set; }
        public DateTime LoggedAt { get; set; }
        
        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}
