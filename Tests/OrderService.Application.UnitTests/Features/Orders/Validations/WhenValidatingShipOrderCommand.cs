//This is a good place to do Fluent validation test to avoid doing many Integration test 
namespace OrderService.Application.UnitTests.Features.Orders;

public class WhenValidatingShipOrderCommand
{
    private readonly ShipOrderCommandValidator validator;

    public WhenValidatingShipOrderCommand()
    {
        validator = new ShipOrderCommandValidator();
    }

    [Fact]
    public void ReturnsNoValidationErrorWhenRequestIsValid()
    {
        var command = new ShipOrderCommand()
        {
            Id = Guid.NewGuid(),
        };
        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ReturnsValidationsErrorsWhenIdIsInvalid()
    {
        var command = new ShipOrderCommand() { Id = default };
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(cmd => cmd.Id);
    }    
}
