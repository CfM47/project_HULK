using System;

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
}
