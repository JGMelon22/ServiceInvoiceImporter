namespace ServiceInvoiceImporter.Core.Domains.Invoices.Dtos.Responses;

public class NotaFiscalResponse
{
    public int Id { get; init; }
    public int Numero { get; init; }
    public string CNPJPrestador { get; init; } = string.Empty;
    public string CNPJTomador { get; init; } = string.Empty;
    public DateOnly DataEmissao { get; init; }
    public string DescricaoServico { get; init; } = string.Empty;
    public decimal ValorTotal { get; init; }
    public DateTime DataCriacao { get; init; }
}