using FluentAssertions;
using OrderService.Domain.Seedwork;
using System.Linq;
using Xunit;

namespace OrderService.Domain.UnitTests.Seedwork;
#nullable disable
public class AgregateRootTests
{

    #region Equals
    [Fact]
    public void Equals_When_Id_Is_Equal_But_Different_Type_Returns_False()
    {
        var obj1 = new AggregateRoot1(1);
        var obj2 = new AggregateRoot2(1);

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_When_Both_Nulls_Returns_True()
    {
        Entity<int> obj1 = null;
        Entity<int> obj2 = null;
        bool result = obj1 == obj2;
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_When_Same_Reference_Returns_True()
    {
        var TestEntity = new AggregateRoot1(1);

        bool result = TestEntity.Equals(TestEntity);

        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_When_Same_Id_True()
    {
        var obj1 = new AggregateRoot1(1);
        var obj2 = new AggregateRoot1(1);

        bool result = obj1.Equals(obj2);

        result.Should().BeTrue();
    }


    [Fact]
    public void Equals_When_Same_Id_But_Is_0_False()
    {
        var obj1 = new AggregateRoot1(0);
        var obj2 = new AggregateRoot1(0);

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_When_Same_Id_Are_Diferent_False()
    {
        var obj1 = new AggregateRoot1(0);
        var obj2 = new AggregateRoot1(1);

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_When_One_Null_Values_String_False()
    {
        var obj1 = new AggregateRoot3("");
        var obj2 = new AggregateRoot3(null);

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }
    #endregion

    #region Domain Events
    [Fact]
    public void Can_Raise_Domain_Events()
    {
        var obj1 = new AggregateRoot1(1);
        var @event = new TestDomainEvent() { TestProperty = 1 };

        obj1.RaiseTestEvents(@event);

        obj1.DomainEvents.Should().NotBeEmpty();
        obj1.DomainEvents.Single().Should().Be(@event);
    }

    [Fact]
    public void Can_Clear_Domain_Events()
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
