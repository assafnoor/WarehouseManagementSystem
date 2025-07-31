using FluentValidation;

namespace WMS.Application.Clients.Commands.Archive;

public class ArchiveClientCommandValidator : AbstractValidator<ArchiveClientCommand>
{
    public ArchiveClientCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}