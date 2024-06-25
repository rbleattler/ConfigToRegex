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

}
