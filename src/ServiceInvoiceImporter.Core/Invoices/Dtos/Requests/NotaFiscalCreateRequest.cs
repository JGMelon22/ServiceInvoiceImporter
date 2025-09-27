namespace ServiceInvoiceImporter.Core.Domains.Invoices.Dtos.Requests;

public record NotaFiscalCreateRequest
(
    int Numero,
    string CNPJPrestador,
    string CNPJTomador,
    DateOnly DataEmissao,
    string DescricaoServico,
    decimal ValorTotal
);