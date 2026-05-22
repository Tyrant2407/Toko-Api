using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TokoApi.Migrations
{
    /// <inheritdoc />
    public partial class TambahTabelTransaksiDanStok : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stok",
                table: "DaftarProduk",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DaftarTransaksi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProdukId = table.Column<int>(type: "int", nullable: false),
                    JumlahBeli = table.Column<int>(type: "int", nullable: false),
                    TotalHarga = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TanggalTransaksi = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Pembeli = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaftarTransaksi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaftarTransaksi_DaftarProduk_ProdukId",
                        column: x => x.ProdukId,
                        principalTable: "DaftarProduk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DaftarTransaksi_ProdukId",
                table: "DaftarTransaksi",
                column: "ProdukId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DaftarTransaksi");

            migrationBuilder.DropColumn(
                name: "Stok",
                table: "DaftarProduk");
        }
    }
}
