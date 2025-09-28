using Microsoft.EntityFrameworkCore;
using ServiceInvoiceImporter.Core.Domains.Invoices.Dtos.Responses;
using ServiceInvoiceImporter.Core.Shared;
using ServiceInvoiceImporter.Infrastructure.Data;
using ServiceInvoiceImporter.Infrastructure.Interfaces.Services;
using ServiceInvoiceImporter.Core.Domains.Invoices.Mappings;
using System.Xml;
using System.Xml.Linq;
using static ServiceInvoiceImporter.Core.Domains.Invoices.Mappings.NotaFiscalMappingExtensions;

namespace ServiceInvoiceImporter.Infrastructure.Services;

public class XmlProcessorService : IXmlProcessorService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<AppDbContext> _logger;

    public XmlProcessorService(AppDbContext dbContext, ILogger<AppDbContext> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<ApiResponse<NotaFiscalResponse?>> ObterNotaPorNumeroAsync(int numero)
    {
        try
        {
            var nota = await _dbContext.NotasFiscais
                .FirstOrDefaultAsync(n => n.Numero == numero);

            if (nota == null)
            {
                return ApiResponse<NotaFiscalResponse?>.Error($"Nota fiscal {numero} não encontrada");
            }

            return ApiResponse<NotaFiscalResponse?>.Success(
                nota.ToResponse()
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter nota fiscal por número: {Numero}", numero);
            return ApiResponse<NotaFiscalResponse?>.Error(
                "Erro ao consultar nota fiscal",
                new List<string> { ex.Message }
            );
        }
    }

    public async Task<ProcessamentoResultResponse> ProcessarXmlAsync(IFormFile arquivo)
    {
        var validationResult = ValidarArquivoXml(arquivo);
        if (!validationResult.IsValid)
        {
            return new ProcessamentoResultResponse
            {
                Sucesso = false,
                Mensagem = validationResult.ErrorMessage
            };
        }

        try
        {
            using var stream = new StreamReader(arquivo.OpenReadStream());
            string xmlContent = await stream.ReadToEndAsync();

            var doc = XDocument.Parse(xmlContent);
            var xmlData = ExtrairDadosXml(doc);

            if (xmlData == null)
            {
                return new ProcessamentoResultResponse
                {
                    Sucesso = false,
                    Mensagem = "Não foi possível extrair dados do XML"
                };
            }
            var notaFiscal = xmlData.ToNotaFiscalFromXml();

            // Verificar duplicata
            var notaExistente = await _dbContext.NotasFiscais
                .FirstOrDefaultAsync(n => n.Numero == notaFiscal.Numero);

            if (notaExistente != null)
            {
                return new ProcessamentoResultResponse
                {
                    Sucesso = false,
                    Mensagem = $"Nota fiscal {notaFiscal.Numero} já existe no banco de dados",
                    NotasProcessadasDetalhes = { notaExistente.ToResponse() }
                };
            }

            _dbContext.NotasFiscais.Add(notaFiscal);
            await _dbContext.SaveChangesAsync();

            return new ProcessamentoResultResponse
            {
                Sucesso = true,
                Mensagem = "Nota fiscal processada com sucesso",
                NotasProcessadas = 1,
                NotasProcessadasDetalhes = { notaFiscal.ToResponse() }
            };

        }
        catch (XmlException ex)
        {
            _logger.LogError(ex, "XML mal formado: {FileName}", arquivo.FileName);
            return new ProcessamentoResultResponse
            {
                Sucesso = false,
                Mensagem = ex.Message,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar arquivo XML: {FileName}", arquivo.FileName);
            return new ProcessamentoResultResponse
            {
                Sucesso = false,
                Mensagem = "Erro ao processar arquivo",
                // Assumindo que existe uma propriedade para detalhes do erro
                // Se não existir, remova esta linha
                // DetalhesErro = ex.Message
            };
        }
    }

    // Método auxiliar para validar extensão do arquivo. 
    // A fins de simplicidade mantive como um método de validação local e não uma service dedicada
    private (bool IsValid, string ErrorMessage) ValidarArquivoXml(IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
        {
            _logger.LogWarning("Tentativa de upload sem arquivo");
            return (false, "Arquivo não fornecido ou vazio");
        }

        var extension = Path.GetExtension(arquivo.FileName);
        if (!string.Equals(extension, ".xml", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogWarning("Extensão inválida: {Extension} para arquivo {FileName}",
                extension, arquivo.FileName);

            return (false, $"Extensão de arquivo inválida: '{extension}'. Esperado: .xml");
        }

        return (true, string.Empty);
    }

    private NotaFiscalXmlData? ExtrairDadosXml(XDocument doc)
    {
        try
        {
            var notaElement = doc.Element("NotaFiscal");
            if (notaElement == null) return null;

            return new NotaFiscalXmlData
            {
                Numero = int.Parse(notaElement.Element("Numero")?.Value ?? "0"),
                CNPJPrestador = notaElement.Element("Prestador")?.Element("CNPJ")?.Value ?? "",
                CNPJTomador = notaElement.Element("Tomador")?.Element("CNPJ")?.Value ?? "",
                DataEmissao = DateOnly.Parse(notaElement.Element("DataEmissao")?.Value ?? DateTime.Now.ToString()),
                DescricaoServico = notaElement.Element("Servico")?.Element("Descricao")?.Value ?? "",
                ValorTotal = decimal.Parse(notaElement.Element("Servico")?.Element("Valor")?.Value ?? "0")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao extrair dados do XML");
            return null;
        }
    }
}