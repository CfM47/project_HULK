using Hulk;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;

public class IfElseStatement : HulkExpression
{
    public IfElseStatement(HulkExpression Cond, HulkExpression IfExp, HulkExpression ElseExp)
    {
        Condtion = Cond;
        IfExpression = IfExp;
        ElseExpression = ElseExp;
        Value = Result(Cond, IfExp, ElseExp);
    }
    public HulkExpression Condtion { get; protected set; }
    public HulkExpression IfExpression { get; protected set; }
    public HulkExpression ElseExpression { get; protected set; }
    private object Result(HulkExpression Cond, HulkExpression IfExp, HulkExpression ElseExp)
    {
        if(Cond.Value is not bool)
            throw new Exception("Boolean expression expected");
        else
        {
            var condition = (bool)Cond.Value;
            if (condition)
                return IfExp.Value;
            if (IfExp != null)
                if (ElseExp == null)
                    return null;
                return ElseExp.Value;
        }
        return null;
    }
}
public enum Types { number, boolean, hstring }
public class VariableDeclaration : HulkExpression
{
    public VariableDeclaration(List<string> name, string type, HulkExpression ValueExp)
    {
        SetValue(ValueExp.Value);
    }
    private void SetType(string type)
    {
        switch (type)
        {
            case "number":
                Type = Types.number;
                break;
            case "boolean":
                Type = Types.boolean;
                break;
            case "string":
                Type = Types.hstring;
                break;
            default:
                throw new Exception();

        }
    }
    private void SetValue(object val)
    {
        if (Type == Types.number)
        {
            if (Value is double)
                Value = val;
            else
                throw new Exception();
        }
        else if (Type == Types.boolean)
        {
            if (Value is bool)
                Value = val;
            else
                throw new Exception();
        }
        else
        {
            if (Value is string)
                Value = val;
            else
                throw new Exception();
        }
    }
    public List<string> Name { get;}
    public Types Type { get; private set; }
}
