using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceInvoiceImporter.Core.Domains.Invoices.Entities;

namespace ServiceInvoiceImporter.Infrastructure.Data.Configuration;

public class NotaFiscalConfiguration : IEntityTypeConfiguration<NotaFiscal>
{
    public void Configure(EntityTypeBuilder<NotaFiscal> builder)
    {
        builder.ToTable("NotasFiscais");

        builder.HasKey(n => n.Numero);

        builder.Property(n => n.Numero)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(n => n.CNPJPrestador)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(n => n.CNPJTomador)
            .IsRequired()
            .HasMaxLength(14);

        builder.Property(n => n.DataEmissao)
            .IsRequired();

        builder.Property(n => n.DescricaoServico)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(n => n.ValorTotal)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.HasIndex(n => n.Numero)
            .HasDatabaseName("IDX_NotaFiscal_Numero")
            .IsUnique(); ;
    }
}