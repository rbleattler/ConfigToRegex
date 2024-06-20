using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Serialization;

namespace RegexRules;

public interface IRegexSerializable
{

    string SerializeYaml();

    string SerializeJson();

    IRegexSerializable DeserializeYaml(string yamlString);

    IRegexSerializable DeserializeJson(string jsonString);

}
