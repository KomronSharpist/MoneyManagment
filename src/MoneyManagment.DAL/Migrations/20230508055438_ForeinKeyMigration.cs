using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoneyManagment.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ForeinKeyMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionCategories_TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DeletedBy", "Email", "FirstName", "ImagePath", "IsDeleted", "IsVerify", "LastName", "Password", "Role", "Salt", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1L, new DateTime(2023, 5, 8, 5, 54, 37, 944, DateTimeKind.Utc).AddTicks(8095), null, "admin@gmail.com", "admin", "wwwroot\\uploads\\images\\1c6aee695d58469a8fa431333374f906.png", false, true, "admin", "$2a$11$L8TlL9xtC4YQu/fbHv6jzOCwIgp2tvAPgyPb4EWZvyhsJ8ERp7CYG", 1, "b24ed81a-6853-4bed-910d-a0cf432a335f", null, null },
                    { 2L, new DateTime(2023, 5, 8, 5, 54, 37, 944, DateTimeKind.Utc).AddTicks(8100), null, "user@gmail.com", "user", "wwwroot\\\\uploads\\\\images\\\\3d89777ec7f74a00b77e97397fce10e6.jpg", false, true, "user", "$2a$11$Vhb626LK0A0vS4C5BAhMuO3ybHR5g1mhcoVp0mo4cCF05dq22F6f2", 1, "27ffbd0d-569d-4e1c-bb6b-48b846463982", null, null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionCategories_TransactionCategoryId",
                table: "Transactions",
                column: "TransactionCategoryId",
                principalTable: "TransactionCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionCategories_TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionCategories_TransactionCategoryId",
                table: "Transactions",
                column: "TransactionCategoryId",
                principalTable: "TransactionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
