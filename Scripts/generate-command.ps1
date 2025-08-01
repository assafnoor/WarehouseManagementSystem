param (
    [string]$Entity,
    [string]$Operation
)

if (-not $Entity -or -not $Operation) {
    Write-Host "❌ Usage: .\generate-command.ps1 -Entity Resource -Operation Create"
    exit
}

$basePath = "../src/WMS.Application/${Entity}s/Commands/$Operation"
New-Item -ItemType Directory -Force -Path $basePath | Out-Null

$commandName = "${Operation}${Entity}Command"
$handlerName = "${commandName}Handler"
$validatorName = "${commandName}Validator"
$variableName = "${Entity.Substring(0,1).ToLower()}$($Entity.Substring(1))"

# 1. Command
@"
using ErrorOr;
using MediatR;
using WMS.Application.${Entity}s.Common;

namespace WMS.Application.${Entity}s.Commands.${Operation};

public record ${commandName}(
    $(if ($Operation -eq "Update") { "Guid Id,\n    " })string Name
) : IRequest<ErrorOr<${Entity}Result>>;
"@ | Out-File "$basePath\$commandName.cs" -Encoding utf8

# 2. Handler
@"
using ErrorOr;
using MediatR;
using WMS.Application.Common.Interface.Persistance;
using WMS.Application.${Entity}s.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.${Entity}Aggregate;

namespace WMS.Application.${Entity}s.Commands.${Operation};

public class ${handlerName} :
    IRequestHandler<${commandName}, ErrorOr<${Entity}Result>>
{
    private readonly I${Entity}Repository _$variableName`Repository;
    private readonly IUnitOfWork _unitOfWork;
    public ${handlerName}(I${Entity}Repository $variableName`Repository , IUnitOfWork unitOfWork)
    {
        _$variableName`Repository = $variableName`Repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<${Entity}Result>> Handle(
        ${commandName} command,
        CancellationToken cancellationToken)
    {
        // TODO: implement $Operation logic
        throw new NotImplementedException();
    }
}
"@ | Out-File "$basePath\$handlerName.cs" -Encoding utf8

# 3. Validator
@"
using FluentValidation;

namespace WMS.Application.${Entity}s.Commands.${Operation};

public class ${validatorName} : AbstractValidator<${commandName}>
{
    public ${validatorName}()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        $(if ($Operation -eq "Update") { "RuleFor(x => x.Id).NotEmpty();" })
    }
}
"@ | Out-File "$basePath\$validatorName.cs" -Encoding utf8

Write-Host "✅ Created: $commandName, $handlerName, $validatorName in $basePath"
