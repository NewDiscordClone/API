using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatId",
                table: "AspNetRoles");

            migrationBuilder.AlterColumn<string>(
                name: "ChatId",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Color", "ConcurrencyStamp", "IsAdmin", "Name", "NormalizedName", "Priority", "ServerId" },
                values: new object[,]
                {
                    { new Guid("0b7b7f0c-fd18-4cdc-b2d1-c862c77bfdd5"), "#FF0000", null, false, "GROUP-CHAT-OWNER", null, 1, null },
                    { new Guid("b36944ab-83e0-4447-bce4-ac699b6d3dc2"), "#FF0000", null, false, "PRIVATE-CHAT-MEMBER", null, 0, null },
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
                    { 9, "ManageMessages", "true", new Guid("0b7b7f0c-fd18-4cdc-b2d1-c862c77bfdd5") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("7aef2538-e1b3-42d7-a3db-a2809a81ac91"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("ba1ce081-e200-41da-9fb2-3d317627c9d4"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c1ce2c28-9cf4-4ab1-bddb-b99c5408ee34"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0b7b7f0c-fd18-4cdc-b2d1-c862c77bfdd5"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("b36944ab-83e0-4447-bce4-ac699b6d3dc2"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("ff55d9b4-b26f-47be-bf76-bdba29896ca3"));

            migrationBuilder.AlterColumn<string>(
                name: "ChatId",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChatId",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
