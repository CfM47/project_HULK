using Hulk;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

public class BooleanVariable: HulkExpression
{
	public BooleanVariable(string exp, bool val) 
	{
        Expression = exp;
        Value = new HulkBoolean(val);
	}
}
#region Boolean Literals
public class Negation : UnaryFunction
{
    public Negation(string expression, HulkExpression Arg) : base(expression, Arg)
    {
    }

    public override Variable Evaluate(Variable arg)
	{
        if (!(arg is HulkBoolean))
        {
            throw new Exception("The \"!\" can only take a boolean as argument");
        }
        else
        {
            HulkBoolean a = arg as HulkBoolean;
            return new HulkBoolean(!a.Value);
        }
    }
}
public class Conjunction : BinaryFunction
{
    public Conjunction(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
    }

    public override Variable Evaluate(Variable left, Variable right)
    {
        if(!(left is HulkBoolean) || !(right is HulkBoolean))
        {
            throw new Exception("The \"&&\" can only take a boolean as argument");
        }
        else
        {
            HulkBoolean a = left as HulkBoolean;
            HulkBoolean b = right as HulkBoolean;
            return new HulkBoolean(a.Value && b.Value);
        }
    }
}
public class Disjunction: BinaryFunction
{
    public Disjunction(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
    }
    public override Variable Evaluate(Variable left, Variable right)
    {
        if (!(left is HulkBoolean) || !(right is HulkBoolean))
        {
            throw new Exception("The \"||\" can only take a boolean as argument");
        }
        else
        {
            HulkBoolean a = left as HulkBoolean;
            HulkBoolean b = right as HulkBoolean;
            return new HulkBoolean(a.Value || b.Value);
        }
    }
}
#endregion
#region Arithmetic comparison Operators
public class LowerThan : BinaryFunction
{
    public LowerThan(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
    }

    public override Variable Evaluate(Variable left, Variable right)
    {
        if (!(left is HulkNumber) || !(right is HulkNumber))
        {
            throw new Exception("The \"<\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = left as HulkNumber;
            HulkNumber b = right as HulkNumber;
            return new HulkBoolean(a.Value < b.Value);
        }
    }
}
public class GreaterThan : BinaryFunction
{
    public GreaterThan(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
    }

    public override Variable Evaluate(Variable left, Variable right)
    {
        if (!(left is HulkNumber) || !(right is HulkNumber))
        {
            throw new Exception("The \">\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = left as HulkNumber;
            HulkNumber b = right as HulkNumber;
            return new HulkBoolean(a.Value > b.Value);
        }
    }
}
public class LowerOrEqualThan: BinaryFunction
{
    public LowerOrEqualThan(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
    }

    public override Variable Evaluate(Variable left, Variable right)
    {
        if (!(left is HulkNumber) || !(right is HulkNumber))
        {
            throw new Exception("The \"<=\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = left as HulkNumber;
            HulkNumber b = right as HulkNumber;
            return new HulkBoolean(a.Value <= b.Value);
        }
    }
}
public class GreaterOrEqualThan: BinaryFunction
{
    public GreaterOrEqualThan(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
    }

    public override Variable Evaluate(Variable left, Variable right)
    {
        if (!(left is HulkNumber) || !(right is HulkNumber))
        {
            throw new Exception("The \">=\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = left as HulkNumber;
            HulkNumber b = right as HulkNumber;
            return new HulkBoolean(a.Value >= b.Value);
        }
    }
}
public class Equal : BinaryFunction
{
    public Equal(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
    }
    public override Variable Evaluate(Variable left, Variable right)
    {        
        HulkNumber a = left as HulkNumber;
        HulkNumber b = right as HulkNumber;
        return new HulkBoolean(a.Value == b.Value);
    }
}
public class UnEqual : BinaryFunction
{
    public UnEqual(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
    }

    public override Variable Evaluate(Variable left, Variable right)
    {
        if (!(left is HulkNumber) || !(right is HulkNumber))
        {
            throw new Exception("The \"<\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = left as HulkNumber;
            HulkNumber b = right as HulkNumber;
            return new HulkBoolean(a.Value != b.Value);
        }
    }
}
#endregion


