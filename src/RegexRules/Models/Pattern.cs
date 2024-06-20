using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using NJsonSchema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RegexRules;

[YamlSerializable]
public class Pattern : IPattern
{
  private string _type = "Literal";

  IPatternValue IPattern.Value
  {
    get => Value;
    set => Value = (PatternValue)value;
  }
  IQuantifier? IPattern.Quantifiers
  {
    get => Quantifiers;
    set => Quantifiers = (Quantifier?)value;
  }


  [JsonPropertyName("Id")]
  [YamlMember(Alias = "Id")]
  public string? Id { get; set; } = Guid.NewGuid().ToString();

  [Required]
  [JsonPropertyName("Type")]
  [YamlMember(Alias = "Type")]
  // [AllowedValues("Literal", "Anchor", "CharacterClass", "Group")]
  public string Type
  {
    get => _type;
    set
    {
      if (IsValidPatternType(value) == false)
      {
        throw new ArgumentException("Invalid Pattern Type");
      }
      else
      {
        _type = value;
      }
    }
  }

  [JsonPropertyName("Value")]
  [YamlMember(Alias = "Value")]
  public PatternValue Value { get; set; }

  [JsonPropertyName("Quantifiers")]
  [YamlMember(Alias = "Quantifiers")]
  public Quantifier? Quantifiers { get; set; }

  [JsonPropertyName("Properties")]
  [YamlMember(Alias = "Properties")]
  public IPatternProperties? Properties { get; set; }

  [JsonPropertyName("Message")]
  [YamlMember(Alias = "Message")]
  public string? Message { get; set; }

  [JsonIgnore]
  [YamlIgnore]
  JsonSchema IPattern.JsonSchema => JsonSchema.FromType(GetType());

  public Pattern(string patternObject)
  {
    if (patternObject.StartsWith("{"))
    {
      DeserializeJson(patternObject);
    }
    else if (Regex.IsMatch(patternObject, Validation.Patterns.Yaml))
    {
      DeserializeYaml(patternObject);
    }
    if (Value == null)
    {
      Value = new PatternValue(string.Empty);
    }

  }

  public Pattern(string id, string type, PatternValue value, Quantifier quantifiers, IPatternProperties properties, string message)
  {
    Id = id;
    Type = type;
    Value = value;
    Quantifiers = quantifiers;
    Properties = properties;
    Message = message;
  }


  public Pattern()
  {
    Value = new PatternValue(string.Empty);
  }

  public Pattern(PatternValue pattern)
  {
    Value = pattern;
  }

  public override string ToString()
  {
    return Value.ToString()!;
  }

  public static implicit operator PatternValue(Pattern pattern)
  {
    return pattern.Value;
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
    var pattern = JsonSerializer.Deserialize<Pattern>(patternObject);
    if (pattern != null)
    {
      Id = pattern.Id;
      Type = pattern.Type;
      Value = (PatternValue)pattern.Value;
      Quantifiers = (Quantifier?)pattern.Quantifiers;
      Properties = pattern.Properties;
      Message = pattern.Message;
    }
  }

  private static bool IsValidPatternType(string type)
  {
    return type == "Literal" || type == "Anchor" || type == "CharacterClass" || type == "Group";
  }


  public string SerializeYaml()
  {
    //TODO: Implement Pattern.SerializeYaml()
    throw new NotImplementedException();
  }

  public string SerializeJson()
  {
    //TODO: Implement Pattern.SerializeJson()
    throw new NotImplementedException();
  }

  void IRegexSerializable.DeserializeYaml(string yamlString)
  {
    DeserializeYaml(yamlString);
  }

  void IRegexSerializable.DeserializeJson(string jsonString)
  {
    DeserializeJson(jsonString);
  }

  public string ToRegex()
  {
    //TODO: Implement Pattern.ToRegex()
    throw new NotImplementedException();
  }

}
