namespace RegexRules;

public interface IRegexSerializable
{

    string SerializeYaml();

    string SerializeJson();

    void DeserializeYaml(string yamlString);

    void DeserializeJson(string jsonString);

    string ToRegex();

}
