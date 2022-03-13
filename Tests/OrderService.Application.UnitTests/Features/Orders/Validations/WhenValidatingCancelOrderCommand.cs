//This is a good place to do Fluent validation test to avoid doing many Integration test 
namespace OrderService.Application.UnitTests.Features.Orders;

public class WhenValidatingCancelOrderCommand
{
    private readonly CancelOrderCommandValidator validator;

    public WhenValidatingCancelOrderCommand()
    {
        validator = new CancelOrderCommandValidator();
    }

    [Fact]
    public void ReturnsNoValidationErrorWhenRequestIsValid()
    {
        var command = new CancelOrderCommand()
        {
            Id = Guid.NewGuid(),
        };
        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ReturnsValidationsErrorsWhenIdIsInvalid()
    {
        var command = new CancelOrderCommand() { Id = default };
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(cmd => cmd.Id);
    }    
}
