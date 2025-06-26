using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixaccountautogen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                columns: new[] { "CreationDate", "Email" },
                values: new object[] { new DateTime(2025, 6, 26, 10, 23, 54, 750, DateTimeKind.Local).AddTicks(9955), "user@gmail.com" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                columns: new[] { "CreationDate", "Email" },
                values: new object[] { new DateTime(2025, 6, 26, 10, 23, 54, 750, DateTimeKind.Local).AddTicks(9951), "artisan@gmail.com" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                columns: new[] { "CreationDate", "Email" },
                values: new object[] { new DateTime(2025, 6, 26, 10, 23, 54, 750, DateTimeKind.Local).AddTicks(9922), "admin@gmail.com" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                columns: new[] { "CreationDate", "Email" },
                values: new object[] { new DateTime(2025, 6, 26, 10, 23, 54, 750, DateTimeKind.Local).AddTicks(9946), "staff@gmail.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                columns: new[] { "CreationDate", "Email" },
                values: new object[] { new DateTime(2025, 6, 2, 11, 29, 22, 388, DateTimeKind.Local).AddTicks(600), "user" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                columns: new[] { "CreationDate", "Email" },
                values: new object[] { new DateTime(2025, 6, 2, 11, 29, 22, 388, DateTimeKind.Local).AddTicks(598), "artisan" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                columns: new[] { "CreationDate", "Email" },
                values: new object[] { new DateTime(2025, 6, 2, 11, 29, 22, 388, DateTimeKind.Local).AddTicks(573), "admin" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                columns: new[] { "CreationDate", "Email" },
                values: new object[] { new DateTime(2025, 6, 2, 11, 29, 22, 388, DateTimeKind.Local).AddTicks(594), "staff" });
        }
    }
}
