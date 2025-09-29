using Microsoft.AspNetCore.Http;
using ServiceInvoiceImporter.Core.Domains.Invoices.Dtos.Responses;
using ServiceInvoiceImporter.Core.Shared;

namespace ServiceInvoiceImporter.Infrastructure.Interfaces.Services;

public interface IXmlProcessorService
{
    Task<ProcessamentoResultResponse> ProcessarXmlAsync(IFormFile arquivo);
    Task<ProcessamentoResultResponse> ProcessarXmlLoteAsync(List<IFormFile> arquivos);
    Task<ApiResponse<NotaFiscalResponse?>> ObterNotaPorNumeroAsync(int numero);
}