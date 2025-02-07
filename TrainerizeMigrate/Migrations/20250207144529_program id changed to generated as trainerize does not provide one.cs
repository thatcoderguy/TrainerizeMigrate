using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class programidchangedtogeneratedastrainerizedoesnotprovideone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "new_id",
                table: "TrainingProgram");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "new_id",
                table: "TrainingProgram",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
