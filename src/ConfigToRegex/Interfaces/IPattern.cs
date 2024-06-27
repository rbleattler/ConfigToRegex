using YamlDotNet.Serialization;
using NJsonSchema;
using NJsonSchema.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ConfigToRegex;

public interface IPattern : IRegexSerializable
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

}