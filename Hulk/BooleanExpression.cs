using Hulk;

#region Boolean Literals
public class Negation : UnaryFunction
{
    public Negation(HulkExpression Arg) : base(Arg)
    {
    }

    public override object Evaluate(object arg)
    {
        if (!(arg is bool))
        {
            throw new SemanticError("Operator `!`", "boolean", arg.GetType().Name);
        }
        else
        {
            bool a = (bool)arg;
            return !a;
        }
    }
}
public class Conjunction : BinaryFunction
{
    public Conjunction(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (!(left is bool) || !(right is bool))
        {
            var conflictiveType = !(left is bool) ? left.GetType().Name : right.GetType().Name;
            throw new SemanticError("Operator `&&`", "boolean", conflictiveType);
        }
        else
        {
            bool a = (bool)left;
            bool b = (bool)right;
            return a && b;
        }
    }
}
public class Disjunction : BinaryFunction
{
    public Disjunction(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }
    public override object Evaluate(object left, object right)
    {
        if (!(left is bool) || !(right is bool))
        {
            var conflictiveType = !(left is bool) ? left.GetType().Name : right.GetType().Name;
            throw new SemanticError("Operator `&&`", "boolean", conflictiveType);
        }
        else
        {
            bool a = (bool)left;
            bool b = (bool)right;
            return a || b;
        }
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
        if (!(left is double) || !(right is double))
        {
            var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
            throw new SemanticError("Operator `<`", "number", conflictiveType);
        }
        else
        {
            double a = (double)left;
            double b = (double)right;
            return a < b;
        }
    }
}
public class GreaterThan : BinaryFunction
{
    public GreaterThan(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (!(left is double) || !(right is double))
        {
            var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
            throw new SemanticError("Operator `>`", "number", conflictiveType);
        }
        else
        {
            double a = (double)left;
            double b = (double)right;
            return a > b;
        }
    }
}
public class LowerEqualThan : BinaryFunction
{
    public LowerEqualThan(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (!(left is double) || !(right is double))
        {
            var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
            throw new SemanticError("Operator `<=`", "number", conflictiveType);
        }
        else
        {
            double a = (double)left;
            double b = (double)right;
            return a <= b;
        }
    }
}
public class GreaterEqualThan : BinaryFunction
{
    public GreaterEqualThan(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (!(left is double) || !(right is double))
        {
            var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
            throw new SemanticError("Operator `>=`", "number", conflictiveType);
        }
        else
        {
            double a = (double)left;
            double b = (double)right;
            return a >= b;
        }
    }
}
public class Equal : BinaryFunction
{
    public Equal(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }
    public override object Evaluate(object left, object right)
    {
        return left == right;
    }
}
public class UnEqual : BinaryFunction
{
    public UnEqual(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        return left != right;
    }
}
#endregion


