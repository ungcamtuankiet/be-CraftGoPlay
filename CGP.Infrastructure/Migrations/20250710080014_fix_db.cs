using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fix_db : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_ArtisanId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ArtisanId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ArtisanId",
                table: "Order");

            migrationBuilder.AddColumn<Guid>(
                name: "ArtisanId",
                table: "OrderItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 10, 15, 0, 12, 817, DateTimeKind.Local).AddTicks(9208));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 10, 15, 0, 12, 817, DateTimeKind.Local).AddTicks(9205));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 10, 15, 0, 12, 817, DateTimeKind.Local).AddTicks(9119));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 10, 15, 0, 12, 817, DateTimeKind.Local).AddTicks(9201));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArtisanId",
                table: "OrderItem");

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
                value: new DateTime(2025, 7, 7, 22, 9, 58, 505, DateTimeKind.Local).AddTicks(9072));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 7, 22, 9, 58, 505, DateTimeKind.Local).AddTicks(9070));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 7, 22, 9, 58, 505, DateTimeKind.Local).AddTicks(9044));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 7, 22, 9, 58, 505, DateTimeKind.Local).AddTicks(9066));

            migrationBuilder.CreateIndex(
                name: "IX_Order_ArtisanId",
                table: "Order",
                column: "ArtisanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_User_ArtisanId",
                table: "Order",
                column: "ArtisanId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
