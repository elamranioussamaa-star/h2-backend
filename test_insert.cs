using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using H2_Trainning.Data;
using H2_Trainning.Models;

class ProgramTest
{
    static void Main(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer("Server=tcp:h2-project.database.windows.net,1433;Initial Catalog=free-sql-db-8400713;Persist Security Info=False;User ID=h2project;Password=Oussama2002@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
        
        using var context = new ApplicationDbContext(optionsBuilder.Options);
        
        try 
        {
            // Find a valid coach ID
            var coach = context.Users.FirstOrDefault(u => u.Role == "Coach");
            if (coach == null) 
            {
                Console.WriteLine("No coach found.");
                return;
            }

            var program = new H2_Trainning.Models.Program
            {
                Title = "Test EF Program",
                CoachId = coach.Id,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Days = new List<ProgramDay>
                {
                    new ProgramDay
                    {
                        Name = "Test Day",
                        DayNumber = 1,
                        IsRestDay = false,
                        Exercises = new List<Exercise>(),
                        Meals = new List<Meal>()
                    }
                }
            };

            context.Programs.Add(program);
            context.SaveChanges();
            Console.WriteLine("Successfully saved program!");
            
            // Cleanup
            context.Programs.Remove(program);
            context.SaveChanges();
        } 
        catch (Exception ex) 
        {
            Console.WriteLine("EXCEPTION OCCURRED: " + ex.ToString());
        }
    }
}
