using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class changedimportfieldtonewidfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imported",
                table: "Body_Weight_Point");

            migrationBuilder.AddColumn<int>(
                name: "newbodystatid",
                table: "Body_Weight_Point",
                type: "INTEGER",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "newbodystatid",
                table: "Body_Weight_Point");

            migrationBuilder.AddColumn<bool>(
                name: "imported",
                table: "Body_Weight_Point",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
