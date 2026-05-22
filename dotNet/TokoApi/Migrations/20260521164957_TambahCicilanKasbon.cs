using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TokoApi.Migrations
{
    /// <inheritdoc />
    public partial class TambahCicilanKasbon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "JumlahDibayar",
                table: "DaftarTransaksi",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JumlahDibayar",
                table: "DaftarTransaksi");
        }
    }
}
