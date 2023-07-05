using Hulk;
using System;

public class NumVariable : HulkExpression
{
    public NumVariable(double val)
    {
        Value = val;
    }
}
public class Addition : BinaryFunction
{
    public Addition(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (!(left is double) || !(right is double))
        {
            throw new Exception("The \"+\" can only take a number as argument");
        }
        else
        {
            double a = (double)left;
            double b = (double)right;
            return a + b;
        }
    }
}
public class Subtraction: BinaryFunction
{
    public Subtraction(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (!(left is double) || !(right is double))
        {
            throw new Exception("The \"-\" can only take a number as argument");
        }
        else
        {
            double a = (double)left;
            double b = (double)right;
            return a - b;
        }
    }
}
public class Multiplication : BinaryFunction
{
    public Multiplication(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (!(left is double) || !(right is double))
        {
            throw new Exception("The \"+\" can only take a number as argument");
        }
        else
        {
            double a = (double)left;
            double b = (double)right;
            return a * b;
        }
    }
}
public class Division : BinaryFunction
{
    public Division(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (!(left is double) || !(right is double))
        {
            throw new Exception("The \"/\" can only take a number as argument");
        }
        else
        {
            double a = (double)left;
            double b = (double)right;
            if (b == 0d)
                throw new Exception("Cannot divide by 0");
            return a / b;
        }
    }
}
public class Module : BinaryFunction
{
    public Module(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }
    public override object Evaluate(object left, object right)
    {
        if (!(left is double) || !(right is double))
        {
            throw new Exception("The \"%\" can only take a number as argument");
        }
        else
        {
            double a = (double)left;
            double b = (double)right;
            return a % b;
        }
    }
}
public class Power : BinaryFunction
{
    public Power(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }

    public override object Evaluate(object left, object right)
    {
        if (!(left is double) || !(right is double))
        {
            throw new Exception("The \"^\" can only take a number as argument");
        }
        else
        {
            double a = (double)left;
            double b = (double)right;
            return Math.Pow(a, b);
        }
    }
}
public class Logarithm : BinaryFunction
{
    public Logarithm(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
    {
    }
    public override object Evaluate(object left, object right)
    {
        if (!(left is double) || !(right is double))
        {
            throw new Exception("The \"log()\" can only take a number as argument");
        }
        else
        {
            double a = (double)left; //base
            double b = (double)right; //value
            return Math.Log(b, a);
        }
    }
}
public class SquaredRoot : UnaryFunction
{
    public SquaredRoot(HulkExpression Arg) : base(Arg)
    {
    }

    public override object Evaluate(object arg)
    {
        if (!(arg is double))
        {
            throw new Exception("The \"sqrt()\" can only take a number as argument");
        }
        else
        {
            double a = (double)arg;
            return Math.Sqrt(a);
        }
    }
}
public class Sine : UnaryFunction
{
    public Sine(HulkExpression Arg) : base(Arg)
    {
    }

    public override object Evaluate(object arg)
    {
        if (!(arg is double))
        {
            throw new Exception("The \"sin()\" can only take a number as argument");
        }
        else
        {
            double a = (double)arg;
            return Math.Sin(a);
        }
    }
}
public class Cosine : UnaryFunction
{
    public Cosine(HulkExpression Arg) : base(Arg)
    {
    }
    public override object Evaluate(object arg)
    {
        if (!(arg is double))
        {
            throw new Exception("The \"cos()\" can only take a number as argument");
        }
        else
        {
            double a = (double)arg;
            return Math.Cos(a);
        }
    }
}
public class ERaised : UnaryFunction
{
    public ERaised(HulkExpression Arg) : base(Arg)
    {
    }
    public override object Evaluate(object arg)
    {
        if (!(arg is double))
        {
            throw new Exception("The \"exp()\" can only take a number as argument");
        }
        else
        {
            double a = (double)arg;
            return Math.Exp(a);
        }
    }
}
public class Rand : HulkExpression
{
    public Rand(string expression)
    {
        Random random = new Random();
        Value = random.NextDouble();
    }
}
