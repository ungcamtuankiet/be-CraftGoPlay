using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deleteattributebalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Balance",
                table: "User");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 5, 30, 20, 4, 22, 569, DateTimeKind.Local).AddTicks(7962));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 5, 30, 20, 4, 22, 569, DateTimeKind.Local).AddTicks(7959));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 5, 30, 20, 4, 22, 569, DateTimeKind.Local).AddTicks(7935));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 5, 30, 20, 4, 22, 569, DateTimeKind.Local).AddTicks(7956));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Balance",
                table: "User",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                columns: new[] { "Balance", "CreationDate" },
                values: new object[] { null, new DateTime(2025, 5, 29, 22, 59, 45, 39, DateTimeKind.Local).AddTicks(5816) });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                columns: new[] { "Balance", "CreationDate" },
                values: new object[] { null, new DateTime(2025, 5, 29, 22, 59, 45, 39, DateTimeKind.Local).AddTicks(5813) });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                columns: new[] { "Balance", "CreationDate" },
                values: new object[] { null, new DateTime(2025, 5, 29, 22, 59, 45, 39, DateTimeKind.Local).AddTicks(5791) });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                columns: new[] { "Balance", "CreationDate" },
                values: new object[] { null, new DateTime(2025, 5, 29, 22, 59, 45, 39, DateTimeKind.Local).AddTicks(5810) });
        }
    }
}
