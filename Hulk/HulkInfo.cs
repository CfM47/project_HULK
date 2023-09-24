namespace Hulk;

public static class HulkInfo
{
    //clase que contiene información sobre el lenguaje. Operaciones, palabras clave, etc.
    public static readonly string[] KeyWords = {"function", "if", "else", "let", "in"};
    public static readonly string[] BinaryOperations = { "=>", "=", ":=", "+", "-", "*", "/", "%", "^", "==", "!=", ">", "<", ">=", "<=", "&", "|", "@", "@@" };
    public static readonly string[] UnaryOperations = { "+", "-", "!" };
    public static readonly string[] Constants = { "PI", "E" };
    public static readonly string[] BuiltInFunctions = {"log", "sqrt", "sin","cos", "exp", "print", "rand"};
    public static readonly string[] HulkTypes = { "number", "boolean", "string" };
    public static bool IsCorrectName(string name)
    {
        foreach (char c in name)
            if (!char.IsLetterOrDigit(c))
                return false;
        if (KeyWords.Contains(name) || Constants.Contains(name) || BuiltInFunctions.Contains(name) || HulkTypes.Contains(name))
            return false;
        if (double.TryParse(name, out _))
            return false;
        return !char.IsDigit(name[0]);
    }
    public static bool AllCorrectNames(IEnumerable<string> Names)
    {
        foreach (string name in Names)
        {
            bool result = IsCorrectName(name);
            if (!result)
                return false;
        }
        return true;
    }
    public const int StackLimit = 1000; //este es el maximo numero de llamadas que puede recibir una funcion antes de que lance un StackOverflowException
}

