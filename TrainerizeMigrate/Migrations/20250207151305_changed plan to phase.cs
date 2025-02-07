using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class changedplantophase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingProgramPlan");

            migrationBuilder.CreateTable(
                name: "TrainingProgramPhase",
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
                    table.PrimaryKey("PK_TrainingProgramPhase", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingProgramPhase");

            migrationBuilder.CreateTable(
                name: "TrainingProgramPlan",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    endDate = table.Column<string>(type: "TEXT", nullable: false),
                    instruction = table.Column<string>(type: "TEXT", nullable: false),
                    modified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    new_id = table.Column<int>(type: "INTEGER", nullable: false),
                    planType = table.Column<string>(type: "TEXT", nullable: false),
                    startDate = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingProgramPlan", x => x.id);
                });
        }
    }
}
