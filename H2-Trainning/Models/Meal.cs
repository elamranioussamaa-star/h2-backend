using System.ComponentModel.DataAnnotations;

namespace H2_Trainning.Models
{
    public class Meal
    {
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string Name { get; set; }
        [Required, MaxLength(100)]
        public string Macros { get; set; }     // "P: 30g, C: 40g, F: 10g"
        [Required, MaxLength(50)]
        public string Time { get; set; }       // "Breakfast", "12:00 PM"
        public string? PhotoUrl { get; set; }  // relative path to uploaded file
        public string? Ingredients { get; set; }
        public int ProgramId { get; set; }     // FK
        public Program Program { get; set; }
        public int SortOrder { get; set; }
    }
}
