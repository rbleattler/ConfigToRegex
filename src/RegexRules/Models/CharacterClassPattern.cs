using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using FluentRegex;
using System.Text.Json;
using NJsonSchema;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("RegexRulesTests")]
namespace RegexRules;

public class CharacterClassPattern : ICharacterClass
{
    private PatternValue _value = new(string.Empty);
    private string _type = "CharacterClass";

    string? IPattern.Id { get => Id; set => Id = value; }
    IPatternValue IPattern.Value
    {
        get => Value;
        set => Value = (PatternValue)value;
    }

    IQuantifier? IPattern.Quantifiers
    {
        get => Quantifiers;
        set => Quantifiers = (Quantifier?)value;
    }

    string IPattern.Type
    {
        get => Type;
        set => Type = value;
    }

    [JsonPropertyName("Id")]
    [YamlMember(Alias = "Id")]
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("Type")]
    [YamlMember(Alias = "Type")]
    public string Type
    {
        get => _type;
        set => _ = value;
    }

    [JsonPropertyName("Value")]
    [YamlMember(Alias = "Value")]
    public PatternValue Value
    {
        get
        {
            return _value;
        }
        set
        {
            var isValid = IsValidCharacterClassType(value) || IsValidCharacterClass(value);
            if (isValid == false)
            {
                throw new ArgumentException("Invalid CharacterClass.\n"
                                            + "Valid types are: "
                                            + string.Join(", ", GetValidCharacterClassTypes())
                                            + "\n"
                                            + "Valid CharacterClass literals are: "
                                            + string.Join(", ", GetValidCharacterClassLiterals()));
            }
            else
            {
                // Get the value of
                if (IsValidCharacterClass(value))
                {
                    _value = new PatternValue(value);
                }
                else
                {
                    var stringValue = GetCharacterClass(value);
                    _value = new PatternValue(stringValue!);
                }
            }
        }
    }

    [JsonPropertyName("Quantifiers")]
    [YamlMember(Alias = "Quantifiers")]
    public Quantifier? Quantifiers { get; set; }

    [JsonPropertyName("Message")]
    [YamlMember(Alias = "Message")]
    public string? Message { get; set; }

    [JsonPropertyName("Properties")]
    [YamlMember(Alias = "Properties")]
    public IPatternProperties? Properties { get; set; }

    [JsonIgnore]
    [YamlIgnore]
    public JsonSchema JsonSchema => JsonSchema.FromType(GetType());

    public CharacterClassPattern(string value, Quantifier? quantifiers = null)
    {
        Value = new PatternValue(value);
        Quantifiers = quantifiers;
    }

    public CharacterClassPattern(PatternValue value, Quantifier? quantifiers = null)
    {
        Value = value;
        Quantifiers = quantifiers;
    }

    public CharacterClassPattern()
    {
        Value = new PatternValue(string.Empty);
    }

    public CharacterClassPattern(string CharacterClassPatternObject)
    {
        if (CharacterClassPatternObject.StartsWith("\\"))
        {
            if (IsValidCharacterClass(CharacterClassPatternObject))
            {
                Value = new PatternValue(GetCharacterClass(CharacterClassPatternObject));
            }
            else
            {
                throw new ArgumentException("Invalid CharacterClass Literal (" + CharacterClassPatternObject + ") Valid literals are: " + string.Join(", ", GetValidCharacterClassLiterals()));

            }
        }
        else if (IsValidCharacterClassType(CharacterClassPatternObject))
        {
            Value = new PatternValue(CharacterClassPatternObject);
        }
        else if (string.IsNullOrWhiteSpace(CharacterClassPatternObject))
        {
            Value = new PatternValue(string.Empty);
        }
        else if (IsJson(CharacterClassPatternObject))
        {
            DeserializeJson(CharacterClassPatternObject);
        }
        else if (IsYaml(CharacterClassPatternObject))
        {
            DeserializeYaml(CharacterClassPatternObject);
        }
        else
        {
            throw new ArgumentException("Invalid CharacterClass Type (" + CharacterClassPatternObject + ") Valid types are: " + string.Join(", ", GetValidCharacterClassTypes()));
        }
    }

    public override string? ToString() => Value!.ToString() ?? string.Empty;

    internal bool IsJson(string patternObject) => ((IPattern)this).IsJson(patternObject);

    internal bool IsYaml(string patternObject) => ((IPattern)this).IsYaml(patternObject);

    internal void DeserializeYaml(string patternObject)
    {
        dynamic? pattern;
        if (patternObject != null)
        {
            try
            {
                pattern = new Deserializer().Deserialize(patternObject, typeof(CharacterClassPattern)); //Deserialize<CharacterClassPattern>(patternObject);
            }
            catch
            {
                try
                {
                    pattern = new Deserializer().Deserialize<object>(patternObject) as CharacterClassPattern;
                }
                catch
                {
                    throw new ArgumentException("Invalid Yaml");
                }
            }

            if (pattern != null)
            {
                Id = pattern.Id ?? Guid.NewGuid().ToString();
                Type = pattern!.Type ?? "Literal";
                Value = pattern!.Value ?? new PatternValue(string.Empty);
                Quantifiers = pattern!.Quantifiers ?? null;
            }
        }
        // var pattern = new Deserializer().Deserialize<CharacterClassPattern>(patternObject);
        // if (pattern != null)
        // {
        //     Id = pattern.Id ?? Guid.NewGuid().ToString();
        //     Type = pattern.Type ?? "Literal";
        //     Value = pattern.Value ?? new PatternValue(string.Empty);
        //     Quantifiers = pattern.Quantifiers ?? null;
        // }
    }

    internal void DeserializeJson(string patternObject)
    {
        dynamic? pattern;
        try
        {
            pattern = JsonSerializer.Deserialize<CharacterClassPattern>(patternObject);
        }
        catch
        {
            var basicObject = JsonSerializer.Deserialize<object>(patternObject);
            try
            {
                pattern = basicObject as CharacterClassPattern;
            }
            catch
            {
                throw new ArgumentException("Invalid Json");
            }
        }
        // This may be dumb, but it works
        if (pattern != null)
        {
            Id = pattern.Id;
            Type = pattern.Type;
            Value = pattern.Value;
            Quantifiers = pattern.Quantifiers;
        }
    }

    public static string GetCharacterClass(string value)
    {
        // get the value of the appropriate CharacterClass from the static class CharacterClasses in FluentRegex
        string CharacterClass = default!;
        var validTypes = GetValidCharacterClassTypes();
        if (validTypes.Contains(value))
        {
            CharacterClass = (string)typeof(CharacterClasses).GetField(value)!.GetValue(null)!;
        }
        if (string.IsNullOrWhiteSpace(CharacterClass))
        {
            var validValues = GetValidCharacterClassValues();
            if (validValues.Contains(value))
            {
                CharacterClass = value;
            }
        }
        if (string.IsNullOrWhiteSpace(CharacterClass))
        {
            throw new ArgumentException("Invalid CharacterClass Type. Valid types are: " + string.Join(", ", GetValidCharacterClassTypes()));
        }
        return CharacterClass ?? string.Empty;
    }

    private static List<string?> GetValidCharacterClassValues()
    {
        return typeof(CharacterClasses).GetFields().Select(f => f.GetValue(null)!.ToString()).ToList()!;
    }

    internal static bool IsValidCharacterClass(string literal)
    {
        return GetValidCharacterClassLiterals().Contains(literal);
    }

    private static List<string?> GetValidCharacterClassLiterals()
    {
        return typeof(CharacterClasses).GetFields().Select(f => f.GetValue(null))!.ToList<object>().ConvertAll<string?>(x => x?.ToString())!;
    }

    internal static bool IsValidCharacterClassType(string type)
    {
        return GetValidCharacterClassTypes().Contains(type);
    }

    internal static List<string?> GetValidCharacterClassTypes()
    {

        return typeof(CharacterClasses).GetFields().Select(f => f.Name).ToList()!;
    }

    internal static bool IsValidPatternType(string type)
    {
        return type == "Literal" || type == "CharacterClass" || type == "CharacterClass" || type == "Group";
    }

        public string SerializeYaml()
    {
        var serializer = new SerializerBuilder().Build();
        var yaml = serializer.Serialize(this);
        return yaml;
    }

    public string SerializeJson()
    {
        var json = JsonSerializer.Serialize(this);
        return json;
    }

    void IRegexSerializable.DeserializeYaml(string yamlString)
    {
        DeserializeYaml(yamlString);
    }

    void IRegexSerializable.DeserializeJson(string jsonString)
    {
        DeserializeJson(jsonString);
    }

    public string ToRegex()
    {
        var regex = Value.ToString();
        if (Quantifiers != null)
        {
            regex += Quantifiers.ToRegex();
        }
        return regex!;
    }
}