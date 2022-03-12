namespace OrderService.Api.FunctionalTests.Features.Orders;

public class CreateOrderAddressApiRequestBuilder
{
    private string country = string.Empty;
    private string city = string.Empty;
    private string street = string.Empty;
    private string number = string.Empty;

    public CreateOrderAddressApiRequestBuilder WithCountry(string country)
    {
        this.country = country;
        return this;
    }

    public CreateOrderAddressApiRequestBuilder WithCity(string city)
    {
        this.city = city;
        return this;
    }

    public CreateOrderAddressApiRequestBuilder WithStreet(string street)
    {
        this.street = street;
        return this;
    }

    public CreateOrderAddressApiRequestBuilder WithNumber(string number)
    {
        this.number = number;
        return this;
    }

    public CreateOrderAddressApiRequest Build() => new()
    {
        Country = country,
        City = city,
        Street = street,
        Number = number,
    };
}

