using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eshop.Api.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class customerToUserRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLogged",
                table: "Contacts",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLogged",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Contacts");
        }
    }
}
