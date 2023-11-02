namespace Hulk;
/// <summary>
/// Clase estatica que contiene información sobre el lenguaje Hulk. Operaciones, palabras clave, etc.
/// </summary>
public static class HulkInfo
{
    /// <summary>
    /// Palabras clave del lenguaje
    /// </summary>
    public static readonly string[] KeyWords = {"function", "if", "else", "let", "in"};
    /// <summary>
    /// Tokens de operaciones binarias
    /// </summary>
    public static readonly string[] BinaryOperations = { "=>", "=", ":=", "+", "-", "*", "/", "%", "^", "==", "!=", ">", "<", ">=", "<=", "&", "|", "@", "@@" };
    /// <summary>
    /// Tokens de operaciones unarias
    /// </summary>
    public static readonly string[] UnaryOperations = { "+", "-", "!" };
    /// <summary>
    /// Nombre de constantes predefinidas del lenguaje
    /// </summary>
    public static readonly string[] Constants = { "PI", "E" };
    /// <summary>
    /// Nombre de funciones predefinidas del lenguaje
    /// </summary>
    public static readonly string[] BuiltInFunctions = {"log", "sqrt", "sin","cos", "exp", "print", "rand"};
    /// <summary>
    /// Tipo de datos predefinidos del lenguaje
    /// </summary>
    public static readonly string[] HulkTypes = { "number", "boolean", "string" };
    /// <summary>
    /// Indica si un nombre es valido para una variable, funcion, etc.
    /// </summary>
    /// <param name="name">Nombre a chequear</param>
    /// <returns></returns>
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
    /// <summary>
    /// Indica si varios nombres son validos para variables, funciones, etc.
    /// </summary>
    /// <param name="Names">Nombres chequear</param>
    /// <returns></returns>
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
    /// <summary>
    /// Maximo numero de llamadas que puede recibir una funcion antes de que lance un OverflowException
    /// </summary>
    public const int StackLimit = 1000;
}

