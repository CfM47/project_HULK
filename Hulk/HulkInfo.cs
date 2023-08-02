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
            if (!Char.IsLetterOrDigit(c))
                return false;
        if (HulkInfo.KeyWords.Contains(name))
            return false;
        if (double.TryParse(name, out _))
            return false;
        if (Char.IsDigit(name[0]))
            return false;
        return true;
    }
    public static bool AllCorrectNames(IEnumerable<string> Names) 
    {
        foreach(string name in Names)
        {
            var result = IsCorrectName(name);
            if (!result)
                return false;
        }
        return true;

    }
}

