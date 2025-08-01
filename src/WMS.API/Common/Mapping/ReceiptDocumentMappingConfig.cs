using Mapster;
using WMS.Application.ReceiptDocuments.Commands.Create;
using WMS.Application.ReceiptDocuments.Common;
using WMS.Contracts.ReceiptDocument;

namespace WMS.API.Common.Mapping;

public class ReceiptDocumentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateReceiptDocumentRequest, CreateReceiptDocumentCommand>();

        config.NewConfig<ReceiptDocumentResult, ReceiptDocumentResponse>();
    }
}