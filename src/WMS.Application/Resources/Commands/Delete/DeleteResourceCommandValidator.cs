using FluentValidation;

namespace WMS.Application.Resources.Commands.Delete;

public class DeleteResourceCommandValidator : AbstractValidator<DeleteResourceCommand>
{
    public DeleteResourceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}