using FluentValidation.TestHelper;
using WMS.Application.Resources.Commands.Create;
using Xunit;

namespace WMS.Application.UnitTest;

public class ResourceCommandValidatorTests
{
    private readonly ResourceCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var result = _validator.TestValidate(new ResourceCommand(""));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Too_Long()
    {
        var longName = new string('A', 256);
        var result = _validator.TestValidate(new ResourceCommand(longName));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Has_LeadingOrTrailingSpaces()
    {
        var result = _validator.TestValidate(new ResourceCommand("  MyName  "));
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Is_Valid()
    {
        var result = _validator.TestValidate(new ResourceCommand("Valid Name"));
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }
}