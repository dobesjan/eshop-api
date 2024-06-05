using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Api.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class customerIdentityAbstraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Orders",
                newName: "BillingAddressId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Addresses",
                newName: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BillingAddressId",
                table: "Orders",
                column: "BillingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Addresses_BillingAddressId",
                table: "Orders",
                column: "BillingAddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Addresses_BillingAddressId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BillingAddressId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "BillingAddressId",
                table: "Orders",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "Addresses",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
