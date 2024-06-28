using System.Text.Json.Serialization;
using YamlDotNet.Serialization;
using FluentRegex;
using System.Text.Json;
using NJsonSchema;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("ConfigToRegexTests")]
namespace ConfigToRegex;

//TODO: Custom CharacterClasses (I.E. [afv], [cC-rR], etc... basically building a custom character class), using the FluentRegex.CustomCharacterClassBuilder class


/// <summary>
/// Represents a Regular Expression Character Class as an object.
/// </summary>
public class CharacterClassPattern : ICharacterClass
{
    private PatternValue _value = new(string.Empty);
    private string _type = "CharacterClass";

    // Inherited Properties

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

    // Properties

    /// <summary>
    /// The unique identifier for the pattern
    /// </summary>
    [JsonPropertyName("Id")]
    [YamlMember(Alias = "Id")]
    public string? Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// The type of pattern
    /// </summary>
    [JsonPropertyName("Type")]
    [YamlMember(Alias = "Type")]
    public string Type
    {
        get => _type;
        set => _ = value;
    }

    /// <summary>
    /// The value of the pattern
    /// </summary>
    /// <value><see cref="PatternValue"/></value>
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


    /// <summary>
    /// The <see cref="Quantifier"/>s for the pattern.
    /// </summary>
    /// <value><see cref="Quantifier"/></value>
    [JsonPropertyName("Quantifiers")]
    [YamlMember(Alias = "Quantifiers")]
    public Quantifier? Quantifiers { get; set; }


    /// <summary>
    /// The message for the pattern. This is an optional field, used to provide additional information about the pattern.
    /// </summary>
    /// <value><see cref="string"/></value>
    [JsonPropertyName("Message")]
    [YamlMember(Alias = "Message")]
    public string? Message { get; set; }

    /// <summary>
    /// The <see cref="PatternProperties"/> for the pattern. This is not required except on <see cref="GroupPattern"/>s.
    /// </summary>
    [JsonPropertyName("Properties")]
    [YamlMember(Alias = "Properties")]
    public IPatternProperties? Properties { get; set; }

    /// <summary>
    /// The <see cref="JsonSchema"/> for the pattern. This is a generated schema based on the properties of the pattern.
    /// </summary>
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
        // Value = new PatternValue(string.Empty);
    }

    public CharacterClassPattern(string CharacterClassPatternObject)
    {
        if (CharacterClassPatternObject.StartsWith('\\'))
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

    void IRegexSerializable.DeserializeYaml(string yamlString)
    {
        DeserializeYaml(yamlString);
    }

    void IRegexSerializable.DeserializeJson(string jsonString)
    {
        DeserializeJson(jsonString);
    }

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
                pattern = null;
            }
            if (pattern == null)
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

    private static List<string?> GetValidCharacterClassValues()
    {
        return typeof(CharacterClasses).GetFields().Select(f => f.GetValue(null)!.ToString()).ToList()!;
    }

    internal static bool IsValidCharacterClass(string literal)
    {
        return GetValidCharacterClassLiterals().Contains(literal) || IsCustomCharacterClass(literal);
    }

    internal static bool IsCustomCharacterClass(string literal)
    {
        bool isValidRegex;
        try
        {
            isValidRegex = null != Regex.Match(literal, @"^\[.*\]$");
        }
        catch
        {
            isValidRegex = false;
        }
        return literal.StartsWith('[') && literal.EndsWith(']') && isValidRegex;
    }

    private static List<string?> GetValidCharacterClassLiterals()
    {
        return typeof(CharacterClasses).GetFields()
                                       .Select(f => f.GetValue(null))!
                                       .ToList<object>()
                                       .ConvertAll(x => x?.ToString())!;
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

    // public methods

    /// <summary>
    /// Returns a string representation of the pattern (the <see cref="Value"/>) of the pattern.)
    /// </summary>
    /// <returns></returns>
    public override string? ToString() => Value!.ToString() ?? string.Empty;

    /// <summary>
    /// Get's the value of the appropriate CharacterClass from the static class <see cref="CharacterClass" /> from the FluentRegex library.
    /// </summary>
    /// <param name="value"></param>
    /// <returns><see cref="string"/></returns>
    /// <exception cref="ArgumentException"></exception>
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
        return CharacterClass;
    }

    /// <summary>
    /// Serializes the <see cref="CharacterClassPattern"/> to a YAML string.
    /// </summary>
    /// <returns><see cref="string"/></returns>
    public string SerializeYaml()
    {
        var serializer = new SerializerBuilder().Build();
        var yaml = serializer.Serialize(this);
        return yaml;
    }

    /// <summary>
    /// Serializes the <see cref="CharacterClassPattern"/> to a JSON string.
    /// </summary>
    /// <returns><see cref="string"/></returns>
    public string SerializeJson()
    {
        var json = JsonSerializer.Serialize(this);
        return json;
    }

    /// <summary>
    /// Returns the pattern as a regular expression string.
    /// </summary>
    /// <returns><see cref="string"/></returns>
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