param (
    [Parameter(Mandatory = $true, HelpMessage = "The entity name (e.g., Resource, Product)")]
    [ValidateNotNullOrEmpty()]
    [string]$Entity,
    
    [Parameter(Mandatory = $true, HelpMessage = "The operation type (e.g., Create, Update, Delete)")]
    [ValidateNotNullOrEmpty()]
    [ValidateSet("Create", "Update", "Delete", "Activate", "Deactivate")]
    [string]$Operation,
    
    [Parameter(HelpMessage = "Base path for the WMS Application")]
    [string]$BasePath = "../src/WMS.Application"
)

# Input validation and setup
$Entity = $Entity.Trim()
$Operation = $Operation.Trim()

if (-not (Test-Path $BasePath)) {
    Write-Error "❌ Base path '$BasePath' does not exist. Please verify the path."
    exit 1
}

# Generate names and paths
$targetPath = Join-Path $BasePath "${Entity}s" "Commands" $Operation
$commandName = "${Operation}${Entity}Command"
$handlerName = "${commandName}Handler"
$validatorName = "${commandName}Validator"
$variableName = $Entity.Substring(0, 1).ToLower() + $Entity.Substring(1)

Write-Host "🔧 Generating CQRS components for: $Entity ($Operation)" -ForegroundColor Cyan

# Create directory structure
try {
    New-Item -ItemType Directory -Force -Path $targetPath | Out-Null
    Write-Host "📁 Created directory: $targetPath" -ForegroundColor Green
}
catch {
    Write-Error "❌ Failed to create directory: $_"
    exit 1
}

# Template functions for better maintainability
function Get-CommandTemplate {
    param($CommandName, $Entity, $Operation)
    
    $idParameter = if ($Operation -eq "Update") { "`n    Guid Id," } else { "" }
    
    return @"
using ErrorOr;
using MediatR;
using WMS.Application.${Entity}s.Common;

namespace WMS.Application.${Entity}s.Commands.${Operation};

public record ${CommandName}($idParameter
    string Name
) : IRequest<ErrorOr<${Entity}Result>>;
"@
}

function Get-HandlerTemplate {
    param($HandlerName, $CommandName, $Entity, $Operation, $VariableName)
    
    return @"
using ErrorOr;
using MediatR;
using WMS.Application.Common.Interfaces.Persistence;
using WMS.Application.${Entity}s.Common;
using WMS.Domain.Common.ErrorCatalog;
using WMS.Domain.${Entity}Aggregate;

namespace WMS.Application.${Entity}s.Commands.${Operation};

public class ${HandlerName} : IRequestHandler<${CommandName}, ErrorOr<${Entity}Result>>
{
    private readonly I${Entity}Repository _${VariableName}Repository;

    public ${HandlerName}(I${Entity}Repository ${VariableName}Repository)
    {
        _${VariableName}Repository = ${VariableName}Repository;
    }

    public async Task<ErrorOr<${Entity}Result>> Handle(
        ${CommandName} command,
        CancellationToken cancellationToken)
    {
        // TODO: Implement $Operation logic for $Entity
        // Example structure:
        // 1. Validate business rules
        // 2. $(if ($Operation -eq "Create") { "Create new entity" } elseif ($Operation -eq "Update") { "Retrieve and update entity" } else { "Process $Operation operation" })
        // 3. Save changes
        // 4. Return result
        
        throw new NotImplementedException("$Operation$Entity operation not yet implemented");
    }
}
"@
}

function Get-ValidatorTemplate {
    param($ValidatorName, $CommandName, $Operation)
    
    $idValidation = if ($Operation -eq "Update") { "`n        RuleFor(x => x.Id).NotEmpty();" } else { "" }
    
    return @"
using FluentValidation;

namespace WMS.Application.${Entity}s.Commands.${Operation};

public class ${ValidatorName} : AbstractValidator<${CommandName}>
{
    public ${ValidatorName}()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(255)
            .WithMessage("Name cannot exceed 255 characters");$idValidation
    }
}
"@
}

# Generate files
$files = @(
    @{ Name = "$commandName.cs"; Content = Get-CommandTemplate $commandName $Entity $Operation; Type = "Command" },
    @{ Name = "$handlerName.cs"; Content = Get-HandlerTemplate $handlerName $commandName $Entity $Operation $variableName; Type = "Handler" },
    @{ Name = "$validatorName.cs"; Content = Get-ValidatorTemplate $validatorName $commandName $Operation; Type = "Validator" }
)

foreach ($file in $files) {
    $filePath = Join-Path $targetPath $file.Name
    
    try {
        $file.Content | Out-File $filePath -Encoding UTF8
        Write-Host "✅ Created $($file.Type): $($file.Name)" -ForegroundColor Green
    }
    catch {
        Write-Error "❌ Failed to create $($file.Name): $_"
        exit 1
    }
}

# Summary
Write-Host "`n🎉 Successfully generated CQRS components:" -ForegroundColor Yellow
Write-Host "   📄 Command: $commandName" -ForegroundColor White
Write-Host "   🔧 Handler: $handlerName" -ForegroundColor White  
Write-Host "   ✅ Validator: $validatorName" -ForegroundColor White
Write-Host "   📁 Location: $targetPath" -ForegroundColor White

# Optional: Open in VS Code if available
if (Get-Command code -ErrorAction SilentlyContinue) {
    $openInVSCode = Read-Host "`n🔧 Open generated files in VS Code? (y/N)"
    if ($openInVSCode -eq 'y' -or $openInVSCode -eq 'Y') {
        code $targetPath
        Write-Host "🚀 Opened in VS Code" -ForegroundColor Green
    }
}