using System.Runtime.CompilerServices;
using FluentRegex;
using RegexRules;
using YamlDotNet.Serialization;

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
        var pattern = new CharacterClassPattern("Digit", new Quantifier { Min = 1, Max = 3 });

        Assert.Equal("\\d", pattern.ToString());
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
        var pattern = new CharacterClassPattern("\\d");

        Assert.True(pattern.IsYaml("Id: test"));
        // Will throw an exception if the pattern is not valid yaml

        Assert.False(pattern.IsYaml("not yaml"));
    }

    [Fact]
    public void DeserializeJson_DeserializesCorrectly()
    {
        var pattern = new CharacterClassPattern("\\d");
        pattern.DeserializeJson("{\"Id\":\"test\",\"Type\":\"CharacterClass\",\"Value\":{\"Value\":\"Digit\"}}");

        Assert.Equal("CharacterClass", pattern.Type);
        Assert.Equal("\\d", pattern.Value.ToString());
    }

    [Fact]
    public void DeserializeYaml_DeserializesCorrectly()
    {
        var pattern = new CharacterClassPattern("\\d");
        var basicObject = new Deserializer().Deserialize<object>(ReadFileAsString(AllTestFiles[0]));
        pattern.DeserializeYaml(ReadFileAsString(AllTestFiles[0]));

        Assert.Equal("CharacterClass", pattern.Type);
        Assert.Equal("\\d", pattern.Value.ToString());
    }

    [Fact]
    public void GetCharacterClass_ReturnsCorrectValue()
    {
        Assert.Equal(CharacterClasses.Digit, CharacterClassPattern.GetCharacterClass("Digit"));
        Assert.Throws<ArgumentException>(() => CharacterClassPattern.GetCharacterClass("Invalid"));
    }

}