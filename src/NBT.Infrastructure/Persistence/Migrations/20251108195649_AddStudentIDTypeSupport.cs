using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NBT.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentIDTypeSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryOfOrigin",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IDType",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Nationality",
                table: "Students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryOfOrigin",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IDType",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Nationality",
                table: "Students");
        }
    }
}
