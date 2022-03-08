using FluentValidation.TestHelper;
using OrderService.Application.Features.Orders;
using System;
using Xunit;

//This is a good place to do Fluent validation test to avoid doing many Integration test 
namespace OrderService.Application.UnitTests.Features.Orders;


public class WhenValidatingCreateOrderCommand
{
    private readonly CreateOrderCommandValidator validator;

    public WhenValidatingCreateOrderCommand()
    {
        validator = new CreateOrderCommandValidator();
    }

    [Fact]
    public void Returns_No_Validation_Error_When_Request_Is_Valid()
    {
        var command = new CreateOrderCommand()
        {
            Id = Guid.NewGuid(),
            Items = new CreateOrderCommandItem[]
            {
                    new CreateOrderCommandItem()
                    {
                        Sku =   "prod01",
                        Quantity = 1,
                    }
            }

        };
        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }


    [Fact]
    public void Returns_Validations_Errors_For_All_Required_Properties()
    {
        var command = new CreateOrderCommand();
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(cmd => cmd.Id);
        result.ShouldHaveValidationErrorFor(cmd => cmd.Items);
    }

    [Fact]
    public void Returns_Validations_Errors_For_Invalid_Id()
    {
        var command = new CreateOrderCommand()
        {
            Id = default,
            Items = new CreateOrderCommandItem[]
            {
                    new CreateOrderCommandItem()
                    {
                        Sku =   "prod01",
                        Quantity = 1,
                    }
            }

        };
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(cmd => cmd.Id);
        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Items);
    }

    [Fact]
    public void Returns_Validations_Errors_For_Invalid_SKU()
    {
        var command = new CreateOrderCommand()
        {
            Id = Guid.NewGuid(),
            Items = new CreateOrderCommandItem[]
            {
                    new CreateOrderCommandItem()
                    {
                        Sku =   string.Empty,
                        Quantity = 1,
                    }
            }

        };
        var result = validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Id);
        result.ShouldHaveValidationErrorFor(cmd => cmd.Items);
    }

    [Fact]
    public void Returns_Validations_Errors_For_Invalid_Quantity()
    {
        var command = new CreateOrderCommand()
        {
            Id = Guid.NewGuid(),
            Items = new CreateOrderCommandItem[]
            {
                    new CreateOrderCommandItem()
                    {
                        Sku =   "prod01",
                        Quantity = -1,
                    }
            }

        };
        var result = validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(cmd => cmd.Id);
        result.ShouldHaveValidationErrorFor(cmd => cmd.Items);
    }
}
