using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTableVoucher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Delivery_Amount",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Product_Amount",
                table: "Order",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinOrderValue = table.Column<double>(type: "float", nullable: false),
                    MaxDiscountAmount = table.Column<double>(type: "float", nullable: false),
                    UsedCount = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Voucher", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderVoucher",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VoucherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_OrderVoucher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderVoucher_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderVoucher_Voucher_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "Voucher",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 16, 11, 3, 3, 72, DateTimeKind.Local).AddTicks(8768));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 16, 11, 3, 3, 72, DateTimeKind.Local).AddTicks(8765));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 16, 11, 3, 3, 72, DateTimeKind.Local).AddTicks(8737));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 7, 16, 11, 3, 3, 72, DateTimeKind.Local).AddTicks(8761));

            migrationBuilder.CreateIndex(
                name: "IX_OrderVoucher_OrderId",
                table: "OrderVoucher",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderVoucher_VoucherId",
                table: "OrderVoucher",
                column: "VoucherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderVoucher");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropColumn(
                name: "Delivery_Amount",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Product_Amount",
                table: "Order");

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
    }
}
