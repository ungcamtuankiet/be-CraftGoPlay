using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixGame1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FarmlandCrop_Crop_CropId",
                table: "FarmlandCrop");

            migrationBuilder.DropTable(
                name: "Crop");

            migrationBuilder.DropColumn(
                name: "HarvestDate",
                table: "FarmlandCrop");

            migrationBuilder.DropColumn(
                name: "LastWateredAt",
                table: "FarmlandCrop");

            migrationBuilder.DropColumn(
                name: "HasCrop",
                table: "FarmLand");

            migrationBuilder.DropColumn(
                name: "PlotIndex",
                table: "FarmLand");

            migrationBuilder.DropColumn(
                name: "PositionX",
                table: "FarmLand");

            migrationBuilder.RenameColumn(
                name: "WateredCount",
                table: "FarmlandCrop",
                newName: "TileId");

            migrationBuilder.RenameColumn(
                name: "PlantDate",
                table: "FarmlandCrop",
                newName: "StageEndsAtUtc");

            migrationBuilder.RenameColumn(
                name: "IsHarvested",
                table: "FarmlandCrop",
                newName: "NeedsWater");

            migrationBuilder.RenameColumn(
                name: "CropId",
                table: "FarmlandCrop",
                newName: "SeedId");

            migrationBuilder.RenameIndex(
                name: "IX_FarmlandCrop_CropId",
                table: "FarmlandCrop",
                newName: "IX_FarmlandCrop_SeedId");

            migrationBuilder.RenameColumn(
                name: "PositionY",
                table: "FarmLand",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "IsDug",
                table: "FarmLand",
                newName: "Watered");

            migrationBuilder.AddColumn<DateTime>(
                name: "HarvestableAtUtc",
                table: "FarmlandCrop",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "HarvestedAtUtc",
                table: "FarmlandCrop",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "FarmlandCrop",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextWaterDueAtUtc",
                table: "FarmlandCrop",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "PlantedAtUtc",
                table: "FarmlandCrop",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "FarmlandCrop",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "WaterExpiresAt",
                table: "FarmLand",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 31, 0, 19, 42, 960, DateTimeKind.Local).AddTicks(4866));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 31, 0, 19, 42, 960, DateTimeKind.Local).AddTicks(4893));

            migrationBuilder.AddForeignKey(
                name: "FK_FarmlandCrop_Item_SeedId",
                table: "FarmlandCrop",
                column: "SeedId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FarmlandCrop_Item_SeedId",
                table: "FarmlandCrop");

            migrationBuilder.DropColumn(
                name: "HarvestableAtUtc",
                table: "FarmlandCrop");

            migrationBuilder.DropColumn(
                name: "HarvestedAtUtc",
                table: "FarmlandCrop");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "FarmlandCrop");

            migrationBuilder.DropColumn(
                name: "NextWaterDueAtUtc",
                table: "FarmlandCrop");

            migrationBuilder.DropColumn(
                name: "PlantedAtUtc",
                table: "FarmlandCrop");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "FarmlandCrop");

            migrationBuilder.DropColumn(
                name: "WaterExpiresAt",
                table: "FarmLand");

            migrationBuilder.RenameColumn(
                name: "TileId",
                table: "FarmlandCrop",
                newName: "WateredCount");

            migrationBuilder.RenameColumn(
                name: "StageEndsAtUtc",
                table: "FarmlandCrop",
                newName: "PlantDate");

            migrationBuilder.RenameColumn(
                name: "SeedId",
                table: "FarmlandCrop",
                newName: "CropId");

            migrationBuilder.RenameColumn(
                name: "NeedsWater",
                table: "FarmlandCrop",
                newName: "IsHarvested");

            migrationBuilder.RenameIndex(
                name: "IX_FarmlandCrop_SeedId",
                table: "FarmlandCrop",
                newName: "IX_FarmlandCrop_CropId");

            migrationBuilder.RenameColumn(
                name: "Watered",
                table: "FarmLand",
                newName: "IsDug");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "FarmLand",
                newName: "PositionY");

            migrationBuilder.AddColumn<DateTime>(
                name: "HarvestDate",
                table: "FarmlandCrop",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastWateredAt",
                table: "FarmlandCrop",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasCrop",
                table: "FarmLand",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PlotIndex",
                table: "FarmLand",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionX",
                table: "FarmLand",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Crop",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CropType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GrowthStage = table.Column<int>(type: "int", nullable: false),
                    GrowthTimeHours = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WaterCount = table.Column<int>(type: "int", nullable: false),
                    WateringIntervalHours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Crop_User_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 26, 22, 50, 57, 209, DateTimeKind.Local).AddTicks(2290));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 26, 22, 50, 57, 209, DateTimeKind.Local).AddTicks(2330));

            migrationBuilder.CreateIndex(
                name: "IX_Crop_ApplicationUserId",
                table: "Crop",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FarmlandCrop_Crop_CropId",
                table: "FarmlandCrop",
                column: "CropId",
                principalTable: "Crop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
