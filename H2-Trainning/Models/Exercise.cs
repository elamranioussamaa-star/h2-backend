using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;

namespace H2_Trainning.Models
{
    public enum MediaType { Image, Video }

    public class Exercise
    {
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string Name { get; set; }
        [Required, Range(1, 20)]
        public int Sets { get; set; }
        [Required, MaxLength(30)]
        public string Reps { get; set; }
        [MaxLength(500)]
        public string? Notes { get; set; }
        public string? MediaUrl { get; set; }
        public MediaType? MediaType { get; set; } // Daba ghadi i-t-3ref
        public int ProgramDayId { get; set; }
        public ProgramDay ProgramDay { get; set; }
        public int SortOrder { get; set; }
    }

}
