using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using NJsonSchema;

namespace RegexRules;

public interface IGroup : IPattern, IRegexSerializable
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

  public new IQuantifier? Quantifiers { get; set; }

  public int Position { get; set; }

  public List<Pattern> Patterns { get; set; }

  [JsonIgnore]
  [YamlIgnore]
  JsonSchema IPattern.JsonSchema => JsonSchema.FromType(GetType());

  private void DeserializeYaml(string groupObjectPattern)
  {
    var deserializer = new Deserializer();
    var pattern = deserializer.Deserialize<GroupPattern>(groupObjectPattern) ?? throw new Exception("Invalid YAML");

    Id = pattern.Id;
    Message = pattern.Message;
    Position = pattern.Position;
    Patterns = pattern.Patterns;
    Properties = pattern.Properties;
    Quantifiers = pattern.Quantifiers;
  }

  private void DeserializeJson(string groupObjectPattern)
  {
    var pattern = JsonSerializer.Deserialize<GroupPattern>(groupObjectPattern) ?? throw new Exception("Invalid JSON");

    Id = pattern.Id;
    Message = pattern.Message;
    Position = pattern.Position;
    Patterns = pattern.Patterns;
    Properties = pattern.Properties;
    Quantifiers = pattern.Quantifiers;

  }

}
