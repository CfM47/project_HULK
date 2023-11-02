using System.Globalization;

namespace Hulk;

/// <summary>
/// Representa los errores de cualquier tipo que se puedan dar en el lenguaje
/// </summary>
public abstract class HulkException : Exception
{
    public override string Message => (MessageStart + MessageDefinition + ".").Replace("hstring", "string");
    /// <summary>
    /// Caption que inicia el mensaje de error
    /// </summary>
    public string MessageStart { get; protected set; }
    /// <summary>
    /// Descripcion del error
    /// </summary>
    public string MessageDefinition { get; protected set; }
}
/// <summary>
/// Representa los errores ocurridos en una instruccion
/// </summary>
public class InstrucctionError : HulkException
{
    public InstrucctionError(HulkException ex, int instrucctionNumber, int instrucctionsCount)
    {
        MessageStart = ex.MessageStart;
        string messageEnd = instrucctionsCount > 1 ? $" (on instrucction {instrucctionNumber})" : "";
        MessageDefinition = ex.MessageDefinition + messageEnd;
    }
}
/// <summary>
/// Representa un error generico
/// </summary>
public class DefaultError : HulkException
{
    public DefaultError(string message)
    {
        MessageStart = "! ERROR : ";
        MessageDefinition = message;
    }
    public DefaultError(string message, string errorEspecification)
    {
        errorEspecification = errorEspecification.ToUpper(new CultureInfo("en-US"));
        MessageStart = $"! {errorEspecification} ERROR :";
        MessageDefinition = message;
    }
}
/// <summary>
/// Representa un error lexico. Se producen por la presencia de tokens invalidos.
/// </summary>
public class LexicalError : HulkException
{
    public LexicalError(string invalidToken)
    {
        MessageStart = "! LEXICAL ERROR : ";
        InvalidToken = invalidToken;
        ExpectedToken = "token";
        MessageDefinition = $"`{InvalidToken}` is not a valid {ExpectedToken}";

    }
    public LexicalError(string invalidToken, string expectedToken)
    {
        MessageStart = "! LEXICAL ERROR : ";
        InvalidToken = invalidToken;
        ExpectedToken = expectedToken;
        MessageDefinition = $"`{InvalidToken}` is not a valid {ExpectedToken}";
    }
    public string InvalidToken { get; }
    public string ExpectedToken { get; }
}
/// <summary>
/// Representa un error sintactico. Se produce por expresiones incompletas
/// </summary>
public class SyntaxError : HulkException
{
    public SyntaxError(string missingPart, string place)
    {
        MessageStart = "! SYNTAX ERROR : ";
        MissingPart = missingPart;
        MissingPlace = place;
        MessageDefinition = $"Missing {MissingPart} in {MissingPlace}";
    }
    public string MissingPart { get; }
    public string MissingPlace { get; }
}
/// <summary>
/// Representa un error semantico. Se produce por el uso incorrecto de los tipos y argumentos
/// </summary>
public class SemanticError : HulkException
{
    public SemanticError(string expression, string expected, string received)
    {
        MessageStart = "! SEMANTIC ERROR : ";
        Expression = expression;
        ExpressionReceived = received;
        ExpressionExpected = expected;
        MessageDefinition = $"{Expression} receives `{expected}`, not `{received}`";
    }
    public string Expression { get; }
    public string ExpressionReceived { get; }
    public string ExpressionExpected { get; }
}
/// <summary>
/// Representa un error producido por la sobrecarga de la pila de llamados
/// </summary>
public class OverFlowError : HulkException
{
    public OverFlowError(string functionName)
    {
        FunctionName = functionName;
        MessageStart = "! FUNCTION ERROR : ";
        MessageDefinition = $"Function '{FunctionName}' reached call stack limit (callstack limit is {HulkInfo.StackLimit})";
    }
    public string FunctionName { get; }
}
