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
        
        try {
            var program = db.Programs
                .Include(p => p.Days.OrderBy(d => d.DayNumber))
                    .ThenInclude(d => d.Exercises.OrderBy(e => e.SortOrder))
                .Include(p => p.Days.OrderBy(d => d.DayNumber))
                    .ThenInclude(d => d.Meals.OrderBy(m => m.SortOrder))
                .AsSplitQuery()
                .FirstOrDefault();

            if (program != null) {
                Console.WriteLine("Program found: " + program.Title + " with " + program.Days.Count + " days.");
            } else {
                Console.WriteLine("No programs in DB");
            }
        } catch (Exception ex) {
            Console.WriteLine("EXCEPTION OCCURRED:");
            Console.WriteLine(ex.ToString());
        }
    }
}
