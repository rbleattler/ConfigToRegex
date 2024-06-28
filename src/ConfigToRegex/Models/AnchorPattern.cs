using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using FluentRegex;
using System.Text.Json;
using NJsonSchema;

namespace ConfigToRegex;

/// <summary>
/// Represents Regular Expression Anchor as an object.
/// </summary>
public class AnchorPattern : IAnchor
{
    // fields

    private readonly string _type = "Anchor";
    private PatternValue? _value;


    // inherited properties
    string? IPattern.Id { get => Id; set => Id = value; }

    IPatternValue IPattern.Value
    {
        get => _value ?? new PatternValue(string.Empty);
        set => _value = (PatternValue)value;
    }

    IQuantifier? IPattern.Quantifiers
    {
        get => Quantifiers;
        set => Quantifiers = (Quantifier?)value;
    }

    [JsonPropertyName("Properties")]
    [YamlMember(Alias = "Properties")]
    public IPatternProperties? Properties { get; set; }

    string IPattern.Type
    {
        get => Type;
        set => Type = value;
    }

    // properties

    /// <summary>
    /// The unique identifier for the pattern
    /// </summary>
    [JsonPropertyName("Id")]
    [YamlMember(Alias = "Id")]
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// The type of pattern
    /// </summary>
    [JsonPropertyName("Type")]
    [YamlMember(Alias = "Type")]
    public string Type
    {
        get => _type;
        set => _ = value;
    }

    /// <summary>
    /// The value of the pattern
    /// </summary>
    [JsonPropertyName("Value")]
    [YamlMember(Alias = "Value")]
    public PatternValue Value
    {
        get
        {
            return _value ?? new PatternValue(string.Empty);
        }
        set
        {
            var isValid = IsValidAnchorType(value) || IsValidAnchor(value);
            if (isValid == false)
            {
                throw new ArgumentException("Invalid Anchor.\n"
                                            + "Valid types are: " + string.Join(", ", GetValidAnchorTypes())
                                            + "\nValid Anchor literals are: " + string.Join(", ", GetValidAnchorLiterals()));
            }
            else
            {
                if (IsValidAnchorType(value))
                {
                    _value = new PatternValue(GetAnchor(value));
                }
                else
                {
                    _value = new PatternValue(value);
                }
            }
        }
    }

    /// <summary>
    /// The <see cref="Quantifier"/>s for the pattern.
    /// </summary>
    [JsonPropertyName("Quantifiers")]
    [YamlMember(Alias = "Quantifiers")]
    public Quantifier? Quantifiers { get; set; }

    /// <summary>
    /// The message for the pattern. This is an optional field, used to provide additional information about the pattern.
    /// </summary>
    [JsonPropertyName("Message")]
    [YamlMember(Alias = "Message")]
    public string? Message { get; set; }

    /// <summary>
    /// The JSON schema for the pattern. This is a generated property and is not intended to be set.
    /// </summary>
    [JsonIgnore]
    [YamlIgnore]
    public JsonSchema JsonSchema => JsonSchema.FromType(GetType());

    // constructors

    public AnchorPattern(string value, Quantifier? quantifiers = null)
    {
        Value = new PatternValue(value);
        Quantifiers = quantifiers;
    }

    public AnchorPattern(PatternValue value, Quantifier? quantifiers = null)
    {
        Value = value;
        Quantifiers = quantifiers;
    }

    public AnchorPattern()
    { }

    public AnchorPattern(string anchorPatternObject)
    {
        if (string.IsNullOrWhiteSpace(anchorPatternObject))
        {
            Value = new PatternValue(string.Empty);
        }
        if (IsJson(anchorPatternObject))
        {
            DeserializeJson(anchorPatternObject);
        }
        else if (IsYaml(anchorPatternObject))
        {
            DeserializeYaml(anchorPatternObject);
        }
    }

    void IRegexSerializable.DeserializeYaml(string yamlString)
    {
        DeserializeYaml(yamlString);
    }

    void IRegexSerializable.DeserializeJson(string jsonString)
    {
        DeserializeJson(jsonString);
    }

    private bool IsJson(string patternObject) => ((IPattern)this).IsJson(patternObject);

    private bool IsYaml(string patternObject) => ((IPattern)this).IsYaml(patternObject);

    private static string GetAnchor(string value)
    {
        // get the value of the appropriate anchor from the static class Anchors in FluentRegex
        string anchor = default!;
        typeof(Anchors).GetFields().ToList().ForEach(f =>
        {
            if (f.Name.ToLower() == value.ToLower())
            {
                anchor = (string)f.GetValue(null)!;
            }
        });
        if (anchor == default)
        {
            throw new ArgumentException("Invalid Anchor Type. (" + value + ") Valid types are: " + string.Join(", ", GetValidAnchorTypes()));
        }
        return anchor;
    }

    internal static bool IsValidAnchorType(string type)
    {
        return GetValidAnchorTypes().Contains(type);
    }

    private static List<string?> GetValidAnchorTypes()
    {
        // get all anchor type names from static class Anchors in FluentRegex
        return typeof(Anchors).GetFields().Select(f => f.Name).ToList()!;
    }

    internal static bool IsValidAnchor(string literal)
    {
        return GetValidAnchorLiterals().Contains(literal);
    }

    private static List<string?> GetValidAnchorLiterals()
    {
        return typeof(Anchors).GetFields().Select(f => f.GetValue(null)!.ToString()).ToList()!;
    }

    private static bool IsValidPatternType(string type)
    {
        return type == "Literal" || type == "Anchor" || type == "CharacterClass" || type == "Group";
    }

    /// <summary>
    /// Returns a string representation of the pattern (the <see cref="Value"/>) of the pattern.)
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return Value;
    }


    // public methods

    /// <summary>
    /// Deserializes a YAML string into a <see cref="AnchorPattern"/>
    /// </summary>
    /// <param name="patternObject"></param>
    /// <exception cref="ArgumentException"></exception>
    public void DeserializeYaml(string patternObject)
    {
        dynamic? pattern = null;
        var deserializer = new Deserializer();
        if (!string.IsNullOrWhiteSpace(patternObject))
        {
            pattern = deserializer.Deserialize<AnchorPattern>(patternObject);
            if (pattern == null)
            {
                try
                {
                    pattern = deserializer.Deserialize<object>(patternObject) as AnchorPattern;
                }
                catch
                {
                    throw new ArgumentException("Unable to deserialize YAML string:\n" + patternObject);
                }
            }

        }

        if (pattern != null)
        {
            Id = pattern.Id ?? Guid.NewGuid().ToString();
            Message = pattern.Message ?? string.Empty;
            Type = pattern.Type ?? "Literal";
            Properties = pattern.Properties ?? null;
            Value = pattern.Value ?? new PatternValue(string.Empty);
            Quantifiers = pattern.Quantifiers ?? null;
        }
    }

    /// <summary>
    /// Deserializes a JSON string into a <see cref="AnchorPattern"/>
    /// </summary>
    /// <param name="anchorObjectPattern"></param>
    /// <exception cref="Exception"></exception>
    public void DeserializeJson(string anchorObjectPattern)
    {
        var pattern = JsonSerializer.Deserialize<AnchorPattern>(anchorObjectPattern) ?? throw new Exception("Invalid JSON");

        Id = pattern.Id;
        Message = pattern.Message;
        Type = pattern.Type;
        Value = pattern.Value;
        Properties = pattern.Properties;
        Quantifiers = pattern.Quantifiers;
    }

    /// <summary>
    /// Serializes the <see cref="AnchorPattern"/> to a YAML string
    /// </summary>
    /// <returns><see cref="string"/></returns>
    public string SerializeYaml()
    {
        var serializer = new SerializerBuilder().Build();
        var yaml = serializer.Serialize(this);
        return yaml;
    }

    /// <summary>
    /// Serializes the <see cref="AnchorPattern"/> to a JSON string
    /// </summary>
    /// <returns><see cref="string"/></returns>
    public string SerializeJson()
    {
        var json = JsonSerializer.Serialize(this);
        return json;
    }

    /// <summary>
    /// Returns the pattern as a regular expression string.
    /// </summary>
    /// <returns><see cref="string"/></returns>
    public string ToRegex()
    {
        return Value.ToRegex();
    }
}