using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class changedtablenametomakereferenceseasier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Excerisize_Excersizeid",
                table: "Tag");

            migrationBuilder.RenameColumn(
                name: "Excersizeid",
                table: "Tag",
                newName: "CustomExcersizeid");

            migrationBuilder.RenameIndex(
                name: "IX_Tag_Excersizeid",
                table: "Tag",
                newName: "IX_Tag_CustomExcersizeid");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Excerisize_CustomExcersizeid",
                table: "Tag",
                column: "CustomExcersizeid",
                principalTable: "Excerisize",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Excerisize_CustomExcersizeid",
                table: "Tag");

            migrationBuilder.RenameColumn(
                name: "CustomExcersizeid",
                table: "Tag",
                newName: "Excersizeid");

            migrationBuilder.RenameIndex(
                name: "IX_Tag_CustomExcersizeid",
                table: "Tag",
                newName: "IX_Tag_Excersizeid");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Excerisize_Excersizeid",
                table: "Tag",
                column: "Excersizeid",
                principalTable: "Excerisize",
                principalColumn: "id");
        }
    }
}
