namespace ConfigToRegex.Validation;

public class Patterns
{
    public const string Yaml = @"(?:-?[\s\w\d]*):{1}(?:[\s\w\d\[\]]*)";
}
