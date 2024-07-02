# IPatternProperties

Namespace: ConfigToRegex

Represents the properties of a Regular Expression Pattern. (This is only used for [GroupPattern](./configtoregex.grouppattern.md) objects.)

```csharp
public interface IPatternProperties
```

## Properties

### **Name**

```csharp
public abstract string Name { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **GroupType**

```csharp
public abstract string GroupType { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

### **NamedGroupStyle**

```csharp
public abstract string NamedGroupStyle { get; set; }
```

#### Property Value

[String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

## Methods

### **IsJson(String)**

```csharp
bool IsJson(string patternPropertiesObject)
```

#### Parameters

`patternPropertiesObject` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>

### **IsYaml(String)**

```csharp
bool IsYaml(string patternPropertiesObject)
```

#### Parameters

`patternPropertiesObject` [String](https://docs.microsoft.com/en-us/dotnet/api/system.string)<br>

#### Returns

[Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean)<br>
