using FluentValidation;

namespace WMS.Application.Clients.Commands.Create;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255)
               .Must(name => name == name.Trim());
        RuleFor(x => x.address)
            .NotEmpty()
            .MaximumLength(255);
    }
}