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
            Id = notaFiscal.Id,
            Numero = notaFiscal.Numero,
            CNPJPrestador = notaFiscal.CNPJPrestador,
            CNPJTomador = notaFiscal.CNPJTomador,
            DataEmissao = notaFiscal.DataEmissao,
            DescricaoServico = notaFiscal.DescricaoServico,
            ValorTotal = notaFiscal.ValorTotal,
            DataCriacao = notaFiscal.DataCriacao
        };

    public static IEnumerable<NotaFiscalResponse> ToResponse(this IEnumerable<NotaFiscal> notasFiscais)
        => notasFiscais.Select(nf => nf.ToResponse());

    public static List<NotaFiscalResponse> ToResponseList(this List<NotaFiscal> notasFiscais)
        => notasFiscais.Select(nf => nf.ToResponse()).ToList();

    public static NotaFiscal ToNotaFiscalFromXml(this NotaFiscalXmlData xmlData)
        => new NotaFiscal
        {
            Numero = xmlData.Numero,
            CNPJPrestador = xmlData.CNPJPrestador,
            CNPJTomador = xmlData.CNPJTomador,
            DataEmissao = xmlData.DataEmissao,
            DescricaoServico = xmlData.DescricaoServico,
            ValorTotal = xmlData.ValorTotal
        };

    // Classe auxiliar para dados extraídos do XML
    public class NotaFiscalXmlData
    {
        public int Numero { get; set; }
        public string CNPJPrestador { get; set; } = string.Empty;
        public string CNPJTomador { get; set; } = string.Empty;
        public DateOnly DataEmissao { get; set; }
        public string DescricaoServico { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
    }
}