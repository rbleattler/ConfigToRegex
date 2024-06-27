using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using NJsonSchema;
using System.Text.RegularExpressions;


namespace ConfigToRegex;

[YamlSerializable]

public class PatternValue : IPatternValue
{

  [JsonPropertyName("Value")]
  [YamlMember(Alias = "Value", Description = "The value of the pattern.")]
  public dynamic? Value { get; set; }

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
    else if (!string.IsNullOrEmpty(value) && value.StartsWith('{') && value.EndsWith('}'))
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

  }


  public override string? ToString()
  {
    // We want to return the value of the pattern as a string. If the value is another type of pattern, we want to return the value of that pattern as a string.
    if (Value is PatternValue patternValue)
    {
      return patternValue.ToString();
    }
    return Value.ToString();

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
    var pattern = deserializer.Deserialize<PatternValue>(yamlString);
    if (pattern != null)
    {
      Value = pattern.Value ?? string.Empty;
    }
  }

  public void DeserializeJson(string jsonString)
  {
    var pattern = JsonSerializer.Deserialize<PatternValue>(jsonString) ?? throw new ArgumentException("Invalid JSON");

    if (null != pattern)
    {
      Value = pattern.Value ?? string.Empty;
    }
  }

  void IRegexSerializable.DeserializeJson(string jsonString)
  {
    DeserializeJson(jsonString);
  }

  void IRegexSerializable.DeserializeYaml(string yamlString)
  {
    DeserializeYaml(yamlString);
  }

  public string ToRegex()
  {
    if (Value is PatternValue patternValue)
    {
      return patternValue.ToRegex();
    }
    return Value!.ToString();
  }
}