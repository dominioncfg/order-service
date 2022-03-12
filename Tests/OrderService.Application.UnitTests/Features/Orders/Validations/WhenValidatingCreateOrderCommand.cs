//This is a good place to do Fluent validation test to avoid doing many Integration test 
namespace OrderService.Application.UnitTests.Features.Orders;

public class WhenValidatingCreateOrderCommand
{
    private readonly Guid Id = Guid.NewGuid();
    private readonly Guid BuyerId = Guid.NewGuid();
    private const string Sku = "prod01";
    private const decimal UnitPrice = 10;
    private const int Quantity = 1;
    private const string AddressCountry = "Spain";
    private const string AddressCity = "Madrid";
    private const string AddressStreet = "Gran Via";
    private const string AddressNumber = "55";

    private readonly CreateOrderCommandValidator validator;

    public WhenValidatingCreateOrderCommand()
    {
        validator = new CreateOrderCommandValidator();
    }

    [Fact]
    public void ReturnsNoValidationErrorWhenRequestIsValid()
    {
        CreateOrderCommand command = ValidCommand();
     
        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }   

    [Fact]
    public void ReturnsValidationsErrorsForAllRequiredProperties()
    {
        var command = new CreateOrderCommand();
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(cmd => cmd.Id);
        result.ShouldHaveValidationErrorFor(cmd => cmd.BuyerId);
        result.ShouldHaveValidationErrorFor(cmd => cmd.Address.Country);
        result.ShouldHaveValidationErrorFor(cmd => cmd.Address.City);
        result.ShouldHaveValidationErrorFor(cmd => cmd.Address.Street);
        result.ShouldHaveValidationErrorFor(cmd => cmd.Address.Number);
        result.ShouldHaveValidationErrorFor(cmd => cmd.Items);
    }

    [Fact]
    public void ReturnsValidationsErrorsForInvalidId()
    {
        var command = ValidCommand() with
        {
            Id = default
        };
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(cmd => cmd.Id);

        result.ShouldNotHaveValidationErrorFor(cmd => cmd.BuyerId);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Country);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.City);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Street);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Number);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Items);
    }

    [Fact]
    public void ReturnsValidationsErrorsForInvalidBuyerId()
    {
        var command = ValidCommand() with
        {
            BuyerId = default
        };
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(cmd => cmd.BuyerId);       

        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Id);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Country);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.City);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Street);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Number);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Items);
    }

    [Fact]
    public void ReturnsValidationsErrorsWhenNoAddress()
    {
        var command = ValidCommand() with
        {
            Address = default!
        };
        var result = validator.TestValidate(command);
        
        result.ShouldHaveValidationErrorFor(cmd => cmd.Address);
       
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Id);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.BuyerId);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Country);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.City);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Street);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Number);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Items);
    }

    [Fact]
    public void ReturnsValidationsErrorsWhenAddressWithNoValues()
    {
        var command = ValidCommand() with
        {
            Address = new()
        };
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(cmd => cmd.Address.Country);
        result.ShouldHaveValidationErrorFor(cmd => cmd.Address.City);
        result.ShouldHaveValidationErrorFor(cmd => cmd.Address.Street);
        result.ShouldHaveValidationErrorFor(cmd => cmd.Address.Number);

        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Id);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.BuyerId);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Items);
    }

    [Fact]
    public void ReturnsValidationsErrorsForInvalidSKU()
    {
        var command = ValidCommand() with
        {
            Items = new CreateOrderCommandItem[]
            {
                new CreateOrderCommandItem()
                {
                    Sku =   String.Empty,
                    UnitPrice = UnitPrice,
                    Quantity = Quantity,
                }
            }
        };
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(cmd => cmd.Items);

        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Id);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.BuyerId);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Country);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.City);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Street);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Number);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ReturnsValidationsErrorsForNegativeUnitPrice(decimal invalidPrice)
    {
        var command = ValidCommand() with
        {
            Items = new CreateOrderCommandItem[]
            {
                new CreateOrderCommandItem()
                {
                    Sku =   Sku,
                    UnitPrice = invalidPrice,
                    Quantity = Quantity,
                }
            }
        };
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(cmd => cmd.Items);

        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Id);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.BuyerId);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Country);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.City);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Street);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Number);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ReturnsValidationsErrorsForNegativeQuantity(int invalidQuantity)
    {
        var command = ValidCommand() with
        {
            Items = new CreateOrderCommandItem[]
            {
                new CreateOrderCommandItem()
                {
                    Sku =   Sku,
                    UnitPrice = UnitPrice,
                    Quantity = invalidQuantity,
                }
            }
        };
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(cmd => cmd.Items);

        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Id);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.BuyerId);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Country);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.City);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Street);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Address.Number);
    }

    private CreateOrderCommand ValidCommand()
    {
        return new CreateOrderCommand()
        {
            Id = Id,
            BuyerId = BuyerId,
            Address = new CreateOrderCommandAddress()
            {
                Country = AddressCountry,
                City = AddressCity,
                Street = AddressStreet,
                Number = AddressNumber,
            },
            Items = new[]
            {
                new CreateOrderCommandItem()
                {
                    Sku =   Sku,
                    UnitPrice = UnitPrice,
                    Quantity = Quantity,
                }
            }
        };
    }
}
