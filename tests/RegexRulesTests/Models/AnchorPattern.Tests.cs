using System.Runtime.CompilerServices;
using RegexRules;

namespace RegexRulesTests;

public class AnchorPatternTests : RegexRuleTestCore
{

    public string[] AllTestFiles => GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "anchorPattern*.yml") ?? Array.Empty<string>();

    [Fact]
    public void AllAnchorPatterns_ConstructValidObjects_FromValidYaml()
    {
        for (var i = 0; i < AllTestFiles.Length; i++)
        {
            var anchorPattern = new AnchorPattern(ReadFileAsString(AllTestFiles[i]));
            Console.WriteLine($"Test file: {AllTestFiles[i]}");
            Assert.NotNull(anchorPattern);
        }
    }
}