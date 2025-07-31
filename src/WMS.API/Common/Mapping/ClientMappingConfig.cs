using Mapster;
using WMS.Application.Clients.Commands.Create;
using WMS.Application.Clients.Common;
using WMS.Contracts.Clients;

namespace WMS.API.Common.Mapping;

public class ClientMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddClientRequest, CreateClientCommand>();

        config.NewConfig<ClientResult, ClientResponse>()
            .Map(dest => dest.address, src => src.Address!.Street);
    }
}