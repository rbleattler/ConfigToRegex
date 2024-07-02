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
    public T DeserializeYaml<T>(string yamlString) where T : IRegexSerializable
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
            // new object of type T
            T obj = Activator.CreateInstance<T>();
            obj.DeserializeJson(jsonString);
            return obj;
        }
        catch (Exception ex)
        {
            throw new InvalidJsonException("Invalid JSON : " + ex.Message, ex);
        }
    }

    /// <summary>
    /// Serializes an object of type <typeparamref name="T"/> to a YAML string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="patternObject"></param>
    /// <returns> A YAML representation of the object passed to <paramref name="patternObject"/>. </returns>
    /// <exception cref="ArgumentNullException"></exception>
    public string SerializeYaml<T>(T? patternObject) where T : IRegexSerializable
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
    public string ConvertYamlToJson<T>(string yamlString) where T : IRegexSerializable
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
    public string ConvertYamlToRegex<T>(string yamlString) where T : IRegexSerializable
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
    public string ConvertJsonToRegex<T>(string jsonString) where T : IRegexSerializable
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
    public string ConvertJsonToYaml<T>(string jsonString) where T : IRegexSerializable
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
    public T DeserializeYamlFromFile<T>(string filePath) where T : IRegexSerializable
    {
        var yamlString = ReadConfigFile(filePath);
        return DeserializeYaml<T>(yamlString);
    }

    /// <summary>
    /// Converts a YAML file to a JSON representation of the object defined in the file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <returns> A JSON representation of the YAML file passed to <paramref name="filePath"/>. </returns>
    public string ConvertYamlFileToJson<T>(string filePath) where T : IRegexSerializable
    {
        var yamlString = ReadConfigFile(filePath);
        return ConvertYamlToJson<T>(yamlString);
    }

    /// <summary>
    /// Converts a JSON file to a YAML representation of the object defined in the file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <returns> A YAML representation of the JSON file passed to <paramref name="filePath"/>. </returns>
    public string ConvertJsonFileToYaml<T>(string filePath) where T : IRegexSerializable
    {
        var jsonString = ReadConfigFile(filePath);
        return ConvertJsonToYaml<T>(jsonString);
    }

    /// <summary>
    /// Converts a YAML file to a regular expression based on the object defined in the file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <returns> A regular expression string representation of the YAML file passed to <paramref name="filePath"/>. </returns>
    public string ConvertYamlFileToRegex<T>(string filePath) where T : IRegexSerializable
    {
        var yamlString = ReadConfigFile(filePath);
        return ConvertYamlToRegex<T>(yamlString);
    }

    /// <summary>
    /// Converts a JSON file to a regular expression based on the object defined in the file.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filePath"></param>
    /// <returns> A regular expression string representation of the JSON file passed to <paramref name="filePath"/>. </returns>
    public string ConvertJsonFileToRegex<T>(string filePath) where T : IRegexSerializable
    {
        var jsonString = ReadConfigFile(filePath);
        return ConvertJsonToRegex<T>(jsonString);
    }

    /// <summary>
    /// Tries to parse a configuration file to an object of type <see cref="IRegexSerializable"/>.
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="result"></param>
    /// <returns> A boolean indicating whether the parsing was successful. </returns>
    public bool TryParseFile(out IRegexSerializable? result, string filePath)
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
    public bool TryParse(ref IRegexSerializable? result, string fileContents)
    {
        if (StringUtilities.IsJson(fileContents))
        {
            result = DeserializeJson<IRegexSerializable>(fileContents);
            return true;
        }
        else if (StringUtilities.IsYaml(fileContents))
        {
            result = DeserializeYaml<IRegexSerializable>(fileContents);
            return true;
        }
        else
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
    public string ConvertFileToRegex(string filePath)
    {
        IRegexSerializable? parsedFile;
        try
        {
            _ = TryParseFile(out parsedFile, filePath);
        }
        catch
        {
            throw new InvalidConfigException("Invalid configuration file. Valid JSON or YAML is required.");
        }
        return parsedFile!.ToRegex();
    }

    private static string ReadConfigFile(string filePath)
    {
        return File.ReadAllText(filePath);
    }

}