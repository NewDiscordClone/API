using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddInitialRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserPassive",
                table: "Relationships",
                newName: "Passive");

            migrationBuilder.RenameColumn(
                name: "UserActive",
                table: "Relationships",
                newName: "Active");

            migrationBuilder.InsertData(
                table: "Relationships",
                columns: new[] { "Active", "Passive", "RelationshipType" },
                values: new object[,]
                {
                    { new Guid("7aef2538-e1b3-42d7-a3db-a2809a81ac91"), new Guid("ba1ce081-e200-41da-9fb2-3d317627c9d4"), 2 },
                    { new Guid("c1ce2c28-9cf4-4ab1-bddb-b99c5408ee34"), new Guid("7aef2538-e1b3-42d7-a3db-a2809a81ac91"), 1 },
                    { new Guid("c1ce2c28-9cf4-4ab1-bddb-b99c5408ee34"), new Guid("ba1ce081-e200-41da-9fb2-3d317627c9d4"), 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Relationships",
                keyColumns: new[] { "Active", "Passive" },
                keyValues: new object[] { new Guid("7aef2538-e1b3-42d7-a3db-a2809a81ac91"), new Guid("ba1ce081-e200-41da-9fb2-3d317627c9d4") });

            migrationBuilder.DeleteData(
                table: "Relationships",
                keyColumns: new[] { "Active", "Passive" },
                keyValues: new object[] { new Guid("c1ce2c28-9cf4-4ab1-bddb-b99c5408ee34"), new Guid("7aef2538-e1b3-42d7-a3db-a2809a81ac91") });

            migrationBuilder.DeleteData(
                table: "Relationships",
                keyColumns: new[] { "Active", "Passive" },
                keyValues: new object[] { new Guid("c1ce2c28-9cf4-4ab1-bddb-b99c5408ee34"), new Guid("ba1ce081-e200-41da-9fb2-3d317627c9d4") });

            migrationBuilder.RenameColumn(
                name: "Passive",
                table: "Relationships",
                newName: "UserPassive");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Relationships",
                newName: "UserActive");
        }
    }
}
