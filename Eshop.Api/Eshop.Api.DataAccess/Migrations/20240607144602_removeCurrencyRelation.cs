using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Api.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removeCurrencyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProducts_Currencies_CurrencyId",
                table: "OrderProducts");

            migrationBuilder.DropIndex(
                name: "IX_OrderProducts_CurrencyId",
                table: "OrderProducts");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "OrderProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "OrderProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProducts_CurrencyId",
                table: "OrderProducts",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProducts_Currencies_CurrencyId",
                table: "OrderProducts",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
