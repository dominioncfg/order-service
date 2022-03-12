using OrderService.Domain.Orders;

namespace OrderService.Tests.Common.Builders;

public class OrderAdressBuilder
{
    private string country = string.Empty;
    private string city = string.Empty;
    private string street = string.Empty;
    private string number = string.Empty;

    public OrderAdressBuilder WithCountry(string country)
    {
        this.country = country;
        return this;
    }

    public OrderAdressBuilder WithCity(string city)
    {
        this.city = city;
        return this;
    }

    public OrderAdressBuilder WithStreet(string street)
    {
        this.street = street;
        return this;
    }

    public OrderAdressBuilder WithNumber(string number)
    {
        this.number = number;
        return this;
    }

    public CreateOrderAddressArgs Build() => new()
    {
        Country = country,
        City = city,
        Street = street,
        Number = number,
    };

}