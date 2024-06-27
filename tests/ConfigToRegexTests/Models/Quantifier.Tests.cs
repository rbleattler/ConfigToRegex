using ConfigToRegex;
using System.Runtime.Serialization;

namespace ConfigToRegexTests;


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

    [Fact]
    public void CanBeGreedy_ReturnsTrue_WhenGreedyIsTrue_And_PatternDoesNotEndWithQuantifier()
    {
        // Arrange
        var quantifier = new Quantifier
        {
            Greedy = true
        };
        var pattern = "abc";

        // Act
        var result = quantifier.CanBeGreedy(pattern);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanBeGreedy_ReturnsFalse_WhenPatternEndsWithQuantifier()
    {
        // Arrange
        var quantifier = new Quantifier();
        var pattern = "abc*";

        // Act
        var result = quantifier.CanBeGreedy(pattern);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanBeGreedy_ReturnsFalse_WhenPatternEndsWithExactlyQuantifier()
    {
        // Arrange
        var quantifier = new Quantifier();
        var pattern = "abc{2}";

        // Act
        var result = quantifier.CanBeGreedy(pattern);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanBeGreedy_ReturnsFalse_WhenPatternEndsWithMinAndMaxQuantifier()
    {
        // Arrange
        var quantifier = new Quantifier();
        var pattern = "abc{2,5}";

        // Act
        var result = quantifier.CanBeGreedy(pattern);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanBeGreedy_ReturnsFalse_WhenPatternEndsWithLazyQuantifier()
    {
        // Arrange
        var quantifier = new Quantifier();
        var pattern = "abc?";

        // Act
        var result = quantifier.CanBeGreedy(pattern);

        // Assert
        Assert.False(result);
    }
}