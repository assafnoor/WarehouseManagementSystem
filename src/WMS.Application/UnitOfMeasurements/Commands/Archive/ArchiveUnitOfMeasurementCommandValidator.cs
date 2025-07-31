using FluentValidation;

namespace WMS.Application.UnitOfMeasurements.Commands.Archive;

public class ArchiveUnitOfMeasurementCommandValidator : AbstractValidator<ArchiveUnitOfMeasurementCommand>
{
    public ArchiveUnitOfMeasurementCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}