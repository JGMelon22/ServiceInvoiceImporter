namespace ServiceInvoiceImporter.Core.Domains.Invoices.Dtos.Requests;

record NotaFiscalCreateRequest
(
        int Numero,
        string CNPJPrestador,
        string CNPJTomador,
        DateTime DataEmissao,
        string DescricaoServico,
        decimal ValorTotal
);