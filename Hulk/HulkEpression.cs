namespace Hulk
{
    public abstract class HulkExpression
    {
        public object? Value { get; protected set; }
    }
    public abstract class BinaryFunction : HulkExpression
    {
        public BinaryFunction(HulkExpression leftArgument, HulkExpression rightArgument)
        {
            LeftArgument = leftArgument;
            RightArgument = rightArgument;
            Value = Evaluate(LeftArgument.Value, RightArgument.Value);
        }
        public HulkExpression LeftArgument { get; protected set; }
        public HulkExpression RightArgument { get; protected set; }
        public abstract object Evaluate(object left, object right);
    }
    public abstract class UnaryFunction : HulkExpression
    {
        public UnaryFunction(HulkExpression Arg)
        {
            Argument = Arg;
            Value = Evaluate(Arg.Value);
        }
        public HulkExpression Argument { get; protected set; }
        public abstract object Evaluate(object arg);
    }
    public class Variable : HulkExpression
    {
        public Variable(object value)
        {
            Value = value;
            SetType();
        }
        public Variable(string name, object value)
        {
            Name = name;
            Value = value;
            SetType();
        }
        public string? Name { get; protected set; }
        private void SetType()
        {
            if (Value is double)
                Type = Types.number;
            else if (Value is bool)
                Type = Types.boolean;
            else if (Value is string)
                Type = Types.hstring;
            else
                Type = null;
        }
        public Types? Type { get; protected set; }
    }
}