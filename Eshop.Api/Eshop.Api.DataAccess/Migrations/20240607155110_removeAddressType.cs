using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Eshop.Api.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class removeAddressType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AddressTypes_AddressTypeId",
                table: "Addresses");

            migrationBuilder.DropTable(
                name: "AddressTypes");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_AddressTypeId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "AddressTypeId",
                table: "Addresses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressTypeId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AddressTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AddressTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Billing" },
                    { 2, "Delivery" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AddressTypeId",
                table: "Addresses",
                column: "AddressTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AddressTypes_AddressTypeId",
                table: "Addresses",
                column: "AddressTypeId",
                principalTable: "AddressTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
