using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceInvoiceImporter.Core.Domains.Invoices.Entities;
using ServiceInvoiceImporter.Infrastructure.Data;
using ServiceInvoiceImporter.Infrastructure.Services;

namespace ServiceInvoiceImporter.Infrastructure.UnitTests.Services;

public class XmlProcessorServiceTests
{
    private readonly Mock<ILogger<AppDbContext>> _loggerMock;
    private readonly AppDbContext _dbContext;
    private readonly XmlProcessorService _service;

    public XmlProcessorServiceTests()
    {
        _loggerMock = new Mock<ILogger<AppDbContext>>();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new AppDbContext(options);
        _service = new XmlProcessorService(_dbContext, _loggerMock.Object);
    }

    #region ObterNotaPorNumeroAsync Tests

    [Fact]
    public async Task Should_ReturnNotaFiscal_When_NotaExists()
    {
        // Arrange
        var nota = new NotaFiscal
        {
            Numero = 12345,
            CNPJPrestador = "12345678000190",
            CNPJTomador = "98765432000100",
            DataEmissao = DateOnly.FromDateTime(DateTime.Now),
            DescricaoServico = "Serviço de Teste",
            ValorTotal = 1000.00m
        };
        _dbContext.NotasFiscais.Add(nota);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _service.ObterNotaPorNumeroAsync(12345);

        // Assert
        Assert.True(result.Sucesso);
        Assert.NotNull(result.Dados);
        Assert.Equal(12345, result.Dados.Numero);
    }

    [Fact]
    public async Task Should_ReturnError_When_NotaDoesNotExist()
    {
        // Arrange
        var numeroInexistente = 99999;

        // Act
        var result = await _service.ObterNotaPorNumeroAsync(numeroInexistente);

        // Assert
        Assert.False(result.Sucesso);
        Assert.Null(result.Dados);
        Assert.Contains("não encontrada", result.Mensagem);
    }

    #endregion

    #region ProcessarXmlAsync Tests

    [Fact]
    public async Task Should_ProcessXmlSuccessfully_When_ValidXmlProvided()
    {
        // Arrange
        var xmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<NotaFiscal>
    <Numero>12345</Numero>
    <DataEmissao>2025-09-29</DataEmissao>
    <Prestador>
        <CNPJ>12345678000190</CNPJ>
    </Prestador>
    <Tomador>
        <CNPJ>98765432000100</CNPJ>
    </Tomador>
    <Servico>
        <Descricao>Serviço de Consultoria</Descricao>
        <Valor>5000.00</Valor>
    </Servico>
</NotaFiscal>";

        var file = CreateMockFormFile("nota.xml", xmlContent);

        // Act
        var result = await _service.ProcessarXmlAsync(file);

        // Assert
        Assert.True(result.Sucesso);
        Assert.Equal(1, result.NotasProcessadas);
        Assert.Single(result.NotasProcessadasDetalhes);
        Assert.Equal(12345, result.NotasProcessadasDetalhes.First().Numero);
    }

    [Fact]
    public async Task Should_ReturnError_When_FileIsNull()
    {
        // Arrange
        IFormFile? file = null;

        // Act
        var result = await _service.ProcessarXmlAsync(file!);

        // Assert
        Assert.False(result.Sucesso);
        Assert.Contains("não fornecido ou vazio", result.Mensagem);
    }

    [Fact]
    public async Task Should_ReturnError_When_FileExtensionIsInvalid()
    {
        // Arrange
        var file = CreateMockFormFile("nota.txt", "<xml>content</xml>");

        // Act
        var result = await _service.ProcessarXmlAsync(file);

        // Assert
        Assert.False(result.Sucesso);
        Assert.Contains("Extensão de arquivo inválida", result.Mensagem);
    }

    [Fact]
    public async Task Should_ReturnError_When_XmlIsMalformed()
    {
        // Arrange
        var invalidXml = "<NotaFiscal><Numero>123</Numero>"; // XML incompleto
        var file = CreateMockFormFile("nota.xml", invalidXml);

        // Act
        var result = await _service.ProcessarXmlAsync(file);

        // Assert
        Assert.False(result.Sucesso);
    }

    [Fact]
    public async Task Should_ReturnError_When_DuplicateNotaFiscalExists()
    {
        // Arrange
        var nota = new NotaFiscal
        {
            Numero = 12345,
            CNPJPrestador = "12345678000190",
            CNPJTomador = "98765432000100",
            DataEmissao = DateOnly.FromDateTime(DateTime.Now),
            DescricaoServico = "Serviço Existente",
            ValorTotal = 1000.00m
        };
        _dbContext.NotasFiscais.Add(nota);
        await _dbContext.SaveChangesAsync();

        var xmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<NotaFiscal>
    <Numero>12345</Numero>
    <DataEmissao>2025-09-29</DataEmissao>
    <Prestador>
        <CNPJ>12345678000190</CNPJ>
    </Prestador>
    <Tomador>
        <CNPJ>98765432000100</CNPJ>
    </Tomador>
    <Servico>
        <Descricao>Serviço de Consultoria</Descricao>
        <Valor>5000.00</Valor>
    </Servico>
</NotaFiscal>";

        var file = CreateMockFormFile("nota.xml", xmlContent);

        // Act
        var result = await _service.ProcessarXmlAsync(file);

        // Assert
        Assert.False(result.Sucesso);
        Assert.Contains("já existe no banco de dados", result.Mensagem);
    }

    #endregion

    #region ProcessarXmlLoteAsync Tests

    [Fact]
    public async Task Should_ProcessAllFilesSuccessfully_When_AllFilesAreValid()
    {
        // Arrange
        var files = new List<IFormFile>
        {
            CreateMockFormFile("nota1.xml", CreateValidXml(11111)),
            CreateMockFormFile("nota2.xml", CreateValidXml(22222)),
            CreateMockFormFile("nota3.xml", CreateValidXml(33333))
        };

        // Act
        var result = await _service.ProcessarXmlLoteAsync(files);

        // Assert
        Assert.True(result.Sucesso);
        Assert.Equal(3, result.NotasProcessadas);
        Assert.Equal(3, result.NotasProcessadasDetalhes.Count);
        Assert.Empty(result.Erros);
        Assert.Contains("Todos os 3 arquivos processados com sucesso", result.Mensagem);
    }

    [Fact]
    public async Task Should_ProcessPartially_When_SomeFilesAreInvalid()
    {
        // Arrange
        var files = new List<IFormFile>
        {
            CreateMockFormFile("nota1.xml", CreateValidXml(11111)),
            CreateMockFormFile("nota2.txt", "<invalid>content</invalid>"), // Extensão inválida
            CreateMockFormFile("nota3.xml", CreateValidXml(33333))
        };

        // Act
        var result = await _service.ProcessarXmlLoteAsync(files);

        // Assert
        Assert.True(result.Sucesso);
        Assert.Equal(2, result.NotasProcessadas);
        Assert.Equal(2, result.NotasProcessadasDetalhes.Count);
        Assert.Single(result.Erros);
        Assert.Contains("Processamento parcial: 2 sucesso(s), 1 falha(s)", result.Mensagem);
    }

    [Fact]
    public async Task Should_FailCompletely_When_AllFilesAreInvalid()
    {
        // Arrange
        var files = new List<IFormFile>
        {
            CreateMockFormFile("nota1.txt", "<invalid>content</invalid>"),
            CreateMockFormFile("nota2.pdf", "<invalid>content</invalid>")
        };

        // Act
        var result = await _service.ProcessarXmlLoteAsync(files);

        // Assert
        Assert.False(result.Sucesso);
        Assert.Equal(0, result.NotasProcessadas);
        Assert.Empty(result.NotasProcessadasDetalhes);
        Assert.Equal(2, result.Erros.Count);
        Assert.Contains("Nenhum arquivo foi processado. 2 falha(s)", result.Mensagem);
    }

    [Fact]
    public async Task Should_HandleDuplicatesInBatch_When_DuplicateNotasExist()
    {
        // Arrange
        // Adiciona nota existente
        var notaExistente = new NotaFiscal
        {
            Numero = 11111,
            CNPJPrestador = "12345678000190",
            CNPJTomador = "98765432000100",
            DataEmissao = DateOnly.FromDateTime(DateTime.Now),
            DescricaoServico = "Serviço Existente",
            ValorTotal = 1000.00m
        };
        _dbContext.NotasFiscais.Add(notaExistente);
        await _dbContext.SaveChangesAsync();

        var files = new List<IFormFile>
        {
            CreateMockFormFile("nota1.xml", CreateValidXml(11111)), // Duplicada
            CreateMockFormFile("nota2.xml", CreateValidXml(22222))  // Nova
        };

        // Act
        var result = await _service.ProcessarXmlLoteAsync(files);

        // Assert
        Assert.True(result.Sucesso);
        Assert.Equal(1, result.NotasProcessadas);
        Assert.Single(result.NotasProcessadasDetalhes);
        Assert.Single(result.Erros);
        Assert.Contains("Processamento parcial: 1 sucesso(s), 1 falha(s)", result.Mensagem);
    }

    [Fact]
    public async Task Should_ReturnEmptyResult_When_NoFilesProvided()
    {
        // Arrange
        var files = new List<IFormFile>();

        // Act
        var result = await _service.ProcessarXmlLoteAsync(files);

        // Assert
        Assert.False(result.Sucesso);
        Assert.Equal(0, result.NotasProcessadas);
        Assert.Empty(result.NotasProcessadasDetalhes);
    }

    #endregion

    #region Helper Methods

    private IFormFile CreateMockFormFile(string fileName, string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(stream.Length);
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        fileMock.Setup(f => f.ContentType).Returns("application/xml");

        return fileMock.Object;
    }

    private string CreateValidXml(int numero)
    {
        return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<NotaFiscal>
    <Numero>{numero}</Numero>
    <DataEmissao>2025-09-29</DataEmissao>
    <Prestador>
        <CNPJ>12345678000190</CNPJ>
    </Prestador>
    <Tomador>
        <CNPJ>98765432000100</CNPJ>
    </Tomador>
    <Servico>
        <Descricao>Serviço de Consultoria</Descricao>
        <Valor>5000.00</Valor>
    </Servico>
</NotaFiscal>";
    }

    #endregion
}