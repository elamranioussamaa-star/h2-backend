using System.ComponentModel.DataAnnotations;

namespace H2_Trainning.Models
{
    public class ProgramDay
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } // e.g., "Day 1", "Day Off", "Upper Body"

        public int DayNumber { get; set; } // e.g., 1, 2, 3...
        
        public bool IsRestDay { get; set; } // True if this is a rest day

        [Required]
        public int ProgramId { get; set; }
        public Program Program { get; set; }

        public ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
        public ICollection<Meal> Meals { get; set; } = new List<Meal>();
    }
}
