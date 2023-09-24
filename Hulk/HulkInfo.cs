namespace Hulk;

public static class HulkInfo
{
    //clase que contiene información sobre el lenguaje. Operaciones, palabras clave, etc.
    public static string[] KeyWords = { "+", "-", "*", "/", "^", "&&", "&", "|", "||", "=", "(", ")", ",", "function", "number", "boolean", "string", "let", "in", "=>", "else", "if" };
    public enum BinaryOperations
    {
        Power,
        Multiplication,
        Division,
        Module,
        Addition,
        Subtraction,
        LowerThan,
        GreaterThan,
        LowerEqualThan,
        GreaterEqualThan,
        Equal,
        Unequal,
        AND,
        OR,
    }
    public enum UnaryOperations
    {
        Positive,
        Negative,
        Negation,
    }
    public static bool IsCorrectName(string name)
    {
        foreach (char c in name)
            if (!char.IsLetterOrDigit(c))
                return false;
        if (KeyWords.Contains(name))
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

