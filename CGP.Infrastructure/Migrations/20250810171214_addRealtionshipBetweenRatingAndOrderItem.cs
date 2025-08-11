using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addRealtionshipBetweenRatingAndOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Wallet",
                keyColumn: "Id",
                keyValue: new Guid("be15774c-f9c4-4198-91f6-31ed0f17bded"));

            migrationBuilder.AddColumn<Guid>(
                name: "OrderItemId",
                table: "Ratings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 11, 0, 12, 12, 956, DateTimeKind.Local).AddTicks(8022));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 11, 0, 12, 12, 956, DateTimeKind.Local).AddTicks(8019));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 11, 0, 12, 12, 956, DateTimeKind.Local).AddTicks(7987));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 11, 0, 12, 12, 956, DateTimeKind.Local).AddTicks(8013));

            migrationBuilder.InsertData(
                table: "Wallet",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeleteBy", "DeletionDate", "IsDeleted", "ModificationBy", "ModificationDate", "Type", "User_Id" },
                values: new object[] { new Guid("33626e9e-3000-472d-b6b0-1fd826acc6b4"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, null, null, 2, new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471") });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_OrderItemId",
                table: "Ratings",
                column: "OrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_OrderItem_OrderItemId",
                table: "Ratings",
                column: "OrderItemId",
                principalTable: "OrderItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_OrderItem_OrderItemId",
                table: "Ratings");

            migrationBuilder.DropIndex(
                name: "IX_Ratings_OrderItemId",
                table: "Ratings");

            migrationBuilder.DeleteData(
                table: "Wallet",
                keyColumn: "Id",
                keyValue: new Guid("33626e9e-3000-472d-b6b0-1fd826acc6b4"));

            migrationBuilder.DropColumn(
                name: "OrderItemId",
                table: "Ratings");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 9, 22, 16, 57, 317, DateTimeKind.Local).AddTicks(520));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 9, 22, 16, 57, 317, DateTimeKind.Local).AddTicks(516));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 9, 22, 16, 57, 317, DateTimeKind.Local).AddTicks(485));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 9, 22, 16, 57, 317, DateTimeKind.Local).AddTicks(510));

            migrationBuilder.InsertData(
                table: "Wallet",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeleteBy", "DeletionDate", "IsDeleted", "ModificationBy", "ModificationDate", "Type", "User_Id" },
                values: new object[] { new Guid("be15774c-f9c4-4198-91f6-31ed0f17bded"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, null, null, 2, new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471") });
        }
    }
}
