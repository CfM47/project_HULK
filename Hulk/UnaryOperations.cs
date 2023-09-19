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

        public override Types CheckType()
        {
            var argType = Argument.CheckType();
            if(argType != Types.dynamic && argType != Types.boolean)
                throw new SemanticError("Operator `!`", "boolean", argType.ToString());
            return Types.boolean;
        }

        public override object Evaluate(object arg)
        {
            if (arg is bool b)
                return !b;
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
        public override Types CheckType()
        {
            var argType = Argument.CheckType();
            if (argType != Types.dynamic && argType != Types.number)
                throw new SemanticError("Operator `+`", "number", argType.ToString());
            return Types.number;
        }

        public override object Evaluate(object arg)
        {
            if (arg is double x)
                return x;
            throw new SemanticError("Operator `+`", "number", arg.GetHulkTypeAsString());
        }
    }
    public class Negative : UnaryFunction
    {
        public Negative(HulkExpression Arg) : base(Arg)
        {
        }

        public override Types CheckType()
        {
            var argType = Argument.CheckType();
            if (argType != Types.dynamic && argType != Types.number)
                throw new SemanticError("Operator `-`", "number", argType.ToString());
            return Types.number;
        }

        public override object Evaluate(object arg)
        {
            if (arg is double x)
                return -x;
            throw new SemanticError("Operator `-`", "number", arg.GetHulkTypeAsString());
        }
    }
    public class SquaredRoot : UnaryFunction
    {
        public SquaredRoot(HulkExpression Arg) : base(Arg)
        {
        }

        public override Types CheckType()
        {
            var argType = Argument.CheckType();
            if (argType != Types.dynamic && argType != Types.number)
                throw new SemanticError("Function `sqrt`", "number", argType.ToString());
            return Types.number;
        }

        public override object Evaluate(object arg)
        {
            if (arg is double x)
                return Math.Sqrt(x);
            throw new SemanticError("Function `sqrt`", "number", arg.GetHulkTypeAsString());
        }
    }
    public class Sine : UnaryFunction
    {
        public Sine(HulkExpression Arg) : base(Arg)
        {
        }

        public override Types CheckType()
        {
            var argType = Argument.CheckType();
            if (argType != Types.dynamic && argType != Types.number)
                throw new SemanticError("Function `sin`", "number", argType.ToString());
            return Types.number;
        }

        public override object Evaluate(object arg)
        {
            if (arg is double x)
                return Math.Sin(x);
            throw new SemanticError("Function `sin`", "number", arg.GetHulkTypeAsString());
        }
    }
    public class Cosine : UnaryFunction
    {
        public Cosine(HulkExpression Arg) : base(Arg)
        {
        }

        public override Types CheckType()
        {
            var argType = Argument.CheckType();
            if (argType != Types.dynamic && argType != Types.number)
                throw new SemanticError("Function `cos`", "number", argType.ToString());
            return Types.number;
        }

        public override object Evaluate(object arg)
        {
            if (arg is double x)
                return Math.Cos(x);
            throw new SemanticError("Function `cos`", "number", arg.GetHulkTypeAsString());
        }
    }
    public class ERaised : UnaryFunction
    {
        public ERaised(HulkExpression Arg) : base(Arg)
        {
        }

        public override Types CheckType()
        {
            var argType = Argument.CheckType();
            if (argType != Types.dynamic && argType != Types.number)
                throw new SemanticError("Function `exp`", "number", argType.ToString());
            return Types.number;
        }

        public override object Evaluate(object arg)
        {
            if (arg is double x)
                return Math.Exp(x);
            throw new SemanticError("Function `exp`", "number", arg.GetHulkTypeAsString());
        }
    }
    public class Rand : HulkExpression
    {
        public Rand()
        {
        }

        public override Types CheckType()
        {
            return Types.number;
        }

        public override object GetValue(bool execute)
        {
            Random random = new();
            return random.NextDouble();
        }
    }
    #endregion
}

