using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using FluentRegex;
using System.Text.Json;
using NJsonSchema;
using System.Text.RegularExpressions;

namespace RegexRules;

public class AnchorPattern : IAnchor
{
    private string _type = "Anchor";

    string? IPattern.Id { get => Id; set => Id = value; }
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

    string IPattern.Type
    {
        get => Type;
        set => Type = value;
    }

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
            return Value;
        }
        set
        {
            var isValid = IsValidAnchorType(value);
            if (isValid == false)
            {
                var validTypes = GetValidAnchorTypes();
                throw new ArgumentException("Invalid Anchor Type. Valid types are: " + string.Join(", ", validTypes));
            }
            else
            {
                // Get the value of
                var stringValue = GetAnchor(value);
                new PatternValue(stringValue!);
            }
        }
    }

    [JsonPropertyName("Quantifiers")]
    [YamlMember(Alias = "Quantifiers")]
    public Quantifier? Quantifiers { get; set; }

    [JsonPropertyName("Message")]
    [YamlMember(Alias = "Message")]
    public string? Message { get; set; }

    [JsonPropertyName("Properties")]
    [YamlMember(Alias = "Properties")]
    public IPatternProperties? Properties { get; set; }

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
        Value = new PatternValue(string.Empty);
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
        }
    }

    private void DeserializeJson(string patternObject)
    {
        var pattern = JsonSerializer.Deserialize<IPattern>(patternObject);
        if (pattern != null)
        {
            Id = pattern.Id;
            Type = pattern.Type;
            Value = (PatternValue)pattern.Value;
            Quantifiers = (Quantifier?)pattern.Quantifiers;
        }
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
            throw new ArgumentException("Invalid Anchor Type. Valid types are: " + string.Join(", ", GetValidAnchorTypes()));
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
        // return typeof(Anchors).GetFields().Select(f => f.GetValue(null)).ToList();
        return typeof(Anchors).GetFields().Select(f => f.Name).ToList()!;
    }

    private static bool IsValidPatternType(string type)
    {
        return type == "Literal" || type == "Anchor" || type == "CharacterClass" || type == "Group";
    }
}