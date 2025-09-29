namespace ServiceInvoiceImporter.Core.Domains.Invoices.Entities;

public class NotaFiscal
{
    public int Numero { get; set; }
    public string CNPJPrestador { get; set; } = string.Empty;
    public string CNPJTomador { get; set; } = string.Empty;
    public DateOnly DataEmissao { get; set; }
    public string DescricaoServico { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
}