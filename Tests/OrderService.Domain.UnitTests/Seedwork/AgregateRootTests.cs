namespace OrderService.Domain.UnitTests.Seedwork;
#nullable disable
public class AgregateRootTests
{

    #region Equals
    [Fact]
    public void EqualsWhenIdIsEqualButDifferentTypeReturnsFalse()
    {
        var obj1 = new AggregateRoot1(1);
        var obj2 = new AggregateRoot2(1);

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }

    [Fact]
    public void EqualsWhenBothNullsReturnsTrue()
    {
        Entity<int> obj1 = null;
        Entity<int> obj2 = null;
        bool result = obj1 == obj2;
        result.Should().BeTrue();
    }

    [Fact]
    public void EqualsWhenSameReferenceReturnsTrue()
    {
        var TestEntity = new AggregateRoot1(1);

        bool result = TestEntity.Equals(TestEntity);

        result.Should().BeTrue();
    }

    [Fact]
    public void EqualsWhenSameIdTrue()
    {
        var obj1 = new AggregateRoot1(1);
        var obj2 = new AggregateRoot1(1);

        bool result = obj1.Equals(obj2);

        result.Should().BeTrue();
    }


    [Fact]
    public void EqualsWhenSameIdButIs0False()
    {
        var obj1 = new AggregateRoot1(0);
        var obj2 = new AggregateRoot1(0);

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }

    [Fact]
    public void EqualsWhenSameIdAreDiferentFalse()
    {
        var obj1 = new AggregateRoot1(0);
        var obj2 = new AggregateRoot1(1);

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }

    [Fact]
    public void EqualsWhenOneNullValuesStringFalse()
    {
        var obj1 = new AggregateRoot3("");
        var obj2 = new AggregateRoot3(null);

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }
    #endregion

    #region Domain Events
    [Fact]
    public void CanRaiseDomainEvents()
    {
        var obj1 = new AggregateRoot1(1);
        var @event = new TestDomainEvent() { TestProperty = 1 };

        obj1.RaiseTestEvents(@event);

        obj1.DomainEvents.Should().NotBeEmpty();
        obj1.DomainEvents.Single().Should().Be(@event);
    }

    [Fact]
    public void CanClearDomainEvents()
    {
        var obj1 = new AggregateRoot1(1);
        var @event = new TestDomainEvent() { TestProperty = 1 };
        obj1.RaiseTestEvents(@event);
        obj1.DomainEvents.Should().NotBeEmpty();
        obj1.ClearEvents();

        obj1.DomainEvents.Should().BeEmpty();
    }
    #endregion

    #region Agregate Roots
    private class AggregateRoot1 : AggregateRoot<int>
    {
        public void RaiseTestEvents<T>(T @event) where T : IDomainEvent => AddDomainEvent(@event);
        public AggregateRoot1() { }
        public AggregateRoot1(int id) { Id = id; }
    }

    private class AggregateRoot2 : AggregateRoot<int>
    {
        public AggregateRoot2() { }
        public AggregateRoot2(int id) { Id = id; }
    }

    private class AggregateRoot3 : AggregateRoot<string>
    {
        public AggregateRoot3() { }
        public AggregateRoot3(string id) { Id = id; }
    }
    private class TestDomainEvent : IDomainEvent
    {
        public int TestProperty { get; set; }
    }
    #endregion
}

#nullable enable
