namespace RegexRules;

public interface IPatternValue : IRegexSerializable
{

  public dynamic Value { get; set; }

}