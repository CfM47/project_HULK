using Hulk;
using System;

public abstract class ArithmeticExpression : HulkExpression
{
    public double Value { get; protected set; }
}
public class Number : ArithmeticExpression
{
    public Number(string exp)
    {
        Expression = exp;
        Value = int.Parse(exp);
    }
}
public class NumVariable : ArithmeticExpression
{
    public NumVariable(string exp, double val)
    {
        Expression = exp;
        Value = val;
    }
}
