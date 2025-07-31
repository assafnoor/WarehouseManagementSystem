using FluentValidation;

namespace WMS.Application.UnitOfMeasurements.Commands.Update;

public class UpdateUnitOfMeasurementCommandValidator : AbstractValidator<UpdateUnitOfMeasurementCommand>
{
    public UpdateUnitOfMeasurementCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Id).NotEmpty();
    }
}