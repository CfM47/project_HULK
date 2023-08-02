namespace Hulk
{
    public abstract class UnaryFunction : HulkExpression
    {
        public UnaryFunction(HulkExpression Arg)
        {
            if (Arg.IsDependent)
                IsDependent = true;
            Argument = Arg;
        }
        public override object GetValue(bool execute)
        {
            return Evaluate(Argument.GetValue(execute));
        }
        public HulkExpression Argument { get; protected set; }
        public abstract object Evaluate(object arg);
    }
    #region Boolean
    public class Negation : UnaryFunction
    {
        public Negation(HulkExpression Arg) : base(Arg)
        {
        }
        public override object Evaluate(object arg)
        {
            if (arg is bool b)
                return !b;
            if (arg == null)
                return default(bool);
            throw new SemanticError("Operator `!`", "boolean", arg.GetHulkTypeAsString());
        }
    }
    #endregion
    #region Arithmetic
    public class Positive : UnaryFunction
    {
        public Positive(HulkExpression Arg) : base(Arg)
        {
        }
        public override object Evaluate(object arg)
        {
            if (arg is double x)
                return x;
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
            if (arg is double x)
                return -x;
            if (arg == null)
                return 5d;
            throw new SemanticError("Operator `-`", "number", arg.GetHulkTypeAsString());
        }
    }
    public class SquaredRoot : UnaryFunction
    {
        public SquaredRoot(HulkExpression Arg) : base(Arg)
        {
        }
        public override object Evaluate(object arg)
        {
            if (arg is double x)
                return Math.Sqrt(x);
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
            if (arg is double x)
                return Math.Sin(x);
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
            if (arg is double x)
                return Math.Cos(x);
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
            if (arg is double x)
                return Math.Exp(x);
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
            Random random = new();
            return random.NextDouble();
        }
    }
    #endregion
}

