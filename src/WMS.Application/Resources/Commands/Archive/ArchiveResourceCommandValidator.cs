using FluentValidation;

namespace WMS.Application.Resources.Commands.Archive;

public class ArchiveResourceCommandValidator : AbstractValidator<ArchiveResourceCommand>
{
    public ArchiveResourceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
    ;
    }
}