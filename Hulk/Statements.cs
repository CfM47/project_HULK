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
    }
}
public enum Types { number, boolean, hstring }
public class VariableDeclaration : HulkExpression
{
    public VariableDeclaration(List<string> names, string type, HulkExpression ValueExp)
    {
        Names = names;
        SetType(type);
        Value = ValueExp == null ? null : GetValue(ValueExp.Value);
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
    private object GetValue(object val)
    {
        switch (Type)
        {
            case Types.number:
                if (val is double)
                    return val;
                else
                    throw new Exception();
            case Types.boolean:
                if (val is bool)
                    return val;
                else
                    throw new Exception();
            case Types.hstring:
                if (val is string)
                    return val;
                else
                    throw new Exception();
            default :
                throw new Exception();
        }
    }
    public List<string> Names { get;}
    public Types Type { get; private set; }
}
public class FunctionDeclaration: HulkExpression
{
    public FunctionDeclaration(string name, List<string> argNames, HulkExpression DefinitionExp)
    {
        Value = null;
    }
    public string FunctionName { get; private set; }
    public List<HulkExpression> Arguments { get; private set; }
}
