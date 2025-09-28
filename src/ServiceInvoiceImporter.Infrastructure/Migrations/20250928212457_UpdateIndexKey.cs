using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceInvoiceImporter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIndexKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IDX_NotaFiscal_CnpjPrestador",
                table: "NotasFiscais");

            migrationBuilder.DropIndex(
                name: "IDX_NotaFiscal_Id",
                table: "NotasFiscais");

            migrationBuilder.CreateIndex(
                name: "IDX_NotaFiscal_Numero",
                table: "NotasFiscais",
                column: "Numero");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IDX_NotaFiscal_Numero",
                table: "NotasFiscais");

            migrationBuilder.CreateIndex(
                name: "IDX_NotaFiscal_CnpjPrestador",
                table: "NotasFiscais",
                column: "CNPJPrestador");

            migrationBuilder.CreateIndex(
                name: "IDX_NotaFiscal_Id",
                table: "NotasFiscais",
                column: "Id");
        }
    }
}
