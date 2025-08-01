using FluentValidation;

namespace WMS.Application.ReceiptDocuments.Commands.Update;

public class UpdateReceiptDocumentCommandValidator : AbstractValidator<UpdateReceiptDocumentCommand>
{
    public UpdateReceiptDocumentCommandValidator()
    {
        RuleFor(x => x.DocumentNumber)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Date)
            .NotEmpty();
        RuleFor(x => x.Id)
        .NotEmpty();

        When(x => x.ReceiptResources is not null && x.ReceiptResources.Any(), () =>
        {
            RuleForEach(x => x.ReceiptResources!).SetValidator(new UpdateReceiptResourceCommandValidator());
        });
    }
}

public class UpdateReceiptResourceCommandValidator : AbstractValidator<UpdateReceiptResourceCommand>
{
    public UpdateReceiptResourceCommandValidator()
    {
        RuleFor(x => x.ResourceId)
            .NotEmpty();

        RuleFor(x => x.UnitOfMeasurementId)
            .NotEmpty();

        RuleFor(x => x.Quantity);
    }
}