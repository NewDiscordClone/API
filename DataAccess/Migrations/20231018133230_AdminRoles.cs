using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AdminRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0b7b7f0c-fd18-4cdc-b2d1-c862c77bfdd5"),
                column: "IsAdmin",
                value: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e26c471b-5418-4de1-bc35-d35310ab4ca7"),
                column: "IsAdmin",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0b7b7f0c-fd18-4cdc-b2d1-c862c77bfdd5"),
                column: "IsAdmin",
                value: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e26c471b-5418-4de1-bc35-d35310ab4ca7"),
                column: "IsAdmin",
                value: false);
        }
    }
}
