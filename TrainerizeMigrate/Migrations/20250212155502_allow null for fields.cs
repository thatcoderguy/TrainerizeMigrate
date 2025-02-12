using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class allownullforfields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "workoutId",
                table: "TrainingSessionWorkout",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "newdailyWorkoutId",
                table: "TrainingSessionWorkout",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "newdailyExerciseID",
                table: "TrainingSessionStat",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "newdailyWorkoutId",
                table: "TrainingSessionWorkout");

            migrationBuilder.DropColumn(
                name: "newdailyExerciseID",
                table: "TrainingSessionStat");

            migrationBuilder.AlterColumn<int>(
                name: "workoutId",
                table: "TrainingSessionWorkout",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
