using Microsoft.EntityFrameworkCore.Migrations;

namespace PolyclinicsSystemBackend.Migrations
{
    public partial class Addedfieldfordoctortype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DoctorType",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorType",
                table: "AspNetUsers");
        }
    }
}
