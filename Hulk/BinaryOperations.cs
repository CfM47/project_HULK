using Microsoft.CSharp.RuntimeBinder;

namespace Hulk;

/// <summary>
/// Representa una operacion binaria. Expresiones del tipo  [expresion] [operacion] [expresion]
/// </summary>
public abstract class BinaryFunction : HulkExpression
{
    /// <summary>
    /// Contructor de un objeto que representa a una funcion binaria
    /// </summary>
    /// <param name="leftArgument">Miembro izquierdo</param>
    /// <param name="rightArgument">Miembro derecho</param>
    public BinaryFunction(HulkExpression leftArgument, HulkExpression rightArgument)
    {
        if (leftArgument.IsDependent || rightArgument.IsDependent)
            IsDependent = true;
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
    }
    #region Methods
    public override object GetValue(bool execute) => Evaluate(LeftArgument.GetValue(execute), RightArgument.GetValue(execute));
    public override Types CheckType()
    {
        if (EnteredType == Types.dynamic)
            return ReturnedType;
        var leftType = LeftArgument.CheckType();
        var rightType = RightArgument.CheckType();
        if (leftType != EnteredType && leftType != Types.dynamic)
            throw new SemanticError($"Operator `{OperationToken}`", EnteredType.ToString(), leftType.ToString());
        if (rightType != EnteredType && rightType != Types.dynamic)
            throw new SemanticError($"Operator `{OperationToken}`", EnteredType.ToString(), rightType.ToString());
        return ReturnedType;
    }
    /// <summary>
    /// Evalua la expresion
    /// </summary>
    /// <param name="left">Valor del miembro izquierdo</param>
    /// <param name="right">Valor del miembro derecho</param>
    /// <returns>Valor de la expresion</returns>
    /// <exception cref="SemanticError"></exception>
    public object Evaluate(object left, object right)
    {
        if (left.GetType() == AcceptedType && right.GetType() == AcceptedType || AcceptedType == typeof(object))
            return Operation(left, right);
        var conflictiveType = left.GetType() != AcceptedType ? left.GetHulkTypeAsString() : right.GetHulkTypeAsString();
        throw new SemanticError($"Operator `{OperationToken}`", ReturnedType.ToString(), conflictiveType);
    }
    #endregion
    #region Properties
    /// <summary>
    /// Expresion que representa al miembro izquierdo de la expresion
    /// </summary>
    public HulkExpression LeftArgument { get; protected set; }
    /// <summary>
    /// Expresion que representa al miembro derecho de la expresion
    /// </summary>
    public HulkExpression RightArgument { get; protected set; }
    /// <summary>
    /// Tipo de retorno aceptado
    /// </summary>
    public Types ReturnedType { get; protected set; }
    /// <summary>
    /// Tipo de extrada aceptado
    /// </summary>
    public Types EnteredType { get; protected set; }
    /// <summary>
    /// Tipo aceptado del objeto que devuelven los argumentos
    /// </summary>
    public Type AcceptedType { get; protected set; }
    public string OperationToken { get; protected set; }
    public BinaryOperation Operation { get; protected set; }
    public delegate object BinaryOperation(object left, object right);
    #endregion
}
#region Boolean Literals
/// <summary>
/// Clase que representa la conjuncion logica. Expresion del tipo [expresion] & [expresion]
/// </summary>
public class Conjunction : BinaryFunction
{
    public Conjunction(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.boolean;
        EnteredType = Types.boolean;
        AcceptedType = typeof(bool);
        OperationToken = "&";
        object func(object a, object b) => (bool)a && (bool)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa la disyuncion logica. Expresion del tipo [expresion] | [expresion]
/// </summary>
public class Disjunction : BinaryFunction
{
    public Disjunction(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.boolean;
        EnteredType = Types.boolean;
        AcceptedType = typeof(bool);
        OperationToken = "|";
        object func(object a, object b) => (bool)a || (bool)b;
        Operation = func;
    }
}
#endregion
#region Arithmetic comparison Operators
/// <summary>
/// Clase que representa la operacion menor que. Expresion del tipo [expresion] < [expresion]
/// </summary>
public class LowerThan : BinaryFunction
{
    public LowerThan(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.boolean;
        EnteredType = Types.number;
        AcceptedType = typeof(double);
        OperationToken = "<";
        object func(object a, object b) => (double)a < (double)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa la operacion mayor que. Expresion del tipo [expresion] > [expresion]
/// </summary>
public class GreaterThan : BinaryFunction
{
    public GreaterThan(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.boolean;
        EnteredType = Types.number;
        AcceptedType = typeof(double);
        OperationToken = ">";
        object func(object a, object b) => (double)a > (double)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa la operacion menor o igual que. Expresion del tipo [expresion] <= [expresion]
/// </summary>
public class LowerEqualThan : BinaryFunction
{
    public LowerEqualThan(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.boolean;
        EnteredType = Types.number;
        AcceptedType = typeof(double);
        OperationToken = "<=";
        object func(object a, object b) => (double)a <= (double)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa la operacion mayor igual que. Expresion del tipo [expresion] >= [expresion]
/// </summary>
public class GreaterEqualThan : BinaryFunction
{
    public GreaterEqualThan(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.boolean;
        EnteredType = Types.number;
        AcceptedType = typeof(double);
        OperationToken = ">=";
        object func(object a, object b) => (double)a >= (double)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa la operacion de igualdad. Expresion del tipo [expresion] == [expresion]
/// </summary>
public class Equal : BinaryFunction
{
    public Equal(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.boolean;
        EnteredType = Types.dynamic;
        AcceptedType = typeof(object);
        OperationToken = "==";
        object func(object a, object b)
        {
            try
            {
                return (dynamic)a == (dynamic)b;
            }
            catch (RuntimeBinderException ex)
            {
                string message = ex.Message;
                message = message.Replace("'", "`");
                message = message.Replace("bool", "boolean");
                message = message.Replace("int", "number");
                message = message.Replace("double", "number");
                throw new DefaultError(message, "semantic");
            }
        }
        Operation = func;
    }
}
/// <summary>
/// Clase que representa la operacion desigualdad que. Expresion del tipo [expresion] != [expresion]
/// </summary>
public class UnEqual : BinaryFunction
{
    public UnEqual(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.boolean;
        EnteredType = Types.dynamic;
        AcceptedType = typeof(object);
        OperationToken = "!=";
        object func(object a, object b)
        {
            try
            {
                return (dynamic)a != (dynamic)b;
            }
            catch (RuntimeBinderException ex)
            {
                string message = ex.Message;
                message = message.Replace("'", "`");
                message = message.Replace("bool", "boolean");
                message = message.Replace("int", "number");
                message = message.Replace("double", "number");
                throw new DefaultError(message, "semantic");
            }
        }
        Operation = func;
    }
}
#endregion
#region Arithmetic Basic Operations
/// <summary>
/// Clase que representa la adicion. Expresion del tipo [expresion] + [expresion]
/// </summary>
public class Addition : BinaryFunction
{
    public Addition(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.number;
        EnteredType = Types.number;
        AcceptedType = typeof(double);
        OperationToken = "+";
        object func(object a, object b) => (double)a + (double)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa la sustraccion. Expresion del tipo [expresion] - [expresion]
/// </summary>
public class Subtraction : BinaryFunction
{
    public Subtraction(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.number;
        EnteredType = Types.number;
        AcceptedType = typeof(double);
        OperationToken = "-";
        object func(object a, object b) => (double)a - (double)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa la multiplicacion. Expresion del tipo [expresion] * [expresion]
/// </summary>
public class Multiplication : BinaryFunction
{
    public Multiplication(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.number;
        EnteredType = Types.number;
        AcceptedType = typeof(double);
        OperationToken = "*";
        object func(object a, object b) => (double)a * (double)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa la division. Expresion del tipo [expresion] / [expresion]
/// </summary>
public class Division : BinaryFunction
{
    public Division(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.number;
        EnteredType = Types.number;
        AcceptedType = typeof(double);
        OperationToken = "/";
        object func(object a, object b) => (double)b == 0 ? throw new DefaultError("Atempted to divide by 0", "arithmetic") : (double)a / (double)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa la operacion modulo o resto. Expresion del tipo [expresion] % [expresion]
/// </summary>
public class Module : BinaryFunction
{
    public Module(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.number;
        EnteredType = Types.number;
        AcceptedType = typeof(double);
        OperationToken = "%";
        object func(object a, object b) => (double)b == 0 ? throw new DefaultError("Atempted to divide by 0", "arithmetic") : (double)a % (double)b;
        Operation = func;
    }
}
/// <summary>
/// Clase que representa la exponenciacion. Expresion del tipo [expresion] ^ [expresion]
/// </summary>
public class Power : BinaryFunction
{
    public Power(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.number;
        EnteredType = Types.number;
        AcceptedType = typeof(double);
        OperationToken = "^";
        object func(object a, object b) => Math.Pow((double)a, (double)b);
        Operation = func;
    }
}
/// <summary>
/// Clase que representa el llamado a la funcion logaritmo. Expresion del tipo log([expresion],[expresion])
/// </summary>
public class Logarithm : BinaryFunction
{
    public Logarithm(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.number;
        EnteredType = Types.number;
        AcceptedType = typeof(double);
        OperationToken = "log";
        object func(object a, object b) => Math.Log((dynamic)a, (dynamic)b);
        Operation = func;
    }
}
#endregion
#region String Operations
/// <summary>
/// Clase que representa la concatenacion de expresiones. Expresion del tipo [expresion] @ [expresion]
/// </summary>
public class SimpleConcatenation : BinaryFunction
{
    public SimpleConcatenation(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.hstring;
        EnteredType = Types.dynamic;
        AcceptedType = typeof(object);
        OperationToken = "@";
        object func(object a, object b) => (dynamic)a.ToString() + (dynamic)b.ToString();
        Operation = func;
    }
}
/// <summary>
/// Clase que representa la concatenacion con espacio en blanco intermedio. Expresion del tipo [expresion] @@ [expresion]
/// </summary>
public class WhiteSpaceConcatenation : BinaryFunction
{
    public WhiteSpaceConcatenation(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
        ReturnedType = Types.hstring;
        EnteredType = Types.dynamic;
        AcceptedType = typeof(object);
        OperationToken = "@@";
        object func(object a, object b) => (dynamic)a.ToString() + " " + (dynamic)b.ToString();
        Operation = func;
    }
}
#endregion
