using Hulk;
using System;

public enum Types { Void, number, boolean, hstring , dynamic}
public class IfElseStatement : HulkExpression
{
    public IfElseStatement(HulkExpression Cond, HulkExpression IfExp, HulkExpression ElseExp)
    {
        if(Cond.IsDependent || IfExp.IsDependent || ElseExp.IsDependent)
            IsDependent = true;
        Condition = Cond;
        IfExpression = IfExp;
        ElseExpression = ElseExp;
        
    }
    public override object GetValue(bool execute)
    {
        return Result(Condition, IfExpression, ElseExpression, execute);
    }
    public HulkExpression Condition { get; protected set; }
    public HulkExpression IfExpression { get; protected set; }
    public HulkExpression ElseExpression { get; protected set; }
    private object Result(HulkExpression Cond, HulkExpression IfExp, HulkExpression ElseExp, bool execute)
    {
        if (Cond.GetValue(execute) is not bool)
            throw new SemanticError("if-else condition", "boolean", Cond.GetValue(execute).GetHulkTypeAsString());
        else
        {
            var condition = (bool)Cond.GetValue(execute);
            if (condition)
                return IfExp.GetValue(execute);
            if (IfExp != null)
                if (ElseExp == null)
                    return null;
            return ElseExp.GetValue(execute);
        }
    }
}
public class VariableDeclaration : HulkDeclaration
{
    public VariableDeclaration(List<string> names, string type, HulkExpression ValueExp)
    {
        Names = names;
        SetType(type);
        bool valueOk = ValueExp == null? true : IsOkValue(ValueExp.GetValue(false));
        if (!valueOk)
            throw new SemanticError("Variable declaration", type, ValueExp.GetValue(false).GetHulkTypeAsString());
        ValueExpression = ValueExp;
    }
    public VariableDeclaration(List<string> names, HulkExpression ValueExp)
    {
        Names = names;        
        SetType(ValueExp.GetValue(false));
        ValueExpression = ValueExp;
    }
    public HulkExpression ValueExpression { get; private set; }
    public object Value { get => ValueExpression == null? null : ValueExpression.GetValue(false); set { }}
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
    private void SetType(object value) 
    {
        if (value is double)
            Type = Types.number;
        else if (value is bool)
            Type = Types.boolean;
        else if (value is string)
            Type = Types.hstring;
        else if (value == null)
            Type = Types.dynamic;
        else
            throw new Exception();
    }
    private bool IsOkValue(object val)
    {
        bool okNumber = (val is double) && (Type == Types.number);
        bool okBoolean = (val is bool) && (Type == Types.boolean);
        bool okString = (val is string) && (Type == Types.hstring);
        if (okNumber || okBoolean || okString || val == null)
            return true;
        return false;
    }
    public override void AddToMemory(Memory Memoria)
    {
        foreach (string name in Names)
        {
            var options = Variable.VariableOptions.InitializedVariable;
            Variable newVar;
            if (ValueExpression == null)
            {
                options = Variable.VariableOptions.NonInitialized;
                newVar = new Variable(name, null, Type, options);

            }
            else
                newVar = new Variable(name, ValueExpression.GetValue(false), Type, options);
            Memoria.AddNewVariable(name, newVar);
        }
    }

    public List<string> Names { get; }
    public Types Type { get; private set; }
}
public class FunctionDeclaration : HulkDeclaration
{
    public FunctionDeclaration(string name, List<string> argNames)
    {
        FunctionName = name;
        ArgumentNames = argNames;
        Arguments = new Dictionary<string, Variable>();
        SetArgs(ArgumentNames);
    }
    public object Evaluate(List<HulkExpression> Args, bool execute)
    {
        if (Args.Count != Arguments.Count)
            throw new SemanticError($"Function `{FunctionName}`", $"{Arguments.Count} argument(s)", $"{ Args.Count } argument(s)");
        else 
        {
            List<object> OldValues = new List<object>();
            for (int i = 0; i < Args.Count; i++)
            {
                string key = ArgumentNames[i];
                Variable v = Arguments[key];
                OldValues.Add(v.Value);
                v.Value = Args[i].GetValue(false);
            }
            var result = Definition.GetValue(execute);
            for(int i = 0; i < OldValues.Count; i++)
            {
                string key = ArgumentNames[i];
                Variable v = Arguments[key];
                v.Value = OldValues[i];
            }
            return result;
        }        
    }
    public void Define(HulkExpression definition)
    {
        Definition = definition;
        CheckDefinition();
    }
    public void CheckDefinition()
    {
        try
        {
            Definition.GetValue(false);
        }
        catch (SemanticError ex)
        {
            throw ex;
        }
    }
    private void SetArgs(List<string> argNames)
    {        
        foreach (string arg in argNames)
        {
            object val = default;
            Arguments.Add(arg, new Variable(arg, val, Types.dynamic, Variable.VariableOptions.FunctionArgument));
        }
    }
    public override void AddToMemory(Memory Memoria)
    {
        Memoria.AddNewFunction(this.FunctionName, this);
    }
    public List<string> ArgumentNames { get; }
    public string FunctionName { get; private set; }
    public Dictionary<string, Variable> Arguments { get; private set; }
    public HulkExpression Definition { get; private set; }
}
public class LetInStatement : HulkExpression
{
    public LetInStatement(Dictionary<string, Variable> Variables)
    {
        StoredVariables = Variables;
    }
    public override object GetValue(bool execute)
    {
        return Body.GetValue(execute);
    }
    public void Define(HulkExpression Definition)
    {
        Body = Definition;
    }
    public Dictionary<string, Variable> StoredVariables { get; private set;}
    public HulkExpression Body { get; private set; }
}
