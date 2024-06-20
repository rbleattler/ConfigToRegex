using System.Runtime.CompilerServices;
using RegexRules;

namespace RegexRulesTests;

public class PatternValueTests : RegexRuleTestCore
{

    public string[] AllTestFiles => GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "patternValue*.yml") ?? Array.Empty<string>();

    [Fact]
    public void AllPatternValue_ConstructValidObjects_FromValidYaml()
    {
        for (var i = 0; i < AllTestFiles.Length; i++)
        {
            var PatternValue = new PatternValue(ReadFileAsString(AllTestFiles[i]));
            // Console.WriteLine($"Test file: {AllTestFiles[i]}");
            Assert.NotNull(PatternValue);
        }
    }
}