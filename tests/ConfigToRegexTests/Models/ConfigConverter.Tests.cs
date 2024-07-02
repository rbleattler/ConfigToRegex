using ConfigToRegex;
using ConfigToRegex.Helpers;
using ConfigToRegex.Exceptions;

namespace ConfigToRegexTests;

public class ConfigConverterTests
{
    [Fact(DisplayName = "DeserializeYaml throws InvalidYamlException for invalid YAML")]
    public void DeserializeYaml_InvalidYaml_ThrowsInvalidYamlException()
    {
        var converter = new ConfigConverter();
        var invalidYaml = "invalid: yaml: -";

        Assert.Throws<InvalidYamlException>(() => converter.DeserializeYaml<IRegexSerializable>(invalidYaml));
    }

    [Fact(DisplayName = "DeserializeYaml successfully deserializes valid YAML")]
    public void DeserializeYaml_ValidYaml_ReturnsDeserializedObject()
    {
        var converter = new ConfigConverter();
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
        var converter = new ConfigConverter();
        var pattern = new Pattern { Properties = null, Id = "TestId", Type = "Literal", Value = new PatternValue { Value = "abc" } };

        // Act
        var yamlString = converter.SerializeYaml(pattern);

        // Assert
        var expectedYaml = "Properties: \r\nId: TestId\r\nType: Literal\r\nValue:\r\n  Value: abc\r\nQuantifiers: \r\nMessage: \r\n";
        Assert.Equal(expectedYaml, yamlString);
    }

    // Example for a DeserializeJson method
    [Fact(DisplayName = "DeserializeJson throws InvalidJsonException for invalid JSON")]
    public void DeserializeJson_InvalidJson_ThrowsInvalidJsonException()
    {
        // Setup
        var invalidJson = "{invalid: json}";

        // Act & Assert
        Assert.Throws<InvalidJsonException>(() => ConfigConverter.DeserializeJson<Pattern>(invalidJson));
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
            pattern = ConfigConverter.DeserializeJson<Pattern>(json);

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
        var jsonString = ConfigConverter.SerializeJson(pattern);

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
            "\"Message\":null" +
            "}";

        var converter = new ConfigConverter();

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

        var expectedYaml = "Properties: " + "\r\n" +
        "Id: TestId" + "\r\n" +
        "Type: Literal" + "\r\n" +
        "Value:" + "\r\n" +
        "  Value: abc" + "\r\n" +
        "Quantifiers: " + "\r\n" +
        "Message: " + "\r\n";


        var converter = new ConfigConverter();

        // Act
        var actualYaml = converter.ConvertJsonToYaml<Pattern>(jsonString);

        // Assert
        Assert.Equal(expectedYaml, actualYaml);
    }

}