﻿using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistence;
using WMS.Application.UnitOfMeasurements.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.UnitOfMeasurementAggregate;

namespace WMS.Application.UnitOfMeasurements.Commands.Create;

public class CreateUnitOfMeasurementCommandHandler :
    IRequestHandler<CreateUnitOfMeasurementCommand, ErrorOr<UnitOfMeasurementResult>>
{
    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUnitOfMeasurementCommandHandler(IUnitOfMeasurementRepository unitOfMeasurementRepository, IUnitOfWork unitOfWork)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<UnitOfMeasurementResult>> Handle(
        CreateUnitOfMeasurementCommand command,
        CancellationToken cancellationToken)
    {
        // Check if any UnitOfMeasurement with this name exists (active or archived)
        var existingUnitOfMeasurement = await _unitOfMeasurementRepository.GetByNameAsync(command.Name);
        if (existingUnitOfMeasurement is not null)
        {
            return Errors.UnitOfMeasurement.NameAlreadyExists;
        }

        var unitOfMeasurement = UnitOfMeasurement.Create(command.Name);

        await _unitOfMeasurementRepository.AddAsync(unitOfMeasurement);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UnitOfMeasurementResult(unitOfMeasurement.Id.Value, unitOfMeasurement.Name, unitOfMeasurement.IsActive);
    }
}