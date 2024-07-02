using System.Text.Json;
using YamlDotNet.Serialization;

namespace ConfigToRegex;

public class PatternList : List<Pattern>
{
    public PatternList()
    { }

    public PatternList(IEnumerable<Pattern> patterns) : base(patterns)
    { }

    public PatternList(string patternListObject)
    {
        if (!string.IsNullOrWhiteSpace(patternListObject))
        {
            if (patternListObject.StartsWith('['))
            {
                var patternList = JsonSerializer.Deserialize<PatternList>(patternListObject);
                AddRange(patternList!);
            }
            else
            {
                var deserializer = new Deserializer();
                var patternList = deserializer.Deserialize<PatternList>(patternListObject);
                AddRange(patternList);
            }
        }
    }
}