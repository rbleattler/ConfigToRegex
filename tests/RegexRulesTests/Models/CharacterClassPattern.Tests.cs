using System.Runtime.CompilerServices;
using FluentRegex;
using RegexRules;

namespace RegexRulesTests;

public class CharacterClassPatternTests : RegexRuleTestCore
{
    public string[] AllTestFiles => GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "characterClassPattern*.yml") ?? Array.Empty<string>();

    [Fact]
    public void AllCharacterClassPatterns_ConstructValidObjects_FromValidYaml()
    {
        for (var i = 0; i < AllTestFiles.Length; i++)
        {
            var characterClassPattern = new CharacterClassPattern(ReadFileAsString(AllTestFiles[i]));
            Console.WriteLine($"Test file: {AllTestFiles[i]}");
            Assert.NotNull(characterClassPattern);
        }
    }

    [Fact]
    public void Constructor_InitializesCorrectly()
    {
        //FIXME: Fails on Evaluation of CharacterClassPattern.ToString(). not sure why, but it is crashing the test
        var pattern = new CharacterClassPattern("Digit", new Quantifier{ Min = 1, Max = 3 });

        Assert.Equal("\\D", pattern.ToString());
        Assert.Equal(1, pattern.Quantifiers?.Min);
        Assert.Equal(3, pattern.Quantifiers?.Max);
    }

    [Fact]
    public void ToString_ReturnsCorrectValue()
    {
        var pattern = new CharacterClassPattern("Word");

        Assert.Equal("\\w", pattern.ToString());
    }

    [Fact]
    public void IsJson_IdentifiesJsonCorrectly()
    {
        var pattern = new CharacterClassPattern("Digit");

        Assert.True(pattern.IsJson("{}"));
        Assert.False(pattern.IsJson("not json"));
    }

    [Fact]
    public void IsYaml_IdentifiesYamlCorrectly()
    {
        var pattern = new CharacterClassPattern();

        Assert.True(pattern.IsYaml("Id: test"));
        Assert.False(pattern.IsYaml("not yaml"));
    }

    [Fact]
    public void DeserializeJson_DeserializesCorrectly()
    {
        var pattern = new CharacterClassPattern();
        pattern.DeserializeJson("{\"Id\":\"test\",\"Type\":\"CharacterClass\",\"Value\":\"test value\"}");

        Assert.Equal("test", pattern.Id);
        Assert.Equal("CharacterClass", pattern.Type);
        Assert.Equal("test value", pattern.Value.ToString());
    }

    [Fact]
    public void DeserializeYaml_DeserializesCorrectly()
    {
        var pattern = new CharacterClassPattern();
        pattern.DeserializeYaml("Id: test\nType: CharacterClass\nValue: test value");

        Assert.Equal("test", pattern.Id);
        Assert.Equal("CharacterClass", pattern.Type);
        Assert.Equal("test value", pattern.Value.ToString());
    }

    [Fact]
    public void GetCharacterClass_ReturnsCorrectValue()
    {
        Assert.Equal(CharacterClasses.Digit, CharacterClassPattern.GetCharacterClass("Digit"));
        Assert.Throws<ArgumentException>(() => CharacterClassPattern.GetCharacterClass("Invalid"));
    }

}