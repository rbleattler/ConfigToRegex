using YamlDotNet;
using YamlDotNet.Serialization;
using RegexRules;
using System.Text.Json;

namespace RegexRulesTests;

public class PatternTests : RegexRuleTestCore
{

    public string[] AllTestFiles => GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "basicPattern*.yml") ?? Array.Empty<string>();

    [Fact]
    public void AllPatterns_ConstructValidObjects_FromValidYaml()
    {
        for (var i = 0; i < AllTestFiles.Length; i++)
        {
            var Pattern = new Pattern(ReadFileAsString(AllTestFiles[i]));
            Console.WriteLine($"Test file: {AllTestFiles[i]}");
            Assert.NotNull(Pattern);
        }
    }

    [Fact]
    public void DefaultConstructorTest()
    {
        var pattern = new Pattern();
        Assert.NotNull(pattern.Value);
    }

    [Fact]
    public void ConstructorWithPatternValueTest()
    {
        var patternValue = new PatternValue("test");
        var pattern = new Pattern(patternValue);
        Assert.Equal(patternValue, pattern.Value);
    }

    [Fact]
    public void ConstructorWithAllParametersTest()
    {
        var patternValue = new PatternValue("test");
        var quantifier = new Quantifier();
        var properties = new PatternProperties();
        var pattern = new Pattern("id", "Literal", patternValue, quantifier, properties, "message");
        Assert.Equal("id", pattern.Id);
        Assert.Equal("Literal", pattern.Type);
        Assert.Equal(patternValue, pattern.Value);
        Assert.Equal(quantifier, pattern.Quantifiers);
        Assert.Equal(properties, pattern.Properties);
        Assert.Equal("message", pattern.Message);
    }

    [Fact]
    public void Constructor_WithStringParameter_Yaml()
    {
        var basicPatternFile = GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "basicPattern.yml");
        var patternYaml = ReadFileAsString(basicPatternFile[0]);
        // Convert from Yaml to Json to simplify the test
        var patternFromYaml = new Pattern(patternYaml);
        Assert.IsType<Guid>(Guid.Parse(patternFromYaml.Id!));
        Assert.Equal("Literal", patternFromYaml.Type);
        Assert.Equal("test", patternFromYaml.Value.Value);

    }
    [Fact]
    public void Constructor_WithStringParameter_Json()
    {
        var basicPatternFile = GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "basicPattern.yml");
        var patternYaml = ReadFileAsString(basicPatternFile[0]);
        // Convert from Yaml to Json to simplify the test
        var patternJson = YamlToJson(patternYaml);
        var patternFromJson = new Pattern(patternJson);
        Assert.IsType<string>(patternFromJson.Id);
        Assert.Equal("Literal", patternFromJson.Type);
        Assert.Equal("test", patternFromJson.Value.ToString());
    }

    private string YamlToJson(string patternYaml)
    {
        // use System.Text.Json and/or YamlDotNet to convert the Yaml string to Json
        var deserializer = new Deserializer();
        var pattern = deserializer.Deserialize<Pattern>(patternYaml);
        var jsonPattern = JsonSerializer.Serialize(pattern);
        return jsonPattern;
    }

    [Fact]
    public void TypeSetterTest()
    {
        var pattern = new Pattern();
        Assert.Throws<ArgumentException>(() => pattern.Type = "InvalidType");
        pattern.Type = "Anchor";
        Assert.Equal("Anchor", pattern.Type);
    }


    [Fact]
    public void ImplicitConversionToPatternValueTest()
    {
        var patternValue = new PatternValue("test");
        var pattern = new Pattern(patternValue);
        PatternValue convertedValue = pattern;
        Assert.Equal(patternValue, convertedValue);
    }
}
