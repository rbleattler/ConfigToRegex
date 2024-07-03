using System.Text.Json;
using ConfigToRegex.Exceptions;
using YamlDotNet.Core;

namespace ConfigToRegex.Helpers;

//TODO: Convert all tests to use FluentAssertions

/// <summary>
/// Converts configurations (JSON, YAML) to regular expressions.
/// </summary>
/// <remarks>
/// This class is responsible for converting configurations to regular expressions.
/// It accepts configurations in JSON or YAML format.
/// It can take string or file input.
/// </remarks>
public class ConfigSerializer
{

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigSerializer"/> class.
    /// </summary>
    public ConfigSerializer()
    {

    }

    /// <summary>
    /// Deserializes a YAML string to an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="yamlString"></param>
    /// <returns> An object of type <typeparamref name="T"/>. </returns>
    /// <exception cref="InvalidYamlException"></exception>
    public static T DeserializeYaml<T>(string yamlString) where T : IRegexSerializable
    {
        try
        {
            // new object of type T
            T obj = Activator.CreateInstance<T>();
            obj.DeserializeYaml(yamlString);
            return obj;
        }
        catch (YamlException)
        {
            throw new InvalidYamlException("Invalid YAML");
        }
    }

    /// <summary>
    /// Deserializes a JSON string to an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonString"></param>
    /// <returns> An object of type <typeparamref name="T"/>. </returns>
    /// <exception cref="InvalidJsonException"></exception>
    public static T DeserializeJson<T>(string jsonString) where T : IRegexSerializable
    {
        try
        {
            // Check if T is exactly IRegexSerializable
            return DeserializeOrDefault<T>(jsonString);
        }
        catch (Exception ex)
        {
            throw new InvalidJsonException("Invalid JSON : " + ex.Message, ex);
        }
    }

    private static T DeserializeOrDefault<T>(string inputString) where T : IRegexSerializable
    {
        var isJson = StringUtilities.IsJson(inputString);
        var isYaml = StringUtilities.IsYaml(inputString);
        Type defaultType = typeof(Pattern);
        dynamic? obj;
        if (typeof(T) == typeof(IRegexSerializable))
        {
            obj = Activator.CreateInstance(defaultType);
        }
        else
        {
            // T is a concrete type that implements IRegexSerializable
            obj = Activator.CreateInstance<T>();
        }
        if (isJson)
        {
            obj?.DeserializeJson(inputString);
        }
        else if (isYaml)
        {
            obj?.DeserializeYaml(inputString);
        }
        return obj;
    }


    /// <summary>
    /// Serializes an object of type <typeparamref name="T"/> to a YAML string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="patternObject"></param>
    /// <returns> A YAML representation of the object passed to <paramref name="patternObject"/>. </returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string SerializeYaml<T>(T? patternObject) where T : IRegexSerializable
    {
        if (patternObject is null)
        {
            throw new ArgumentNullException(nameof(patternObject));
        }
        return patternObject.SerializeYaml();
    }

    /// <summary>
    /// Serializes an object of type <typeparamref name="T"/> to a JSON string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="patternObject"></param>
    /// <returns> A JSON representation of the object passed to <paramref name="patternObject"/>. </returns>
    public static string SerializeJson<T>(T? patternObject) where T : IRegexSerializable
    {
        if (patternObject is null)
        {
            return string.Empty;
        }
        return JsonSerializer.Serialize(patternObject);
    }

    /// <summary>
    /// Converts a YAML string to a JSON representation of the object defined in the string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="yamlString"></param>
    /// <returns> A JSON representation of the YAML string passed to <paramref name="yamlString"/>. </returns>
    public static string ConvertYamlToJson<T>(string yamlString) where T : IRegexSerializable
    {
        var patternObject = DeserializeYaml<T>(yamlString);
        return JsonSerializer.Serialize(patternObject);
    }

    /// <summary>
    /// Converts a YAML string to a regular expression based on the object defined in the string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="yamlString"></param>
    /// <returns> A regular expression string representation of the YAML string passed to <paramref name="yamlString"/>. </returns>
    public static string ConvertYamlToRegex<T>(string yamlString) where T : IRegexSerializable
    {
        var patternObject = DeserializeYaml<T>(yamlString);
        return patternObject.ToRegex();
    }

    /// <summary>
    /// Converts a JSON string to a regular expression based on the object defined in the string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonString"></param>
    /// <returns> A regular expression string representation of the JSON string passed to <paramref name="jsonString"/>. </returns>
    public static string ConvertJsonToRegex<T>(string jsonString) where T : IRegexSerializable
    {
        var patternObject = DeserializeJson<T>(jsonString);
        return patternObject.ToRegex();
    }

    /// <summary>
    /// Converts a JSON string to a YAML representation of the object defined in the string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonString"></param>
    /// <returns> A YAML representation of the JSON string passed to <paramref name="jsonString"/>. </returns>
    public static string ConvertJsonToYaml<T>(string jsonString) where T : IRegexSerializable
    {
        var patternObject = DeserializeJson<T>(jsonString);
        return SerializeYaml(patternObject);
    }

    /// <summary>
    /// Deserializes a YAML file to an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <returns> An object of type <typeparamref name="T"/>. </returns>
    public static T DeserializeYamlFromFile<T>(string filePath) where T : IRegexSerializable
    {
        var yamlString = ReadConfigFile(filePath);
        return DeserializeYaml<T>(yamlString);
    }

    /// <summary>
    /// Deserializes a JSON file to an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <returns> An object of type <typeparamref name="T"/>. </returns>
    public static T DeserializeJsonFromFile<T>(string filePath) where T : IRegexSerializable
    {
        var jsonString = ReadConfigFile(filePath);
        return DeserializeJson<T>(jsonString);
    }

    // /// <summary>
    // /// Converts a YAML file to a JSON representation of the object defined in the file.
    // /// </summary>
    // /// <typeparam name="T"></typeparam>
    // /// <param name="filePath"></param>
    // /// <returns> A JSON representation of the YAML file passed to <paramref name="filePath"/>. </returns>
    // public static string ConvertYamlFileToJson<T>(string filePath) where T : IRegexSerializable
    // {
    //     var yamlString = ReadConfigFile(filePath);
    //     return ConvertYamlToJson<T>(yamlString);
    // }

    // /// <summary>
    // /// Converts a JSON file to a YAML representation of the object defined in the file.
    // /// </summary>
    // /// <typeparam name="T"></typeparam>
    // /// <param name="filePath"></param>
    // /// <returns> A YAML representation of the JSON file passed to <paramref name="filePath"/>. </returns>
    // public static string ConvertJsonFileToYaml<T>(string filePath) where T : IRegexSerializable
    // {
    //     var jsonString = ReadConfigFile(filePath);
    //     return ConvertJsonToYaml<T>(jsonString);
    // }

    /// <summary>
    /// Converts a config file to a regular expression based on the object defined in the file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <returns> A regular expression string representation of the config file passed to <paramref name="filePath"/>. </returns>
    public static string ParseConfigFileToRegex<T>(string filePath) where T : IRegexSerializable
    {
        var configString = ReadConfigFile(filePath);
        if (StringUtilities.IsJson(configString))
            return ConvertJsonToRegex<T>(configString);
        else if (StringUtilities.IsYaml(configString))
            return ConvertYamlToRegex<T>(configString);
        else
            throw new InvalidConfigException("Invalid configuration file. Valid JSON or YAML is required.");
    }

    /// <summary>
    /// Tries to parse a configuration file to an object of type <see cref="IRegexSerializable"/>.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="result"></param>
    /// <returns> A boolean indicating whether the parsing was successful. </returns>
    public static bool TryParseFile(out IRegexSerializable? result, string filePath)
    {
        result = null;
        string fileContents = ReadConfigFile(filePath);
        try
        {
            return TryParse(ref result, fileContents);
        }
        catch
        {
            return false; // Deserialization failed
        }
    }

    /// <summary>
    /// Tries to parse a configuration string to an object of type <see cref="IRegexSerializable"/>.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="fileContents"></param>
    /// <returns> A boolean indicating whether the parsing was successful. </returns>
    public static bool TryParse(ref IRegexSerializable? result, string fileContents)
    {
        try
        {
            result = DeserializeOrDefault<IRegexSerializable>(fileContents);
            return true;
        }
        catch
        {
            return false; // Invalid format
        }

    }

    /// <summary>
    /// Converts a configuration file to a regular expression.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns> A regular expression string representation of the configuration file passed to <paramref name="filePath"/>. </returns>
    /// <exception cref="InvalidConfigException"></exception>
    public static string ConvertFileToRegex(string filePath)
    {
        IRegexSerializable? parsedFile;
        bool success;
        try
        {
            success = TryParseFile(out parsedFile, filePath);
        }
        catch
        {
            throw new InvalidConfigException("Invalid configuration file. Valid JSON or YAML is required.");
        }
        if (parsedFile is null)
        {
            throw new InvalidConfigException("Invalid configuration file. Valid JSON or YAML is required.");
        }
        if (success)
        {
            return parsedFile!.ToRegex();
        }
        throw new InvalidConfigException("Invalid configuration file. Valid JSON or YAML is required.");
    }

    private static string ReadConfigFile(string filePath)
    {
        return File.ReadAllText(filePath);
    }

}