using Microsoft.EntityFrameworkCore;
using ServiceInvoiceImporter.Core.Domains.Invoices.Entities;
using ServiceInvoiceImporter.Infrastructure.Data.Configuration;

namespace ServiceInvoiceImporter.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<NotaFiscal> NotasFiscais { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new NotaFiscalConfiguration());
    }
}