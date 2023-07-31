namespace Hulk
{
    public class Positive : UnaryFunction
    {
        public Positive(HulkExpression Arg) : base(Arg)
        {
        }
        public override object Evaluate(object arg)
        {
            if (arg is not double)
                throw new SemanticError("Operator `+`", "`number`", arg.GetType().Name);
                
            return arg;
        }
    }
    public class Negative : UnaryFunction 
    {
        public Negative(HulkExpression Arg) : base(Arg)
        {
        }
        public override object Evaluate(object arg)
        {
            if (arg is not double)
                throw new SemanticError("Operator `-`", "`number`", arg.GetType().Name);
            return -(double)arg;
        }
    }
    public class Addition : BinaryFunction
    {
        public Addition(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
        {
        }

        public override object Evaluate(object left, object right)
        {
            bool bothNumber = (left is double) && (right is double);
            bool bothString = (left is string) && (right is string);
            bool bothNull = (left is null) && (right is null);
            bool case1 = (left is null) && (right is double);
            bool case2 = (left is double) && (right is null);
            if (bothNumber)
            {
                double a = (double)left;
                double b = (double)right;
                return a + b;
            }
            if (bothString)
            {
                string a = (string)left;
                string b = (string)right;
                return a + b;
            }
            if (bothNull || case1 || case2)
                return (dynamic)left + (dynamic)right;
            throw new Exception($"Cannot perform operation `+` between `{left.GetType().Name}` and `{right.GetType().Name}`");
        }
    }
    public class Subtraction : BinaryFunction
    {
        public Subtraction(HulkExpression leftArgument, HulkExpression rightArgument) : base(leftArgument, rightArgument)
        {
        }
        public override object Evaluate(object left, object right)
        {
            if (!(left is double) || !(right is double))
            {
                var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
                throw new SemanticError("Operator `-`", "number", conflictiveType);
            }
            double a = (double)left;
            double b = (double)right;
            return a - b;
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
                var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
                throw new SemanticError("Operator `*`", "number", conflictiveType);
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
                var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
                throw new SemanticError("Operator `/`", "number", conflictiveType);
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
                var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
                throw new SemanticError("Operator `%`", "number", conflictiveType);
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
                var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
                throw new SemanticError("Operator `^`", "number", conflictiveType);
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
                var conflictiveType = !(left is double) ? left.GetType().Name : right.GetType().Name;
                throw new SemanticError("Function `log`", "number", conflictiveType);
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
                throw new SemanticError("Function `sqrt()`", "number", arg.GetType().Name);
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
                throw new SemanticError("Function `sin()`", "number", arg.GetType().Name);
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
                throw new SemanticError("Function `cos()`", "number", arg.GetType().Name);
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
                throw new SemanticError("Function `sqrt()`", "number", arg.GetType().Name);
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