using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class addedtablestostoreworkoutsessions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingSessionWorkout",
                columns: table => new
                {
                    dailyWorkoutId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    workoutId = table.Column<int>(type: "INTEGER", nullable: false),
                    workout = table.Column<string>(type: "TEXT", nullable: true),
                    date = table.Column<string>(type: "TEXT", nullable: true),
                    rpe = table.Column<int>(type: "INTEGER", nullable: true),
                    numOfComments = table.Column<int>(type: "INTEGER", nullable: true),
                    notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSessionWorkout", x => x.dailyWorkoutId);
                });

            migrationBuilder.CreateTable(
                name: "TrainingSessionStat",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    dailyExerciseID = table.Column<long>(type: "INTEGER", nullable: false),
                    setNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    reps = table.Column<int>(type: "INTEGER", nullable: false),
                    weight = table.Column<double>(type: "REAL", nullable: false),
                    distance = table.Column<double>(type: "REAL", nullable: true),
                    time = table.Column<double>(type: "REAL", nullable: true),
                    calories = table.Column<double>(type: "REAL", nullable: true),
                    level = table.Column<double>(type: "REAL", nullable: true),
                    speed = table.Column<double>(type: "REAL", nullable: true),
                    TrainingSessionWorkoutdailyWorkoutId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSessionStat", x => x.id);
                    table.ForeignKey(
                        name: "FK_TrainingSessionStat_TrainingSessionWorkout_TrainingSessionWorkoutdailyWorkoutId",
                        column: x => x.TrainingSessionWorkoutdailyWorkoutId,
                        principalTable: "TrainingSessionWorkout",
                        principalColumn: "dailyWorkoutId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSessionStat_TrainingSessionWorkoutdailyWorkoutId",
                table: "TrainingSessionStat",
                column: "TrainingSessionWorkoutdailyWorkoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingSessionStat");

            migrationBuilder.DropTable(
                name: "TrainingSessionWorkout");
        }
    }
}
