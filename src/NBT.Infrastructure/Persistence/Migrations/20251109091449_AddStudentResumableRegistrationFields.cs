using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NBT.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentResumableRegistrationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RawScore",
                table: "TestResults");

            migrationBuilder.RenameColumn(
                name: "PerformanceBand",
                table: "TestResults",
                newName: "OverallPerformanceBand");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Payments",
                newName: "TotalAmount");

            migrationBuilder.AddColumn<string>(
                name: "VenueType",
                table: "Venues",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "TestSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSunday",
                table: "TestSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "TestType",
                table: "TestResults",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "ALPerformanceLevel",
                table: "TestResults",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ALScore",
                table: "TestResults",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "TestResults",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MATPerformanceLevel",
                table: "TestResults",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MATScore",
                table: "TestResults",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QLPerformanceLevel",
                table: "TestResults",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "QLScore",
                table: "TestResults",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RegistrationId",
                table: "TestResults",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "EmailVerificationOTP",
                table: "Students",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailVerified",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRegistrationComplete",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "OTPExpiry",
                table: "Students",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationCompletedDate",
                table: "Students",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegistrationStep",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountPaid",
                table: "Payments",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "IntakeYear",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionReference = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExternalTransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    RecordedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestDateCalendar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestDate = table.Column<DateTime>(type: "date", nullable: false),
                    ClosingBookingDate = table.Column<DateTime>(type: "date", nullable: false),
                    IsSunday = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsOnline = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IntakeYear = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestDateCalendar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestPricing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IntakeYear = table.Column<int>(type: "int", nullable: false),
                    TestType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "date", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestPricing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VenueAvailability",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VenueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TestDate = table.Column<DateTime>(type: "date", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    UnavailableReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VenueAvailability", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VenueAvailability_Venues_VenueId",
                        column: x => x.VenueId,
                        principalTable: "Venues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_Barcode",
                table: "TestResults",
                column: "Barcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestResults_RegistrationId",
                table: "TestResults",
                column: "RegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_Date",
                table: "PaymentTransactions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_PaymentId",
                table: "PaymentTransactions",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_Reference",
                table: "PaymentTransactions",
                column: "TransactionReference",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_Status",
                table: "PaymentTransactions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_TestDateCalendar_IntakeYear",
                table: "TestDateCalendar",
                column: "IntakeYear");

            migrationBuilder.CreateIndex(
                name: "IX_TestDateCalendar_IsActive",
                table: "TestDateCalendar",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TestDateCalendar_IsOnline",
                table: "TestDateCalendar",
                column: "IsOnline");

            migrationBuilder.CreateIndex(
                name: "IX_TestDateCalendar_IsSunday",
                table: "TestDateCalendar",
                column: "IsSunday");

            migrationBuilder.CreateIndex(
                name: "IX_TestDateCalendar_TestDate",
                table: "TestDateCalendar",
                column: "TestDate",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TestPricing_IntakeYear",
                table: "TestPricing",
                column: "IntakeYear");

            migrationBuilder.CreateIndex(
                name: "IX_TestPricing_IsActive",
                table: "TestPricing",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_TestPricing_TestType",
                table: "TestPricing",
                column: "TestType");

            migrationBuilder.CreateIndex(
                name: "IX_TestPricing_YearTypeActive",
                table: "TestPricing",
                columns: new[] { "IntakeYear", "TestType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_VenueAvailability_IsAvailable",
                table: "VenueAvailability",
                column: "IsAvailable");

            migrationBuilder.CreateIndex(
                name: "IX_VenueAvailability_TestDate",
                table: "VenueAvailability",
                column: "TestDate");

            migrationBuilder.CreateIndex(
                name: "IX_VenueAvailability_VenueDate",
                table: "VenueAvailability",
                columns: new[] { "VenueId", "TestDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VenueAvailability_VenueId",
                table: "VenueAvailability",
                column: "VenueId");

            migrationBuilder.AddForeignKey(
                name: "FK_TestResults_Registrations_RegistrationId",
                table: "TestResults",
                column: "RegistrationId",
                principalTable: "Registrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TestResults_Registrations_RegistrationId",
                table: "TestResults");

            migrationBuilder.DropTable(
                name: "PaymentTransactions");

            migrationBuilder.DropTable(
                name: "TestDateCalendar");

            migrationBuilder.DropTable(
                name: "TestPricing");

            migrationBuilder.DropTable(
                name: "VenueAvailability");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_Barcode",
                table: "TestResults");

            migrationBuilder.DropIndex(
                name: "IX_TestResults_RegistrationId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "VenueType",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "TestSessions");

            migrationBuilder.DropColumn(
                name: "IsSunday",
                table: "TestSessions");

            migrationBuilder.DropColumn(
                name: "ALPerformanceLevel",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "ALScore",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "MATPerformanceLevel",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "MATScore",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "QLPerformanceLevel",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "QLScore",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "RegistrationId",
                table: "TestResults");

            migrationBuilder.DropColumn(
                name: "EmailVerificationOTP",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsEmailVerified",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsRegistrationComplete",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "OTPExpiry",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "RegistrationCompletedDate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "RegistrationStep",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IntakeYear",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "OverallPerformanceBand",
                table: "TestResults",
                newName: "PerformanceBand");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Payments",
                newName: "Amount");

            migrationBuilder.AlterColumn<string>(
                name: "TestType",
                table: "TestResults",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<decimal>(
                name: "RawScore",
                table: "TestResults",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
