# IGroup

Namespace: ConfigToRegex

Represents a Regular Expression Group as an object.

```csharp
public interface IGroup : IPattern, IRegexSerializable
```

Implements [IPattern](./configtoregex.ipattern.md), [IRegexSerializable](./configtoregex.iregexserializable.md)

## Properties

### **Quantifiers**

```csharp
public abstract Quantifier Quantifiers { get; set; }
```

#### Property Value

[Quantifier](./configtoregex.quantifier.md)<br>

### **Position**

```csharp
public abstract int Position { get; set; }
```

#### Property Value

[Int32](https://docs.microsoft.com/en-us/dotnet/api/system.int32)<br>

### **Patterns**

```csharp
public abstract PatternList Patterns { get; set; }
```

#### Property Value

[PatternList](./configtoregex.patternlist.md)<br>
