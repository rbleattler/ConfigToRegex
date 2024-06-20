using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace RegexRules;

public interface IPatternValue : IRegexSerializable
{

  public dynamic Value { get; set; }

}