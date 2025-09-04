using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixTableOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 9, 4, 20, 37, 26, 566, DateTimeKind.Local).AddTicks(817));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 9, 4, 20, 37, 26, 566, DateTimeKind.Local).AddTicks(838));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "Transaction",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 9, 4, 14, 54, 33, 660, DateTimeKind.Local).AddTicks(3320));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 9, 4, 14, 54, 33, 660, DateTimeKind.Local).AddTicks(3339));
        }
    }
}
