using FluentValidation;

namespace WMS.Application.Resources.Commands.Update;

public class UpdateResourceCommandValidator : AbstractValidator<UpdateResourceCommand>

{
    public UpdateResourceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255)
               .Must(name => name == name.Trim());

        RuleFor(x => x.Id).NotEmpty();
    }
}