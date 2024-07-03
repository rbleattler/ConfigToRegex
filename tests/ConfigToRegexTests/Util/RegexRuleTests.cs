using System.Runtime.CompilerServices;

namespace ConfigToRegexTests;

public abstract class RegexRuleTestCore
{
    private static readonly string[] DefaultSearchPatterns = ["*.yml", "*.yaml", "*.json"];

    public static string ExampleFilesDirectory = GetExamplesDirectory();

    public static string ReadFileAsString(string file, [CallerFilePath] string filePath = "")
    {
        var fullPath = Path.GetFullPath(file);
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"File not found: {fullPath}");
        }
        return File.ReadAllText(fullPath);
    }

    public static string[] GetAllTestFiles([CallerFilePath] string directory = "", string[]? searchPatterns = null)
    {
        if (searchPatterns == null)
        {
            searchPatterns = DefaultSearchPatterns;
        }
        var directoryPath = Path.GetFullPath(directory);
        var enumOptions = new EnumerationOptions
        {
            RecurseSubdirectories = true,
            MatchCasing = MatchCasing.CaseInsensitive
        };
        var files = searchPatterns.SelectMany(searchPattern => Directory.GetFiles(directoryPath, searchPattern, enumOptions)).ToArray();
        return files;
    }

    public static string[] GetAllTestFiles([CallerFilePath] string directory = "", string searchPattern = "*.yml")
    {
        var directoryPath = Path.GetFullPath(directory);
        var enumOptions = new EnumerationOptions
        {
            RecurseSubdirectories = true,
            MatchCasing = MatchCasing.CaseInsensitive
        };
        var files = Directory.GetFiles(directoryPath, searchPattern, enumOptions);
        return files;
    }

    public static string GetExamplesDirectory([CallerFilePath] string? directory = "")
    {
        if (string.IsNullOrWhiteSpace(directory))
        {
            directory = Directory.GetCurrentDirectory();
        }
        string? combinedPath = Path.Combine(Path.GetDirectoryName(directory)!, "../examples");
        return Path.GetFullPath(combinedPath);
    }

}