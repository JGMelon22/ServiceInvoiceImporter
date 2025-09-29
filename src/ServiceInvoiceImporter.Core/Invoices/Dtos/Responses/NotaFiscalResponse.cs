namespace ServiceInvoiceImporter.Core.Domains.Invoices.Dtos.Responses;

public record NotaFiscalResponse
{
    public int Numero { get; init; }
    public string CNPJPrestador { get; init; } = string.Empty;
    public string CNPJTomador { get; init; } = string.Empty;
    public DateOnly DataEmissao { get; init; }
    public string DescricaoServico { get; init; } = string.Empty;
    public decimal ValorTotal { get; init; }
}