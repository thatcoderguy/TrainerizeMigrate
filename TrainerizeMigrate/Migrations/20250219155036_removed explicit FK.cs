using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class removedexplicitFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExcersize_TrainingPlanWorkout_PlanWorkoutId",
                table: "WorkoutExcersize");

            migrationBuilder.RenameColumn(
                name: "PlanWorkoutId",
                table: "WorkoutExcersize",
                newName: "PlanWorkoutid");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutExcersize_PlanWorkoutId",
                table: "WorkoutExcersize",
                newName: "IX_WorkoutExcersize_PlanWorkoutid");

            migrationBuilder.AlterColumn<int>(
                name: "PlanWorkoutid",
                table: "WorkoutExcersize",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExcersize_TrainingPlanWorkout_PlanWorkoutid",
                table: "WorkoutExcersize",
                column: "PlanWorkoutid",
                principalTable: "TrainingPlanWorkout",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutExcersize_TrainingPlanWorkout_PlanWorkoutid",
                table: "WorkoutExcersize");

            migrationBuilder.RenameColumn(
                name: "PlanWorkoutid",
                table: "WorkoutExcersize",
                newName: "PlanWorkoutId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkoutExcersize_PlanWorkoutid",
                table: "WorkoutExcersize",
                newName: "IX_WorkoutExcersize_PlanWorkoutId");

            migrationBuilder.AlterColumn<int>(
                name: "PlanWorkoutId",
                table: "WorkoutExcersize",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutExcersize_TrainingPlanWorkout_PlanWorkoutId",
                table: "WorkoutExcersize",
                column: "PlanWorkoutId",
                principalTable: "TrainingPlanWorkout",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
