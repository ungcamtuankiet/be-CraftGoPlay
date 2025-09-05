using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixOrderTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PointChangeAmount",
                table: "Voucher",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

/*            migrationBuilder.AddColumn<int>(
                name: "DeliveriesCount",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReasonDeliveryFailed",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);*/

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
/*            migrationBuilder.DropColumn(
                name: "DeliveriesCount",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ReasonDeliveryFailed",
                table: "Order");*/

            migrationBuilder.AlterColumn<double>(
                name: "PointChangeAmount",
                table: "Voucher",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 31, 23, 15, 15, 146, DateTimeKind.Local).AddTicks(7743));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 31, 23, 15, 15, 146, DateTimeKind.Local).AddTicks(7790));
        }
    }
}
