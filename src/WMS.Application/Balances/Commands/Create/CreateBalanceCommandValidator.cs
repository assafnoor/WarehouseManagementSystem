using FluentValidation;

namespace WMS.Application.Balances.Commands.Create;

public class CreateBalanceCommandValidator : AbstractValidator<CreateBalanceCommand>
{
    public CreateBalanceCommandValidator()
    {
    }
}