using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NBT.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRoomAllocationWithStudentLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoomAllocations_SessionRoom",
                table: "RoomAllocations");

            migrationBuilder.DropColumn(
                name: "AllocatedStudents",
                table: "RoomAllocations");

            migrationBuilder.AddColumn<DateTime>(
                name: "AllocationDate",
                table: "RoomAllocations",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "RoomAllocations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeatNumber",
                table: "RoomAllocations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "RoomAllocations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RoomAllocations_SessionStudent",
                table: "RoomAllocations",
                columns: new[] { "TestSessionId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoomAllocations_StudentId",
                table: "RoomAllocations",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomAllocations_Students_StudentId",
                table: "RoomAllocations",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomAllocations_Students_StudentId",
                table: "RoomAllocations");

            migrationBuilder.DropIndex(
                name: "IX_RoomAllocations_SessionStudent",
                table: "RoomAllocations");

            migrationBuilder.DropIndex(
                name: "IX_RoomAllocations_StudentId",
                table: "RoomAllocations");

            migrationBuilder.DropColumn(
                name: "AllocationDate",
                table: "RoomAllocations");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "RoomAllocations");

            migrationBuilder.DropColumn(
                name: "SeatNumber",
                table: "RoomAllocations");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "RoomAllocations");

            migrationBuilder.AddColumn<int>(
                name: "AllocatedStudents",
                table: "RoomAllocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RoomAllocations_SessionRoom",
                table: "RoomAllocations",
                columns: new[] { "TestSessionId", "RoomId" },
                unique: true);
        }
    }
}
