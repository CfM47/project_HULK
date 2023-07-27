using Hulk;
public enum Types { number, boolean, hstring , dynamic}
public class IfElseStatement : HulkExpression
{
    public IfElseStatement(HulkExpression Cond, HulkExpression IfExp, HulkExpression ElseExp)
    {
        Condition = Cond;
        IfExpression = IfExp;
        ElseExpression = ElseExp;
        
    }
    public override object GetValue()
    {
        return Result(Condition, IfExpression, ElseExpression);
    }
    public HulkExpression Condition { get; protected set; }
    public HulkExpression IfExpression { get; protected set; }
    public HulkExpression ElseExpression { get; protected set; }
    private object Result(HulkExpression Cond, HulkExpression IfExp, HulkExpression ElseExp)
    {
        if (Cond.GetValue() is not bool)
            throw new Exception("Boolean expression expected");
        else
        {
            var condition = (bool)Cond.GetValue();
            if (condition)
                return IfExp.GetValue();
            if (IfExp != null)
                if (ElseExp == null)
                    return null;
            return ElseExp.GetValue();
        }
    }
}
public class VariableDeclaration : HulkExpression
{
    public VariableDeclaration(List<string> names, string type, HulkExpression ValueExp)
    {
        Names = names;
        SetType(type);
        //SetValue(ValueExp.GetValue());
        ValueExpression = ValueExp;
    }
    public VariableDeclaration(List<string> names, HulkExpression ValueExp)
    {
        Names = names;
        if (ValueExp is Variable)
            Type = ((Variable)ValueExp).Type;
        else
            SetType(ValueExp.GetValue());
        ValueExpression = ValueExp;
        //SetValue(ValueExp.GetValue());
    }
    public HulkExpression ValueExpression { get; private set; }
    public object Value { get; set; } 
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
            throw new Exception();
        else
            throw new Exception();
    }
    private void SetValue(object val)
    {
        switch (Type)
        {
            case Types.number:
                if (val is double)
                    Value = val;
                else
                    throw new Exception();
                break;
            case Types.boolean:
                if (val is bool)
                    Value = val;
                else
                    throw new Exception();
                break;
            case Types.hstring:
                if (val is string)
                    Value = val;
                else
                    throw new Exception();
                break;
            case Types.dynamic:
                Value = val;
                break;
            default:
                throw new Exception();
        }
    }
    public override object GetValue()
    {
        return ValueExpression.GetValue(); ;
    }
    public List<string> Names { get; }
    public Types Type { get; private set; }
}
public class FunctionDeclaration : HulkExpression
{
    public FunctionDeclaration(string name, List<string> argNames)
    {
        FunctionName = name;
        ArgumentNames = argNames;
        Arguments = new Dictionary<string, Variable>();
        SetArgs(ArgumentNames);
    }
    public override object GetValue()
    {
        return null;
    }
    public object Evaluate(List<HulkExpression> Args)
    {
        if (Args.Count != Arguments.Count)
            throw new Exception();
        else 
        {
            List<object> OldValues = new List<object>();
            for (int i = 0; i < Args.Count; i++)
            {
                string key = ArgumentNames[i];
                Variable v = Arguments[key];
                OldValues.Add(v.Value);
                v.Value = Args[i].GetValue();
            }
            var result = Definition.GetValue();
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
    }
    private void SetArgs(List<string> argNames)
    {
        foreach (string arg in argNames)
            Arguments.Add(arg, new Variable(arg, default, Types.dynamic));
    }
    public List<string> ArgumentNames { get; }
    public string FunctionName { get; private set; }
    public Dictionary<string, Variable> Arguments { get; private set; }
    public HulkExpression Definition { get; private set; }
}
public class LetInStatement : HulkExpression
{
    public LetInStatement(List<Dictionary<string, Variable>> Storage, Dictionary<string, Variable> Variables)
    {
        VariableStorage = Storage;
        VariableStorage.Add(Variables);
    }
    public override object GetValue()
    {
        return Body.GetValue();
    }
    public void Define(HulkExpression Definition)
    {
        Body = Definition;
    }
    public List<Dictionary<string, Variable>> VariableStorage { get; private set;}
    public HulkExpression Body { get; private set; }
}
