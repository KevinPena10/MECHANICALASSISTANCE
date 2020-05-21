using Microsoft.EntityFrameworkCore.Migrations;

namespace MechanicalAssistance.Web.Migrations
{
    public partial class addlatitudeAndlength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Services",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 500);

            migrationBuilder.AddColumn<double>(
                name: "latitude",
                table: "Services",
                maxLength: 200,
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "length",
                table: "Services",
                maxLength: 200,
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latitude",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "length",
                table: "Services");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Services",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
