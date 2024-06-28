namespace ConfigToRegex;

/// <summary>
/// Represents a Regular Expression Pattern Value as an object.
/// </summary>
public interface IPatternValue : IRegexSerializable
{

  public dynamic Value { get; set; }

}