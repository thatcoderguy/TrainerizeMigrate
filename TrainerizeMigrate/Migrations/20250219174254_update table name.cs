using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class updatetablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Body_Weight_Point");

            migrationBuilder.CreateTable(
                name: "Body_Stat_Point",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    date = table.Column<string>(type: "TEXT", nullable: true),
                    newbodystatid = table.Column<int>(type: "INTEGER", nullable: true),
                    bodyWeight = table.Column<double>(type: "REAL", nullable: false),
                    restingHeartRate = table.Column<int>(type: "INTEGER", nullable: false),
                    bodyFatPercent = table.Column<double>(type: "REAL", nullable: false),
                    bodyMassIndex = table.Column<double>(type: "REAL", nullable: false),
                    caliperMode = table.Column<int>(type: "INTEGER", nullable: false),
                    chest = table.Column<double>(type: "REAL", nullable: false),
                    shoulders = table.Column<double>(type: "REAL", nullable: false),
                    rightBicep = table.Column<double>(type: "REAL", nullable: false),
                    leftBicep = table.Column<double>(type: "REAL", nullable: false),
                    rightThigh = table.Column<double>(type: "REAL", nullable: false),
                    leftThigh = table.Column<double>(type: "REAL", nullable: false),
                    rightCalf = table.Column<double>(type: "REAL", nullable: false),
                    leftCalf = table.Column<double>(type: "REAL", nullable: false),
                    waist = table.Column<double>(type: "REAL", nullable: false),
                    BodyWeightId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Body_Stat_Point", x => x.id);
                    table.ForeignKey(
                        name: "FK_Body_Stat_Point_Body_Weight_BodyWeightId",
                        column: x => x.BodyWeightId,
                        principalTable: "Body_Weight",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Body_Stat_Point_BodyWeightId",
                table: "Body_Stat_Point",
                column: "BodyWeightId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Body_Stat_Point");

            migrationBuilder.CreateTable(
                name: "Body_Weight_Point",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BodyWeightId = table.Column<Guid>(type: "TEXT", nullable: true),
                    bodyFatPercent = table.Column<double>(type: "REAL", nullable: false),
                    bodyMassIndex = table.Column<double>(type: "REAL", nullable: false),
                    bodyWeight = table.Column<double>(type: "REAL", nullable: false),
                    caliperMode = table.Column<int>(type: "INTEGER", nullable: false),
                    chest = table.Column<double>(type: "REAL", nullable: false),
                    date = table.Column<string>(type: "TEXT", nullable: true),
                    leftBicep = table.Column<double>(type: "REAL", nullable: false),
                    leftCalf = table.Column<double>(type: "REAL", nullable: false),
                    leftThigh = table.Column<double>(type: "REAL", nullable: false),
                    newbodystatid = table.Column<int>(type: "INTEGER", nullable: true),
                    restingHeartRate = table.Column<int>(type: "INTEGER", nullable: false),
                    rightBicep = table.Column<double>(type: "REAL", nullable: false),
                    rightCalf = table.Column<double>(type: "REAL", nullable: false),
                    rightThigh = table.Column<double>(type: "REAL", nullable: false),
                    shoulders = table.Column<double>(type: "REAL", nullable: false),
                    waist = table.Column<double>(type: "REAL", nullable: false)
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
    }
}
