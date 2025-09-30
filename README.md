# ServiceInvoiceImporter

Sistema de importação de notas fiscais de serviço desenvolvido como do tech challenge da UneCont.

## 📋 Sobre o Projeto

O ServiceInvoiceImporter é uma aplicação desenvolvida em C# para automatizar o processo de importação e processamento de notas fiscais de serviço no formato XML.

## 🚀 Tecnologias Utilizadas

- **C#**
- **.NET Framework/Core**
- **Entity Framework**
- **SQL Server**

## 🎯 Funcionalidades

- Importação de notas uma ou várias notas fisjcais de serviço no formato XML
- Processamento e validação de dados
- Armazenamento estruturado em banco de dados
- Tratamento de erros e logs de operação

## 📁 Estrutura do Projeto
```
── src
│   ├── ServiceInvoiceImporter.API
│   │   ├── Endpoints
│   │   │   └── NotasFiscaisEndpoints.cs
│   │   ├── Extensions
│   │   │   └── IocExtensions.cs
│   │   ├── Program.cs
│   ├── ServiceInvoiceImporter.Core
│   │   ├── Invoices
│   │   │   ├── Dtos
│   │   │   │   ├── Requests
│   │   │   │   │   └── NotaFiscalCreateRequest.cs
│   │   │   │   └── Responses
│   │   │   │       ├── NotaFiscalResponse.cs
│   │   │   │       └── ProcessamentoResultResponse.cs
│   │   │   ├── Entities
│   │   │   │   └── NotaFiscal.cs
│   │   │   └── Mappings
│   │   │       └── NotaFiscalMappingExtensions.cs
│   └── ServiceInvoiceImporter.Infrastructure
│       ├── Data
│       │   ├── AppDbContext.cs
│       │   └── Configuration
│       │       └── NotaFiscalConfiguration.cs
│       ├── Interfaces
│       │   └── Services
│       │       └── IXmlProcessorService.cs
│       └── Services
│           └── XmlProcessorService.cs
└── tests
    └── UnitTets
        └── ServiceInvoiceImporter.Infrastructure.UnitTests
            └── Services
                └── XmlProcessorServiceTests.cs
```