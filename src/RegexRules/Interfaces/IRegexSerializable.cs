using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace RegexRules;

public interface IRegexSerializable
{

    string SerializeYaml();

    string SerializeJson();

    void DeserializeYaml(string yamlString);

    void DeserializeJson(string jsonString);

    string ToRegex();

}
