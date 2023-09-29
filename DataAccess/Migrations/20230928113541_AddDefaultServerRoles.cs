using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultServerRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Color", "ConcurrencyStamp", "IsAdmin", "Name", "NormalizedName", "Priority", "ServerId" },
                values: new object[,]
                {
                    { new Guid("8ce979a9-c05c-4d70-bb45-6476892ced6c"), "#FF0000", null, false, "SERVER-MEMBER", null, 0, null },
                    { new Guid("e26c471b-5418-4de1-bc35-d35310ab4ca7"), "#FF0000", null, false, "SERVER-OWNER", null, 100, null }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 10, "ChangeServerName", "true", new Guid("8ce979a9-c05c-4d70-bb45-6476892ced6c") },
                    { 11, "ChangeServerName", "true", new Guid("e26c471b-5418-4de1-bc35-d35310ab4ca7") },
                    { 12, "ManageServer", "true", new Guid("e26c471b-5418-4de1-bc35-d35310ab4ca7") },
                    { 13, "ManageRoles", "true", new Guid("e26c471b-5418-4de1-bc35-d35310ab4ca7") },
                    { 14, "ManageChannels", "true", new Guid("e26c471b-5418-4de1-bc35-d35310ab4ca7") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8ce979a9-c05c-4d70-bb45-6476892ced6c"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e26c471b-5418-4de1-bc35-d35310ab4ca7"));
        }
    }
}
