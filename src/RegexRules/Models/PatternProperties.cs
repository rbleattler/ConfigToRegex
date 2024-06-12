using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace RegexRules;

[YamlSerializable]
public class PatternProperties : IPatternProperties, ISerializable
{
  [JsonPropertyName("Name")]
  [YamlMember(Alias = "Name")]
  public string? Name { get; set; }

  [JsonPropertyName("GroupType")]
  [YamlMember(Alias = "GroupType")]
  [JsonConverter(typeof(JsonStringEnumConverter))]
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



  public void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    info.AddValue("Name", Name);
    info.AddValue("GroupType", GroupType);
    info.AddValue("NamedGroupStyle", NamedGroupStyle);
  }
}