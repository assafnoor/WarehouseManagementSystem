using FluentValidation;

namespace WMS.Application.ReceiptDocuments.Commands.Create;

public class CreateReceiptDocumentCommandValidator : AbstractValidator<CreateReceiptDocumentCommand>
{
    public CreateReceiptDocumentCommandValidator()
    {
        RuleFor(x => x.DocumentNumber)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Date)
            .NotEmpty();

        RuleForEach(x => x.ReceiptResources).SetValidator(new ReceiptResourceCommandValidator());
    }
}

public class ReceiptResourceCommandValidator : AbstractValidator<ReceiptResourceCommand>
{
    public ReceiptResourceCommandValidator()
    {
        RuleFor(x => x.ResourceId)
            .NotEmpty();

        RuleFor(x => x.UnitOfMeasurementId)
            .NotEmpty();

        RuleFor(x => x.Quantity);
    }
}