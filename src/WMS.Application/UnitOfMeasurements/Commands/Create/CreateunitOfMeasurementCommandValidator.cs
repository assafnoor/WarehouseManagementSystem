using FluentValidation;

namespace WMS.Application.UnitOfMeasurements.Commands.Create;

public class CreateUnitOfMeasurementCommandValidator : AbstractValidator<CreateUnitOfMeasurementCommand>
{
    public CreateUnitOfMeasurementCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);
    }
}