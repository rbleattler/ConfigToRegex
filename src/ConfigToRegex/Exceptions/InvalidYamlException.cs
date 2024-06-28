namespace ConfigToRegex;

public class InvalidYamlException : Exception
{
    public InvalidYamlException()
    {
    }

    public InvalidYamlException(string message)
        : base(message)
    {
    }

    public InvalidYamlException(string message, Exception inner)
        : base(message, inner)
    {
    }
}