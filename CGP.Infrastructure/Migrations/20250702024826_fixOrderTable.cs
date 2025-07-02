using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ArtisanId",
                table: "Order",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 2, 9, 48, 25, 300, DateTimeKind.Local).AddTicks(8279));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 2, 9, 48, 25, 300, DateTimeKind.Local).AddTicks(8276));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 2, 9, 48, 25, 300, DateTimeKind.Local).AddTicks(8253));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 2, 9, 48, 25, 300, DateTimeKind.Local).AddTicks(8272));

            migrationBuilder.CreateIndex(
                name: "IX_Order_ApplicationUserId",
                table: "Order",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ArtisanId",
                table: "Order",
                column: "ArtisanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_ApplicationUserId",
                table: "Order",
                column: "ApplicationUserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_ArtisanId",
                table: "Order",
                column: "ArtisanId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_ApplicationUserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_ArtisanId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ApplicationUserId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ArtisanId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ArtisanId",
                table: "Order");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 2, 9, 33, 43, 1, DateTimeKind.Local).AddTicks(207));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 2, 9, 33, 43, 1, DateTimeKind.Local).AddTicks(204));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 2, 9, 33, 43, 1, DateTimeKind.Local).AddTicks(184));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 2, 9, 33, 43, 1, DateTimeKind.Local).AddTicks(200));
        }
    }
}
