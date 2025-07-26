using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixTableReturnRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnRequest_User_OrderId",
                table: "ReturnRequest");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 26, 10, 31, 24, 575, DateTimeKind.Local).AddTicks(1219));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 26, 10, 31, 24, 575, DateTimeKind.Local).AddTicks(1216));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 26, 10, 31, 24, 575, DateTimeKind.Local).AddTicks(1188));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 26, 10, 31, 24, 575, DateTimeKind.Local).AddTicks(1212));

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequest_UserId",
                table: "ReturnRequest",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnRequest_User_UserId",
                table: "ReturnRequest",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnRequest_User_UserId",
                table: "ReturnRequest");

            migrationBuilder.DropIndex(
                name: "IX_ReturnRequest_UserId",
                table: "ReturnRequest");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 26, 8, 40, 29, 269, DateTimeKind.Local).AddTicks(4930));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 26, 8, 40, 29, 269, DateTimeKind.Local).AddTicks(4926));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 26, 8, 40, 29, 269, DateTimeKind.Local).AddTicks(4901));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 26, 8, 40, 29, 269, DateTimeKind.Local).AddTicks(4922));

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnRequest_User_OrderId",
                table: "ReturnRequest",
                column: "OrderId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
