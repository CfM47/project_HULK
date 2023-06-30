using Hulk;
using System;

public class NumVariable : HulkExpression
{
    public NumVariable(string exp, double val)
    {
        Expression = exp;
        Value = new HulkNumber(val);
    }
}
public class Addition : BinaryFunction
{
    public Addition(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
        if (leftArgument == null)
            Value = new HulkNumber(0);
    }

    public override Variable Evaluate(Variable left, Variable right)
    {
        if (!(left is HulkNumber) || !(right is HulkNumber))
        {
            throw new Exception("The \"+\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = left as HulkNumber;
            HulkNumber b = right as HulkNumber;
            return new HulkNumber(a.Value + b.Value);
        }
    }
}
public class Subtraction: BinaryFunction
{
    public Subtraction(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
        if (leftArgument == null)
            Value = new HulkNumber(0);
    }

    public override Variable Evaluate(Variable left, Variable right)
    {
        if (!(left is HulkNumber) || !(right is HulkNumber))
        {
            throw new Exception("The \"-\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = left as HulkNumber;
            HulkNumber b = right as HulkNumber;
            return new HulkNumber(a.Value - b.Value);
        }
    }
}
public class Multiplication : BinaryFunction
{
    public Multiplication(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
    }

    public override Variable Evaluate(Variable left, Variable right)
    {
        if (!(left is HulkNumber) || !(right is HulkNumber))
        {
            throw new Exception("The \"+\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = left as HulkNumber;
            HulkNumber b = right as HulkNumber;
            return new HulkNumber(a.Value * b.Value);
        }
    }
}
public class Division : BinaryFunction
{
    public Division(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
    }

    public override Variable Evaluate(Variable left, Variable right)
    {
        if (!(left is HulkNumber) || !(right is HulkNumber))
        {
            throw new Exception("The \"/\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = left as HulkNumber;
            HulkNumber b = right as HulkNumber;
            if (b.Value == 0d)
                throw new Exception("Cannot divide by 0");
            return new HulkNumber(a.Value / b.Value);
        }
    }
}
public class Module : BinaryFunction
{
    public Module(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
    }
    public override Variable Evaluate(Variable left, Variable right)
    {
        if (!(left is HulkNumber) || !(right is HulkNumber))
        {
            throw new Exception("The \"%\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = left as HulkNumber;
            HulkNumber b = right as HulkNumber;
            return new HulkNumber(a.Value % b.Value);
        }
    }
}
public class Power : BinaryFunction
{
    public Power(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
    }

    public override Variable Evaluate(Variable left, Variable right)
    {
        if (!(left is HulkNumber) || !(right is HulkNumber))
        {
            throw new Exception("The \"^\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = left as HulkNumber;
            HulkNumber b = right as HulkNumber;
            return new HulkNumber(Math.Pow(a.Value, b.Value));
        }
    }
}
public class Logarithm : BinaryFunction
{
    public Logarithm(string expression, HulkExpression leftArgument, HulkExpression rightArgument) : base(expression, leftArgument, rightArgument)
    {
    }
    public override Variable Evaluate(Variable left, Variable right)
    {
        if (!(left is HulkNumber) || !(right is HulkNumber))
        {
            throw new Exception("The \"log()\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = left as HulkNumber; //base
            HulkNumber b = right as HulkNumber; //value
            return new HulkNumber(Math.Log(b.Value, a.Value));
        }
    }
}
public class SquaredRoot : UnaryFunction
{
    public SquaredRoot(string expression, HulkExpression Arg) : base(expression, Arg)
    {
    }

    public override Variable Evaluate(Variable arg)
    {
        if (!(arg is HulkNumber))
        {
            throw new Exception("The \"sqrt()\" can only take a number as argument");
        }
        else
        {
            HulkNumber  a = arg as HulkNumber;
            return new HulkNumber(Math.Sqrt(a.Value));
        }
    }
}
public class Sine : UnaryFunction
{
    public Sine(string expression, HulkExpression Arg) : base(expression, Arg)
    {
    }

    public override Variable Evaluate(Variable arg)
    {
        if (!(arg is HulkNumber))
        {
            throw new Exception("The \"sin()\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = arg as HulkNumber;
            return new HulkNumber(Math.Sin(a.Value));
        }
    }
}
public class Cosine : UnaryFunction
{
    public Cosine(string expression, HulkExpression Arg) : base(expression, Arg)
    {
    }
    public override Variable Evaluate(Variable arg)
    {
        if (!(arg is HulkNumber))
        {
            throw new Exception("The \"cos()\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = arg as HulkNumber;
            return new HulkNumber(Math.Cos(a.Value));
        }
    }
}
public class ERaised : UnaryFunction
{
    public ERaised(string expression, HulkExpression Arg) : base(expression, Arg)
    {
    }
    public override Variable Evaluate(Variable arg)
    {
        if (!(arg is HulkNumber))
        {
            throw new Exception("The \"exp()\" can only take a number as argument");
        }
        else
        {
            HulkNumber a = arg as HulkNumber;
            return new HulkNumber(Math.Exp(a.Value));
        }
    }
}
public class Rand : HulkExpression
{
    public Rand(string expression)
    {
        Expression = expression;
        Random random = new Random();
        Value = new HulkNumber(random.NextDouble());
    }
}
