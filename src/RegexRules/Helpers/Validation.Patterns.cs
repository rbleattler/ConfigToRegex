using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace RegexRules.Validation;

public class Patterns
{
    public const string Yaml = @"(?:-?[\s\w\d]*):{1}(?:[\s\w\d\[\]]*)";
}
