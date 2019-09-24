using Microsoft.EntityFrameworkCore.Migrations;

namespace backend_webapi.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItemProducts_OrderItems_ProductId",
                table: "OrderItemProducts");

            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "Sellers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Sellers");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItemProducts_OrderItems_ProductId",
                table: "OrderItemProducts",
                column: "ProductId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
