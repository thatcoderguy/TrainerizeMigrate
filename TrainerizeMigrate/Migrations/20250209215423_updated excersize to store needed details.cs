using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class updatedexcersizetostoreneededdetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "accessLevel",
                table: "TrainingPlanWorkout");

            migrationBuilder.AddColumn<int>(
                name: "superSetID",
                table: "WorkoutExcersize",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "targetDetailText",
                table: "WorkoutExcersize",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "targetDetailTime",
                table: "WorkoutExcersize",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "targetDetailType",
                table: "WorkoutExcersize",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "superSetID",
                table: "WorkoutExcersize");

            migrationBuilder.DropColumn(
                name: "targetDetailText",
                table: "WorkoutExcersize");

            migrationBuilder.DropColumn(
                name: "targetDetailTime",
                table: "WorkoutExcersize");

            migrationBuilder.DropColumn(
                name: "targetDetailType",
                table: "WorkoutExcersize");

            migrationBuilder.AddColumn<string>(
                name: "accessLevel",
                table: "TrainingPlanWorkout",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
