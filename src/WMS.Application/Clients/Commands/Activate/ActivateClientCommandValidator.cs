using FluentValidation;

namespace WMS.Application.Clients.Commands.Activate;

public class ActivateClientCommandValidator : AbstractValidator<ActivateClientCommand>
{
    public ActivateClientCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}