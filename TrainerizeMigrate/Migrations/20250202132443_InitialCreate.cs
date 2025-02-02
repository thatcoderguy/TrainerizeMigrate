using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Body_Weight",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    unit = table.Column<string>(type: "TEXT", nullable: false),
                    goal = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Body_Weight", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Body_Weight_Point",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    date = table.Column<string>(type: "TEXT", nullable: false),
                    value = table.Column<double>(type: "REAL", nullable: false),
                    BodyWeightId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Body_Weight_Point", x => x.id);
                    table.ForeignKey(
                        name: "FK_Body_Weight_Point_Body_Weight_BodyWeightId",
                        column: x => x.BodyWeightId,
                        principalTable: "Body_Weight",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Body_Weight_Point_BodyWeightId",
                table: "Body_Weight_Point",
                column: "BodyWeightId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Body_Weight_Point");

            migrationBuilder.DropTable(
                name: "Body_Weight");
        }
    }
}
