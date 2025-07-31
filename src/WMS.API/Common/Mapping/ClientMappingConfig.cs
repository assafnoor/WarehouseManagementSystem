using Mapster;
using WMS.Application.Clients.Commands.Create;
using WMS.Application.Clients.Common;
using WMS.Application.Clients.Queries.GetAll;
using WMS.Application.Clients.Queries.GetById;
using WMS.Contracts.Clients;

namespace WMS.API.Common.Mapping;

public class ClientMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddClientRequest, CreateClientCommand>();

        config.NewConfig<Guid, GetByIdClientQuery>()
          .ConstructUsing(id => new GetByIdClientQuery(id));

        config.NewConfig<GetAllClientsRequest, GetAllClientsQuery>();

        config.NewConfig<ClientResult, ClientResponse>()
            .Map(dest => dest.address, src => src.Address!.Street);
    }
}