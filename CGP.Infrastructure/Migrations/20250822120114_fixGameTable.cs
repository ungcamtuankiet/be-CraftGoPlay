using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixGameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crop_User_UserId",
                table: "Crop");

            migrationBuilder.DropIndex(
                name: "IX_Crop_UserId",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "IsHarvested",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "PlantedAt",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Crop");

            migrationBuilder.RenameColumn(
                name: "TileY",
                table: "Crop",
                newName: "WateringIntervalHours");

            migrationBuilder.RenameColumn(
                name: "TileX",
                table: "Crop",
                newName: "GrowthTimeHours");

            migrationBuilder.AddColumn<Guid>(
                name: "CropId",
                table: "Inventory",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastWateredAt",
                table: "FarmlandCrop",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WateredCount",
                table: "FarmlandCrop",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                table: "Crop",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Crop",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Crop",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Crop",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 22, 19, 1, 13, 437, DateTimeKind.Local).AddTicks(8600));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 22, 19, 1, 13, 437, DateTimeKind.Local).AddTicks(8621));

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_CropId",
                table: "Inventory",
                column: "CropId");

            migrationBuilder.CreateIndex(
                name: "IX_Crop_ApplicationUserId",
                table: "Crop",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Crop_User_ApplicationUserId",
                table: "Crop",
                column: "ApplicationUserId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Crop_CropId",
                table: "Inventory",
                column: "CropId",
                principalTable: "Crop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crop_User_ApplicationUserId",
                table: "Crop");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Crop_CropId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_CropId",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Crop_ApplicationUserId",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "CropId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "LastWateredAt",
                table: "FarmlandCrop");

            migrationBuilder.DropColumn(
                name: "WateredCount",
                table: "FarmlandCrop");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Crop");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Crop");

            migrationBuilder.RenameColumn(
                name: "WateringIntervalHours",
                table: "Crop",
                newName: "TileY");

            migrationBuilder.RenameColumn(
                name: "GrowthTimeHours",
                table: "Crop",
                newName: "TileX");

            migrationBuilder.AddColumn<bool>(
                name: "IsHarvested",
                table: "Crop",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PlantedAt",
                table: "Crop",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Crop",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 22, 17, 26, 5, 66, DateTimeKind.Local).AddTicks(5167));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 22, 17, 26, 5, 66, DateTimeKind.Local).AddTicks(5190));

            migrationBuilder.CreateIndex(
                name: "IX_Crop_UserId",
                table: "Crop",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Crop_User_UserId",
                table: "Crop",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
