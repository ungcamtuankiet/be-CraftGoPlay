using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixQuesttable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClaimed",
                table: "UserQuest");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "Quest");

            migrationBuilder.DropColumn(
                name: "RequiredCount",
                table: "Quest");

            migrationBuilder.DropColumn(
                name: "RewardAmount",
                table: "Quest");

            migrationBuilder.DropColumn(
                name: "RewardType",
                table: "Quest");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "UserQuest",
                newName: "RewardClaimed");

            migrationBuilder.RenameColumn(
                name: "Target",
                table: "Quest",
                newName: "Reward");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Quest",
                newName: "IsDaily");

            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "UserQuest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "UserQuest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "QuestType",
                table: "Quest",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 9, 3, 22, 59, 0, 222, DateTimeKind.Local).AddTicks(8382));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 9, 3, 22, 59, 0, 222, DateTimeKind.Local).AddTicks(8401));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "UserQuest");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserQuest");

            migrationBuilder.RenameColumn(
                name: "RewardClaimed",
                table: "UserQuest",
                newName: "IsCompleted");

            migrationBuilder.RenameColumn(
                name: "Reward",
                table: "Quest",
                newName: "Target");

            migrationBuilder.RenameColumn(
                name: "IsDaily",
                table: "Quest",
                newName: "IsActive");

            migrationBuilder.AddColumn<bool>(
                name: "IsClaimed",
                table: "UserQuest",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "QuestType",
                table: "Quest",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Quest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RequiredCount",
                table: "Quest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RewardAmount",
                table: "Quest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RewardType",
                table: "Quest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 9, 3, 13, 13, 31, 590, DateTimeKind.Local).AddTicks(4492));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 9, 3, 13, 13, 31, 590, DateTimeKind.Local).AddTicks(4515));
        }
    }
}
