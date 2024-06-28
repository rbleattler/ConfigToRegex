using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ConfigToRegex;

/// <summary>
/// Represents the properties of a Regular Expression Pattern as an object. This is only used for <see cref="GroupPattern"/> objects.
/// </summary>
[YamlSerializable]
public class PatternProperties : IPatternProperties
{
  /// <summary>
  /// The name of the group
  /// </summary>
  [JsonPropertyName("Name")]
  [YamlMember(Alias = "Name")]
  public string? Name { get; set; }

  /// <summary>
  /// The type of group
  /// </summary>
  /// <value>NonCapturing, Capturing, NamedCapturing</value>
  [JsonPropertyName("GroupType")]
  [YamlMember(Alias = "GroupType")]
  [AllowedValues("NonCapturing", "Capturing", "NamedCapturing")]
  public string? GroupType { get; set; }

  /// <summary>
  /// The style of the named group
  /// </summary>
  /// <value>SingleQuote, AngleBrackets, PStyle</value>
  [JsonPropertyName("NamedGroupStyle")]
  [YamlMember(Alias = "NamedGroupStyle")]
  [AllowedValues("SingleQuote", "AngleBrackets", "PStyle")]
  public string? NamedGroupStyle { get; set; }

  public PatternProperties()
  { }

  public PatternProperties(string patternPropertiesObject)
  {
    if (!string.IsNullOrWhiteSpace(patternPropertiesObject))
    {
      if (patternPropertiesObject.StartsWith('{'))
      {
        var patternProperties = JsonSerializer.Deserialize<PatternProperties>(patternPropertiesObject);
        Name = patternProperties?.Name;
        GroupType = patternProperties?.GroupType;
        NamedGroupStyle = patternProperties?.NamedGroupStyle;
      }
      else
      {
        var deserializer = new Deserializer();
        var patternProperties = deserializer.Deserialize<PatternProperties>(patternPropertiesObject);
        Name = patternProperties.Name;
        GroupType = patternProperties.GroupType;
        NamedGroupStyle = patternProperties.NamedGroupStyle;
      }
    }
  }

  /// <summary>
  /// Serializes the object to a YAML string
  /// </summary>
  /// <returns><see cref="string"/> representing the object in YAML format</returns>
  public string SerializeYaml()
  {
    var serializer = new SerializerBuilder().Build();
    var yaml = serializer.Serialize(this);
    return yaml;
  }

  /// <summary>
  /// Serializes the object to a JSON string
  /// </summary>
  /// <returns><see cref="string"/> representing the object in JSON format</returns>
  public string SerializeJson()
  {
    var json = JsonSerializer.Serialize(this);
    return json;
  }

  /// <summary>
  /// Deserializes a YAML string to a <see cref="PatternProperties"/>
  /// </summary>
  /// <param name="yamlString"></param>
  public void DeserializeYaml(string yamlString)
  {
    var deserializer = new Deserializer();
    var pattern = deserializer.Deserialize<PatternProperties>(yamlString);
    if (pattern != null)
    {
      Name = pattern.Name ?? string.Empty;
      GroupType = pattern.GroupType ?? default;
      NamedGroupStyle = pattern.NamedGroupStyle ?? default;
    }
  }

  /// <summary>
  /// Deserializes a JSON string to a <see cref="PatternProperties"/>
  /// </summary>
  /// <param name="jsonString"></param>
  /// <exception cref="Exception"></exception>
  public void DeserializeJson(string jsonString)
  {
    var pattern = JsonSerializer.Deserialize<PatternProperties>(jsonString) ?? throw new InvalidJsonException("Invalid JSON");

    Name = pattern.Name ?? string.Empty;
    GroupType = pattern.GroupType ?? string.Empty;
    NamedGroupStyle = pattern.NamedGroupStyle ?? string.Empty;

  }

}