# IRegexSerializable

Namespace: ConfigToRegex

An interface for serializing and deserializing configuration objects, with the ability to convert them to a Regular Expression.

```csharp
public interface IRegexSerializable
```

## Methods

### **SerializeYaml()**

```csharp
string SerializeYaml()
```

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **SerializeJson()**

```csharp
string SerializeJson()
```

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **DeserializeYaml(String)**

```csharp
void DeserializeYaml(string yamlString)
```

#### Parameters

`yamlString` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **DeserializeJson(String)**

```csharp
void DeserializeJson(string jsonString)
```

#### Parameters

`jsonString` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **ToRegex()**

```csharp
string ToRegex()
```

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>
