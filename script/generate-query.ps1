param (
    [string]$Entity,
    [string]$Operation
)

if (-not $Entity -or -not $Operation) {
    Write-Host "❌ Usage: .\generate-query.ps1 -Entity Resource -Operation Get"
    exit
}

$basePath = "../src/WMS.Application/${Entity}s/Queries/$Operation"
New-Item -ItemType Directory -Force -Path $basePath | Out-Null

$queryName = "${Operation}${Entity}Query"
$handlerName = "${queryName}Handler"
$variableName = "${Entity.Substring(0,1).ToLower()}$($Entity.Substring(1))"

# 1. Query
@"
using ErrorOr;
using MediatR;
using WMS.Application.${Entity}s.Common;

namespace WMS.Application.${Entity}s.Queries.${Operation};

public record ${queryName}(
    Guid Id
) : IRequest<ErrorOr<${Entity}Result>>;
"@ | Out-File "$basePath\$queryName.cs" -Encoding utf8

# 2. Handler
@"
using ErrorOr;
using MediatR;
using MyApp.Application.Common.Interfaces.Persistance;
using WMS.Application.${Entity}s.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.${Entity}Aggregate;

namespace WMS.Application.${Entity}s.Queries.${Operation};

public class ${handlerName} :
    IRequestHandler<${queryName}, ErrorOr<${Entity}Result>>
{
    private readonly I${Entity}Repository _$variableName`Repository;

    public ${handlerName}(I${Entity}Repository $variableName`Repository)
    {
        _$variableName`Repository = $variableName`Repository;
    }

    public async Task<ErrorOr<${Entity}Result>> Handle(
        ${queryName} query,
        CancellationToken cancellationToken)
    {
        var $variableName = await _$variableName`Repository.GetByIdAsync(query.Id);

        if ($variableName is null)
            return Errors.${Entity}.NotFound;

        return new ${Entity}Result($variableName.Id.Value, $variableName.Name);
    }
}
"@ | Out-File "$basePath\$handlerName.cs" -Encoding utf8

Write-Host "✅ Created Query: $queryName and $handlerName in $basePath"
