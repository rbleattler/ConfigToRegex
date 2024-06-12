using System.Runtime.CompilerServices;
using RegexRules;

namespace RegexRulesTests;

public class GroupPatternTests : RegexRuleTestCore
{

    public string[] AllTestFiles => GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "groupPattern*.yml") ?? Array.Empty<string>();

    [Fact]
    public void AllGroupPatterns_ConstructValidObjects_FromValidYaml()
    {
        for (var i = 0; i < AllTestFiles.Length; i++)
        {
            var groupPattern = new GroupPattern(ReadFileAsString(AllTestFiles[i]));
            Console.WriteLine($"Test file: {AllTestFiles[i]}");
            Assert.NotNull(groupPattern);
        }
    }
}