using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace RegexRules;

[YamlSerializable]
public class PatternProperties : IPatternProperties
{
  [JsonPropertyName("Name")]
  [YamlMember(Alias = "Name")]
  public string? Name { get; set; }

  [JsonPropertyName("GroupType")]
  [YamlMember(Alias = "GroupType")]
  [AllowedValues("NonCapturing", "Capturing", "NamedCapturing")]
  public string? GroupType { get; set; }

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

  public string SerializeYaml()
  {
    var serializer = new SerializerBuilder().Build();
    var yaml = serializer.Serialize(this);
    return yaml;
  }

  public string SerializeJson()
  {
    var json = JsonSerializer.Serialize(this);
    return json;
  }

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

  public void DeserializeJson(string jsonString)
  {
    var pattern = JsonSerializer.Deserialize<PatternProperties>(jsonString) ?? throw new Exception("Invalid JSON");

    Name = pattern.Name ?? string.Empty;
    GroupType = pattern.GroupType ?? string.Empty;
    NamedGroupStyle = pattern.NamedGroupStyle ?? string.Empty;

  }

}