using FluentValidation;

namespace WMS.Application.Resources.Commands.Create;

public class ResourceCommandValidator : AbstractValidator<ResourceCommand>
{
    public ResourceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255)
               .Must(name => name == name.Trim());
    }
}