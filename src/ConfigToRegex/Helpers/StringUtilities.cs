
using System.Text.RegularExpressions;

namespace ConfigToRegex.Helpers;

/// <summary>
/// Utilities for basic string manipulation and validation.
/// </summary>
public static partial class StringUtilities
{
    [GeneratedRegex(Patterns.Yaml)]
    private static partial Regex MyRegex();

    /// <summary>
    /// Checks if a string is a JSON object.
    /// </summary>
    /// <param name="stringObject"></param>
    /// <returns> <see cref="bool"/> : True if the string is a JSON object, false otherwise. </returns>
    public static bool IsJson(string stringObject)
    {
        return stringObject.StartsWith('{') && stringObject.EndsWith('}');
    }

    /// <summary>
    /// Checks if a string is a YAML object.
    /// </summary>
    /// <param name="stringObject"></param>
    /// <returns> <see cref="bool"/> : True if the string is a YAML object, false otherwise. </returns>
    public static bool IsYaml(string stringObject)
    {
        return MyRegex().IsMatch(stringObject);
    }


}