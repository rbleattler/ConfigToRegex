using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using NJsonSchema;

namespace ConfigToRegex;

/// <summary>
/// Represents a Regular Expression Character Class as an object.
/// </summary>
public interface ICharacterClass : IPattern
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
