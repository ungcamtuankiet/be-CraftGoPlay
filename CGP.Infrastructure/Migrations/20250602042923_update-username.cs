using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateusername : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                column: "CreationDate",
                value: new DateTime(2025, 6, 2, 11, 29, 22, 388, DateTimeKind.Local).AddTicks(598));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 6, 2, 11, 29, 22, 388, DateTimeKind.Local).AddTicks(573));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                columns: new[] { "CreationDate", "Email" },
                values: new object[] { new DateTime(2025, 6, 2, 11, 29, 22, 388, DateTimeKind.Local).AddTicks(594), "staff" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                columns: new[] { "CreationDate", "Email" },
                values: new object[] { new DateTime(2025, 5, 30, 23, 1, 46, 507, DateTimeKind.Local).AddTicks(5568), "shop" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 5, 30, 23, 1, 46, 507, DateTimeKind.Local).AddTicks(5566));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 5, 30, 23, 1, 46, 507, DateTimeKind.Local).AddTicks(5543));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                columns: new[] { "CreationDate", "Email" },
                values: new object[] { new DateTime(2025, 5, 30, 23, 1, 46, 507, DateTimeKind.Local).AddTicks(5561), "user" });
        }
    }
}
