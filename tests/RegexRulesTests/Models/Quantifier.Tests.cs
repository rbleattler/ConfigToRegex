using Xunit;
using RegexRules;
using System;
using System.Runtime.Serialization;

namespace RegexRulesTests;


public class QuantifierTests
{
    [Fact]
    public void ToRegex_WhenExactlyHasValue_ReturnsCorrectRegex()
    {
        Quantifier quantifier = new Quantifier { Exactly = 3 };
        Assert.Equal("{3}", quantifier.ToRegex(""));
    }

    [Fact]
    public void ToRegex_WhenMinIsZeroAndMaxIsNotSet_ReturnsStar()
    {
        Quantifier quantifier = new Quantifier { Min = 0 };
        Assert.Equal("*", quantifier.ToRegex("a"));
    }

    [Fact]
    public void ToRegex_WhenMinIsOneAndMaxIsNotSet_ReturnsPlus()
    {
        Quantifier quantifier = new Quantifier { Min = 1 };
        Assert.Equal("+", quantifier.ToRegex("a"));
    }

    [Fact]
    public void ToRegex_WhenMinIsZeroAndMaxIsOne_ReturnsQuestionMark()
    {
        Quantifier quantifier = new Quantifier { Min = 0, Max = 1 };
        Assert.Equal("?", quantifier.ToRegex("a"));
    }

    [Fact]
    public void ToRegex_WhenMaxIsLessThanMin_ThrowsException()
    {
        Quantifier quantifier = new Quantifier { Min = 2, Max = 1 };
        Assert.Throws<SerializationException>(() => quantifier.ToRegex("a"));
    }

    [Fact]
    public void ToRegex_WhenMinAndMaxHaveValuesAndMaxIsGreaterThanOrEqualToMin_ReturnsCorrectRegex()
    {
        Quantifier quantifier = new Quantifier { Min = 2, Max = 3 };
        Assert.Equal("{2,3}", quantifier.ToRegex("a"));
    }

    [Fact]
    public void ToRegex_WhenGreedyIsTrue_ReturnsStar()
    {
        Quantifier quantifier = new Quantifier { Greedy = true };
        Assert.Equal("*", quantifier.ToRegex("a"));
    }

    [Fact]
    public void ToRegex_WhenLazyIsTrue_ReturnsQuestionMark()
    {
        Quantifier quantifier = new Quantifier { Lazy = true };
        Assert.Equal("?", quantifier.ToRegex("a"));
    }
}