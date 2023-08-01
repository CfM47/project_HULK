using Hulk;

#region Boolean Literals
public class Negation : UnaryFunction
{
    public Negation(HulkExpression Arg) : base(Arg)
    {
    }

    public override object Evaluate(object arg)
    {
        if (arg is bool)
            return (bool)arg;
        if (arg == null)
            return default(bool);
        throw new SemanticError("Operator `+`", "number", arg.GetHulkTypeAsString());
    }
}
public class Conjunction : BinaryFunction
{
    public Conjunction(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (left == null)
            left = right;
        if (right == null)
            right = left;
        if (left == null && right == null)
            return default(bool);
        if ((left is bool && right is bool))
            return (dynamic)left && (dynamic)right;
        var conflictiveType = !(left is bool) ? left.GetType().Name : right.GetType().Name;
        throw new SemanticError("Operator `&&`", "boolean", conflictiveType);
    }
}
public class Disjunction : BinaryFunction
{
    public Disjunction(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }
    public override object Evaluate(object left, object right)
    {
        if (left == null)
            left = right;
        if (right == null)
            right = left;
        if (left == null && right == null)
            return default(bool);
        if ((left is bool && right is bool))
            return (dynamic)left || (dynamic)right;
        var conflictiveType = !(left is bool) ? left.GetType().Name : right.GetType().Name;
        throw new SemanticError("Operator `||`", "boolean", conflictiveType);
    }
}
#endregion
#region Arithmetic comparison Operators
public class LowerThan : BinaryFunction
{
    public LowerThan(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (left == null)
            left = right;
        if (right == null)
            right = left;
        if (left == null && right == null)
            return default(bool);
        if ((left is double && right is double))
            return (dynamic)left < (dynamic)right;
        var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
        throw new SemanticError("Operator `<`", "number", conflictiveType);
    }
}
public class GreaterThan : BinaryFunction
{
    public GreaterThan(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (left == null)
            left = right;
        if (right == null)
            right = left;
        if (left == null && right == null)
            return default(bool);
        if ((left is double && right is double))
            return (dynamic)left > (dynamic)right;
        var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
        throw new SemanticError("Operator `>`", "number", conflictiveType);
    }
}
public class LowerEqualThan : BinaryFunction
{
    public LowerEqualThan(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (left == null)
            left = right;
        if (right == null)
            right = left;
        if (left == null && right == null)
            return default(bool);
        if ((left is double && right is double))
            return (dynamic)left <= (dynamic)right;
        var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
        throw new SemanticError("Operator `<=`", "number", conflictiveType);
    }
}
public class GreaterEqualThan : BinaryFunction
{
    public GreaterEqualThan(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (left == null)
            left = right;
        if (right == null)
            right = left;
        if (left == null && right == null)
            return default(bool);
        if ((left is double && right is double))
            return (dynamic)left >= (dynamic)right;
        var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
        throw new SemanticError("Operator `>=`", "number", conflictiveType);
    }
}
public class Equal : BinaryFunction
{
    public Equal(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }
    public override object Evaluate(object left, object right)
    {
        return (dynamic)left == (dynamic)right;
    }
}
public class UnEqual : BinaryFunction
{
    public UnEqual(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        return (dynamic)left != (dynamic)right;
    }
}
#endregion


