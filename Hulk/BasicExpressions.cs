namespace Hulk;

public class Asignment : HulkExpression
{
    public Asignment(List<Variable> Vars, HulkExpression ValueExp)
    {
        Variables = Vars;
        CheckValue(ValueExp);
        ValueExpression = ValueExp;
    }
    #region Methods
    private void CheckValue(HulkExpression ValueExp)
    {
        Types type = ValueExp.CheckType();
        foreach (Variable v in Variables)
        {
            if (v.Type != type)
                throw new DefaultError($"Cannot asign value of type `{type}` to `{v.Type}` variable", "asignment");
        }
    }
    private void ChangeValues()
    {
        var val = ValueExpression.GetValue(false);
        foreach (Variable v in Variables)
        {
            v.Value = val;
            v.Options = Variable.VariableOptions.InitializedVariable;
        }
    }
    public override object GetValue(bool execute)
    {
        if (execute)
            ChangeValues();
        return ValueExpression.GetValue(execute);
    }
    public override Types CheckType() => ValueExpression.CheckType();
    #endregion
    #region Propierties
    public List<Variable> Variables { get; protected set; }
    public HulkExpression ValueExpression { get; protected set; }
    #endregion
}
public class FunctionCall : HulkExpression
{
    public FunctionCall(string name, List<HulkExpression> Args, FunctionDeclaration Def)
    {
        foreach (var arg in Args)
        {
            if (arg.IsDependent)
                IsDependent = true;
        }
        Name = name;
        CheckArgs(Args);
        Arguments = Args;
        Definition = Def;
    }
    #region Methods
    private void CheckArgs(List<HulkExpression> Args)
    {
        foreach (var arg in Args)
        {
            arg.CheckType();
        }
    }
    public override object GetValue(bool execute)
    {
        try
        {
            return Definition.Evaluate(Arguments, execute);
        }
        catch (SemanticError ex)
        {
            throw new SemanticError($"Function `{Name}`", ex.ExpressionExpected, ex.ExpressionReceived);
        }
    }

    public override Types CheckType() => Definition.ReturnedType;
    #endregion
    #region Properties
    public string Name { get; protected set; }
    public List<HulkExpression> Arguments { get; protected set; }
    public FunctionDeclaration Definition { get; protected set; }
    #endregion
}
public class PrintFunc : HulkExpression
{
    public PrintFunc(HulkExpression Arg, Print printHandler)
    {
        Argument = Arg;
        PrintHandler = printHandler;
    }
    public override object GetValue(bool execute)
    {
        if (execute)
            PrintHandler(Argument.GetValue(execute));
        return Argument.GetValue(false);
    }
    public override Types CheckType() => Argument.CheckType();
    Print PrintHandler;
    public HulkExpression Argument { get; }

}
public class Variable : HulkExpression
{
    #region Constructors
    //constructor para cuando te encuentras numeros, valores de verdad o strings en lo salvaje
    public Variable(object value)
    {
        Value = value;
        Options = VariableOptions.Value;
        SetType();
    }
    //constructor para las declaraciones de variable sin tipo
    public Variable(string name, object value)
    {
        Name = name;
        Value = value;
        Options = VariableOptions.InitializedVariable;
        SetType();
    }
    //constructor para las declaraciones de variable con tipo
    public Variable(string name, object value, Types type, VariableOptions options)
    {
        Name = name;
        Options = options;
        if (options is VariableOptions.Dependent or VariableOptions.FunctionArgument)
            IsDependent = true;
        Types enteredType;
        if (value is HulkExpression expression)
        {
            enteredType = expression.CheckType();
        }
        else if (value is double)
            enteredType = Types.number;
        else if (value is bool)
            enteredType = Types.number;
        else if (value is string)
            enteredType = Types.hstring;
        else
            enteredType = Types.dynamic;
        if(type == enteredType || type == Types.dynamic || enteredType == Types.dynamic)
        {
            Value = value;
            Type = type;
        }
        else
            throw new SemanticError($"Variable `{Name}`", $"{Type}", enteredType.ToString());

    }
    #endregion
    #region Methods
    public override object GetValue(bool execute)
    {
        return Options switch
        {
            VariableOptions.NonInitialized => throw new DefaultError("Use of unasigned variable"),
            VariableOptions.FunctionArgument => Value,
            _ => IsDependent ? ((HulkExpression)Value).GetValue(execute) : Value
        }; 
    }

    private void SetType()
    {
        if (Value is double)
            Type = Types.number;
        else if (Value is bool)
            Type = Types.boolean;
        else if (Value is string)
            Type = Types.hstring;
        //else
        //    Type = null;
    }
    public override Types CheckType() => Type;
    #endregion
    #region Properties
    public string? Name { get; protected set; }
    public Types Type { get; protected set; }
    public enum VariableOptions { Value, InitializedVariable, NonInitialized, FunctionArgument, Dependent }
    public VariableOptions Options { get; set; }
    public object Value { get; set; }
    #endregion
}
