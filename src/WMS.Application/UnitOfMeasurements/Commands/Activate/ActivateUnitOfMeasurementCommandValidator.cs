using FluentValidation;

namespace WMS.Application.UnitOfMeasurements.Commands.Activate;

public class ActivateUnitOfMeasurementCommandValidator : AbstractValidator<ActivateUnitOfMeasurementCommand>
{
    public ActivateUnitOfMeasurementCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}