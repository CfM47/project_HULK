﻿namespace Hulk
{
    public abstract class HulkExpression
    {
        public abstract object GetValue();
    }
    public abstract class BinaryFunction : HulkExpression
    {
        public BinaryFunction(HulkExpression leftArgument, HulkExpression rightArgument)
        {
            LeftArgument = leftArgument;
            RightArgument = rightArgument;
        }
        public override object GetValue()
        {
            return Evaluate(LeftArgument.GetValue(), RightArgument.GetValue());
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

        }
        public override object GetValue()
        {
            return Evaluate(Argument.GetValue());
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
        public object Value { get; set; }
        public override object GetValue()
        {
            return Value;
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
    public class FunctionCall: HulkExpression
    {
        public FunctionCall(string name, List<HulkExpression> Args, FunctionDeclaration Def)
        {
            Name = name;
            Arguments = Args;
            Definition = Def;
        }
        public override object GetValue()
        {
            return Definition.Evaluate(Arguments);
        }
        public string Name { get; protected set; }
        public List<HulkExpression> Arguments { get; protected set; }
        public FunctionDeclaration Definition { get; protected set; }
    }
}