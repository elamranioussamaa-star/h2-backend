using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace H2_Trainning.Migrations
{
    /// <inheritdoc />
    public partial class AddWeeklyPrograms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Programs_ProgramId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Programs_ProgramId",
                table: "Meals");

            migrationBuilder.RenameColumn(
                name: "ProgramId",
                table: "Meals",
                newName: "ProgramDayId");

            migrationBuilder.RenameIndex(
                name: "IX_Meals_ProgramId",
                table: "Meals",
                newName: "IX_Meals_ProgramDayId");

            migrationBuilder.RenameColumn(
                name: "ProgramId",
                table: "Exercises",
                newName: "ProgramDayId");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_ProgramId",
                table: "Exercises",
                newName: "IX_Exercises_ProgramDayId");

            migrationBuilder.CreateTable(
                name: "ProgramDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    IsRestDay = table.Column<bool>(type: "bit", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramDays_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramDays_ProgramId",
                table: "ProgramDays",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_ProgramDays_ProgramDayId",
                table: "Exercises",
                column: "ProgramDayId",
                principalTable: "ProgramDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_ProgramDays_ProgramDayId",
                table: "Meals",
                column: "ProgramDayId",
                principalTable: "ProgramDays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_ProgramDays_ProgramDayId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Meals_ProgramDays_ProgramDayId",
                table: "Meals");

            migrationBuilder.DropTable(
                name: "ProgramDays");

            migrationBuilder.RenameColumn(
                name: "ProgramDayId",
                table: "Meals",
                newName: "ProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_Meals_ProgramDayId",
                table: "Meals",
                newName: "IX_Meals_ProgramId");

            migrationBuilder.RenameColumn(
                name: "ProgramDayId",
                table: "Exercises",
                newName: "ProgramId");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_ProgramDayId",
                table: "Exercises",
                newName: "IX_Exercises_ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Programs_ProgramId",
                table: "Exercises",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_Programs_ProgramId",
                table: "Meals",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
