namespace Hulk;

/// <summary>
/// Representa las operaciones unarias. Expresiones del tipo [operacion] [expresion]
/// </summary>
public abstract class UnaryFunction : HulkExpression
{
    /// <summary>
    /// Construye un objeto que representa una operacion unaria
    /// </summary>
    /// <param name="Arg">Argumento de la funcion</param>
    public UnaryFunction(HulkExpression Arg)
    {
        if (Arg.IsDependent)
            IsDependent = true;
        Argument = Arg;
    }
    public override object GetValue(bool execute) => Evaluate(Argument.GetValue(execute));
    public HulkExpression Argument { get; protected set; }
    /// <summary>
    /// Tipo de retorno de la operacion
    /// </summary>
    public HulkTypes ReturnedType { get; protected set; }
    /// <summary>
    /// Tipo de entrada de la operacion aceptado
    /// </summary>
    public HulkTypes EnteredType { get; protected set; }
    /// <summary>
    /// Tipo de valor aceptado para el valor del argumento 
    /// </summary>
    public Type AcceptedType { get; protected set; }
    /// <summary>
    /// Token de las operacion
    /// </summary>
    public string OperationToken { get; protected set; }
    /// <summary>
    /// Funcion que efectua la operacion unaria
    /// </summary>
    public UnaryOperation Operation { get; protected set; }
    /// <summary>
    /// Funcion unaria delegada
    /// </summary>
    /// <param name="arg">Argumento</param>
    /// <returns></returns>
    public delegate object UnaryOperation(object arg);
    /// <summary>
    /// Evalua la operacion unaria
    /// </summary>
    /// <param name="arg">Valor del argumento de la funcion</param>
    /// <returns>Valor de retorno de la operacion unaria</returns>
    /// <exception cref="SemanticError"></exception>
    public object Evaluate(object arg)
    {
        return arg.GetType() == AcceptedType
            ? Operation(arg)
            : throw new SemanticError($"Operator `{OperationToken}`", EnteredType.ToString(), arg.GetHulkTypeAsString());
    }
    public override HulkTypes CheckType()
    {
        HulkTypes argType = Argument.CheckType();
        return argType != HulkTypes.Undetermined && argType != EnteredType
            ? throw new SemanticError($"Operator `{OperationToken}`", EnteredType.ToString(), argType.ToString())
            : ReturnedType;
    }
}
#region Boolean
/// <summary>
/// Representa la negacion logica. Expresiones del tipo ![expresion]
/// </summary>
public class Negation : UnaryFunction
{
    public Negation(HulkExpression Arg) : base(Arg)
    {
        Argument = Arg;
        ReturnedType = HulkTypes.boolean;
        EnteredType = HulkTypes.boolean;
        AcceptedType = typeof(bool);
        OperationToken = "!";
        object func(object a) => !(bool)a;
        Operation = func;
    }
}
#endregion
#region Arithmetic
/// <summary>
/// Representa la operacion unaria positiva. Expresiones del tipo +[expresion]
/// </summary>
public class Positive : UnaryFunction
{
    public Positive(HulkExpression Arg) : base(Arg)
    {
        Argument = Arg;
        ReturnedType = HulkTypes.number;
        EnteredType = HulkTypes.number;
        AcceptedType = typeof(double);
        OperationToken = "+";
        object func(object a) => a;
        Operation = func;
    }
}
/// <summary>
/// Representa la operacion unaria negativa. Expresiones del tipo -[expresion]
/// </summary>
public class Negative : UnaryFunction
{
    public Negative(HulkExpression Arg) : base(Arg)
    {
        Argument = Arg;
        ReturnedType = HulkTypes.number;
        EnteredType = HulkTypes.number;
        AcceptedType = typeof(double);
        OperationToken = "-";
        object func(object a) => -(double)a;
        Operation = func;
    }
}
/// <summary>
/// Representa el llamado a la funcion raiz cuadrada. Expresiones del tipo sqrt([expresion])
/// </summary>
public class SquaredRoot : UnaryFunction
{
    public SquaredRoot(HulkExpression Arg) : base(Arg)
    {
        Argument = Arg;
        ReturnedType = HulkTypes.number;
        EnteredType = HulkTypes.number;
        AcceptedType = typeof(double);
        OperationToken = "sqrt";
        object func(object a) => Math.Sqrt((double)a);
        Operation = func;
    }
}
/// <summary>
/// Representa el llamado a la funcion seno. Expresiones del tipo sin([expresion])
/// </summary>
public class Sine : UnaryFunction
{
    public Sine(HulkExpression Arg) : base(Arg)
    {
        Argument = Arg;
        ReturnedType = HulkTypes.number;
        EnteredType = HulkTypes.number;
        AcceptedType = typeof(double);
        OperationToken = "sin";
        object func(object a) => Math.Sin((double)a);
        Operation = func;
    }
}
/// <summary>
/// Representa el llamado a la funcion coseno. Expresiones del tipo cos([expresion])
/// </summary>
public class Cosine : UnaryFunction
{
    public Cosine(HulkExpression Arg) : base(Arg)
    {
        Argument = Arg;
        ReturnedType = HulkTypes.number;
        EnteredType = HulkTypes.number;
        AcceptedType = typeof(double);
        OperationToken = "cos";
        object func(object a) => Math.Cos((double)a);
        Operation = func;
    }
}
/// <summary>
/// Representa el llamado a la funcion exp (e elevado a una potencia). Expresiones el tipo exp([expresion])
/// </summary>
public class ERaised : UnaryFunction
{
    public ERaised(HulkExpression Arg) : base(Arg)
    {
        Argument = Arg;
        ReturnedType = HulkTypes.number;
        EnteredType = HulkTypes.number;
        AcceptedType = typeof(double);
        OperationToken = "exp";
        object func(object a) => Math.Exp((double)a);
        Operation = func;
    }
}
/// <summary>
/// Representa el llamado a la funcion random. Expresiones del tipo rand([expresion])
/// </summary>
public class Rand : HulkExpression
{
    public Rand()
    {
    }

    public override HulkTypes CheckType() => HulkTypes.number;

    public override object GetValue(bool execute)
    {
        Random random = new();
        return random.NextDouble();
    }
}
#endregion

