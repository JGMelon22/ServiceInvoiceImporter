namespace ServiceInvoiceImporter.Core.Domains.Invoices.Dtos.Responses;

public record ProcessamentoResultResponse
{
    public bool Sucesso { get; init; }
    public string Mensagem { get; init; } = string.Empty;
    public int NotasProcessadas { get; init; }
    public List<string> Erros { get; init; } = new();
    public List<NotaFiscalResponse> NotasProcessadasDetalhes { get; init; } = new();
}
