using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class editTableCategoryAndSubCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "SubCategory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Category",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 1, 13, 15, 13, 412, DateTimeKind.Local).AddTicks(3851));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 1, 13, 15, 13, 412, DateTimeKind.Local).AddTicks(3849));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 1, 13, 15, 13, 412, DateTimeKind.Local).AddTicks(3827));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 1, 13, 15, 13, 412, DateTimeKind.Local).AddTicks(3844));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "SubCategory");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Category");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 1, 13, 3, 25, 679, DateTimeKind.Local).AddTicks(5209));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 1, 13, 3, 25, 679, DateTimeKind.Local).AddTicks(5205));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 1, 13, 3, 25, 679, DateTimeKind.Local).AddTicks(5177));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 1, 13, 3, 25, 679, DateTimeKind.Local).AddTicks(5197));
        }
    }
}
