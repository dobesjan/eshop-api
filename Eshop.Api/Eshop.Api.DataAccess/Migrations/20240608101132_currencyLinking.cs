using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Api.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class currencyLinking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Currencies_CurrencyId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CurrencyId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Payments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CurrencyId",
                table: "Payments",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Currencies_CurrencyId",
                table: "Payments",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
