using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nanma.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMpinToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "mpin_hash",
                table: "users",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "mpin_hash",
                table: "users");
        }
    }
}
