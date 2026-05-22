using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TokoApi.Migrations
{
    /// <inheritdoc />
    public partial class TambahStatusPembayaran : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusPembayaran",
                table: "DaftarTransaksi",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusPembayaran",
                table: "DaftarTransaksi");
        }
    }
}
