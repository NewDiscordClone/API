using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipChatId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonalChatId",
                table: "Relationships",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Relationships",
                keyColumns: new[] { "Active", "Passive" },
                keyValues: new object[] { new Guid("7aef2538-e1b3-42d7-a3db-a2809a81ac91"), new Guid("ba1ce081-e200-41da-9fb2-3d317627c9d4") },
                column: "PersonalChatId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Relationships",
                keyColumns: new[] { "Active", "Passive" },
                keyValues: new object[] { new Guid("c1ce2c28-9cf4-4ab1-bddb-b99c5408ee34"), new Guid("7aef2538-e1b3-42d7-a3db-a2809a81ac91") },
                column: "PersonalChatId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Relationships",
                keyColumns: new[] { "Active", "Passive" },
                keyValues: new object[] { new Guid("c1ce2c28-9cf4-4ab1-bddb-b99c5408ee34"), new Guid("ba1ce081-e200-41da-9fb2-3d317627c9d4") },
                column: "PersonalChatId",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalChatId",
                table: "Relationships");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
