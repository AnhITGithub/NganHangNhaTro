using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_WEB_NC.Migrations
{
    /// <inheritdoc />
    public partial class InsertAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Gender", "ModifiedAt", "Name", "Password", "PhoneNumber", "RoleId" },
                values: new object[] { new Guid("44f6fc83-22b5-49b0-aed3-8fbfb198cb63"), null, "admin", null, null, "Admin", "admin", "1111111111", "role1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("44f6fc83-22b5-49b0-aed3-8fbfb198cb63"));
        }
    }
}
