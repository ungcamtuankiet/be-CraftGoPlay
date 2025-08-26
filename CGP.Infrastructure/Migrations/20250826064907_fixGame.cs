using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Crop_CropId",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Item_ItemId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_CropId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_ItemId",
                table: "Inventory");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 26, 13, 49, 5, 653, DateTimeKind.Local).AddTicks(9443));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 26, 13, 49, 5, 653, DateTimeKind.Local).AddTicks(9467));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 24, 18, 34, 43, 811, DateTimeKind.Local).AddTicks(9630));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 24, 18, 34, 43, 811, DateTimeKind.Local).AddTicks(9653));

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_CropId",
                table: "Inventory",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ItemId",
                table: "Inventory",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Crop_CropId",
                table: "Inventory",
                column: "CropId",
                principalTable: "Crop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Item_ItemId",
                table: "Inventory",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
