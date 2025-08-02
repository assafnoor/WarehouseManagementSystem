using Mapster;
using WMS.Application.ReceiptDocuments.Commands.Create;
using WMS.Application.ReceiptDocuments.Commands.Update;
using WMS.Application.ReceiptDocuments.Common;
using WMS.Application.ReceiptDocuments.Queries.GetAll;
using WMS.Application.ReceiptDocuments.Queries.GetById;
using WMS.Contracts.ReceiptDocument;

namespace WMS.API.Common.Mapping;

public class ReceiptDocumentMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddReceiptDocumentRequest, CreateReceiptDocumentCommand>();
        config.NewConfig<UpdateReceiptDocumentRequest, UpdateReceiptDocumentCommand>();
        config.NewConfig<Guid, GetByIdReceiptDocumentQuery>()
              .ConstructUsing(id => new GetByIdReceiptDocumentQuery(id));
        config.NewConfig<ReceiptDocumentResult, ReceiptDocumentResponse>();
        config.NewConfig<GetAllReceiptDocumentRequest, GetAllReceiptDocumentQuery>();
    }
}