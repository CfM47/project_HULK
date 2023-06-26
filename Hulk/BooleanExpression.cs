using Hulk;
using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

public class BooleanVariable: HulkExpression
{
	public BooleanVariable(string exp, bool val) 
	{
        Expression = exp;
        Value = new Booln(val);
	}
	public Variable Valuen { get; protected set; }
}
public class Negation : UnaryFunction
{
    public Negation(string expression, HulkExpression Arg) : base(expression, Arg)
    {
    }

    public override Variable Evaluate(Variable arg)
	{
        if (!(arg is Booln))
        {
            throw new Exception("The \"!\" can only take a boolean as argument");
        }
        else
        {
            Booln a = arg as Booln;
            return new Booln(!a.Value);
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
        if(!(left is Booln) || !(right is Booln))
        {
            throw new Exception("The \"&&\" can only take a boolean as argument");
        }
        else
        {
            Booln a = left as Booln;
            Booln b = right as Booln;
            return new Booln(a.Value && b.Value);
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
        if (!(left is Booln) || !(right is Booln))
        {
            throw new Exception("The \"&&\" can only take a boolean as argument");
        }
        else
        {
            Booln a = left as Booln;
            Booln b = right as Booln;
            return new Booln(a.Value || b.Value);
        }
    }
}
