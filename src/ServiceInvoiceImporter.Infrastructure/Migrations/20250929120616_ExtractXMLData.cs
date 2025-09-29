using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceInvoiceImporter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtractXMLData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotasFiscais",
                columns: table => new
                {
                    Numero = table.Column<int>(type: "int", nullable: false),
                    CNPJPrestador = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    CNPJTomador = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    DataEmissao = table.Column<DateOnly>(type: "date", nullable: false),
                    DescricaoServico = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ValorTotal = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotasFiscais", x => x.Numero);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_NotaFiscal_Numero",
                table: "NotasFiscais",
                column: "Numero",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotasFiscais");
        }
    }
}
