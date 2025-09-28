using ServiceInvoiceImporter.Infrastructure.Interfaces.Services;
using ServiceInvoiceImporter.Infrastructure.Services;

namespace ServiceInvoiceImporter.API.Extensions;

public static class IocExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IXmlProcessorService, XmlProcessorService>();

        return services;
    }
}