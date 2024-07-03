using ConfigToRegex;

namespace ConfigToRegexTests;
public class PatternValueTests : RegexRuleTestCore
{

  public static string[] AllTestFiles => GetAllTestFiles(directory: ExampleFilesDirectory, searchPattern: "patternValue*.yml") ?? [];

  [Fact]
  public void AllPatternValue_ConstructValidObjects_FromValidYaml()
  {
    for (var i = 0; i < AllTestFiles.Length; i++)
    {
      var PatternValue = new PatternValue(ReadFileAsString(AllTestFiles[i]));
      Assert.NotNull(PatternValue);
    }
  }

  [Fact]
  public void DefaultConstructor_SetsValueTo_Null()
  {
    var patternValue = new PatternValue();
    Assert.Equal(null, patternValue.Value);
  }

  [Fact]
  public void DynamicConstructor_SetsValueToPassedValue()
  {
    var value = "test";
    var patternValue = new PatternValue(value);
    Assert.NotNull(patternValue);
    Assert.Equal(value, patternValue.Value);
  }

  [Fact]
  public void StringConstructor_SetsValueToPassedValue()
  {
    var value = "test";
    var patternValue = new PatternValue(value);
    Assert.Equal(value, patternValue.Value);
  }
  [Fact]
  public void StringConstructor_SetsValueToPassedValue_FromYaml()
  {
    var value = "Value: test";
    var patternValue = new PatternValue(value);
    Assert.Equal("test", patternValue.Value);
  }

  [Fact]
  public void ToString_ReturnsStringValueOfValue()
  {
    var value = "test";
    var patternValue = new PatternValue(value);
    Assert.Equal(value, patternValue.ToString());
  }

  [Fact]
  public void ToRegex_WhenValueIsPatternValue_ReturnsExpectedResult()
  {

    var innerPatternValue = new PatternValue("inner value");
    var patternValue = new PatternValue(innerPatternValue);


    var result = patternValue.ToRegex();


    Assert.Equal("inner value", result);
  }

  [Fact]
  public void ToRegex_WhenValueIsString_ReturnsExpectedResult()
  {

    var patternValue = new PatternValue("test value");
    var result = patternValue.ToRegex();
    Assert.Equal("test value", result);
  }

  [Fact]
  public void ToRegex_WhenValueIsNull_ReturnsExpectedResult()
  {

    var patternValue = new PatternValue(string.Empty);


    var result = patternValue.ToRegex();


    Assert.Equal(string.Empty, result);
  }

  [Fact]
  public void SerializeYaml_WhenValueIsString_ReturnsExpectedResult()
  {
    var patternValue = new PatternValue("test value");
    var expectedYaml = "Value: test value";
    Assert.Equal(expectedYaml, patternValue.SerializeYaml().TrimEnd());
  }

  [Fact]
  public void SerializeYaml_WhenValueIsPatternValue_ReturnsExpectedResult()
  {
    var innerPatternValue = new PatternValue("inner value");
    var patternValue = new PatternValue(innerPatternValue);
    var expectedYaml = "Value: inner value";
    Assert.Equal(expectedYaml, patternValue.SerializeYaml().TrimEnd());
  }

  [Fact]
  public void SerializeJson_WhenValueIsString_ReturnsExpectedResult()
  {
    var patternValue = new PatternValue("test value");
    var expectedJson = "{\"Value\":\"test value\"}";
    Assert.Equal(expectedJson, patternValue.SerializeJson());
  }

  [Fact]
  public void SerializeJson_WhenValueIsPatternValue_ReturnsExpectedResult()
  {
    var innerPatternValue = new PatternValue("inner value");
    var patternValue = new PatternValue(innerPatternValue);
    var expectedJson = "{\"Value\":\"inner value\"}";
    Assert.Equal(expectedJson, patternValue.SerializeJson());
  }

  [Fact]
  public void DeserializeJson_WhenValueIsString_ReturnsExpectedResult()
  {
    var jsonString = "{\"Value\":\"test value\"}";
    var patternValue = new PatternValue();
    patternValue.DeserializeJson(jsonString);
    Assert.Equal("test value", patternValue.Value.ToString());
  }

  [Fact]
  public void DeserializeJson_WhenValueIsPatternValue_ReturnsExpectedResult()
  {
    var jsonString = "{\"Value\":\"inner value\"}";
    var patternValue = new PatternValue();
    patternValue.DeserializeJson(jsonString);
    Assert.Equal("inner value", patternValue.Value.ToString());
  }

  [Fact]
  public void DeserializeYaml_WhenValueIsString_ReturnsExpectedResult()
  {
    var yamlString = "Value: test value";
    var patternValue = new PatternValue();
    patternValue.DeserializeYaml(yamlString);
    Assert.Equal("test value", patternValue.Value);
  }

  [Fact]
  public void DeserializeYaml_WhenValueIsPatternValue_ReturnsExpectedResult()
  {
    var yamlString = "Value: inner value";
    var patternValue = new PatternValue();
    patternValue.DeserializeYaml(yamlString);
    Assert.Equal("inner value", patternValue.Value);
  }

}
