using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CGP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixUniqueRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId_ProductId",
                table: "Ratings");

            migrationBuilder.DeleteData(
                table: "Wallet",
                keyColumn: "Id",
                keyValue: new Guid("33626e9e-3000-472d-b6b0-1fd826acc6b4"));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b469"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 11, 0, 54, 5, 591, DateTimeKind.Local).AddTicks(3215));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b470"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 11, 0, 54, 5, 591, DateTimeKind.Local).AddTicks(3211));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 11, 0, 54, 5, 591, DateTimeKind.Local).AddTicks(3184));

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("8b56687e-8377-4743-aac9-08dcf5c4b47f"),
                column: "CreationDate",
                value: new DateTime(2025, 8, 11, 0, 54, 5, 591, DateTimeKind.Local).AddTicks(3206));

            migrationBuilder.InsertData(
                table: "Wallet",
                columns: new[] { "Id", "CreatedBy", "CreationDate", "DeleteBy", "DeletionDate", "IsDeleted", "ModificationBy", "ModificationDate", "Type", "User_Id" },
                values: new object[] { new Guid("be28e3ff-84f5-4908-b94f-77ce6c5ca9b9"), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, false, null, null, 2, new Guid("8b56687e-8377-4743-aac9-08dcf5c4b471") });

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_UserId_OrderItemId",
                table: "Ratings",
                columns: new[] { "UserId", "OrderItemId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ratings_UserId_OrderItemId",
                table: "Ratings");

            migrationBuilder.DeleteData(
                table: "Wallet",
                keyColumn: "Id",
                keyValue: new Guid("be28e3ff-84f5-4908-b94f-77ce6c5ca9b9"));

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
                name: "IX_Ratings_UserId_ProductId",
                table: "Ratings",
                columns: new[] { "UserId", "ProductId" },
                unique: true);
        }
    }
}
