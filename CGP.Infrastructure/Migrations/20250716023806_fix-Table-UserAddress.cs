using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixTableUserAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "UserAddress");

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "UserAddress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DistrictName",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeNumber",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProviceId",
                table: "UserAddress",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProviceName",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WardCode",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WardName",
                table: "UserAddress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Weight",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "CartItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "ArtisanRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DistrictName",
                table: "ArtisanRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullAddress",
                table: "ArtisanRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HomeNumber",
                table: "ArtisanRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "ArtisanRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ProviceId",
                table: "ArtisanRequest",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ProviceName",
                table: "ArtisanRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WardCode",
                table: "ArtisanRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WardName",
                table: "ArtisanRequest",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 16, 9, 38, 5, 558, DateTimeKind.Local).AddTicks(267));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 16, 9, 38, 5, 558, DateTimeKind.Local).AddTicks(264));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 16, 9, 38, 5, 558, DateTimeKind.Local).AddTicks(240));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 16, 9, 38, 5, 558, DateTimeKind.Local).AddTicks(260));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "DistrictName",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "HomeNumber",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "ProviceId",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "ProviceName",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "WardCode",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "WardName",
                table: "UserAddress");

            migrationBuilder.DropColumn(
                name: "Height",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CartItem");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "ArtisanRequest");

            migrationBuilder.DropColumn(
                name: "DistrictName",
                table: "ArtisanRequest");

            migrationBuilder.DropColumn(
                name: "FullAddress",
                table: "ArtisanRequest");

            migrationBuilder.DropColumn(
                name: "HomeNumber",
                table: "ArtisanRequest");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "ArtisanRequest");

            migrationBuilder.DropColumn(
                name: "ProviceId",
                table: "ArtisanRequest");

            migrationBuilder.DropColumn(
                name: "ProviceName",
                table: "ArtisanRequest");

            migrationBuilder.DropColumn(
                name: "WardCode",
                table: "ArtisanRequest");

            migrationBuilder.DropColumn(
                name: "WardName",
                table: "ArtisanRequest");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "UserAddress",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "UserAddress",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 10, 21, 16, 8, 410, DateTimeKind.Local).AddTicks(8999));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 10, 21, 16, 8, 410, DateTimeKind.Local).AddTicks(8996));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 10, 21, 16, 8, 410, DateTimeKind.Local).AddTicks(8973));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 10, 21, 16, 8, 410, DateTimeKind.Local).AddTicks(8993));
        }
    }
}
