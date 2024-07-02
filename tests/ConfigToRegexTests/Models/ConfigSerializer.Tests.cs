using ConfigToRegex;
using ConfigToRegex.Helpers;
using ConfigToRegex.Exceptions;
using FluentAssertions;
using Xunit;


namespace ConfigToRegexTests;

public class ConfigSerializerTests
{
    [Fact(DisplayName = "DeserializeYaml throws InvalidYamlException for invalid YAML")]
    public void DeserializeYaml_InvalidYaml_ThrowsInvalidYamlException()
    {
        var converter = new ConfigSerializer();
        var invalidYaml = "invalid: yaml: -";

        Action act = () => converter.DeserializeYaml<Pattern>(invalidYaml);

        act.Should()
           .Throw<InvalidYamlException>();
    }

    [Fact(DisplayName = "DeserializeYaml successfully deserializes valid YAML")]
    public void DeserializeYaml_ValidYaml_ReturnsDeserializedObject()
    {
        var converter = new ConfigSerializer();
        var validYaml = "Type: Literal\nValue:\n   Value: abc";
        var result = converter.DeserializeYaml<Pattern>(validYaml);

        Assert.NotNull(result);
        Assert.Equal("Literal", result.Type);
        Assert.Equal("abc", result.Value.Value);
    }

    // Example for a SerializeYaml method
    [Fact(DisplayName = "SerializeYaml successfully serializes object to YAML")]
    public void SerializeYaml_ValidObject_ReturnsYamlString()
    {
        // Setup
        var converter = new ConfigSerializer();
        var pattern = new Pattern { Properties = null, Id = "TestId", Type = "Literal", Value = new PatternValue { Value = "abc" } };

        // Act
        var yamlString = converter.SerializeYaml(pattern);

        // Assert
        var expectedYaml = "Properties: Id: TestIdType: LiteralValue:  Value: abcQuantifiers: Message: ";
        expectedYaml = expectedYaml.Replace("\r\n", "").Replace("\n", "").Replace(" ", "");
        yamlString = yamlString.Replace("\r\n", "").Replace("\n", "").Replace(" ", "");
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

        var converter = new ConfigSerializer();

        // Act
        var actualJson = converter.ConvertYamlToJson<Pattern>(yamlString);

        // Assert
        Assert.Equal(expectedJson, actualJson);
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


        var converter = new ConfigSerializer();

        // Act
        var result = converter.ConvertJsonToYaml<Pattern>(jsonString);
        result = result.Replace("\r\n", "")
                       .Replace("\n", "")
                       .Replace(" ", "");
        expectedYaml = expectedYaml.Replace("\r\n", "")
                                   .Replace("\n", "")
                                   .Replace(" ", "");
        // Assert (Using FluentAssertions)
        result.Should().Be(expectedYaml);


    }

}