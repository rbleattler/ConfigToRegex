using System.Runtime.CompilerServices;
using RegexRules;

namespace RegexRulesTests;

public class PatternPropertiesTests : RegexRuleTestCore
{

    public string[] AllTestFiles => GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "patternProperties*.yml") ?? Array.Empty<string>();

    [Fact]
    public void AllPatternProperties_ConstructValidObjects_FromValidYaml()
    {
        for (var i = 0; i < AllTestFiles.Length; i++)
        {
            var PatternProperties = new PatternProperties(ReadFileAsString(AllTestFiles[i]));
            // Console.WriteLine($"Test file: {AllTestFiles[i]}");
            Assert.NotNull(PatternProperties);
        }
    }
}