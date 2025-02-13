using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class addedexcersizefieldtomatchup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "excersizeId",
                table: "TrainingSessionStat",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "excersizeId",
                table: "TrainingSessionStat");
        }
    }
}
