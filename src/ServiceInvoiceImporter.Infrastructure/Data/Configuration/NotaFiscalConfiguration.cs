using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceInvoiceImporter.Core.Domains.Invoices.Entities;

namespace ServiceInvoiceImporter.Infrastructure.Data.Configuration;

public class NotaFiscalConfiguration : IEntityTypeConfiguration<NotaFiscal>
{
    public void Configure(EntityTypeBuilder<NotaFiscal> builder)
    {
        builder.ToTable("NotasFiscais");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.Numero)
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

        builder.Property(n => n.DataCriacao)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.HasIndex(n => n.Id)
            .HasDatabaseName("IDX_NotaFiscal_Id");

        builder.HasIndex(n => n.CNPJPrestador)
            .HasDatabaseName("IDX_NotaFiscal_CnpjPrestador");
    }
}