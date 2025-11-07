using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NBT.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Announcements",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(250)", maxLength:250, nullable: false),
                Summary = table.Column<string>(type: "nvarchar(500)", maxLength:500, nullable: false),
                FullContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Category = table.Column<string>(type: "nvarchar(50)", maxLength:50, nullable: false),
                PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength:50, nullable: false, defaultValue: "Draft"),
                IsFeatured = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength:256, nullable: true),
                LastModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength:256, nullable: true),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Announcements", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetRoles",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Name = table.Column<string>(type: "nvarchar(256)", maxLength:256, nullable: true),
                NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength:256, nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ContactInquiries",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SubmissionDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                Name = table.Column<string>(type: "nvarchar(200)", maxLength:200, nullable: false),
                Email = table.Column<string>(type: "nvarchar(256)", maxLength:256, nullable: false),
                Phone = table.Column<string>(type: "nvarchar(20)", maxLength:20, nullable: true),
                InquiryType = table.Column<string>(type: "nvarchar(50)", maxLength:50, nullable: false),
                Subject = table.Column<string>(type: "nvarchar(250)", maxLength:250, nullable: false),
                Message = table.Column<string>(type: "nvarchar(2000)", maxLength:2000, nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength:50, nullable: false),
                AssignedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Response = table.Column<string>(type: "nvarchar(2000)", maxLength:2000, nullable: true),
                PrivacyConsent = table.Column<bool>(type: "bit", nullable: false),
                ReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength:50, nullable: false),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ContactInquiries", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ContentPages",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(200)", maxLength:200, nullable: false),
                Slug = table.Column<string>(type: "nvarchar(250)", maxLength:250, nullable: false),
                BodyContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                MetaDescription = table.Column<string>(type: "nvarchar(500)", maxLength:500, nullable: true),
                Keywords = table.Column<string>(type: "nvarchar(500)", maxLength:500, nullable: true),
                PublicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength:50, nullable: false, defaultValue: "Draft"),
                CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength:256, nullable: true),
                LastModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength:256, nullable: true),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ContentPages", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "DownloadableResources",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Title = table.Column<string>(type: "nvarchar(250)", maxLength:250, nullable: false),
                Description = table.Column<string>(type: "nvarchar(1000)", maxLength:1000, nullable: false),
                FilePath = table.Column<string>(type: "nvarchar(500)", maxLength:500, nullable: false),
                FileType = table.Column<string>(type: "nvarchar(50)", maxLength:50, nullable: false),
                FileSize = table.Column<long>(type: "bigint", nullable: false),
                Category = table.Column<string>(type: "nvarchar(100)", maxLength:100, nullable: false),
                UploadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                DownloadCount = table.Column<int>(type: "int", nullable: false, defaultValue:0),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength:50, nullable: false, defaultValue: "Active"),
                CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength:256, nullable: true),
                LastModifiedBy = table.Column<string>(type: "nvarchar(256)", maxLength:256, nullable: true),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DownloadableResources", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Users",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(100)", maxLength:100, nullable: false),
                LastName = table.Column<string>(type: "nvarchar(100)", maxLength:100, nullable: false),
                Role = table.Column<string>(type: "nvarchar(50)", maxLength:50, nullable: false),
                InstitutionId = table.Column<string>(type: "nvarchar(100)", maxLength:100, nullable: true),
                Status = table.Column<string>(type: "nvarchar(50)", maxLength:50, nullable: false, defaultValue: "Active"),
                LastLoginDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                PasswordResetToken = table.Column<string>(type: "nvarchar(500)", maxLength:500, nullable: true),
                TokenExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                UserName = table.Column<string>(type: "nvarchar(256)", maxLength:256, nullable: true),
                NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength:256, nullable: true),
                Email = table.Column<string>(type: "nvarchar(256)", maxLength:256, nullable: true),
                NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength:256, nullable: true),
                EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Users", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "AspNetRoleClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1,1"),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserClaims",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                .Annotation("SqlServer:Identity", "1,1"),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                table.ForeignKey(
                    name: "FK_AspNetUserClaims_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserLogins",
            columns: table => new
            {
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                table.ForeignKey(
                    name: "FK_AspNetUserLogins_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserRoles",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                    column: x => x.RoleId,
                    principalTable: "AspNetRoles",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_AspNetUserRoles_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "AspNetUserTokens",
            columns: table => new
            {
                UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                table.ForeignKey(
                    name: "FK_AspNetUserTokens_Users_UserId",
                    column: x => x.UserId,
                    principalTable: "Users",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Announcements_Category",
            table: "Announcements",
            column: "Category");

        migrationBuilder.CreateIndex(
            name: "IX_Announcements_IsFeatured",
            table: "Announcements",
            column: "IsFeatured");

        migrationBuilder.CreateIndex(
            name: "IX_Announcements_PublicationDate",
            table: "Announcements",
            column: "PublicationDate");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetRoleClaims_RoleId",
            table: "AspNetRoleClaims",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "RoleNameIndex",
            table: "AspNetRoles",
            column: "NormalizedName",
            unique: true,
            filter: "[NormalizedName] IS NOT NULL");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserClaims_UserId",
            table: "AspNetUserClaims",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserLogins_UserId",
            table: "AspNetUserLogins",
            column: "UserId");

        migrationBuilder.CreateIndex(
            name: "IX_AspNetUserRoles_RoleId",
            table: "AspNetUserRoles",
            column: "RoleId");

        migrationBuilder.CreateIndex(
            name: "IX_ContactInquiries_ReferenceNumber",
            table: "ContactInquiries",
            column: "ReferenceNumber",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ContactInquiries_Status",
            table: "ContactInquiries",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "IX_ContactInquiries_SubmissionDateTime",
            table: "ContactInquiries",
            column: "SubmissionDateTime");

        migrationBuilder.CreateIndex(
            name: "IX_ContentPages_Slug",
            table: "ContentPages",
            column: "Slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_DownloadableResources_Category",
            table: "DownloadableResources",
            column: "Category");

        migrationBuilder.CreateIndex(
            name: "IX_DownloadableResources_Status",
            table: "DownloadableResources",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "EmailIndex",
            table: "Users",
            column: "NormalizedEmail");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Email",
            table: "Users",
            column: "Email");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Role",
            table: "Users",
            column: "Role");

        migrationBuilder.CreateIndex(
            name: "IX_Users_Status",
            table: "Users",
            column: "Status");

        migrationBuilder.CreateIndex(
            name: "UserNameIndex",
            table: "Users",
            column: "NormalizedUserName",
            unique: true,
            filter: "[NormalizedUserName] IS NOT NULL");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Announcements");

        migrationBuilder.DropTable(
            name: "AspNetRoleClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserClaims");

        migrationBuilder.DropTable(
            name: "AspNetUserLogins");

        migrationBuilder.DropTable(
            name: "AspNetUserRoles");

        migrationBuilder.DropTable(
            name: "AspNetUserTokens");

        migrationBuilder.DropTable(
            name: "ContactInquiries");

        migrationBuilder.DropTable(
            name: "ContentPages");

        migrationBuilder.DropTable(
            name: "DownloadableResources");

        migrationBuilder.DropTable(
            name: "AspNetRoles");

        migrationBuilder.DropTable(
            name: "Users");
    }
}
