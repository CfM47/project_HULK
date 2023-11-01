namespace Hulk;

public class VariableDeclaration : HulkExpression
{
    #region Constructors
    public VariableDeclaration(List<string> names, string type, HulkExpression ValueExp)
    {
        Names = names;
        SetType(type);
        Types enteredType = ValueExp.CheckType();
        if(!(Type == enteredType || enteredType == Types.dynamic))
            throw new SemanticError("Variable declaration", Type.ToString(), enteredType.ToString());
        ValueExpression = ValueExp;
    }
    public VariableDeclaration(List<string> names, HulkExpression ValueExp)
    {
        Names = names;
        Type = ValueExp.CheckType();
        ValueExpression = ValueExp;
    }
    #endregion
    #region Methods
    private void SetType(string type)
    {
        Type = type switch
        {
            "number" => Types.number,
            "boolean" => Types.boolean,
            "string" => Types.hstring,
            _ => throw new DefaultError("Invalid variable type"),
        };
    }
    public override object GetValue(bool execute) => new EmptyReturn();
    public override Types CheckType() => Types.Void;
    #endregion
    #region Properties
    public List<string> Names { get; }
    public HulkExpression ValueExpression { get; private set; }
    public object Value { get => ValueExpression?.GetValue(false); set { } }
    public Types Type { get; private set; }
    #endregion
}
public class FunctionDeclaration : HulkExpression
{
    public FunctionDeclaration(string name, List<string> argNames)
    {
        FunctionName = name;
        ArgumentNames = argNames;
        Arguments = new Dictionary<string, Variable>();
        SetArgs(ArgumentNames);
        ReturnedType = Types.dynamic;
    }
    #region Methods
    int stackNumber = 0;
    public object Evaluate(List<HulkExpression> Args, bool execute)
    {
        if (Args.Count != Arguments.Count)
            throw new SemanticError($"Function `{FunctionName}`", $"{Arguments.Count} argument(s)", $"{Args.Count} argument(s)");
        else
        {
            if (stackNumber > HulkInfo.StackLimit)
                throw new OverFlowError(FunctionName);
            else
                stackNumber++;
            List<object> OldValues = new();
            for (int i = 0; i < Args.Count; i++)
            {
                string key = ArgumentNames[i];
                Variable v = Arguments[key];
                OldValues.Add(v.Value);
                v.Value = Args[i].GetValue(false);
            }
            object result = Definition.GetValue(execute);
            stackNumber--;
            for (int i = 0; i < OldValues.Count; i++)
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
        ReturnedType = CheckDefinition();
    }
    public Types CheckDefinition()
    {
        try
        {
            if (stackNumber > HulkInfo.StackLimit)
                throw new OverFlowError(FunctionName);
            else
                stackNumber++;
            Types result = Definition.CheckType();
            stackNumber--;
            return result;
        }
        catch (SemanticError ex)
        {
            throw ex;
        }
        catch (OverFlowError)
        {
            throw new DefaultError($"Function '{FunctionName}' may reach call stack limit (callstack limit is {HulkInfo.StackLimit})", "function");
        }
    }
    private void SetArgs(List<string> argNames)
    {
        foreach (string arg in argNames)
        {
            object val = default;
            if (!Arguments.TryAdd(arg, new Variable(arg, val, Types.dynamic, Variable.VariableOptions.FunctionArgument)))
                throw new DefaultError("Function arguments must have diferent names", "declaration");
        }
    }
    public void AddToMemory(HulkMemory Memoria) => Memoria.AddNewFunction(FunctionName, this);
    public override object GetValue(bool execute) => new EmptyReturn();
    public override Types CheckType() => Types.Void;
    #endregion
    #region Properties
    public List<string> ArgumentNames { get; }
    public string FunctionName { get; private set; }
    public Dictionary<string, Variable> Arguments { get; private set; }
    public HulkExpression Definition { get; private set; }
    public Types ReturnedType { get; private set; }
    #endregion
}