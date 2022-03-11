//This is a good place to do Fluent validation test to avoid doing many Integration test 
namespace OrderService.Application.UnitTests.Features.Orders;

public class WhenValidatingGetOrderByIdQuery
{
    private readonly GetOrderByIdQueryValidator validator;

    public WhenValidatingGetOrderByIdQuery()
    {
        validator = new GetOrderByIdQueryValidator();
    }

    [Fact]
    public void Returns_No_Validation_Error_When_Request_Is_Valid()
    {
        var command = new GetOrderByIdQuery()
        {
            Id = Guid.NewGuid(),
        };
        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }


    [Fact]
    public void Returns_Validations_Errors_When_Id_Is_Invalid()
    {
        var command = new GetOrderByIdQuery() { Id = default };
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(cmd => cmd.Id);
    }    
}
