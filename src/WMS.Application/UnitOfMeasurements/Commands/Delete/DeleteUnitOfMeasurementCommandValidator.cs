using FluentValidation;

namespace WMS.Application.UnitOfMeasurements.Commands.Delete;

public class DeleteUnitOfMeasurementCommandValidator : AbstractValidator<DeleteUnitOfMeasurementCommand>
{
    public DeleteUnitOfMeasurementCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}