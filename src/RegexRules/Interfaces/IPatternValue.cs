using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace RegexRules;

public interface IPatternValue : ISerializable
{

  [JsonPropertyName("Value")]
  [YamlMember(Alias = "Value", Description = "The value of the pattern.")]
  public dynamic Value { get; set; }

}