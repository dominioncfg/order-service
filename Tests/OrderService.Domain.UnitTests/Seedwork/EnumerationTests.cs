namespace OrderService.Domain.UnitTests.Seedwork;

public class EnumerationTests
{
    #region Equals
    [Fact]
    public void EqualsWhenIdIsEqualButDifferentTypeReturnsFalse()
    {
        var obj1 = TestEnumeration1.T1V1;
        var obj2 = TestEnumeration1.T1V2;

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }

    [Fact]
    public void EqualsWhenSameReferenceReturnsTrue()
    {
        bool result = TestEnumeration1.T1V1.Equals(TestEnumeration1.T1V1);
        result.Should().BeTrue();
    }

    [Fact]
    public void EqualsWhenSameIdTrueEvenIfValueIsDifferent()
    {
        var obj1 = TestEnumeration1.T1V1;
        var obj2 = new TestEnumeration1(1, "SS");

        bool result = obj1.Equals(obj2);

        result.Should().BeTrue();
    }

    [Fact]
    public void EqualsWhenSameIdAreDiferentFalse()
    {
        var obj1 = TestEnumeration1.T1V1;
        var obj2 = TestEnumeration1.T1V2;

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }

    [Fact]
    public void EqualsWhenOtherIsNotEnumFalse()
    {
        var obj1 = TestEnumeration1.T1V1;
        var obj2 = NonEnumerationClass.T3V1;

        bool result = obj1.Equals(obj2);

        result.Should().BeFalse();
    }
    #endregion

    #region From Id
    [Fact]
    public void CanMapFromId()
    {
        var obj1 = Enumeration.FromValue<TestEnumeration1>(1);

        bool result = obj1.Equals(TestEnumeration1.T1V1);

        result.Should().BeTrue();
    }

    [Fact]
    public void MapFromIdThrowsExceptionWhenNotFound()
    {
        Action action = () => Enumeration.FromValue<TestEnumeration1>(10);

        action.Should().Throw<InvalidOperationException>();
    }
    #endregion

    #region From Name
    [Fact]
    public void CanMapFromName()
    {
        var obj1 = Enumeration.FromDisplayName<TestEnumeration1>("Value1");

        bool result = obj1.Equals(TestEnumeration1.T1V1);

        result.Should().BeTrue();
    }

    [Fact]
    public void MapFromNameThrowsExceptionWhenNotFound()
    {
        Action action = () => Enumeration.FromDisplayName<TestEnumeration1>("InvalidValue");

        action.Should().Throw<InvalidOperationException>();
    }
    #endregion

    #region Get All
    [Fact]
    public void CanGetAll()
    {
        var values = Enumeration.GetAll<TestEnumeration1>();

        values.Should().NotBeNull().And.HaveCount(2);
        values.Should().Contain(TestEnumeration1.T1V1);
        values.Should().Contain(TestEnumeration1.T1V2);
    }

    [Fact]
    public void CanGetAllWhenEmptyWorks()
    {
        var values = Enumeration.GetAll<EmptyEnumerationClass>();
        values.Should().NotBeNull().And.BeEmpty();
    }
    #endregion

    #region Enumerations
    private class TestEnumeration1 : Enumeration
    {
        public static readonly TestEnumeration1 T1V1 = new(1, "Value1");
        public static readonly TestEnumeration1 T1V2 = new(2, "Value2");
        public TestEnumeration1(int id, string name) : base(id, name) { }
    }

    private class TestEnumeration2 : Enumeration
    {
        public static readonly TestEnumeration2 T2V1 = new(1, "Value1");
        public static readonly TestEnumeration2 T2V2 = new(2, "Value2");
        protected TestEnumeration2(int id, string name) : base(id, name) { }
    }
    private class NonEnumerationClass
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public static readonly NonEnumerationClass T3V1 = new(1, "Value1");
        public static readonly NonEnumerationClass T3V2 = new(2, "Value2");
        protected NonEnumerationClass(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    private class EmptyEnumerationClass : Enumeration
    {
        protected EmptyEnumerationClass(int id, string name) : base(id, name) { }
    }

    #endregion
}