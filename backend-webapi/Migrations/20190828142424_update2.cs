using Microsoft.EntityFrameworkCore.Migrations;

namespace backend_webapi.Migrations
{
    public partial class update2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CustomerLatitude",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CustomerLongitude",
                table: "Orders",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerLatitude",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerLongitude",
                table: "Orders");
        }
    }
}
