using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace RegexRules;

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

  public string ToRegex(string pattern)
  {
    // This takes the pattern as a parameter so that we can avoid invalid regex (I.E. **)
    StringBuilder sb = new();

    if (Exactly.HasValue && Exactly > 0)
    {
      sb.Append($"{{{Exactly}}}");
    }
    else if (Min! == 0 && !Max.HasValue)
    {
      sb.Append('*');
    }
    else if (Min == 1 && !Max.HasValue)
    {
      sb.Append('+');
    }
    else if (Min == 0 && Max! == 1)
    {
      sb.Append('?');
    }
    // if min has value, and the value > 0, and max has value, and the value is >= 0 , and Max >= Min
    else if (Max < Min)
    {
      throw new SerializationException("Max must be greater than or equal to Min.");
    }
    else if (Min.HasValue && Max.HasValue && Max >= Min || Min.HasValue || Max.HasValue)
    {
      sb.Append('{');
      if (Min.HasValue)
      {
        sb.Append(Min);
      }
      sb.Append(',');
      if (Max.HasValue)
      {
        sb.Append(Max);
      }
      sb.Append('}');
    }
    if (Greedy.HasValue && Greedy == true && !pattern.EndsWith("*") && !pattern.EndsWith("+") && !pattern.EndsWith("?") && !Exactly.HasValue && !Min.HasValue && !Max.HasValue)
    {
      sb.Append('*');
    }

    if (Lazy.HasValue && Lazy == true && !pattern.EndsWith("?"))
    {
      // If this is appended when there are min/max declared, it can still be valid.
      // Example:
      // | RegexRules |	Description |
      // | ---      |   ---         |
      // | A{2,9}? |	Two to nine As, as few as needed to allow the overall pattern to match (lazy) |
      sb.Append('?');
    }

    return sb.ToString();
  }

}
