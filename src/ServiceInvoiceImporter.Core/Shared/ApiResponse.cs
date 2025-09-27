namespace ServiceInvoiceImporter.Core.Shared;

public sealed class ApiResponse<T>
{
    public bool Sucesso { get; set; } = true;
    public string Mensagem { get; set; } = string.Empty;
    public T? Dados { get; set; }
    public List<string> Erros { get; set; } = new();

    public static ApiResponse<T> Success(T dados)
        => new() { Sucesso = true, Dados = dados };

    public static ApiResponse<T> Error(string mensagem, List<string>? erros = null)
        => new() { Sucesso = false, Mensagem = mensagem, Erros = erros ?? new List<string>() };
}