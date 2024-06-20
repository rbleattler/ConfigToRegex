using System.Runtime.CompilerServices;
using RegexRules;

namespace RegexRulesTests;

public class GroupPatternTests : RegexRuleTestCore
{

    public string[] AllYamlTestFiles => GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "groupPattern*.yml") ?? Array.Empty<string>();
    public string[] AllJsonTestFiles => GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "groupPattern*.json") ?? Array.Empty<string>();

    [Fact]
    public void AllGroupPatterns_ConstructValidObjects_FromValidYaml()
    {
        for (var i = 0; i < AllYamlTestFiles.Length; i++)
        {
            var groupPattern = new GroupPattern(ReadFileAsString(AllYamlTestFiles[i]));
            Console.WriteLine($"Test file: {AllYamlTestFiles[i]}");
            Assert.NotNull(groupPattern);
        }
    }

    [Fact]
    public void GroupPattern_DefaultConstructor_CreatesEmptyPatternsList()
    {
        // Arrange
        var groupPattern = new GroupPattern();

        // Act
        var result = groupPattern.Patterns;

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GroupPattern_ConstructorWithJsonPattern_DeserializesPattern()
    {
        // Arrange
        var jsonPattern = ReadFileAsString(AllJsonTestFiles[0]); // Replace with a valid JSON pattern

        // Act
        var groupPattern = new GroupPattern(jsonPattern);

        // Assert
        // Replace 'expected' with the expected values

        Assert.Equal("Testing Group Pattern", groupPattern.Message);
        Assert.Single(groupPattern.Patterns);
        Assert.Equal("AngleBrackets", groupPattern.Properties.NamedGroupStyle);
        Assert.Equal("testGroup0Name", groupPattern.Properties.Name);
        Assert.Equal("Named", groupPattern.Properties.GroupType);
    }

    [Fact]
    public void GroupPattern_ConstructorWithYamlPattern_DeserializesPattern()
    {
        // Get the test file 2
        var testFile = AllYamlTestFiles!.Where(f => f.EndsWith("2.yml")).FirstOrDefault();
        var yamlPattern = ReadFileAsString(testFile!);

        // Act
        var groupPattern = new GroupPattern(yamlPattern);

        // Assert
        Assert.Equal(2, groupPattern.Patterns.Count);
        Assert.Equal("Testing Group Pattern", groupPattern.Message);
        Assert.Equal("AngleBrackets", groupPattern.Properties.NamedGroupStyle);
        Assert.Equal("testGroup0Name", groupPattern.Properties.Name);
        Assert.Equal("Named", groupPattern.Properties.GroupType);
    }
}