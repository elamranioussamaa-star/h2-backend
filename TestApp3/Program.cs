using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using H2_Trainning.Data;
using H2_Trainning.Models;

class Program
{
    static void Main()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer("Server=tcp:h2-project.database.windows.net,1433;Initial Catalog=free-sql-db-8400713;Persist Security Info=False;User ID=h2project;Password=Oussama2002@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;")
            .Options;
            
        using var db = new ApplicationDbContext(options);
        var coach = db.Users.FirstOrDefault(u => u.Role == "Coach");
        if (coach == null) {
            Console.WriteLine("FAIL: No coach found");
            return;
        }

        var prog = new H2_Trainning.Models.Program {
            Title = "Test Validation",
            CoachId = coach.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Days = new List<ProgramDay> {
                new ProgramDay {
                    Name = "Day 1",
                    DayNumber = 1,
                    IsRestDay = false,
                    Exercises = new List<Exercise> {
                        new Exercise { Name = "Sq", Sets = 3, Reps = "10", SortOrder = 0, MediaType = MediaType.Image }
                    },
                    Meals = new List<Meal> {
                        new Meal { Name = "M1", Macros = "P30", Time = "8AM", SortOrder = 0 }
                    }
                }
            }
        };

        try {
            db.Programs.Add(prog);
            db.SaveChanges();
            Console.WriteLine("SUCCESS DB INSERT");
            db.Programs.Remove(prog);
            db.SaveChanges();
        } catch (Exception ex) {
            Console.WriteLine(ex.ToString());
        }
    }
}
