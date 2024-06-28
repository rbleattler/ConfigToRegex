namespace ConfigToRegex;

/// <summary>
/// An interface for serializing and deserializing configuration objects, with the ability to convert them to a Regular Expression.
/// </summary>
public interface IRegexSerializable
{

    string SerializeYaml();

    string SerializeJson();

    void DeserializeYaml(string yamlString);

    void DeserializeJson(string jsonString);

    string ToRegex();

}
