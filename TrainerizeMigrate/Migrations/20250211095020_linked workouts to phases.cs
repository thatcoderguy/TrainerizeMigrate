using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class linkedworkoutstophases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProgramPhaseid",
                table: "TrainingPlanWorkout",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingPlanWorkout_ProgramPhaseid",
                table: "TrainingPlanWorkout",
                column: "ProgramPhaseid");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingPlanWorkout_TrainingProgramPhase_ProgramPhaseid",
                table: "TrainingPlanWorkout",
                column: "ProgramPhaseid",
                principalTable: "TrainingProgramPhase",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingPlanWorkout_TrainingProgramPhase_ProgramPhaseid",
                table: "TrainingPlanWorkout");

            migrationBuilder.DropIndex(
                name: "IX_TrainingPlanWorkout_ProgramPhaseid",
                table: "TrainingPlanWorkout");

            migrationBuilder.DropColumn(
                name: "ProgramPhaseid",
                table: "TrainingPlanWorkout");
        }
    }
}
