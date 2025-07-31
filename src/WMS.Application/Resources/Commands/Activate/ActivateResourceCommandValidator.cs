using FluentValidation;

namespace WMS.Application.Resources.Commands.Activate;

public class ActivateResourceCommandValidator : AbstractValidator<ActivateResourceCommand>
{
    public ActivateResourceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            ;
    }
}