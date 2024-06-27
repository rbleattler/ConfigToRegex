using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace ConfigToRegex;

public interface IQuantifier : IRegexSerializable
{
  [JsonPropertyName("Min")]
  [YamlMember(Alias = "Min", Description = "The minimum number of times the pattern must match.")]
  public int? Min { get; set; }

  [JsonPropertyName("Max")]
  [YamlMember(Alias = "Max", Description = "The maximum number of times the pattern can match.")]
  public int? Max { get; set; }

  [JsonPropertyName("Exactly")]
  [YamlMember(Alias = "Exactly", Description = "The exact number of times the pattern must match.")]
  public int? Exactly { get; set; }

  [JsonPropertyName("Lazy")]
  [YamlMember(Alias = "Lazy", Description = "Whether the quantifier is lazy.")]
  public bool? Lazy { get; set; }

  [JsonPropertyName("Greedy")]
  [YamlMember(Alias = "Greedy", Description = "Whether the quantifier is greedy.")]
  public bool? Greedy { get; set; }

}
