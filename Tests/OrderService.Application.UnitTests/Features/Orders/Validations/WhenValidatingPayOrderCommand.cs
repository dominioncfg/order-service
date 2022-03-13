//This is a good place to do Fluent validation test to avoid doing many Integration test 
namespace OrderService.Application.UnitTests.Features.Orders;

public class WhenValidatingPayOrderCommand
{
    private readonly PayOrderCommandValidator validator;

    public WhenValidatingPayOrderCommand()
    {
        validator = new PayOrderCommandValidator();
    }

    [Fact]
    public void ReturnsNoValidationErrorWhenRequestIsValid()
    {
        var command = new PayOrderCommand()
        {
            Id = Guid.NewGuid(),
        };
        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void ReturnsValidationsErrorsWhenIdIsInvalid()
    {
        var command = new PayOrderCommand() { Id = default };
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(cmd => cmd.Id);
    }    
}
