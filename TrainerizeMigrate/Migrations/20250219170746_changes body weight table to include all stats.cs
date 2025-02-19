using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrainerizeMigrate.Migrations
{
    /// <inheritdoc />
    public partial class changesbodyweighttabletoincludeallstats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "value",
                table: "Body_Weight_Point");

            migrationBuilder.AddColumn<double>(
                name: "bodyFatPercent",
                table: "Body_Weight_Point",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "bodyMassIndex",
                table: "Body_Weight_Point",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "bodyWeight",
                table: "Body_Weight_Point",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "caliperMode",
                table: "Body_Weight_Point",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "chest",
                table: "Body_Weight_Point",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "leftBicep",
                table: "Body_Weight_Point",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "leftCalf",
                table: "Body_Weight_Point",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "leftThigh",
                table: "Body_Weight_Point",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "restingHeartRate",
                table: "Body_Weight_Point",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "rightBicep",
                table: "Body_Weight_Point",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "rightCalf",
                table: "Body_Weight_Point",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "rightThigh",
                table: "Body_Weight_Point",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "shoulders",
                table: "Body_Weight_Point",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "waist",
                table: "Body_Weight_Point",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bodyFatPercent",
                table: "Body_Weight_Point");

            migrationBuilder.DropColumn(
                name: "bodyMassIndex",
                table: "Body_Weight_Point");

            migrationBuilder.DropColumn(
                name: "bodyWeight",
                table: "Body_Weight_Point");

            migrationBuilder.DropColumn(
                name: "caliperMode",
                table: "Body_Weight_Point");

            migrationBuilder.DropColumn(
                name: "chest",
                table: "Body_Weight_Point");

            migrationBuilder.DropColumn(
                name: "leftBicep",
                table: "Body_Weight_Point");

            migrationBuilder.DropColumn(
                name: "leftCalf",
                table: "Body_Weight_Point");

            migrationBuilder.DropColumn(
                name: "leftThigh",
                table: "Body_Weight_Point");

            migrationBuilder.DropColumn(
                name: "restingHeartRate",
                table: "Body_Weight_Point");

            migrationBuilder.DropColumn(
                name: "rightBicep",
                table: "Body_Weight_Point");

            migrationBuilder.DropColumn(
                name: "rightCalf",
                table: "Body_Weight_Point");

            migrationBuilder.DropColumn(
                name: "rightThigh",
                table: "Body_Weight_Point");

            migrationBuilder.DropColumn(
                name: "shoulders",
                table: "Body_Weight_Point");

            migrationBuilder.DropColumn(
                name: "waist",
                table: "Body_Weight_Point");

            migrationBuilder.AddColumn<double>(
                name: "value",
                table: "Body_Weight_Point",
                type: "REAL",
                nullable: true);
        }
    }
}
