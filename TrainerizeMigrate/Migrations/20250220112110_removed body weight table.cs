using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class removedbodyweighttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Body_Stat_Point_Body_Weight_BodyWeightId",
                table: "Body_Stat_Point");

            migrationBuilder.DropTable(
                name: "Body_Weight");

            migrationBuilder.DropIndex(
                name: "IX_Body_Stat_Point_BodyWeightId",
                table: "Body_Stat_Point");

            migrationBuilder.DropColumn(
                name: "BodyWeightId",
                table: "Body_Stat_Point");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BodyWeightId",
                table: "Body_Stat_Point",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Body_Weight",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    goal = table.Column<string>(type: "TEXT", nullable: true),
                    unit = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Body_Weight", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Body_Stat_Point_BodyWeightId",
                table: "Body_Stat_Point",
                column: "BodyWeightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Body_Stat_Point_Body_Weight_BodyWeightId",
                table: "Body_Stat_Point",
                column: "BodyWeightId",
                principalTable: "Body_Weight",
                principalColumn: "Id");
        }
    }
}
