using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using NJsonSchema;
using System.Text.RegularExpressions;
using ConfigToRegex.Helpers;
using System.ComponentModel;


namespace ConfigToRegex;

/// <summary>
/// Represents the value of a Regular Expression Pattern as an object. This is used on all <see cref="IPattern"/> objects.
/// </summary>
[YamlSerializable]

public class PatternValue : IPatternValue
{

  private object _value;

  /// <summary>
  /// The value of the pattern. This can be a literal value, like a <see cref="string"/>, <see cref="char"/>, or another <see cref="PatternValue"/> object.
  /// </summary>
  [JsonPropertyName("Value")]
  [YamlMember(Alias = "Value", DefaultValuesHandling = DefaultValuesHandling.OmitNull)]
  public object Value
  {
    get => _value;
    set
    {
      if (value is string || value is PatternValue)
        _value = value;
      else if (value is not IRegexSerializable)
        _value = value.ToString()!;
      else
      {
        _value = string.Empty;
        throw new InvalidOperationException("Value must be of type string or PatternValue.");
      }
    }
  }

  /// <summary>
  /// The JSON Schema for the <see cref="PatternValue"/> object. This is a generated property and is not meant to be used directly.
  /// </summary>
  [JsonIgnore]
  [YamlIgnore]
  public JsonSchema JsonSchema => JsonSchema.FromType(GetType());

  /// <summary>
  /// Converts a <see cref="PatternValue"/> object to a <see cref="string"/>.
  /// </summary>
  /// <param name="patternValue"></param>
  public static implicit operator string(PatternValue patternValue)
  {
    if (!string.IsNullOrEmpty(patternValue.Value?.ToString()))
      return patternValue.Value!.ToString();
    else
      return string.Empty;
  }

  /// <summary>
  /// Converts a <see cref="string"/> to a <see cref="PatternValue"/> object.
  /// </summary>
  /// <param name="value"></param>
  public static implicit operator PatternValue(string value)
  {
    var isJson = StringUtilities.IsJson(value);
    var isYaml = StringUtilities.IsYaml(value);
    if (isJson)
    {
      return JsonSerializer.Deserialize<PatternValue>(value)!;
    }
    else if (isYaml)
    {
      return new Deserializer().Deserialize<PatternValue>(value);
    }
    else
    {
      return new PatternValue { Value = value };
    }

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

  void IRegexSerializable.DeserializeJson(string jsonString)
  {
    DeserializeJson(jsonString);
  }

  void IRegexSerializable.DeserializeYaml(string yamlString)
  {
    DeserializeYaml(yamlString);
  }

  /// <summary>
  /// Deserializes a YAML string into a <see cref="PatternValue"/>
  /// </summary>
  /// <returns><see cref="string"/> representing the <see cref="Value"/> of the pattern</returns>
  public override string? ToString()
  {
    // We want to return the value of the pattern as a string. If the value is another type of pattern, we want to return the value of that pattern as a string.

    if (string.IsNullOrWhiteSpace(Value.ToString()))
    {
      throw new InvalidOperationException("Value is null or empty");
    }
    return Value!.ToString();
  }
  /// <summary>
  /// Serializes the object to a YAML string
  /// </summary>
  /// <returns><see cref="string"/> representing the <see cref="Value"/> of the pattern</returns>
  public string SerializeYaml()
  {
    var serializer = new SerializerBuilder().Build();
    var yaml = serializer.Serialize(this);
    return yaml;
  }

  /// <summary>
  /// Serializes the object to a JSON string
  /// </summary>
  /// <returns><see cref="string"/> representing the <see cref="Value"/> of the pattern</returns>
  public string SerializeJson()
  {
    var json = JsonSerializer.Serialize(this);
    return json;
  }

  /// <summary>
  /// Deserializes a YAML string to this instance of <see cref="PatternValue"/>
  /// </summary>
  /// <param name="yamlString"></param>
  /// <exception cref="ArgumentException"></exception>
  public void DeserializeYaml(string yamlString)
  {
    var deserializer = new Deserializer();
    var pattern = deserializer.Deserialize<PatternValue>(yamlString);
    if (pattern != null)
    {
      Value = pattern.Value ?? string.Empty;
    }
  }

  /// <summary>
  /// Deserializes a JSON string to this instance of <see cref="PatternValue"/>
  /// </summary>
  /// <param name="jsonString"></param>
  /// <exception cref="ArgumentException"></exception>
  public void DeserializeJson(string jsonString)
  {
    var pattern = JsonSerializer.Deserialize<PatternValue>(jsonString) ?? throw new ArgumentException("Invalid JSON");


    Value = pattern.Value ?? string.Empty;

  }

  /// <summary>
  /// Converts the <see cref="PatternValue"/> to a <see cref="string"/> representation of a Regular Expression.
  /// </summary>
  /// <returns></returns>
  public string ToRegex()
  {
    if (Value is PatternValue patternValue)
    {
      return patternValue.ToRegex();
    }
    return Value?.ToString() ?? string.Empty;
  }
}