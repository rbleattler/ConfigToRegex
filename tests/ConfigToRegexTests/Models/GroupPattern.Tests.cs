using ConfigToRegex;

namespace ConfigToRegexTests;

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

        var groupPattern = new GroupPattern();


        var result = groupPattern.Patterns;


        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GroupPattern_ConstructorWithJsonPattern_DeserializesPattern()
    {

        var jsonPattern = ReadFileAsString(AllJsonTestFiles[0]); // Replace with a valid JSON pattern


        var groupPattern = new GroupPattern(jsonPattern);


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


        var groupPattern = new GroupPattern(yamlPattern);


        Assert.Equal(2, groupPattern.Patterns.Count);
        Assert.Equal("Testing Group Pattern", groupPattern.Message);
        Assert.Equal("AngleBrackets", groupPattern.Properties.NamedGroupStyle);
        Assert.Equal("testGroup0Name", groupPattern.Properties.Name);
        Assert.Equal("Named", groupPattern.Properties.GroupType);
    }

    // ToRegex:

    //TODO: EmptyGroupPattern
    [Fact(Skip = "TODO: Implement this test")]
    public void ToRegex_EmptyGroupPattern_ReturnsEmptyPattern()
    {
        var groupPattern = new GroupPattern();
        var expected = ""; // Assuming an empty group returns an empty string
        Assert.Equal(expected, groupPattern.ToRegex());
    }

    //TODO: SimplePatterns
    [Fact(Skip = "TODO: Implement this test")]
    public void ToRegex_SimplePatterns_ReturnsCorrectPattern()
    {
        var groupPattern = new GroupPattern();
        // groupPattern.Patterns.Add(new Pattern { Type = "CharacterClass", Value = "[a-z]" });
        var expected = "[a-z]";
        Assert.Equal(expected, groupPattern.ToRegex());
    }

    //TODO: WithQuantifiers
    [Fact(Skip = "TODO: Implement this test")]
    public void ToRegex_WithQuantifiers_ReturnsCorrectPattern()
    {
        var groupPattern = new GroupPattern
        {
            Quantifiers = new Quantifier { Min = 1, Max = 3 }
        };
        var expected = "[a-z]{1,3}";
        Assert.Equal(expected, groupPattern.ToRegex());
    }

    //TODO: NamedGroup
    [Fact(Skip = "TODO: Implement this test")]
    public void ToRegex_NamedGroup_ReturnsCorrectPattern()
    {
        var groupPattern = new GroupPattern();
        groupPattern.Properties.Name = "testGroup";
        groupPattern.Properties.GroupType = "Named";
        var expected = "(?<testGroup>[a-z])"; // Assuming the named group syntax is correct
        Assert.Equal(expected, groupPattern.ToRegex());
    }


    //TODO: Complex patternsrn()
    [Fact(Skip = "TODO: Implement this test")]
    public void ToRegex_ComplexPatterns_ReturnsCorrectPattern()
    {
        var groupPattern = new GroupPattern
        {
            Quantifiers = new Quantifier { Min = 1, Max = 3, Lazy = true }
        };
        var expected = "^[a-z]{1,3}?"; // Assuming this is the correct interpretation of the complex pattern
        Assert.Equal(expected, groupPattern.ToRegex());
    }

    [Fact]
    public void ToRegex_ShouldReturnCorrectGroupPattern_FromYaml()
    {
        var yamlPattern = ReadFileAsString(AllYamlTestFiles[0]);
        var groupPattern = new GroupPattern(yamlPattern);
        var expected = "(?<testGroup0Name>testGroup0)";
        Assert.Equal(expected, groupPattern.ToRegex());
    }
}