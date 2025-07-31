using FluentValidation;

namespace WMS.Application.Resources.Commands.Archive;

public class ArchiveResourceCommandValidator : AbstractValidator<ArchiveResourceCommand>
{
    public ArchiveResourceCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        
    }
}
