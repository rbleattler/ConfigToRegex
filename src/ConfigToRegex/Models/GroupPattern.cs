using FluentRegex;
using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using NJsonSchema;
using ConfigToRegex.Exceptions;

namespace ConfigToRegex;

/// <summary>
/// Represents a Regular Expression Group as an object.
/// </summary>
[YamlSerializable]
public class GroupPattern : IGroup
{
  private readonly string _type = "Group";

  string? IPattern.Id
  {
    get => Properties.Name;
    set => Properties.Name = value!;
  }
  IPatternValue IPattern.Value
  {
    // This is a hack to get around the fact that the Value property is not implemented in GroupPattern but is required by the interface.
    // TODO: Implement Value property in GroupPattern, or remove the Value property from the interface.
    get => (IPatternValue)Patterns;
    set => Patterns = (PatternList)value;
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

  /// <summary>
  /// The unique identifier for the pattern
  /// </summary>
  [JsonPropertyName("Id")]
  [YamlMember(Alias = "Id")]
  public string? Id { get; set; } = Guid.NewGuid().ToString();

  /// <summary>
  /// The type of pattern
  /// </summary>
  [JsonPropertyName("Type")]
  [YamlMember(Alias = "Type")]
  public string Type { get; set; } = "Group";

  /// <summary>
  /// The <see cref="Quantifier"/>s for the pattern
  /// </summary>
  [JsonPropertyName("Quantifiers")]
  [YamlMember(Alias = "Quantifiers")]
  public Quantifier? Quantifiers { get; set; }

  //TODO: Position is not currently implemented for Group construction.
  /// <summary>
  /// The position of the pattern in the pattern group (not currently implemented)
  /// </summary>
  [JsonPropertyName("Position")]
  [YamlMember(Alias = "Position")]
  public int Position { get; set; }

  /// <summary>
  /// The patterns that make up the group
  /// </summary>
  /// <value><see cref="List{Pattern}"/></value>
  [JsonPropertyName("Patterns")]
  [YamlMember(Alias = "Patterns")]
  public PatternList Patterns { get; set; } = [];

  /// <summary>
  /// The message for the pattern. This is an optional property, used to provide additional information about the pattern.
  /// </summary>
  [JsonPropertyName("Message")]
  [YamlMember(Alias = "Message")]
  public string? Message { get; set; }

  /// <summary>
  /// The <see cref="PatternProperties"/> for the pattern.
  /// </summary>
  [JsonPropertyName("Properties")]
  [YamlMember(Alias = "Properties")]
  public PatternProperties Properties { get; set; } = new PatternProperties();


  /// <summary>
  /// The <see cref="JsonSchema"/> for the pattern. This is a generated property and is not intended to be set.
  /// </summary>
  [JsonIgnore]
  [YamlIgnore]
  JsonSchema IPattern.JsonSchema => JsonSchema.FromType(GetType());

  public GroupPattern()
  {
    Patterns = [];
  }

  public GroupPattern(string groupObjectPattern)
  {
    if (string.IsNullOrWhiteSpace(groupObjectPattern))
    {
      Position = 0;
      Patterns = [];
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


  void IRegexSerializable.DeserializeYaml(string yamlString)
  {
    DeserializeYaml(yamlString);
  }

  void IRegexSerializable.DeserializeJson(string jsonString)
  {
    DeserializeJson(jsonString);
  }


  private bool IsJson(string patternObject) => ((IPattern)this).IsJson(patternObject);

  private bool IsYaml(string patternObject) => ((IPattern)this).IsYaml(patternObject);

  private void DeserializeYaml(string groupObjectPattern)
  {
    var deserializer = new Deserializer();
    var pattern = deserializer.Deserialize<GroupPattern>(groupObjectPattern) ?? throw new InvalidYamlException("Invalid YAML");

    Id = pattern.Id;
    Message = pattern.Message;
    Position = pattern.Position;
    Patterns = pattern.Patterns;
    Properties = pattern.Properties;
    Quantifiers = pattern.Quantifiers;
  }

  private void DeserializeJson(string groupObjectPattern)
  {
    var pattern = JsonSerializer.Deserialize<GroupPattern>(groupObjectPattern) ?? throw new InvalidJsonException("Invalid JSON");

    Id = pattern.Id;
    Message = pattern.Message;
    Position = pattern.Position;
    Patterns = pattern.Patterns;
    Properties = pattern.Properties;
    Quantifiers = pattern.Quantifiers;

  }

  private static void ProcessPattern(GroupBuilder regex, Pattern pattern)
  {
    switch (pattern.Type)
    {
      case "Literal":
        regex.AppendLiteral(pattern.ToRegex());
        break;
      case "Anchor":
        var isAnchorType = AnchorPattern.IsValidAnchorType(pattern.Value.ToRegex());
        var isValidAnchor = AnchorPattern.IsValidAnchor(pattern.Value.ToRegex());
        if (isAnchorType)
        {
          regex.StartAnchor()
               .InvokeMethod(pattern.Value)
               .Build();
        }
        else if (isValidAnchor)
        {
          regex.AppendLiteral(pattern.Value);
        }
        break;
      case "CharacterClass":
        var isCharacterClass = CharacterClassPattern.IsValidCharacterClass(pattern.Value.ToRegex()) || CharacterClassPattern.IsCustomCharacterClass(pattern.Value.ToRegex());
        var isCharacterClassType = CharacterClassPattern.IsValidCharacterClassType
        (pattern.Value.ToRegex());

        if (isCharacterClass)
        {
          var literalRegex = pattern.Value.ToRegex();
          regex.AppendLiteral(literalRegex);
        }
        else if (isCharacterClassType)
        {
          regex.StartCharacterClass()
               .InvokeMethod(pattern.Value)
               .Build();
        }
        break;
      case "Group":
        regex.AppendLiteral(pattern.ToRegex());
        break;
      default:
        try
        {
          regex.AppendLiteral(pattern.ToRegex());
        }
        catch
        {
          throw new ArgumentException("Invalid Pattern Type. (" + pattern.Type + ") Valid types are: Literal, Anchor, CharacterClass, Group");
        }
        break;
    }
  }

  /// <summary>
  /// Serializes the <see cref="GroupPattern"/> to a YAML string
  /// </summary>
  /// <returns><see cref="string"/></returns>
  public string SerializeYaml()
  {
    var serializer = new SerializerBuilder().Build();
    var yaml = serializer.Serialize(this);
    return yaml;
  }

  /// <summary>
  /// Serializes the <see cref="GroupPattern"/> to a JSON string
  /// </summary>
  /// <returns><see cref="string"/></returns>
  public string SerializeJson()
  {
    var json = JsonSerializer.Serialize(this);
    return json;
  }

  /// <summary>
  /// Returns the pattern as a regular expression string.
  /// </summary>
  /// <returns><see cref="string"/></returns>
  public string ToRegex()
  {
    // Use FluentRegex to build the regex pattern
    GroupBuilder regex;
    var groupType = (GroupType)Enum.Parse(typeof(GroupType), Properties.GroupType!);

    if (Properties.GroupType != "NamedCapturing")
    {
      // Get FluentRegex.GroupType from Properties.GroupType
      regex = new GroupBuilder(new PatternBuilder(), groupType);
    }
    else
    {
      //TODO: Coverage for other group styles
      regex = new GroupBuilder(new PatternBuilder(), NamedGroupStyle.AngleBrackets, Properties.Name!);
    }
    foreach (var pattern in Patterns)
    {
      ProcessPattern(regex, pattern);
    }
    var outRegex = regex.Build();

    if (Quantifiers != null)
    {
      if (null != Quantifiers.Min && null != Quantifiers.Max)
      {
        outRegex.Times(Quantifiers.Min!.Value, Quantifiers.Max!.Value);
      }
      else if (null != Quantifiers.Min)
      {
        outRegex.Times(Quantifiers.Min!.Value);
      }
      else if (null != Quantifiers.Max)
      {
        outRegex.Times(0, Quantifiers.Max!.Value);
      }

      if (Quantifiers.Lazy == true)
      {
        outRegex.Lazy();
      }

      if (Quantifiers.CanBeGreedy(ToString()!))
      {
        outRegex.AppendLiteral("*");
      }

    }
    return outRegex.Build().ToString();
  }


}