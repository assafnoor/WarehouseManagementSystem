using FluentValidation;

namespace WMS.Application.ReceiptDocuments.Commands.Delete;

public class DeleteReceiptDocumentCommandValidator : AbstractValidator<DeleteReceiptDocumentCommand>
{
    public DeleteReceiptDocumentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}