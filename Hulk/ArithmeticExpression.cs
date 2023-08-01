using System.Diagnostics.CodeAnalysis;

namespace Hulk
{
    public class Positive : UnaryFunction
    {
        public Positive(HulkExpression Arg) : base(Arg)
        {
        }
        public override object Evaluate(object arg)
        {
            if (arg is double)
                return (double)arg;
            if (arg == null)
                return 5d;
            throw new SemanticError("Operator `+`", "number", arg.GetHulkTypeAsString());
        }
    }
    public class Negative : UnaryFunction 
    {
        public Negative(HulkExpression Arg) : base(Arg)
        {
        }
        public override object Evaluate(object arg)
        {
            if (arg is double)
                return -(double)arg;
            if (arg == null)
                return 5d;
            throw new SemanticError("Operator `-`", "number", arg.GetHulkTypeAsString());
        }
    }
    public class Addition : BinaryFunction
    {
        public Addition(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
        {
        }
        public override object Evaluate(object left, object right)
        {
            string expected = "";
            if (left == null)
                left = right;
            if (right == null)
                right = left;
            if (left == null && right == null)
                return default;
            if ((left is double && right is double) || (left is string && right is string))
                return (dynamic)left + (dynamic)right;
            throw new OperationSemanticError("+", left.GetHulkTypeAsString(), right.GetHulkTypeAsString(), expected);
        }
    }
    public class Subtraction : BinaryFunction
    {
        public Subtraction(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
        {
        }
        public override object Evaluate(object left, object right)
        {
            if (left == null)
                left = right;
            if (right == null)
                right = left;
            if (left == null && right == null)
                return 5d;
            if ((left is double && right is double))
                return (dynamic)left - (dynamic)right;
            var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
            throw new SemanticError("Operator `-`", "number", conflictiveType);
        }
    }
    public class Multiplication : BinaryFunction
    {
        public Multiplication(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
        {
        }

        public override object Evaluate(object left, object right)
        {
            if (left == null)
                left = right;
            if (right == null)
                right = left;
            if (left == null && right == null)
                return 5d;
            if ((left is double && right is double))
                return (dynamic)left * (dynamic)right;
            var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
            throw new SemanticError("Operator `*`", "number", conflictiveType);
        }
    }
    public class Division : BinaryFunction
    {
        public Division(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
        {
        }
        public override object Evaluate(object left, object right)
        {
            if (left == null)
                left = right;
            if (right == null)
                right = left;
            if (left == null && right == null)
                return 5d;
            if ((left is double && right is double))
            {
                if ((double)right == 0)
                    throw new Exception("! ARITHMETIC ERROR : atempted to divide by 0");
                return (dynamic)left / (dynamic)right;
            }
            var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
            throw new SemanticError("Operator `/`", "number", conflictiveType);
        }
    }
    public class Module : BinaryFunction
    {
        public Module(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
        {
        }
        public override object Evaluate(object left, object right)
        {
            if (left == null)
                left = right;
            if (right == null)
                right = left;
            if (left == null && right == null)
                return 5d;
            if ((left is double && right is double)) 
            {
                if ((double)right == 0)
                    throw new Exception("! ARITHMETIC ERROR : atempted to divide by 0");
                return (dynamic)left % (dynamic)right;
            }
            var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
            throw new SemanticError("Operator `%`", "number", conflictiveType);
        }
    }
    public class Power : BinaryFunction
    {
        public Power(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
        {
        }
        public override object Evaluate(object left, object right)
        {
            if (left == null)
                left = right;
            if (right == null)
                right = left;
            if (left == null && right == null)
                return 5d;
            if ((left is double && right is double))
                return Math.Pow((dynamic)left,(dynamic)right);
            var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
            throw new SemanticError("Operator `^`", "number", conflictiveType);
        }
    }
    public class Logarithm : BinaryFunction
    {
        public Logarithm(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
        {
        }
        public override object Evaluate(object left, object right)
        {
            if (left == null)
                left = right;
            if (right == null)
                right = left;
            if (left == null && right == null)
                return 5d;
            if ((left is double && right is double))
                return Math.Log((dynamic)left, (dynamic)right);
            var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
            throw new SemanticError("Function `log`", "number", conflictiveType);
        }
    }
    public class SquaredRoot : UnaryFunction
    {
        public SquaredRoot(HulkExpression Arg) : base(Arg)
        {
        }
        public override object Evaluate(object arg)
        {
            if (arg is double)
                return Math.Sqrt((double)arg);
            if (arg == null)
                return 5d;
            throw new SemanticError("Function `sqrt`", "number", arg.GetHulkTypeAsString());
        }
    }
    public class Sine : UnaryFunction
    {
        public Sine(HulkExpression Arg) : base(Arg)
        {
        }
        public override object Evaluate(object arg)
        {
            if (arg is double)
                return Math.Sin((double)arg);
            if (arg == null)
                return 5d;
            throw new SemanticError("Function `sin`", "number", arg.GetHulkTypeAsString());
        }
    }
    public class Cosine : UnaryFunction
    {
        public Cosine(HulkExpression Arg) : base(Arg)
        {
        }
        public override object Evaluate(object arg)
        {
            if (arg is double)
                return Math.Cos((double)arg);
            if (arg == null)
                return 5d;
            throw new SemanticError("Function `cos`", "number", arg.GetHulkTypeAsString());
        }
    }
    public class ERaised : UnaryFunction
    {
        public ERaised(HulkExpression Arg) : base(Arg)
        {
        }
        public override object Evaluate(object arg)
        {
            if (arg is double)
                return Math.Exp((double)arg);
            if (arg == null)
                return 5d;
            throw new SemanticError("Function `exp`", "number", arg.GetHulkTypeAsString());
        }
    }
    public class Rand : HulkExpression
    {
        public Rand()
        {
        }
        public override object GetValue(bool execute)
        {
            Random random = new Random();
            return random.NextDouble();
        }
    }
}