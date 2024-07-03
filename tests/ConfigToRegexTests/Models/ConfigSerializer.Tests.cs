using ConfigToRegex;
using ConfigToRegex.Helpers;
using ConfigToRegex.Exceptions;
using FluentAssertions;
using Xunit;


namespace ConfigToRegexTests;

public class ConfigSerializerTests : RegexRuleTestCore
{
    public static string[] AllTestFiles => GetAllTestFiles(directory: ExampleFilesDirectory, searchPatterns: default) ?? [];

    // We are trimming all the strings to remove any extra spaces, new lines, etc. because nix and windows have different line endings which can cause the tests to fail when comparing strings.
    internal static string TrimString(string input)
    {
        return input.Replace("\r\n", "").Replace("\n", "").Replace(" ", "");
    }

    [Fact(DisplayName = "DeserializeYaml throws InvalidYamlException for invalid YAML")]
    public void DeserializeYaml_InvalidYaml_ThrowsInvalidYamlException()
    {
        var invalidYaml = "invalid: yaml: -";

        Action act = () => ConfigSerializer.DeserializeYaml<Pattern>(invalidYaml);

        act.Should()
           .Throw<InvalidYamlException>();
    }

    [Fact(DisplayName = "DeserializeYaml successfully deserializes valid YAML")]
    public void DeserializeYaml_ValidYaml_ReturnsDeserializedObject()
    {
        var validYaml = "Type: Literal\nValue:\n   Value: abc";
        var result = ConfigSerializer.DeserializeYaml<Pattern>(validYaml);

        Assert.NotNull(result);
        Assert.Equal("Literal", result.Type);
        Assert.Equal("abc", result.Value.Value);
    }

    // Example for a SerializeYaml method
    [Fact(DisplayName = "SerializeYaml successfully serializes object to YAML")]
    public void SerializeYaml_ValidObject_ReturnsYamlString()
    {
        // Setup
        var pattern = new Pattern { Properties = null, Id = "TestId", Type = "Literal", Value = new PatternValue { Value = "abc" } };

        // Act
        var yamlString = ConfigSerializer.SerializeYaml(pattern);

        // Assert
        var expectedYaml = "Properties: Id: TestIdType: LiteralValue:  Value: abcQuantifiers: Message: ";
        // expectedYaml = expectedYaml.Replace("\r\n", "").Replace("\n", "").Replace(" ", "");
        expectedYaml = TrimString(expectedYaml);
        // yamlString = yamlString.Replace("\r\n", "").Replace("\n", "").Replace(" ", "");
        yamlString = TrimString(yamlString);
        Assert.Equal(expectedYaml, yamlString);
    }

    // Example for a DeserializeJson method
    [Fact(DisplayName = "DeserializeJson throws InvalidJsonException for invalid JSON")]
    public void DeserializeJson_InvalidJson_ThrowsInvalidJsonException()
    {
        // Setup
        var invalidJson = "{invalid: json}";

        // Act & Assert
        Assert.Throws<InvalidJsonException>(() => ConfigSerializer.DeserializeJson<Pattern>(invalidJson));
    }

    [Fact(DisplayName = "DeSerializeJson successfully Deserializes object from JSON")]
    public void DeSerializeJson_ValidJson_ReturnsDeserializedObject()
    {
        // Setup
        var jsonString = "{" +
            "\"Properties\":null," +
            "\"Id\":\"TestId\"," +
            "\"Type\":\"Literal\"," +
            "\"Value\":{\"Value\":\"abc\"}," +
            "\"Quantifiers\":null," +
            "\"Message\":null" +
            "}";

        var jsonString2 = "{" +
            "\"Id\":\"TestId\"," +
            "\"Type\":\"Literal\"," +
            "\"Value\":{\"Value\":\"abc\"}" +
            "}";

        Pattern pattern;
        foreach (var json in new[] { jsonString, jsonString2 })
        {
            // Act
            pattern = ConfigSerializer.DeserializeJson<Pattern>(json);

            // Assert
            Assert.NotNull(pattern);
            Assert.Equal("TestId", pattern.Id);
            Assert.Equal("Literal", pattern.Type);
            Assert.IsType<PatternValue>(pattern.Value);
            var patternValueString = pattern.Value.Value!.ToString();
            Assert.Equal("abc", patternValueString);
        }
    }

    // Example for a SerializeJson method
    [Fact(DisplayName = "SerializeJson successfully serializes object to JSON")]
    public void SerializeJson_ValidObject_ReturnsJsonString()
    {
        // Setup
        var pattern = new Pattern { Properties = null, Id = "TestId", Type = "Literal", Value = new PatternValue { Value = "abc" } };

        // Act
        var jsonString = ConfigSerializer.SerializeJson(pattern);

        // Assert
        var expectedJson = "{" +
            "\"Properties\":null," +
            "\"Id\":\"TestId\"," +
            "\"Type\":\"Literal\"," +
            "\"Value\":{\"Value\":\"abc\"}," +
            "\"Quantifiers\":null," +
            "\"Message\":null" +
            "}";
        Assert.Equal(expectedJson, jsonString);
    }

    [Fact(DisplayName = "ConvertYamlToJson_ShouldConvertYamlStringToJson")]
    public void ConvertYamlToJson_ShouldConvertYamlStringToJson()
    {
        // Arrange
        var yamlString = @"
        Id: TestId
        Properties: null
        Type: Literal
        Value:
          Value: abc
        Quantifiers: null
        Message: null
          ";

        var expectedJson = "{" +
            "\"Properties\":null," +
            "\"Id\":\"TestId\"," +
            "\"Type\":\"Literal\"," +
            "\"Value\":{\"Value\":\"abc\"}," +
            "\"Quantifiers\":null," +
            "\"Message\":\"\"" +
            "}";

        // Act
        var actualJson = ConfigSerializer.ConvertYamlToJson<Pattern>(yamlString);

        // Assert
        expectedJson.Should().BeEquivalentTo(actualJson);
    }

    [Fact(DisplayName = "ConvertJsonToYaml_ShouldConvertJsonStringToYaml")]
    public void ConvertJsonToYaml_ShouldConvertJsonStringToYaml()
    {
        // Arrange
        var jsonString = "{" +
            "\"Properties\":null," +
            "\"Id\":\"TestId\"," +
            "\"Type\":\"Literal\"," +
            // "\"Value\": {\"Value\":\"abc\"}," +
            "\"Value\": {\"Value\":\"abc\"}," +
            "\"Quantifiers\":null," +
            "\"Message\":null" +
            "}";

        // var expectedYaml = "Properties: " + "" +
        // "Id: TestId" + "" +
        // "Type: Literal" + "" +
        // "Value:" + "" +
        // "  Value: abc" + "" +
        // "Quantifiers: " + "" +
        // "Message: " + "";

        var expectedYaml = @"Properties:
Id: TestId
Type: Literal
Value:
    Value: abc
Quantifiers:
Message: ";

        // Act
        var result = ConfigSerializer.ConvertJsonToYaml<Pattern>(jsonString);
        result = TrimString(result);
        expectedYaml = TrimString(expectedYaml);
        // Assert (Using FluentAssertions)
        result.Should().Be(expectedYaml);


    }

    [Fact(DisplayName = "DeserializeYamlFromFile_ShouldDeserializeYamlFile")]
    public void DeserializeYamlFromFile_ShouldDeserializeYamlFile()
    {
        // Arrange
        var yamlFile = AllTestFiles.First((f) => f.Contains("basicpattern.yml"));

        // Act
        var pattern = ConfigSerializer.DeserializeYamlFromFile<Pattern>(yamlFile);

        // Assert
        pattern.Should().BeOfType<Pattern>();
        pattern.Type.Should().Be("Literal");
        pattern.Value.ToString().Should().Be("test");
    }

    [Fact(DisplayName = "DeserializeJsonFromFile_ShouldDeserializeJsonFile")]
    public void DeserializeJsonFromFile_ShouldDeserializeJsonFile()
    {
        // Arrange
        var jsonFile = AllTestFiles.First((f) => f.Contains("basicpattern.json"));

        // Act
        var pattern = ConfigSerializer.DeserializeJsonFromFile<Pattern>(jsonFile);

        // Assert
        pattern.Should().BeOfType<Pattern>();
        pattern.Type.Should().Be("Literal");
        pattern.Value.ToString().Should().Be("test");
    }

    [Fact(DisplayName = "ParseConfigFileToRegex_ShouldDeserializeYamlFileToRegex")]
    public void ParseConfigFileToRegex_ShouldDeserializeYamlFileToRegex()
    {
        // Arrange
        var yamlFile = AllTestFiles.First((f) => f.Contains("basicpattern.yml"));

        // Act
        var regex = ConfigSerializer.ParseConfigFileToRegex<Pattern>(yamlFile);

        // Assert
        regex.Should().BeOfType<string>();
        regex.Should().Be("test");
    }
    [Fact(DisplayName = "DeserializeConfigFileToRegex_ShouldDeserializeJsonFileToRegex")]
    public void ParseConfigFileToRegex_ShouldDeserializeJsonFileToRegex()
    {
        // Arrange
        var jsonFile = AllTestFiles.First((f) => f.Contains("basicpattern.json"));

        // Act
        var regex = ConfigSerializer.ParseConfigFileToRegex<Pattern>(jsonFile);

        // Assert
        regex.Should().BeOfType<string>();
        regex.Should().Be("test");
    }
    [Fact(DisplayName = "ConvertFileToRegex_ShouldConvertYamlFileToRegex")]
    public void ConvertFileToRegex_ShouldConvertYamlFileToRegex()
    {
        // Arrange
        var yamlFile = AllTestFiles.First((f) => f.Contains("basicpattern.yml"));

        // Act
        var regex = ConfigSerializer.ConvertFileToRegex(yamlFile);

        // Assert
        regex.Should().BeOfType<string>();
        regex.Should().Be("test");
    }
    [Fact(DisplayName = "ConvertFileToRegex_ShouldConvertJsonFileToRegex")]
    public void ConvertFileToRegex_ShouldConvertJsonFileToRegex()
    {
        // Arrange
        var jsonFile = AllTestFiles.First((f) => f.Contains("basicpattern.json"));

        // Act
        var regex = ConfigSerializer.ConvertFileToRegex(jsonFile);

        // Assert
        regex.Should().BeOfType<string>();
        regex.Should().Be("test");
    }
}