using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using NJsonSchema;

namespace ConfigToRegex;

/// <summary>
/// Represents a Regular Expression Group as an object.
/// </summary>
public interface IGroup : IPattern
{

  string? IPattern.Id
  {
    get => Properties?.Name ?? Guid.NewGuid().ToString();
    set => Properties!.Name = value;
  }

  IPatternValue IPattern.Value
  {
    get => new PatternValue(Patterns);
    set => Patterns = (List<Pattern>)value!;
  }

  public new Quantifier? Quantifiers { get; set; }

  public int Position { get; set; }

  public List<Pattern> Patterns { get; set; }

  [JsonIgnore]
  [YamlIgnore]
  JsonSchema IPattern.JsonSchema => JsonSchema.FromType(GetType());

}
