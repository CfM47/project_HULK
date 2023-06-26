using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Hulk
{
    public abstract class HulkExpression
    {
        public string Expression { get; protected set; }
        public Variable Value { get; protected set; }
    }
    public abstract class BinaryFunction : HulkExpression
    {
        public BinaryFunction(string expression, HulkExpression leftArgument, HulkExpression rightArgument)
        {
            Expression = expression;
            LeftArgument = leftArgument;
            RightArgument = rightArgument;
            Value = Evaluate(LeftArgument.Value, RightArgument.Value);            
        }
        public HulkExpression LeftArgument { get;}
        public HulkExpression RightArgument { get;}
        public abstract Variable Evaluate(Variable left, Variable right);
    }
    public abstract class UnaryFunction : HulkExpression
    {
        public UnaryFunction(string expression, HulkExpression Arg)
        {
            Expression = expression;
            Argument = Arg;
            Value = Evaluate(Arg.Value);
        }
        public HulkExpression Argument { get;}
        public abstract Variable Evaluate(Variable arg);
    }
}