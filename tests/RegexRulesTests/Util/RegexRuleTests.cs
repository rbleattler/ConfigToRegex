using System.Runtime.CompilerServices;
using RegexRules;

namespace RegexRulesTests;

public abstract class RegexRuleTestCore
{

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

    public static string[] GetAllTestFiles([CallerFilePath] string directory = "", string searchPattern = "*.yml")
    {
        var directoryPath = Path.GetFullPath(directory);
        var files = Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
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