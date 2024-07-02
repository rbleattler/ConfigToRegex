using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace ConfigToRegex;

/// <summary>
/// Represents the properties of a Regular Expression Pattern. (This is only used for <see cref="GroupPattern"/> objects.)
/// </summary>
public interface IPatternProperties
{

  public string? Name { get; set; }

  public string? GroupType { get; set; }

  public string? NamedGroupStyle { get; set; }

  public bool IsJson(string patternPropertiesObject)
  {
    return patternPropertiesObject.StartsWith('{');
  }

  public bool IsYaml(string patternPropertiesObject)
  {
    return Regex.IsMatch(patternPropertiesObject, Helpers.Patterns.Yaml);
  }


}