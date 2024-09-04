using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Coordinator.Migrations
{
    /// <inheritdoc />
    public partial class mig_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("599c0b46-fa24-4b81-9286-0259023874be"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("9669c44d-c00c-481b-9bc4-f5047e3bf1ab"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("d1251943-16e3-4985-8e09-c5e3a79dd3ae"));

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("01ded266-f47f-4a69-8b00-17f9978a69bd"), "Stock.API" },
                    { new Guid("49e99b95-9414-4d57-bda0-cbc3266a8e14"), "Order.API" },
                    { new Guid("6a88dbbc-3a63-4584-89f0-fe40ba803378"), "Payment.API" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("01ded266-f47f-4a69-8b00-17f9978a69bd"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("49e99b95-9414-4d57-bda0-cbc3266a8e14"));

            migrationBuilder.DeleteData(
                table: "Nodes",
                keyColumn: "Id",
                keyValue: new Guid("6a88dbbc-3a63-4584-89f0-fe40ba803378"));

            migrationBuilder.InsertData(
                table: "Nodes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("599c0b46-fa24-4b81-9286-0259023874be"), "Order.API" },
                    { new Guid("9669c44d-c00c-481b-9bc4-f5047e3bf1ab"), "Payment.API" },
                    { new Guid("d1251943-16e3-4985-8e09-c5e3a79dd3ae"), "Stock.API" }
                });
        }
    }
}
