using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using NJsonSchema;

namespace RegexRules;

public interface IAnchor : IPattern, IRegexSerializable
{
    string? IPattern.Id
    {
        get => Properties?.Name ?? Guid.NewGuid().ToString();
        set => Properties!.Name = value;
    }

    IPatternValue IPattern.Value
    {
        get => new PatternValue(Value);
        set => Value = (PatternValue)value;
    }

    public new Quantifier? Quantifiers { get; set; }

    [JsonIgnore]
    [YamlIgnore]
    JsonSchema IPattern.JsonSchema => JsonSchema.FromType(GetType());


    private void DeserializeYaml(string anchorObjectPattern)
    {
        var deserializer = new Deserializer();
        var pattern = deserializer.Deserialize<AnchorPattern>(anchorObjectPattern) ?? throw new Exception("Invalid YAML");

        Id = pattern.Id;
        Message = pattern.Message;
        Type = pattern.Type;
        Value = pattern.Value;
        Properties = pattern.Properties;
        Quantifiers = pattern.Quantifiers;
    }

    private void DeserializeJson(string anchorObjectPattern)
    {
        var pattern = JsonSerializer.Deserialize<AnchorPattern>(anchorObjectPattern) ?? throw new Exception("Invalid JSON");

        Id = pattern.Id;
        Message = pattern.Message;
        Type = pattern.Type;
        Value = pattern.Value;
        Properties = pattern.Properties;
        Quantifiers = pattern.Quantifiers;
    }

}
