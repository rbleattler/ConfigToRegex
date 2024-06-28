using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using NJsonSchema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Text;

namespace ConfigToRegex;

/// <summary>
/// Represents a Regular Expression Pattern as an object.
/// </summary>
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

  [JsonPropertyName("Properties")]
  [YamlMember(Alias = "Properties")]
  public IPatternProperties? Properties { get; set; }


  /// <summary>
  /// The unique identifier for the pattern
  /// </summary>
  [JsonPropertyName("Id")]
  [YamlMember(Alias = "Id")]
  public string? Id { get; set; } = Guid.NewGuid().ToString();

  /// <summary>
  /// The type of pattern. The default value is "Literal".
  /// </summary>
  /// <value>Literal, Anchor, CharacterClass, Group</value>
  /// <exception cref="ArgumentException">Thrown when an invalid pattern type is set.</exception>
  [Required]
  [JsonPropertyName("Type")]
  [YamlMember(Alias = "Type")]
  // [AllowedValues("Literal", "Anchor", "CharacterClass", "Group")]
  public string Type
  {
    get => _type;
    set
    {
      if (!IsValidPatternType(value))
      {
        throw new ArgumentException("Invalid Pattern Type");
      }
      else
      {
        _type = value;
      }
    }
  }

  /// <summary>
  /// The <see cref="PatternValue"/> for the pattern. The default value is an empty string.
  /// </summary>
  [JsonPropertyName("Value")]
  [YamlMember(Alias = "Value")]
  public PatternValue Value { get; set; }

  /// <summary>
  /// The <see cref="Quantifier"/>s for the pattern
  /// </summary>
  [JsonPropertyName("Quantifiers")]
  [YamlMember(Alias = "Quantifiers")]
  public Quantifier? Quantifiers { get; set; }

  /// <summary>
  /// The message for the pattern. This is an optional parameter. It is used to provide additional information about the pattern.
  /// </summary>
  [JsonPropertyName("Message")]
  [YamlMember(Alias = "Message")]
  public string? Message { get; set; }

  /// <summary>
  /// The <see cref="JsonSchema"/> for the pattern.
  /// </summary>
  [JsonIgnore]
  [YamlIgnore]
  JsonSchema IPattern.JsonSchema => JsonSchema.FromType(GetType());

  public Pattern(string patternObject)
  {
    if (patternObject.StartsWith('{'))
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


  void IRegexSerializable.DeserializeYaml(string yamlString)
  {
    DeserializeYaml(yamlString);
  }

  void IRegexSerializable.DeserializeJson(string jsonString)
  {
    DeserializeJson(jsonString);
  }

  private static bool IsValidPatternType(string type)
  {
    return type == "Literal" || type == "Anchor" || type == "CharacterClass" || type == "Group";
  }

  /// <summary>
  /// Deserializes a YAML string to a <see cref="Pattern"/>
  /// </summary>
  /// <param name="patternObject"></param>
  public void DeserializeYaml(string patternObject)
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

  /// <summary>
  /// Deserializes a JSON string to a <see cref="Pattern"/>
  /// </summary>
  /// <param name="patternObject"></param>
  public void DeserializeJson(string patternObject)
  {
    var pattern = JsonSerializer.Deserialize<Pattern>(patternObject);
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

  /// <summary>
  /// Returns a string representation of the pattern (the <see cref="Value"/>) of the pattern.)
  /// </summary>
  /// <returns> <see cref="string"/> </returns>
  public override string ToString()
  {
    return Value.ToString()!;
  }

  /// <summary>
  /// Converts a <see cref="Pattern"/> to a <see cref="PatternValue"/>
  /// </summary>
  /// <param name="pattern"></param>
  public static implicit operator PatternValue(Pattern pattern)
  {
    return pattern.Value;
  }

  /// <summary>
  /// Serializes the <see cref="Pattern"/> to a YAML string
  /// </summary>
  /// <returns></returns>
  public string SerializeYaml()
  {
    var serializer = new SerializerBuilder().Build();
    var yaml = serializer.Serialize(this);
    return yaml;
  }

  /// <summary>
  /// Serializes the <see cref="Pattern"/> to a JSON string
  /// </summary>
  /// <returns></returns>
  public string SerializeJson()
  {
    var json = JsonSerializer.Serialize(this);
    return json;
  }

  /// <summary>
  /// Converts the <see cref="Pattern"/> to a regular expression string.
  /// </summary>
  /// <returns></returns>
  public string ToRegex()
  {
    StringBuilder regex = new();
    if (null != Value && !string.IsNullOrEmpty(Value.ToString()) && Type == "Literal")
    {
      regex.Append(Value.ToString());
    }
    else if (null != Value && !string.IsNullOrEmpty(Value.ToRegex()) && (Type == "Anchor" || Type == "CharacterClass" || Type == "Group"))
    {
      regex.Append(Value.ToRegex());
    }

    if (Quantifiers != null)
    {
      regex.Append(Quantifiers.ToRegex());
    }
    return regex.ToString();
  }

}
