using FluentAssertions;
using OrderService.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OrderService.Domain.UnitTests.Seedwork;

public class ValueObjectTests
{
    private static readonly ValueObject APrettyValueObject = new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3"));

    #region Equals
    [Fact]
    public void Equals_When_Both_Nulls_Returns_True()
    {
        bool result = AreEqual(null, null);

        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_When_Same_Reference_Returns_True()
    {
        bool result = AreEqual(APrettyValueObject, APrettyValueObject);

        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_When_Have_Same_Members_Returns_True()
    {
        var obj1 = new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3"));
        var obj2 = new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3"));

        bool result = AreEqual(obj1, obj2);

        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_When_Non_Equality_Components_Are_Different_Returns_True()
    {
        var obj1 = new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3"), notAnEqualityComponent: "xpto");
        var obj2 = new ValueObjectA(1, "2", Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), new ComplexObject(2, "3"), notAnEqualityComponent: "xpto2");

        bool result = AreEqual(obj1, obj2);

        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_When_All_Components_Are_Equals_Returns_True()
    {
        var obj1 = new ValueObjectB(1, "2", 1, 2, 3);
        var obj2 = new ValueObjectB(1, "2", 1, 2, 3);

        bool result = AreEqual(obj1, obj2);

        result.Should().BeTrue();
    }
    #endregion

    #region Non Equals
    [Fact]
    public void Equals_When_First_Member_IsNotEqual_Returns_False()
    {
        var obj1 = new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3"));
        var obj2 = new ValueObjectA(a: 2, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3"));

        bool result = AreEqual(obj1, obj2);

        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_When_Any_Other_Member_IsNotEqual_Returns_False()
    {
        var obj1 = new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3"));
        var obj2 = new ValueObjectA(a: 1, b: null, c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(2, "3"));

        bool result = AreEqual(obj1, obj2);

        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_When_Inner_Member_IsNotEqual_Returns_False()
    {
        var obj1 = new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(a: 2, b: "3"));
        var obj2 = new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(a: 3, b: "3"));

        bool result = AreEqual(obj1, obj2);

        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_When_Different_Types_Returns_False()
    {
        var obj1 = new ValueObjectA(a: 1, b: "2", c: Guid.Parse("97ea43f0-6fef-4fb7-8c67-9114a7ff6ec0"), d: new ComplexObject(a: 2, b: "3"));
        var obj2 = new ValueObjectB(a: 1, b: "2");

        bool result = AreEqual(obj1, obj2);

        result.Should().BeFalse();
    }


    [Fact]
    public void Equals_When_Inner_List_Has_Different_Length_Returns_False()
    {
        var obj1 = new ValueObjectB(1, "2", 1, 2, 3);
        var obj2 = new ValueObjectB(1, "2", 1, 2, 3, 4);

        bool result = AreEqual(obj1, obj2);

        result.Should().BeFalse();
    }
    #endregion

    private static bool AreEqual(ValueObject? instanceA, ValueObject? instanceB)
    {
        return EqualityComparer<ValueObject>.Default.Equals(instanceA, instanceB);
    }

    #region ValueObjects
    private class ValueObjectA : ValueObject
    {
        public ValueObjectA(int a, string? b, Guid c, ComplexObject? d, string? notAnEqualityComponent = null)
        {
            A = a;
            B = b;
            C = c;
            D = d;
            NotAnEqualityComponent = notAnEqualityComponent;
        }

        public int A { get; }
        public string? B { get; }
        public Guid C { get; }
        public ComplexObject? D { get; }
        public string? NotAnEqualityComponent { get; }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return A;
            yield return B;
            yield return C;
            yield return D;
        }
    }

    private class ValueObjectB : ValueObject
    {
        public ValueObjectB(int a, string b, params int[] c)
        {
            A = a;
            B = b;
            C = c.ToList();
        }

        public int A { get; }
        public string B { get; }

        public List<int> C { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return A;
            yield return B;

            foreach (var c in C)
            {
                yield return c;
            }
        }
    }

    private class ComplexObject : IEquatable<ComplexObject>
    {
        public ComplexObject(int a, string b)
        {
            A = a;
            B = b;
        }

        public int A { get; set; }

        public string B { get; set; }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ComplexObject);
        }

        public bool Equals(ComplexObject? other)
        {
            return other != null &&
                   A == other.A &&
                   B == other.B;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(A, B);
        }
    }
    #endregion
}
