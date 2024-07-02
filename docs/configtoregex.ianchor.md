# IAnchor

Namespace: ConfigToRegex

Represents a Regular Expression Anchor as an object.

```csharp
public interface IAnchor : IPattern, IRegexSerializable
```

Implements [IPattern](./configtoregex.ipattern.md), [IRegexSerializable](./configtoregex.iregexserializable.md)

## Properties

### **Quantifiers**

```csharp
public abstract Quantifier Quantifiers { get; set; }
```

#### Property Value

[Quantifier](./configtoregex.quantifier.md)<br>
