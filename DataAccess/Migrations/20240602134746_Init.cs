using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Sparkle.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    ServerId = table.Column<string>(type: "text", nullable: true),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Avatar = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TextStatus = table.Column<string>(type: "character varying(96)", maxLength: 96, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Relationships",
                columns: table => new
                {
                    Active = table.Column<Guid>(type: "uuid", nullable: false),
                    Passive = table.Column<Guid>(type: "uuid", nullable: false),
                    RelationshipType = table.Column<int>(type: "integer", nullable: false),
                    PersonalChatId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relationships", x => new { x.Active, x.Passive });
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
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
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ChatId = table.Column<string>(type: "text", nullable: true),
                    ProfileType = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: true),
                    ServerId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUserProfile",
                columns: table => new
                {
                    RolesId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserProfileId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUserProfile", x => new { x.RolesId, x.UserProfileId });
                    table.ForeignKey(
                        name: "FK_RoleUserProfile_AspNetRoles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUserProfile_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Color", "ConcurrencyStamp", "IsAdmin", "Name", "NormalizedName", "Priority", "ServerId" },
                values: new object[,]
                {
                    { new Guid("0b7b7f0c-fd18-4cdc-b2d1-c862c77bfdd5"), "#FF0000", null, true, "GROUP-CHAT-OWNER", null, 1, null },
                    { new Guid("8ce979a9-c05c-4d70-bb45-6476892ced6c"), "#FF0000", null, false, "SERVER-MEMBER", null, 0, null },
                    { new Guid("b36944ab-83e0-4447-bce4-ac699b6d3dc2"), "#FF0000", null, false, "PRIVATE-CHAT-MEMBER", null, 0, null },
                    { new Guid("e26c471b-5418-4de1-bc35-d35310ab4ca7"), "#FF0000", null, true, "SERVER-OWNER", null, 100, null },
                    { new Guid("ff55d9b4-b26f-47be-bf76-bdba29896ca3"), "#FF0000", null, false, "GROUP-CHAT-MEMBER", null, 0, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Avatar", "ConcurrencyStamp", "DisplayName", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Status", "TextStatus", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("7aef2538-e1b3-42d7-a3db-a2809a81ac91"), 0, null, "a2075b1f-8f89-4df9-9f85-39deaec2b403", null, "dneshotkin@email.com", false, false, null, "DNESHOTKIN@EMAIL.COM", "DNESH2", "AQAAAAIAAYagAAAAEDhRq1TO1+Bt5t+MWYFRaeRu7OrRR8LVhJNio81zmfnaZwdWhwUbHaEuj1vpSOngVg==", null, false, "KDGJ6POASVRE527NHMQZ4FQPGL4OREWT", 3, null, false, "dnesh2" },
                    { new Guid("ba1ce081-e200-41da-9fb2-3d317627c9d4"), 0, null, "93e4df6c-20cc-410b-9082-daac577e184a", "Grabbot", "dneshotkin@ebail.com", false, false, null, "DNESHOTKIN@EBAIL.COM", "DNESH3", "AQAAAAIAAYagAAAAECIvg/r9riF/7qS+ETlEE6L+wNUWORELOOYoI78NY+hKBk2/YP4+0F9nZ12cLs57Sw==", null, false, "X73JA3M4E5VMK35LG7HMINOGF5AWAM5B", 3, null, false, "dnesh3" },
                    { new Guid("c1ce2c28-9cf4-4ab1-bddb-b99c5408ee34"), 0, null, "9372f0b4-1686-4af9-8962-43ae874b7a6f", "Grabtot", "dneshotkin@gmail.com", false, false, null, "DNESHOTKIN@GMAIL.COM", "DNESH1", "AQAAAAIAAYagAAAAELLPwOPnHPJdFRtP7OCgMMQ4n7IAUrj5F7ZFbvkzbwdA1e5o1BSCxm3zf6pordQ1Ow==", null, false, "YNSKJ23UUSGYIOOXJTIKUUTHF3FXW43N", 3, null, false, "dnesh1" }
                });

            migrationBuilder.InsertData(
                table: "Relationships",
                columns: new[] { "Active", "Passive", "PersonalChatId", "RelationshipType" },
                values: new object[,]
                {
                    { new Guid("7aef2538-e1b3-42d7-a3db-a2809a81ac91"), new Guid("ba1ce081-e200-41da-9fb2-3d317627c9d4"), null, 2 },
                    { new Guid("c1ce2c28-9cf4-4ab1-bddb-b99c5408ee34"), new Guid("7aef2538-e1b3-42d7-a3db-a2809a81ac91"), null, 1 },
                    { new Guid("c1ce2c28-9cf4-4ab1-bddb-b99c5408ee34"), new Guid("ba1ce081-e200-41da-9fb2-3d317627c9d4"), null, 1 }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "ManageRoles", "true", new Guid("b36944ab-83e0-4447-bce4-ac699b6d3dc2") },
                    { 2, "ManageServer", "true", new Guid("b36944ab-83e0-4447-bce4-ac699b6d3dc2") },
                    { 3, "ManageMessages", "true", new Guid("b36944ab-83e0-4447-bce4-ac699b6d3dc2") },
                    { 4, "ManageChannels", "true", new Guid("b36944ab-83e0-4447-bce4-ac699b6d3dc2") },
                    { 5, "ManageRoles", "true", new Guid("ff55d9b4-b26f-47be-bf76-bdba29896ca3") },
                    { 6, "ManageServer", "true", new Guid("ff55d9b4-b26f-47be-bf76-bdba29896ca3") },
                    { 7, "ManageMessages", "true", new Guid("ff55d9b4-b26f-47be-bf76-bdba29896ca3") },
                    { 8, "ManageChannels", "true", new Guid("ff55d9b4-b26f-47be-bf76-bdba29896ca3") },
                    { 9, "ManageMessages", "true", new Guid("0b7b7f0c-fd18-4cdc-b2d1-c862c77bfdd5") },
                    { 10, "ChangeServerName", "true", new Guid("8ce979a9-c05c-4d70-bb45-6476892ced6c") },
                    { 11, "ChangeServerName", "true", new Guid("e26c471b-5418-4de1-bc35-d35310ab4ca7") },
                    { 12, "ManageServer", "true", new Guid("e26c471b-5418-4de1-bc35-d35310ab4ca7") },
                    { 13, "ManageRoles", "true", new Guid("e26c471b-5418-4de1-bc35-d35310ab4ca7") },
                    { 14, "ManageChannels", "true", new Guid("e26c471b-5418-4de1-bc35-d35310ab4ca7") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

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
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleUserProfile_UserProfileId",
                table: "RoleUserProfile",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Relationships");

            migrationBuilder.DropTable(
                name: "RoleUserProfile");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
