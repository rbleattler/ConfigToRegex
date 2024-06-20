using System.Runtime.Serialization;
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
  [AllowedValues("NonCapturing", "Capturing", "Named")]
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
    //TODO: Implement PatternProperties.SerializeYaml
    throw new NotImplementedException();
  }

  public string SerializeJson()
  {
    //TODO: Implement PatternProperties.SerializeJson
    throw new NotImplementedException();
  }

  void IRegexSerializable.DeserializeJson(string jsonString)
  {
    //TODO: Implement PatternProperties.DeserializeJson
    throw new NotImplementedException();
  }

  public string ToRegex()
  {
    //TODO: Implement PatternProperties.ToRegex
    throw new NotImplementedException();
  }

  void IRegexSerializable.DeserializeYaml(string yamlString)
  {
    //TODO: Implement PatternProperties.DeserializeYaml
    throw new NotImplementedException();
  }
}