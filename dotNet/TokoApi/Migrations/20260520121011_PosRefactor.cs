using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TokoApi.Migrations
{
    /// <inheritdoc />
    public partial class PosRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaftarTransaksi_DaftarProduk_ProdukId",
                table: "DaftarTransaksi");

            migrationBuilder.DropIndex(
                name: "IX_DaftarTransaksi_ProdukId",
                table: "DaftarTransaksi");

            migrationBuilder.DropColumn(
                name: "JumlahBeli",
                table: "DaftarTransaksi");

            migrationBuilder.DropColumn(
                name: "ProdukId",
                table: "DaftarTransaksi");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalHarga",
                table: "DaftarTransaksi",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.CreateTable(
                name: "DaftarDetailTransaksi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TransaksiId = table.Column<int>(type: "int", nullable: false),
                    ProdukId = table.Column<int>(type: "int", nullable: false),
                    JumlahBeli = table.Column<int>(type: "int", nullable: false),
                    HargaSatuan = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaftarDetailTransaksi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DaftarDetailTransaksi_DaftarProduk_ProdukId",
                        column: x => x.ProdukId,
                        principalTable: "DaftarProduk",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DaftarDetailTransaksi_DaftarTransaksi_TransaksiId",
                        column: x => x.TransaksiId,
                        principalTable: "DaftarTransaksi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DaftarDetailTransaksi_ProdukId",
                table: "DaftarDetailTransaksi",
                column: "ProdukId");

            migrationBuilder.CreateIndex(
                name: "IX_DaftarDetailTransaksi_TransaksiId",
                table: "DaftarDetailTransaksi",
                column: "TransaksiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DaftarDetailTransaksi");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalHarga",
                table: "DaftarTransaksi",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "JumlahBeli",
                table: "DaftarTransaksi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProdukId",
                table: "DaftarTransaksi",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_DaftarTransaksi_ProdukId",
                table: "DaftarTransaksi",
                column: "ProdukId");

            migrationBuilder.AddForeignKey(
                name: "FK_DaftarTransaksi_DaftarProduk_ProdukId",
                table: "DaftarTransaksi",
                column: "ProdukId",
                principalTable: "DaftarProduk",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
