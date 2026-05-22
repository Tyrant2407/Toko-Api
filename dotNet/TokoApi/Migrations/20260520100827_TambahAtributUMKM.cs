using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TokoApi.Migrations
{
    /// <inheritdoc />
    public partial class TambahAtributUMKM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiryDate",
                table: "DaftarProduk",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHalal",
                table: "DaftarProduk",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "DaftarProduk");

            migrationBuilder.DropColumn(
                name: "IsHalal",
                table: "DaftarProduk");
        }
    }
}
