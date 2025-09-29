using ServiceInvoiceImporter.Core.Domains.Invoices.Dtos.Requests;
using ServiceInvoiceImporter.Core.Domains.Invoices.Dtos.Responses;
using ServiceInvoiceImporter.Core.Domains.Invoices.Entities;

namespace ServiceInvoiceImporter.Core.Domains.Invoices.Mappings;

public static class NotaFiscalMappingExtensions
{
    public static NotaFiscal ToDomain(this NotaFiscalCreateRequest request)
        => new NotaFiscal
        {
            Numero = request.Numero,
            CNPJPrestador = request.CNPJPrestador,
            CNPJTomador = request.CNPJTomador,
            DataEmissao = request.DataEmissao,
            DescricaoServico = request.DescricaoServico,
            ValorTotal = request.ValorTotal
        };

    public static NotaFiscalResponse ToResponse(this NotaFiscal notaFiscal)
        => new NotaFiscalResponse
        {
            Numero = notaFiscal.Numero,
            CNPJPrestador = notaFiscal.CNPJPrestador,
            CNPJTomador = notaFiscal.CNPJTomador,
            DataEmissao = notaFiscal.DataEmissao,
            DescricaoServico = notaFiscal.DescricaoServico,
            ValorTotal = notaFiscal.ValorTotal,
        };

    public static List<NotaFiscalResponse> ToResponse(this List<NotaFiscal> notasFiscais)
        => notasFiscais.Select(nf => nf.ToResponse()).ToList();

    public static NotaFiscal ToNotaFiscalFromXml(this NotaFiscal notaFiscal)
        => new NotaFiscal
        {
            Numero = notaFiscal.Numero,
            CNPJPrestador = notaFiscal.CNPJPrestador,
            CNPJTomador = notaFiscal.CNPJTomador,
            DataEmissao = notaFiscal.DataEmissao,
            DescricaoServico = notaFiscal.DescricaoServico,
            ValorTotal = notaFiscal.ValorTotal
        };
}