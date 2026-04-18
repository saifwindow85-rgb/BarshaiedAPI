using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class ProfitmargineComputedColumnSqlFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ProfitMargin",
                table: "Products",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                computedColumnSql: "CASE \r\n                 WHEN [SellPrice] = 0 THEN 0 \r\n                ELSE (([SellPrice] - [CostPrice]) * 100.0 / [SellPrice])\r\n                END",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldComputedColumnSql: "CASE \r\n                        WHEN [CostPrice] = 0 THEN 0 \r\n                        ELSE (([SellPrice] - [CostPrice]) * 100.0 / [CostPrice])\r\n                         END",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "ProfitMargin",
                table: "Products",
                type: "decimal(18,4)",
                precision: 18,
                scale: 4,
                nullable: false,
                computedColumnSql: "CASE \r\n                        WHEN [CostPrice] = 0 THEN 0 \r\n                        ELSE (([SellPrice] - [CostPrice]) * 100.0 / [CostPrice])\r\n                         END",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldPrecision: 18,
                oldScale: 4,
                oldComputedColumnSql: "CASE \r\n                 WHEN [SellPrice] = 0 THEN 0 \r\n                ELSE (([SellPrice] - [CostPrice]) * 100.0 / [SellPrice])\r\n                END",
                oldStored: true);
        }
    }
}
