using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMate.DataManagement.Migrations
{
    /// <inheritdoc />
    public partial class dummyMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProcessCompleted",
                table: "Applicants",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcessCompleted",
                table: "Applicants");
        }
    }
}
