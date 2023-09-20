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
        public Types ReturnedType { get; protected set; }
        public Types EnteredType { get; protected set; }
        public Type AcceptedType { get; protected set; }
        public string OperationToken { get; protected set; }
        public UnaryOperation Operation { get; protected set; }
        public delegate object UnaryOperation(object arg);
        public object Evaluate(object arg)
        {
            if (arg.GetType() == AcceptedType)
                return Operation(arg);
            throw new SemanticError($"Operator `{OperationToken}`", EnteredType.ToString(), arg.GetHulkTypeAsString());
        }
        public override Types CheckType()
        {
            var argType = Argument.CheckType();
            if (argType != Types.dynamic && argType != EnteredType)
                throw new SemanticError($"Operator `{OperationToken}`", EnteredType.ToString(), argType.ToString());
            return ReturnedType;
        }
    }
    #region Boolean
    public class Negation : UnaryFunction
    {
        public Negation(HulkExpression Arg) : base(Arg)
        {
            Argument = Arg;
            ReturnedType = Types.boolean;
            EnteredType = Types.boolean;
            AcceptedType = typeof(bool);
            OperationToken = "!";
            object func(object a) => !(bool)a; ;
            Operation = func;
        }
    }
    #endregion
    #region Arithmetic
    public class Positive : UnaryFunction
    {
        public Positive(HulkExpression Arg) : base(Arg)
        {
            Argument = Arg;
            ReturnedType = Types.number;
            EnteredType = Types.number;
            AcceptedType = typeof(double);
            OperationToken = "+";
            object func(object a) => a; ;
            Operation = func;
        }
    }
    public class Negative : UnaryFunction
    {
        public Negative(HulkExpression Arg) : base(Arg)
        {
            Argument = Arg;
            ReturnedType = Types.number;
            EnteredType = Types.number;
            AcceptedType = typeof(double);
            OperationToken = "-";
            object func(object a) => -(double)a; ;
            Operation = func;
        }
    }
    public class SquaredRoot : UnaryFunction
    {
        public SquaredRoot(HulkExpression Arg) : base(Arg)
        {
            Argument = Arg;
            ReturnedType = Types.number;
            EnteredType = Types.number;
            AcceptedType = typeof(double);
            OperationToken = "sqrt";
            object func(object a) => Math.Sqrt((double)a); ;
            Operation = func;
        }
    }
    public class Sine : UnaryFunction
    {
        public Sine(HulkExpression Arg) : base(Arg)
        {
            Argument = Arg;
            ReturnedType = Types.number;
            EnteredType = Types.number;
            AcceptedType = typeof(double);
            OperationToken = "sin";
            object func(object a) => Math.Sin((double)a); ;
            Operation = func;
        }
    }
    public class Cosine : UnaryFunction
    {
        public Cosine(HulkExpression Arg) : base(Arg)
        {
            Argument = Arg;
            ReturnedType = Types.number;
            EnteredType = Types.number;
            AcceptedType = typeof(double);
            OperationToken = "cos";
            object func(object a) => Math.Cos((double)a); ;
            Operation = func;
        }
    }
    public class ERaised : UnaryFunction
    {
        public ERaised(HulkExpression Arg) : base(Arg)
        {
            Argument = Arg;
            ReturnedType = Types.number;
            EnteredType = Types.number;
            AcceptedType = typeof(double);
            OperationToken = "exp";
            object func(object a) => Math.Exp((double)a); ;
            Operation = func;
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

