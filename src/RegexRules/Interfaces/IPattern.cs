using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using NJsonSchema;
using NJsonSchema.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace RegexRules;

public interface IPattern
{
  public string? Message { get; set; }

  // [JsonSchemaExtensionData("id", "string")]
  public string? Id { get; set; }

  [AllowedValues("Literal", "Anchor", "CharacterClass", "Group")]
  public string Type { get; set; }

  public IPatternValue Value { get; set; }


  public IQuantifier? Quantifiers { get; set; }


  public IPatternProperties? Properties { get; set; }

  [JsonSchemaIgnore]
  [YamlIgnore]
  public JsonSchema JsonSchema { get; }

  private static bool IsValidPatternType(string type)
  {
    return type == "Literal" || type == "Anchor" || type == "CharacterClass" || type == "Group";
  }

  internal bool IsJson(string patternObject)
  {
    return patternObject.StartsWith("{") && patternObject.EndsWith("}");
  }

  internal bool IsYaml(string patternObject)
  {
    return Regex.IsMatch(patternObject, Validation.Patterns.Yaml);
  }

  private void DeserializeYaml(string patternObject)
  {
    var deserializer = new Deserializer();
    var pattern = deserializer.Deserialize<Pattern>(patternObject);
    if (pattern != null)
    {
      Id = pattern.Id ?? Guid.NewGuid().ToString();
      Type = pattern.Type ?? "Literal";
      Value = pattern.Value ?? new PatternValue(string.Empty);
      Quantifiers = pattern.Quantifiers ?? null;
      Properties = pattern.Properties ?? null;
      Message = pattern.Message ?? string.Empty;
    }
  }

  private void DeserializeJson(string patternObject)
  {
    var pattern = JsonSerializer.Deserialize<IPattern>(patternObject);
    if (pattern != null)
    {
      Id = pattern.Id;
      Type = pattern.Type;
      Value = pattern.Value;
      Quantifiers = pattern.Quantifiers;
      Properties = pattern.Properties;
      Message = pattern.Message;
    }
  }

}