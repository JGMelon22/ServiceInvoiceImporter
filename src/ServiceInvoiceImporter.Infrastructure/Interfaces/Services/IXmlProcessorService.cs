using ServiceInvoiceImporter.Core.Domains.Invoices.Dtos.Responses;
using ServiceInvoiceImporter.Core.Shared;

namespace ServiceInvoiceImporter.Infrastructure.Interfaces.Services;

public interface IXmlProcessorService
{
    Task<ProcessamentoResultResponse> ProcessarXmlAsync(IFormFile arquivo);
    Task<ApiResponse<NotaFiscalResponse?>> ObterNotaPorNumeroAsync(int numero);
}