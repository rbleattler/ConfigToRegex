using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using NJsonSchema;

namespace RegexRules;

[YamlSerializable]
public class GroupPattern : IGroup
{
  private string _type = "Group";

  string? IPattern.Id
  {
    get => Properties.Name;
    set => Properties.Name = value!;
  }
  IPatternValue IPattern.Value
  {
    get => (IPatternValue)Patterns;
    set => Patterns = (List<Pattern>)value;
  }
  IQuantifier? IPattern.Quantifiers
  {
    get => Quantifiers;
    set => Quantifiers = (Quantifier?)value;
  }
  string IPattern.Type
  {
    get => _type;
    set => _ = value;
  }

  IPatternProperties? IPattern.Properties
  {
    get => Properties;
    set => Properties = (PatternProperties)value!;
  }

  [JsonPropertyName("Id")]
  [YamlMember(Alias = "Id")]
  public string? Id { get; set; } = Guid.NewGuid().ToString();

  [JsonPropertyName("Type")]
  [YamlMember(Alias = "Type")]
  public string Type { get; set; } = "Group";

  [JsonPropertyName("Quantifiers")]
  [YamlMember(Alias = "Quantifiers")]
  public IQuantifier? Quantifiers { get; set; }

  [JsonPropertyName("Position")]
  [YamlMember(Alias = "Position")]
  public int Position { get; set; }

  [JsonPropertyName("Patterns")]
  [YamlMember(Alias = "Patterns")]
  public List<Pattern> Patterns { get; set; } = new List<Pattern>();

  [JsonPropertyName("Message")]
  [YamlMember(Alias = "Message")]
  public string? Message { get; set; }

  [JsonPropertyName("Properties")]
  [YamlMember(Alias = "Properties")]
  public PatternProperties Properties { get; set; } = new PatternProperties();


  [JsonIgnore]
  [YamlIgnore]
  JsonSchema IPattern.JsonSchema => JsonSchema.FromType(GetType());

  public GroupPattern()
  {
    Patterns = new List<Pattern>();
  }

  public GroupPattern(string groupObjectPattern)
  {
    if (string.IsNullOrWhiteSpace(groupObjectPattern))
    {
      Position = 0;
      Patterns = new List<Pattern>();
      Properties = new PatternProperties();
      Quantifiers = new Quantifier();
      return;
    }
    if (IsJson(groupObjectPattern))
    {
      DeserializeJson(groupObjectPattern);
    }
    if (IsYaml(groupObjectPattern))
    {
      DeserializeYaml(groupObjectPattern);
    }

  }

  private bool IsJson(string patternObject) => ((IPattern)this).IsJson(patternObject);

  private bool IsYaml(string patternObject) => ((IPattern)this).IsYaml(patternObject);

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

    public string SerializeYaml()
    {
        var serializer = new SerializerBuilder().Build();
        var yaml = serializer.Serialize(this);
        return yaml;
    }

    public string SerializeJson()
    {
        var json = JsonSerializer.Serialize(this);
        return json;
    }

  void IRegexSerializable.DeserializeYaml(string yamlString)
  {
    DeserializeYaml(yamlString);
  }

  void IRegexSerializable.DeserializeJson(string jsonString)
  {
    DeserializeJson(jsonString);
  }

  public string ToRegex()
  {
    //TODO: Implement GroupPattern.ToRegex()
    throw new NotImplementedException();
  }
}