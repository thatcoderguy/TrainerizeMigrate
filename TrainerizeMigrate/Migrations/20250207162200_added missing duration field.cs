using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class addedmissingdurationfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "duration",
                table: "TrainingProgramPhase",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duration",
                table: "TrainingProgramPhase");
        }
    }
}
