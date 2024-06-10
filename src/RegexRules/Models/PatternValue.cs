using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using NJsonSchema;
using NJsonSchema.Yaml;
using System.Text.RegularExpressions;


namespace RegexRules;

[YamlSerializable]

public class PatternValue : IPatternValue
{

  [JsonPropertyName("Value")]
  [YamlMember(Alias = "Value", Description = "The value of the pattern.")]
  public dynamic Value { get; set; }

  [JsonIgnore]
  [YamlIgnore]
  public JsonSchema JsonSchema => JsonSchema.FromType(GetType());

  public static implicit operator string(PatternValue patternValue)
  {
    if (patternValue.Value != null)
      return patternValue.Value.ToString();
    else
      return string.Empty;
  }

  public PatternValue(dynamic value)
  {
    Value = value;
  }

  public PatternValue(string value)
  {
    if (value == null)
    {
      Value = string.Empty;
      return;
    }
    else if (!string.IsNullOrEmpty(value) && value.StartsWith("{") && value.EndsWith("}"))
    {
      var deserializedValue = JsonSerializer.Deserialize<PatternValue>(value);
      Value = deserializedValue!.Value ?? string.Empty;
    }
    else if (Regex.Match(value, @".*Value:.*").Success)
    {
      // if this is yaml
      var deserializer = new Deserializer();
      var deserializedValue = deserializer.Deserialize<PatternValue>(value);
      Value = deserializedValue!.Value ?? string.Empty;
    }
    else
    {
      Value = value;
    }
  }


  public PatternValue()
  {
    Value = string.Empty;
  }


  public override string? ToString()
  {
    return Value.ToString();
  }


  public void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    info.AddValue("Value", Value);
  }
}