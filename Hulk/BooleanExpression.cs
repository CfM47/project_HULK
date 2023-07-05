using Hulk;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

public class BooleanVariable: HulkExpression
{
	public BooleanVariable(bool val) 
	{
        Value = val;
	}
}
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
            throw new Exception("The \"!\" can only take a boolean as argument");
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
        if(!(left is bool) || !(right is bool))
        {
            throw new Exception("The \"&&\" can only take a boolean as argument");
        }
        else
        {
            bool a = (bool)left;
            bool b = (bool)right;
            return a && b;
        }
    }
}
public class Disjunction: BinaryFunction
{
    public Disjunction(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }
    public override object Evaluate(object left, object right)
    {
        if (!(left is bool) || !(right is bool))
        {
            throw new Exception("The \"||\" can only take a boolean as argument");
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
            throw new Exception("The \"<\" can only take a number as argument");
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
    public GreaterThan( HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (!(left is double) || !(right is double))
        {
            throw new Exception("The \">\" can only take a number as argument");
        }
        else
        {
            double a = (double)left;
            double b = (double)right;
            return a > b;
        }
    }
}
public class LowerOrEqualThan: BinaryFunction
{
    public LowerOrEqualThan(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (!(left is double) || !(right is double))
        {
            throw new Exception("The \"<=\" can only take a number as argument");
        }
        else
        {
            double a = (double)left;
            double b = (double)right;
            return a <= b;
        }
    }
}
public class GreaterOrEqualThan: BinaryFunction
{
    public GreaterOrEqualThan(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (!(left is double) || !(right is double))
        {
            throw new Exception("The \">=\" can only take a number as argument");
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


