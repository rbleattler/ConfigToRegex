using System.Runtime.CompilerServices;
using System.Text.Json;
using RegexRules;

namespace RegexRulesTests;

public class PatternPropertiesTests : RegexRuleTestCore
{

    public string[] AllTestFiles => GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "patternProperties*.yml") ?? Array.Empty<string>();

    [Fact]
    public void AllPatternProperties_ConstructValidObjects_FromValidYaml()
    {
        for (var i = 0; i < AllTestFiles.Length; i++)
        {
            var PatternProperties = new PatternProperties(ReadFileAsString(AllTestFiles[i]));
            // Console.WriteLine($"Test file: {AllTestFiles[i]}");
            Assert.NotNull(PatternProperties);
        }
    }

    [Fact]
    public void DefaultConstructor_SetsNullProperties()
    {
        var patternProperties = new PatternProperties();

        Assert.Null(patternProperties.Name);
        Assert.Null(patternProperties.GroupType);
        Assert.Null(patternProperties.NamedGroupStyle);
    }

    [Fact]
    public void JsonConstructor_SetsProperties()
    {
        var json = "{\"Name\":\"test\",\"GroupType\":\"NonCapturing\",\"NamedGroupStyle\":\"SingleQuote\"}";
        var patternProperties = new PatternProperties(json);

        Assert.Equal("test", patternProperties.Name);
        Assert.Equal("NonCapturing", patternProperties.GroupType);
        Assert.Equal("SingleQuote", patternProperties.NamedGroupStyle);
    }

    [Fact]
    public void YamlConstructor_SetsProperties()
    {
        var yaml = "Name: test\nGroupType: NonCapturing\nNamedGroupStyle: SingleQuote";
        var patternProperties = new PatternProperties(yaml);

        Assert.Equal("test", patternProperties.Name);
        Assert.Equal("NonCapturing", patternProperties.GroupType);
        Assert.Equal("SingleQuote", patternProperties.NamedGroupStyle);
    }

    [Fact]
    public void InvalidJsonConstructor_Should_Throw()
    {
        Assert.Throws<JsonException>(() => new PatternProperties("{invalid json}"));


    }

    [Fact]
    public void InvalidYamlConstructor_Should_Throw()
    {
        // var patternProperties = ;
        Assert.Throws<YamlDotNet.Core.YamlException>(() => new PatternProperties("invalid yaml"));
    }

}