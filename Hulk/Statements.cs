using Hulk;
using System;
using System.Diagnostics.CodeAnalysis;

public class IfElseStatement : HulkExpression
{
    IfElseStatement(string expression, HulkExpression Cond, HulkExpression IfExp, HulkExpression ElseExp)
    {
        Expression = expression;
        Condtion = Cond;
        IfExpression = IfExp;
        ElseExpression = ElseExp;
        Value = Result(Cond, IfExp, ElseExp);
    }
    public HulkExpression Condtion { get; protected set; }
    public HulkExpression IfExpression { get; protected set; }
    public HulkExpression ElseExpression { get; protected set; }
    private Variable Result(HulkExpression Cond, HulkExpression IfExp, HulkExpression ElseExp)
    {
        if(Cond.Value is not HulkBoolean)
            throw new Exception("Boolean expression expected");
        else
        {
            var condition = Cond.Value as HulkBoolean;
            if (condition.Value)
                return IfExp.Value;
            return ElseExp.Value;
        }
    }
}
