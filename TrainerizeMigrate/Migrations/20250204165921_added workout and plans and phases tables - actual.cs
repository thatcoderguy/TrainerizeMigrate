using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class addedworkoutandplansandphasestablesactual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingPlanWorkout",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    instruction = table.Column<string>(type: "TEXT", nullable: false),
                    accessLevel = table.Column<string>(type: "TEXT", nullable: false),
                    new_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingPlanWorkout", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingProgram",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    durationType = table.Column<string>(type: "TEXT", nullable: false),
                    subscriptionType = table.Column<string>(type: "TEXT", nullable: false),
                    startDate = table.Column<string>(type: "TEXT", nullable: false),
                    endDate = table.Column<string>(type: "TEXT", nullable: false),
                    accessLevel = table.Column<string>(type: "TEXT", nullable: false),
                    new_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingProgram", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingProgramPlan",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    instruction = table.Column<string>(type: "TEXT", nullable: false),
                    startDate = table.Column<string>(type: "TEXT", nullable: false),
                    endDate = table.Column<string>(type: "TEXT", nullable: false),
                    planType = table.Column<string>(type: "TEXT", nullable: false),
                    modified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    new_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingProgramPlan", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutExcersize",
                columns: table => new
                {
                    SystemId = table.Column<Guid>(type: "TEXT", nullable: false),
                    id = table.Column<int>(type: "INTEGER", nullable: false),
                    sets = table.Column<int>(type: "INTEGER", nullable: false),
                    target = table.Column<string>(type: "TEXT", nullable: false),
                    restTime = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanWorkoutid = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutExcersize", x => x.SystemId);
                    table.ForeignKey(
                        name: "FK_WorkoutExcersize_TrainingPlanWorkout_PlanWorkoutid",
                        column: x => x.PlanWorkoutid,
                        principalTable: "TrainingPlanWorkout",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutExcersize_PlanWorkoutid",
                table: "WorkoutExcersize",
                column: "PlanWorkoutid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingProgram");

            migrationBuilder.DropTable(
                name: "TrainingProgramPlan");

            migrationBuilder.DropTable(
                name: "WorkoutExcersize");

            migrationBuilder.DropTable(
                name: "TrainingPlanWorkout");
        }
    }
}
