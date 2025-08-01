using Mapster;
using WMS.Application.Resources.Commands.Create;
using WMS.Application.Resources.Queries.GetById;
using WMS.Contracts.UnitOfMeasurements.Resources;

namespace WMS.API.Common.Mapping;

public class ResourceMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Guid, GetByIdResourceQuery>()
             .ConstructUsing(id => new GetByIdResourceQuery(id));
        config.NewConfig<AddResourceRequest, ResourceCommand>();

        config.NewConfig<RequestDelegateResult, ResourceResponse>();
    }
}