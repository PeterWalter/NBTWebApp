using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NBT.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentSurveyAndEthnicityFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdditionalComments",
                table: "Students",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CareerInterests",
                table: "Students",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ethnicity",
                table: "Students",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasAccessToComputer",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasInternetAccess",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "MotivationForTesting",
                table: "Students",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredStudyField",
                table: "Students",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalComments",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CareerInterests",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Ethnicity",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "HasAccessToComputer",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "HasInternetAccess",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "MotivationForTesting",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "PreferredStudyField",
                table: "Students");
        }
    }
}
