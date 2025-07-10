using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCraftSkillTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CraftSkill",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModificationBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CraftSkill", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserCraftSkill",
                columns: table => new
                {
                    ArtisansId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CraftSkillsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserCraftSkill", x => new { x.ArtisansId, x.CraftSkillsId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserCraftSkill_CraftSkill_CraftSkillsId",
                        column: x => x.CraftSkillsId,
                        principalTable: "CraftSkill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserCraftSkill_User_ArtisansId",
                        column: x => x.ArtisansId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 10, 15, 52, 26, 69, DateTimeKind.Local).AddTicks(114));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 10, 15, 52, 26, 69, DateTimeKind.Local).AddTicks(68));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 10, 15, 52, 26, 69, DateTimeKind.Local).AddTicks(45));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 10, 15, 52, 26, 69, DateTimeKind.Local).AddTicks(64));

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserCraftSkill_CraftSkillsId",
                table: "ApplicationUserCraftSkill",
                column: "CraftSkillsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserCraftSkill");

            migrationBuilder.DropTable(
                name: "CraftSkill");

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
    }
}
