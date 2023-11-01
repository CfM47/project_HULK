namespace Hulk;

public abstract class BinaryFunction : HulkExpression
{
    public BinaryFunction(HulkExpression leftArgument, HulkExpression rightArgument)
    {
        if (leftArgument.IsDependent || rightArgument.IsDependent)
            IsDependent = true;
        LeftArgument = leftArgument;
        RightArgument = rightArgument;
    }
    #region Methods
    public override object GetValue(bool execute) => Evaluate(LeftArgument.GetValue(execute), RightArgument.GetValue(execute));
    //es posible que elimine el siguiente metodo
    protected bool ArgsOk(object left, object right, List<Type> AdmitedTypesName)
    {
        foreach (Type type in AdmitedTypesName)
        {
            if (left == null || right == null)
            {
                if (left == null && right != null)
                {
                    if (right.GetType() == type)
                        return true;
                }
                else if (right == null && left != null)
                {
                    if (left.GetType() == type)
                        return true;
                }
                return true;
            }
            if (left.GetType() == right.GetType() && left.GetType() == type)
                return true;
        }
        return false;
    }
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
    public object Evaluate(object left, object right)
    {
        if (left.GetType() == AcceptedType && right.GetType() == AcceptedType || AcceptedType == typeof(object))
            return Operation(left, right);
        var conflictiveType = left.GetType() != AcceptedType ? left.GetHulkTypeAsString() : right.GetHulkTypeAsString();
        throw new SemanticError($"Operator `{OperationToken}`", ReturnedType.ToString(), conflictiveType);
    }
    #endregion
    #region Properties
    public HulkExpression LeftArgument { get; protected set; }
    public HulkExpression RightArgument { get; protected set; }
    public Types ReturnedType { get; protected set; }
    public Types EnteredType { get; protected set; }
    public Type AcceptedType { get; protected set; }
    public string OperationToken { get; protected set; }
    public BinaryOperation Operation { get; protected set; }
    public delegate object BinaryOperation(object left, object right);
    #endregion
}
#region Boolean Literals
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
        object func(object a, object b) => (dynamic)a && (dynamic)b;
        Operation = func;
    }
}
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
        object func(object a, object b) => (dynamic)a || (dynamic)b;
        Operation = func;
    }
}
#endregion
#region Arithmetic comparison Operators
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
        object func(object a, object b) => (dynamic)a < (dynamic)b;
        Operation = func;
    }
}
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
        object func(object a, object b) => (dynamic)a > (dynamic)b;
        Operation = func;
    }
}
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
        object func(object a, object b) => (dynamic)a <= (dynamic)b;
        Operation = func;
    }
}
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
        object func(object a, object b) => (dynamic)a >= (dynamic)b;
        Operation = func;
    }
}
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
        object func(object a, object b) => (dynamic)a + (dynamic)b;
        Operation = func;
    }
}
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
        object func(object a, object b) => (dynamic)a - (dynamic)b;
        Operation = func;
    }
}
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
        object func(object a, object b) => (dynamic)a * (dynamic)b;
        Operation = func;
    }
}
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
        object func(object a, object b)
        {
            return (double)b == 0 ? throw new DefaultError("Atempted to divide by 0", "arithmetic") : (object)((dynamic)a / (dynamic)b);
        }
        Operation = func;
    }
}
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
        object func(object a, object b)
        {
            return (double)b == 0 ? throw new DefaultError("Atempted to divide by 0", "arithmetic") : (object)((dynamic)a % (dynamic)b);
        }
        Operation = func;
    }
}
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
        object func(object a, object b) => Math.Pow((dynamic)a, (dynamic)b);
        Operation = func;
    }
}
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
