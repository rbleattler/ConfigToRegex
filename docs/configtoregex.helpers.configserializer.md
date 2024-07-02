# ConfigSerializer

Namespace: ConfigToRegex.Helpers

```csharp
public class ConfigSerializer
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [ConfigSerializer](./configtoregex.helpers.configserializer.md)

## Constructors

### **ConfigSerializer()**

Initializes a new instance of the [ConfigSerializer](./configtoregex.helpers.configserializer.md) class.

```csharp
public ConfigSerializer()
```

## Methods

### **DeserializeYaml&lt;T&gt;(String)**

Deserializes a YAML string to an object of type .

```csharp
public T DeserializeYaml<T>(string yamlString)
```

#### Type Parameters

`T`<br>

#### Parameters

`yamlString` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

T<br>
An object of type .

#### Exceptions

[InvalidYamlException](./configtoregex.exceptions.invalidyamlexception.md)<br>

### **DeserializeJson&lt;T&gt;(String)**

Deserializes a JSON string to an object of type .

```csharp
public static T DeserializeJson<T>(string jsonString)
```

#### Type Parameters

`T`<br>

#### Parameters

`jsonString` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

T<br>
An object of type .

#### Exceptions

[InvalidJsonException](./configtoregex.exceptions.invalidjsonexception.md)<br>

### **SerializeYaml&lt;T&gt;(T)**

Serializes an object of type  to a YAML string.

```csharp
public string SerializeYaml<T>(T patternObject)
```

#### Type Parameters

`T`<br>

#### Parameters

`patternObject` T<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
A YAML representation of the object passed to .

#### Exceptions

[ArgumentNullException](https://docs.microsoft.com/en-us/dotnet/api/system.argumentnullexception)<br>

### **SerializeJson&lt;T&gt;(T)**

Serializes an object of type  to a JSON string.

```csharp
public static string SerializeJson<T>(T patternObject)
```

#### Type Parameters

`T`<br>

#### Parameters

`patternObject` T<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
A JSON representation of the object passed to .

### **ConvertYamlToJson&lt;T&gt;(String)**

Converts a YAML string to a JSON representation of the object defined in the string.

```csharp
public string ConvertYamlToJson<T>(string yamlString)
```

#### Type Parameters

`T`<br>

#### Parameters

`yamlString` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
A JSON representation of the YAML string passed to .

### **ConvertYamlToRegex&lt;T&gt;(String)**

Converts a YAML string to a regular expression based on the object defined in the string.

```csharp
public string ConvertYamlToRegex<T>(string yamlString)
```

#### Type Parameters

`T`<br>

#### Parameters

`yamlString` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
A regular expression string representation of the YAML string passed to .

### **ConvertJsonToRegex&lt;T&gt;(String)**

Converts a JSON string to a regular expression based on the object defined in the string.

```csharp
public string ConvertJsonToRegex<T>(string jsonString)
```

#### Type Parameters

`T`<br>

#### Parameters

`jsonString` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
A regular expression string representation of the JSON string passed to .

### **ConvertJsonToYaml&lt;T&gt;(String)**

Converts a JSON string to a YAML representation of the object defined in the string.

```csharp
public string ConvertJsonToYaml<T>(string jsonString)
```

#### Type Parameters

`T`<br>

#### Parameters

`jsonString` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
A YAML representation of the JSON string passed to .

### **DeserializeYamlFromFile&lt;T&gt;(String)**

Deserializes a YAML file to an object of type .

```csharp
public T DeserializeYamlFromFile<T>(string filePath)
```

#### Type Parameters

`T`<br>

#### Parameters

`filePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

T<br>
An object of type .

### **ConvertYamlFileToJson&lt;T&gt;(String)**

Converts a YAML file to a JSON representation of the object defined in the file.

```csharp
public string ConvertYamlFileToJson<T>(string filePath)
```

#### Type Parameters

`T`<br>

#### Parameters

`filePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
A JSON representation of the YAML file passed to .

### **ConvertJsonFileToYaml&lt;T&gt;(String)**

Converts a JSON file to a YAML representation of the object defined in the file.

```csharp
public string ConvertJsonFileToYaml<T>(string filePath)
```

#### Type Parameters

`T`<br>

#### Parameters

`filePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
A YAML representation of the JSON file passed to .

### **ConvertYamlFileToRegex&lt;T&gt;(String)**

Converts a YAML file to a regular expression based on the object defined in the file.

```csharp
public string ConvertYamlFileToRegex<T>(string filePath)
```

#### Type Parameters

`T`<br>

#### Parameters

`filePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
A regular expression string representation of the YAML file passed to .

### **ConvertJsonFileToRegex&lt;T&gt;(String)**

Converts a JSON file to a regular expression based on the object defined in the file.

```csharp
public string ConvertJsonFileToRegex<T>(string filePath)
```

#### Type Parameters

`T`<br>

#### Parameters

`filePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
A regular expression string representation of the JSON file passed to .

### **TryParseFile(IRegexSerializable&, String)**

Tries to parse a configuration file to an object of type [IRegexSerializable](./configtoregex.iregexserializable.md).

```csharp
public bool TryParseFile(IRegexSerializable& result, string filePath)
```

#### Parameters

`result` [IRegexSerializable&](./configtoregex.iregexserializable&.md)<br>

`filePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
A boolean indicating whether the parsing was successful.

### **TryParse(IRegexSerializable&, String)**

Tries to parse a configuration string to an object of type [IRegexSerializable](./configtoregex.iregexserializable.md).

```csharp
public bool TryParse(IRegexSerializable& result, string fileContents)
```

#### Parameters

`result` [IRegexSerializable&](./configtoregex.iregexserializable&.md)<br>

`fileContents` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
A boolean indicating whether the parsing was successful.

### **ConvertFileToRegex(String)**

Converts a configuration file to a regular expression.

```csharp
public string ConvertFileToRegex(string filePath)
```

#### Parameters

`filePath` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
A regular expression string representation of the configuration file passed to .

#### Exceptions

[InvalidConfigException](./configtoregex.exceptions.invalidconfigexception.md)<br>
