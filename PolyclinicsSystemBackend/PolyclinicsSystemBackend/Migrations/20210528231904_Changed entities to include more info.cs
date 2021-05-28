using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PolyclinicsSystemBackend.Migrations
{
    public partial class Changedentitiestoincludemoreinfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TreatmentDate",
                table: "Treatments",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DiagnoseDate",
                table: "Diagnoses",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TreatmentDate",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "DiagnoseDate",
                table: "Diagnoses");
        }
    }
}
