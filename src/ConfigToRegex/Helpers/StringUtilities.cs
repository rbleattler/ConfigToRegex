
using System.Text.RegularExpressions;

namespace ConfigToRegex.Helpers;

public static partial class StringUtilities
{
    public static bool IsJson(string stringObject)
    {
        return stringObject.StartsWith('{') && stringObject.EndsWith('}');
    }

    public static bool IsYaml(string stringObject)
    {
        return MyRegex().IsMatch(stringObject);
    }

    [GeneratedRegex(Patterns.Yaml)]
    private static partial Regex MyRegex();
}