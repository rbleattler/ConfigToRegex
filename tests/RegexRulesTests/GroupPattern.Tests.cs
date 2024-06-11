using RegexRules;

namespace RegexRulesTests;

public class GroupPatternTests
{
    string _groupPatternFile = "..\\..\\..\\..\\..\\examples\\groupPattern.yml";
    string _groupPatternFileContents = string.Empty;

    [Fact]
    public void CanConstructNewGroupPattern_FromGroupPatternFile_WithValidYaml()
    {
        _groupPatternFileContents = File.ReadAllText(_groupPatternFile);
        var groupPattern = new GroupPattern(_groupPatternFileContents);
        Assert.NotNull(groupPattern);
    }
}