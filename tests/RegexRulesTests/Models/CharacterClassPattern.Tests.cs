using System.Runtime.CompilerServices;
using RegexRules;

namespace RegexRulesTests;

public class CharacterClassPatternTests : RegexRuleTestCore
{

    public string[] AllTestFiles => GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "characterClassPattern*.yml") ?? Array.Empty<string>();

    [Fact]
    public void AllCharacterClassPatterns_ConstructValidObjects_FromValidYaml()
    {
        for (var i = 0; i < AllTestFiles.Length; i++)
        {
            var characterClassPattern = new CharacterClassPattern(ReadFileAsString(AllTestFiles[i]));
            Console.WriteLine($"Test file: {AllTestFiles[i]}");
            Assert.NotNull(characterClassPattern);
        }
    }
}