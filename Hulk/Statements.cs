namespace Hulk;

public class IfElseStatement : HulkExpression
{
    public IfElseStatement(HulkExpression Cond, HulkExpression IfExp, HulkExpression ElseExp)
    {
        if (Cond.IsDependent || IfExp.IsDependent || ElseExp.IsDependent)
            IsDependent = true;
        Condition = Cond;
        IfExpression = IfExp;
        ElseExpression = ElseExp;

    }
    #region Methods
    public override object GetValue(bool execute) => Result(Condition, IfExpression, ElseExpression, execute);
    public override Types CheckType()
    {
        Condition.CheckType();
        Types ifType = IfExpression.CheckType();
        Types elseType = ElseExpression.CheckType();
        return ifType == elseType ? ifType : Types.dynamic;
    }
    private object Result(HulkExpression Cond, HulkExpression IfExp, HulkExpression ElseExp, bool execute)
    {
        if (Cond.GetValue(execute) is not bool)
            throw new SemanticError("if-else condition", "boolean", Cond.GetValue(execute).GetHulkTypeAsString());
        else
        {
            bool condition = (bool)Cond.GetValue(execute);
            if (condition)
                return IfExp.GetValue(execute);
            return ElseExp.GetValue(execute);
        }
    }
    #endregion
    #region Properties
    public HulkExpression Condition { get; protected set; }
    public HulkExpression IfExpression { get; protected set; }
    public HulkExpression ElseExpression { get; protected set; }
    #endregion
}
public class LetInStatement : HulkExpression
{
    public LetInStatement(Dictionary<string, Variable> Variables) => StoredVariables = Variables;
    #region Methods
    public override object GetValue(bool execute)
    {
        CheckValues();
        return Body.GetValue(execute);
    }
    private void CheckValues()
    {
        //arreglar
        foreach (Variable V in StoredVariables.Values)
        {
            
            object val;
            if (V.IsDependent)
            {
                bool isOK = false;
                HulkExpression? Exp = V.Value as HulkExpression;
                val = Exp.GetValue(false);
                if (val != null)
                {
                    if (V.Type == Types.boolean && val is bool)
                        isOK = true;
                    else if (V.Type == Types.hstring && val is string)
                        isOK = true;
                    else if (V.Type == Types.number && val is double)
                        isOK = true;
                    else if (V.Type == Types.dynamic)
                        isOK = true;
                }
                if (!isOK)
                    throw new SemanticError("Let-in expression", V.Type.ToString(), val.GetHulkTypeAsString());
            }            
        }
    }
    public override Types CheckType() => Body.CheckType();
    public void Define(HulkExpression Definition) => Body = Definition;
    #endregion
    #region Properties
    public Dictionary<string, Variable> StoredVariables { get; private set; }
    public HulkExpression Body { get; private set; }
    #endregion
}