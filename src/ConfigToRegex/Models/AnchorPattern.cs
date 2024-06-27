using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using FluentRegex;
using System.Text.Json;
using NJsonSchema;

namespace ConfigToRegex;

public class AnchorPattern : IAnchor
{
    // fields

    private string _type = "Anchor";
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

    [JsonPropertyName("Id")]
    [YamlMember(Alias = "Id")]
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("Type")]
    [YamlMember(Alias = "Type")]
    public string Type
    {
        get => _type;
        set => _ = value;
    }

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

    [JsonPropertyName("Quantifiers")]
    [YamlMember(Alias = "Quantifiers")]
    public Quantifier? Quantifiers { get; set; }

    [JsonPropertyName("Message")]
    [YamlMember(Alias = "Message")]
    public string? Message { get; set; }


    [JsonIgnore]
    [YamlIgnore]
    public JsonSchema JsonSchema => JsonSchema.FromType(GetType());

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
    {
        // Value = new PatternValue(string.Empty);
    }

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

    public override string ToString()
    {
        return Value;
    }

    private bool IsJson(string patternObject) => ((IPattern)this).IsJson(patternObject);

    private bool IsYaml(string patternObject) => ((IPattern)this).IsYaml(patternObject);

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

    private static bool IsValidAnchorType(string type)
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
        return Value.ToRegex();
    }
}