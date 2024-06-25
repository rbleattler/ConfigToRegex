using System.Text.RegularExpressions;

namespace RegexRules;

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
    return Regex.IsMatch(patternPropertiesObject, Validation.Patterns.Yaml);
  }


}