using System.Runtime.CompilerServices;
using RegexRules;

namespace RegexRulesTests;

public class PatternTests : RegexRuleTestCore
{

    public string[] AllTestFiles => GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "basicPattern*.yml") ?? Array.Empty<string>();

    [Fact]
    public void AllPatterns_ConstructValidObjects_FromValidYaml()
    {
        for (var i = 0; i < AllTestFiles.Length; i++)
        {
            var Pattern = new Pattern(ReadFileAsString(AllTestFiles[i]));
            Console.WriteLine($"Test file: {AllTestFiles[i]}");
            Assert.NotNull(Pattern);
        }
    }
}