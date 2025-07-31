using Mapster;
using WMS.Application.UnitOfMeasurements.Commands.Create;
using WMS.Application.UnitOfMeasurements.Common;
using WMS.Contracts.UnitOfMeasurements;

namespace WMS.API.Common.Mapping;

public class UnitOfMeasurementMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddUnitOfMeasurementRequest, CreateUnitOfMeasurementCommand>();

        config.NewConfig<UnitOfMeasurementResult, UnitOfMeasurementResponse>();
    }
}