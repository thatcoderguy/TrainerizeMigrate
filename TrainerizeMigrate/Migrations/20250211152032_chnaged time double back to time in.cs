using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class chnagedtimedoublebacktotimein : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "targetDetailTime",
                table: "WorkoutExcersize",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "REAL",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "intervalTime",
                table: "WorkoutExcersize",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "TrainingPlanWorkout",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "intervalTime",
                table: "WorkoutExcersize");

            migrationBuilder.DropColumn(
                name: "type",
                table: "TrainingPlanWorkout");

            migrationBuilder.AlterColumn<double>(
                name: "targetDetailTime",
                table: "WorkoutExcersize",
                type: "REAL",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
