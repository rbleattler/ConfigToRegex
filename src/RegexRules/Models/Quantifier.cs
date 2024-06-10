using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using NJsonSchema;
using YamlDotNet.Serialization;


namespace RegexRules;

[YamlSerializable]
public class Quantifier : IQuantifier
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

  [JsonIgnore]
  [YamlIgnore]
  public JsonSchema JsonSchema => JsonSchema.FromType(GetType());

  public Quantifier()
  {
  }

  public Quantifier(int? min, int? max, int? exactly, bool? lazy, bool? greedy)
  {
    Min = min;
    Max = max;
    Exactly = exactly;
    Lazy = lazy;
    Greedy = greedy;
  }

  public Quantifier(string quantifierObject)
  {
    // deserialize the string using json or yaml

    if (!string.IsNullOrEmpty(quantifierObject) && quantifierObject.StartsWith("{") && quantifierObject.EndsWith("}"))
    {
      var deserializedValue = JsonSerializer.Deserialize<Quantifier>(quantifierObject);
      Min = deserializedValue!.Min;
      Max = deserializedValue!.Max;
      Exactly = deserializedValue!.Exactly;
      Lazy = deserializedValue!.Lazy;
      Greedy = deserializedValue!.Greedy;
    }
    else
    {
      // if this is yaml
      var deserializer = new Deserializer();
      var deserializedValue = deserializer.Deserialize<Quantifier>(quantifierObject);
      Min = deserializedValue!.Min;
      Max = deserializedValue!.Max;
      Exactly = deserializedValue!.Exactly;
      Lazy = deserializedValue!.Lazy;
      Greedy = deserializedValue!.Greedy;
    }
  }

  public void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    // Serialize the object
    info.AddValue("Min", Min);
    info.AddValue("Max", Max);
    info.AddValue("Exactly", Exactly);
    info.AddValue("Lazy", Lazy);
    info.AddValue("Greedy", Greedy);
  }

}
