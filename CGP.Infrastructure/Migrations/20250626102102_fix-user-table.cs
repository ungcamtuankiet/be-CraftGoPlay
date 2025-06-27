using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixusertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                columns: new[] { "CreationDate", "Thumbnail" },
                values: new object[] { new DateTime(2025, 6, 26, 17, 21, 1, 412, DateTimeKind.Local).AddTicks(1692), null });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                columns: new[] { "CreationDate", "Thumbnail" },
                values: new object[] { new DateTime(2025, 6, 26, 17, 21, 1, 412, DateTimeKind.Local).AddTicks(1689), null });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                columns: new[] { "CreationDate", "Thumbnail" },
                values: new object[] { new DateTime(2025, 6, 26, 17, 21, 1, 412, DateTimeKind.Local).AddTicks(1663), null });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                columns: new[] { "CreationDate", "Thumbnail" },
                values: new object[] { new DateTime(2025, 6, 26, 17, 21, 1, 412, DateTimeKind.Local).AddTicks(1684), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "User");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 6, 26, 12, 20, 5, 788, DateTimeKind.Local).AddTicks(8492));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 6, 26, 12, 20, 5, 788, DateTimeKind.Local).AddTicks(8489));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 6, 26, 12, 20, 5, 788, DateTimeKind.Local).AddTicks(8422));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 6, 26, 12, 20, 5, 788, DateTimeKind.Local).AddTicks(8443));
        }
    }
}
