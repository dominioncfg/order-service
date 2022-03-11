namespace OrderService.Domain.UnitTests.Seedwork;
#nullable disable
public class EntityTests
{
    #region Equals
    [Fact]
    public void EqualsWhenIdIsEqualButDifferentTypeReturnsFalse()
    {
        var obj1 = new Entity1(1);
        var obj2 = new Entity2(1);

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
        var TestEntity = new Entity1(1);

        bool result = TestEntity.Equals(TestEntity);

        result.Should().BeTrue();
    }

    [Fact]
    public void EqualsWhenSameIdTrue()
    {
        var obj1 = new Entity1(1);
        var obj2 = new Entity1(1);

        bool result = obj1.Equals(obj2);

        result.Should().BeTrue();
    }


    [Fact]
    public void EqualsWhenSameIdButIs0False()
    {
        var obj1 = new Entity1(0);
        var obj2 = new Entity1(0);

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }

    [Fact]
    public void EqualsWhenSameIdAreDiferentFalse()
    {
        var obj1 = new Entity1(0);
        var obj2 = new Entity1(1);

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }

    [Fact]
    public void EqualsWhenOneNullValuesStringFalse()
    {
        var obj1 = new Entity3("");
        var obj2 = new Entity3(null);

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }
    #endregion

    #region Enitities
    private class Entity1 : Entity<int>
    {
        public Entity1() { }
        public Entity1(int id) { Id = id; }
    }

    private class Entity2 : Entity<int>
    {
        public Entity2() { }
        public Entity2(int id) { Id = id; }
    }

    private class Entity3 : Entity<string>
    {
        public Entity3() { }
        public Entity3(string id) { Id = id; }
    }
    #endregion
}
#nullable enable
