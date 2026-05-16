using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class ShopplingListPageAddedShopplingListItemUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItems_Products_ProductId",
                table: "ShoppingListItems");

            migrationBuilder.DropColumn(
                name: "IsPurchased",
                table: "ShoppingListItems");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "ShoppingListItems");

            migrationBuilder.AddColumn<int>(
                name: "PageId",
                table: "ShoppingListItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Total",
                table: "ShoppingListItems",
                type: "int",
                nullable: false,
                computedColumnSql: "(\r\n                           ISNULL(\r\n                               (SELECT p.SellPrice\r\n                                FROM Products AS p\r\n                                WHERE p.ProductId = ProductId),\r\n                               0\r\n                           ) * Quantity\r\n                       )",
                stored: false);

            migrationBuilder.CreateTable(
                name: "ShoppingListPages",
                columns: table => new
                {
                    PageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Total = table.Column<int>(type: "int", nullable: false, computedColumnSql: "(\r\n                           SELECT ISNULL(SUM(i.Total), 0)\r\n                           FROM ShoppingListItems AS i\r\n                           WHERE i.PageId = PageId\r\n                       )", stored: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingListPages", x => x.PageId);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 14, 16, 57, 1, 65, DateTimeKind.Local).AddTicks(9938));

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingListItems_PageId",
                table: "ShoppingListItems",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItems_Products_ProductId",
                table: "ShoppingListItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItems_ShoppingListPages_PageId",
                table: "ShoppingListItems",
                column: "PageId",
                principalTable: "ShoppingListPages",
                principalColumn: "PageId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItems_Products_ProductId",
                table: "ShoppingListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingListItems_ShoppingListPages_PageId",
                table: "ShoppingListItems");

            migrationBuilder.DropTable(
                name: "ShoppingListPages");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingListItems_PageId",
                table: "ShoppingListItems");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "ShoppingListItems");

            migrationBuilder.DropColumn(
                name: "PageId",
                table: "ShoppingListItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsPurchased",
                table: "ShoppingListItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "ShoppingListItems",
                type: "NVARCHAR(300)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 5, 14, 16, 31, 23, 650, DateTimeKind.Local).AddTicks(5674));

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingListItems_Products_ProductId",
                table: "ShoppingListItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
