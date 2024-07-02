# UniversalSerializer

Namespace: ConfigToRegex.Helpers

```csharp
public static class UniversalSerializer
```

Inheritance [Object](https://docs.microsoft.com/en-us/dotnet/api/system.object) → [UniversalSerializer](./configtoregex.helpers.universalserializer.md)

## Methods

### **SerializeYaml(IRegexSerializable)**

```csharp
public static string SerializeYaml(IRegexSerializable obj)
```

#### Parameters

`obj` [IRegexSerializable](./configtoregex.iregexserializable.md)<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **SerializeJson(IRegexSerializable)**

```csharp
public static string SerializeJson(IRegexSerializable obj)
```

#### Parameters

`obj` [IRegexSerializable](./configtoregex.iregexserializable.md)<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **DeserializeYaml&lt;T&gt;(String)**

```csharp
public static T DeserializeYaml<T>(string yaml)
```

#### Type Parameters

`T`<br>

#### Parameters

`yaml` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

T<br>

### **DeserializeYaml(String, Type)**

```csharp
public static object DeserializeYaml(string yaml, Type T)
```

#### Parameters

`yaml` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`T` [Type](https://docs.microsoft.com/en-us/dotnet/api/system.type)<br>

#### Returns

[Object](https://docs.microsoft.com/en-us/dotnet/api/system.object)<br>

### **DeserializeYaml(String)**

```csharp
public static IRegexSerializable DeserializeYaml(string yaml)
```

#### Parameters

`yaml` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[IRegexSerializable](./configtoregex.iregexserializable.md)<br>

### **DeserializeJson(String)**

```csharp
public static IRegexSerializable DeserializeJson(string json)
```

#### Parameters

`json` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[IRegexSerializable](./configtoregex.iregexserializable.md)<br>

### **Convert(IRegexSerializable, String)**

```csharp
public static string Convert(IRegexSerializable obj, string format)
```

#### Parameters

`obj` [IRegexSerializable](./configtoregex.iregexserializable.md)<br>

`format` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **Convert(String, String)**

```csharp
public static IRegexSerializable Convert(string obj, string format)
```

#### Parameters

`obj` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

`format` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[IRegexSerializable](./configtoregex.iregexserializable.md)<br>
