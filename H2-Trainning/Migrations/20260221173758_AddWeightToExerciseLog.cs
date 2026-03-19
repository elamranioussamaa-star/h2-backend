using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace H2_Trainning.Migrations
{
    /// <inheritdoc />
    public partial class AddWeightToExerciseLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Weight",
                table: "ExerciseLogs",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weight",
                table: "ExerciseLogs");
        }
    }
}
