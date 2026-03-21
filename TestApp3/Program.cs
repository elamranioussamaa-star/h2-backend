using System;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main()
    {
        string connStr = "Server=tcp:h2-project.database.windows.net,1433;Initial Catalog=free-sql-db-8400713;Persist Security Info=False;User ID=h2project;Password=Oussama2002@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;";
        using var conn = new SqlConnection(connStr);
        conn.Open();

        var sqls = new[] {
            "DELETE FROM Exercises",
            "DELETE FROM Meals",
            "ALTER TABLE [Exercises] DROP CONSTRAINT IF EXISTS [FK_Exercises_Programs_ProgramId]",
            "ALTER TABLE [Meals] DROP CONSTRAINT IF EXISTS [FK_Meals_Programs_ProgramId]",
            
            // Just in case it exists already somehow
            "IF OBJECT_ID('ProgramDays') IS NULL BEGIN CREATE TABLE [ProgramDays] ( [Id] int NOT NULL IDENTITY, [Name] nvarchar(100) NOT NULL, [DayNumber] int NOT NULL, [IsRestDay] bit NOT NULL, [ProgramId] int NOT NULL, CONSTRAINT [PK_ProgramDays] PRIMARY KEY ([Id]), CONSTRAINT [FK_ProgramDays_Programs_ProgramId] FOREIGN KEY ([ProgramId]) REFERENCES [Programs] ([Id]) ON DELETE CASCADE ); END",

            // Fix the column names in Exercises and Meals. Check if they exist first.
            "IF COL_LENGTH('Exercises', 'ProgramId') IS NOT NULL BEGIN EXEC sp_rename 'Exercises.ProgramId', 'ProgramDayId', 'COLUMN'; END",
            "IF COL_LENGTH('Meals', 'ProgramId') IS NOT NULL BEGIN EXEC sp_rename 'Meals.ProgramId', 'ProgramDayId', 'COLUMN'; END",

            // Re-apply FKs
            "ALTER TABLE [Exercises] DROP CONSTRAINT IF EXISTS [FK_Exercises_ProgramDays_ProgramDayId]",
            "ALTER TABLE [Exercises] ADD CONSTRAINT [FK_Exercises_ProgramDays_ProgramDayId] FOREIGN KEY ([ProgramDayId]) REFERENCES [ProgramDays] ([Id]) ON DELETE CASCADE",
            
            "ALTER TABLE [Meals] DROP CONSTRAINT IF EXISTS [FK_Meals_ProgramDays_ProgramDayId]",
            "ALTER TABLE [Meals] ADD CONSTRAINT [FK_Meals_ProgramDays_ProgramDayId] FOREIGN KEY ([ProgramDayId]) REFERENCES [ProgramDays] ([Id]) ON DELETE CASCADE"
        };

        foreach (var sql in sqls) {
            try {
                using var cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("SUCCESS: " + sql);
            } catch (Exception ex) {
                Console.WriteLine("ERROR: " + sql);
                Console.WriteLine(ex.Message);
            }
        }
    }
}
