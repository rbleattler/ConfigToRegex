# ConfigToRegex

This dotnet library provides a tool for converting configuration documents (like JSON and YAML) to regular expressions. This idea stems from the belief that, while not everyone understands regular expressions, many can benefit from being able to define a desired pattern in a human-readable format.

## Usage

For the best usage examples, please refer to the unit tests in the `ConfigToRegex.Tests` project. Here is a simple example of how to use the library:

> This example uses a YAML configuration document to define a group regular expression pattern. When this example is run, the output will be a regular expression that matches the patterns defined in the configuration document.

**YAML Configuration Document**: `testGroupPattern.yaml`

```yaml

Type: Group
Message: Testing Group Pattern
Patterns:
  - Type: Literal
    Message: testGroup0 message
    Value:
      Value: testGroup0
Properties:
  Name: testGroup0Name
  GroupType: NamedCapturing
  NamedGroupStyle: AngleBrackets

```

**C# Code**: `Program.cs`

```csharp

using ConfigToRegex;
using System;
using System.IO;

namespace ConfigToRegex.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = File.ReadAllText("testGroupPattern.yaml");
            var groupPattern = new GroupPattern(config);
            var regex = groupPattern.ToRegex()
        }
    }
}

```

**Output**: `regex`

```markdown
(?<testGroup0Name>testGroup0)
```

## Supported Configuration Document Types

- JSON
- YAML

## Supported Pattern Types

- Group
- Literal
- CharacterClass
- Anchor
- Quantifier

## Contributing

If you would like to contribute to this project, please feel free to fork the repository and submit a pull request. If you have any questions or concerns, please feel free to reach out to me directly.
