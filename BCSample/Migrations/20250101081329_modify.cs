using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BCSample.Migrations
{
    /// <inheritdoc />
    public partial class modify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchemaVersion",
                table: "Outbox");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Outbox",
                newName: "Paylaod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Paylaod",
                table: "Outbox",
                newName: "Value");

            migrationBuilder.AddColumn<string>(
                name: "SchemaVersion",
                table: "Outbox",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
