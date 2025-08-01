using FluentValidation;

namespace WMS.Application.Clients.Commands.Update;

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
    public UpdateClientCommandValidator()
    {
        RuleFor(x => x.Name)
             .NotEmpty()
             .MaximumLength(255)
               .Must(name => name == name.Trim());
        RuleFor(x => x.address)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Id).NotEmpty();
    }
}