using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace H2_Trainning.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigrationBeforeUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLog_Assignments_AssignmentId",
                table: "ExerciseLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLog_Exercises_ExerciseId",
                table: "ExerciseLog");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseLog",
                table: "ExerciseLog");

            migrationBuilder.RenameTable(
                name: "ExerciseLog",
                newName: "ExerciseLogs");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseLog_ExerciseId",
                table: "ExerciseLogs",
                newName: "IX_ExerciseLogs_ExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseLog_AssignmentId",
                table: "ExerciseLogs",
                newName: "IX_ExerciseLogs_AssignmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseLogs",
                table: "ExerciseLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseLogs_Assignments_AssignmentId",
                table: "ExerciseLogs",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseLogs_Exercises_ExerciseId",
                table: "ExerciseLogs",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_Assignments_AssignmentId",
                table: "ExerciseLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_Exercises_ExerciseId",
                table: "ExerciseLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseLogs",
                table: "ExerciseLogs");

            migrationBuilder.RenameTable(
                name: "ExerciseLogs",
                newName: "ExerciseLog");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseLogs_ExerciseId",
                table: "ExerciseLog",
                newName: "IX_ExerciseLog_ExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseLogs_AssignmentId",
                table: "ExerciseLog",
                newName: "IX_ExerciseLog_AssignmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseLog",
                table: "ExerciseLog",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseLog_Assignments_AssignmentId",
                table: "ExerciseLog",
                column: "AssignmentId",
                principalTable: "Assignments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseLog_Exercises_ExerciseId",
                table: "ExerciseLog",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
