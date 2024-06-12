using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RegexRules;

public interface IPatternProperties : ISerializable
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

  public bool IsJson(string patternPropertiesObject)
  {
    return patternPropertiesObject.StartsWith('{');
  }

  public bool IsYaml(string patternPropertiesObject)
  {
    return Regex.IsMatch(patternPropertiesObject, Validation.Patterns.Yaml);
  }


}