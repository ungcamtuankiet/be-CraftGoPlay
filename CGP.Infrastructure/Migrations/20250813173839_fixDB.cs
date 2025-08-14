using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnRequest_Order_OrderId",
                table: "ReturnRequest");

            migrationBuilder.DropIndex(
                name: "IX_ReturnRequest_OrderId",
                table: "ReturnRequest");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_OrderItemId",
                table: "Ratings");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "ReturnRequest",
                newName: "OrderItemId");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "OrderItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 14, 0, 38, 37, 588, DateTimeKind.Local).AddTicks(1538));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 14, 0, 38, 37, 588, DateTimeKind.Local).AddTicks(1569));

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequest_OrderItemId",
                table: "ReturnRequest",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_OrderItemId",
                table: "Ratings",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnRequest_OrderItem_OrderItemId",
                table: "ReturnRequest",
                column: "OrderItemId",
                principalTable: "OrderItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReturnRequest_OrderItem_OrderItemId",
                table: "ReturnRequest");

            migrationBuilder.DropIndex(
                name: "IX_ReturnRequest_OrderItemId",
                table: "ReturnRequest");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_OrderItemId",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "OrderItem");

            migrationBuilder.RenameColumn(
                name: "OrderItemId",
                table: "ReturnRequest",
                newName: "OrderId");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 12, 13, 9, 40, 465, DateTimeKind.Local).AddTicks(147));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 12, 13, 9, 40, 465, DateTimeKind.Local).AddTicks(167));

            migrationBuilder.CreateIndex(
                name: "IX_ReturnRequest_OrderId",
                table: "ReturnRequest",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_OrderItemId",
                table: "Ratings",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReturnRequest_Order_OrderId",
                table: "ReturnRequest",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
