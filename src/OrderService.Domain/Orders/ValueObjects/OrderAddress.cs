namespace OrderService.Domain.Orders;

public class OrderAddress : ValueObject
{
    public string Country { get; } = string.Empty; 
    public string City { get; } = string.Empty;
    public string Street { get; } = string.Empty;
    public string Number { get; } = string.Empty;

    private OrderAddress() { }

    public OrderAddress(string country, string city, string street, string number)
    {
        if (string.IsNullOrEmpty(country)  || string.IsNullOrEmpty(city) ||
            string.IsNullOrEmpty(street) || string.IsNullOrEmpty(number))
            throw new InvalidOrderAddressDomainException("Address is not completed");
        
        Country = country;
        City = city;
        Street = street;
        Number = number;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Country;
        yield return City;
        yield return Street;
        yield return Number;
    }
}
