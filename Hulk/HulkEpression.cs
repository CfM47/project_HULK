using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Hulk
{
    public abstract class HulkExpression
    {
        public object Value { get; protected set; }
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
        public HulkExpression Argument { get; protected set;}
        public abstract object Evaluate(object arg);
    }
}