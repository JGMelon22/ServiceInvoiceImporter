using ServiceInvoiceImporter.Core.Domains.Invoices.Dtos.Responses;
using ServiceInvoiceImporter.Core.Shared;
using ServiceInvoiceImporter.Infrastructure.Interfaces.Services;

namespace ServiceInvoiceImporter.API.Endpoints;

public static class NotasFiscaisEndpoints
{
    public static void MapNotasFiscaisEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/notasfiscais")
            .WithTags("NotasFiscais")
            .WithOpenApi();

        // Processa um arquivo XML de nota fiscal via upload
        group.MapPost("processar-xml", async (IFormFile arquivo, IXmlProcessorService xmlProcessor) =>
        {
            var resultado = await xmlProcessor.ProcessarXmlAsync(arquivo);
            return resultado.Sucesso ? Results.Ok(resultado) : Results.BadRequest(resultado);
        })
        .WithName("ProcessarXml")
        .WithSummary("Processa um arquivo XML de nota fiscal via upload")
        .WithDescription("Processa um arquivo XML de nota fiscal via upload")
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<ProcessamentoResultResponse>(200)
        .Produces<ProcessamentoResultResponse>(400);

        // Busca uma nota fiscal específica pelo número
        group.MapGet("{numero:int}", async (int numero, IXmlProcessorService xmlProcessor) =>
        {
            var resultado = await xmlProcessor.ObterNotaPorNumeroAsync(numero);
            return resultado.Sucesso ? Results.Ok(resultado) : Results.NotFound(resultado);
        })
        .WithName("ObterNotaPorNumero")
        .WithSummary("Busca uma nota fiscal específica pelo número")
        .Produces<ApiResponse<NotaFiscalResponse?>>(200)
        .Produces<ApiResponse<NotaFiscalResponse?>>(404);
    }
}