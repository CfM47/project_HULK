namespace Hulk;

/// <summary>
/// Representa las expresiones condicionales. Expresiones del tipo if([condicion]) [expresion] else [expresion]
/// </summary>
public class IfElseStatement : HulkExpression
{
    /// <summary>
    /// Construye un objeto que representa una expresion condicional
    /// </summary>
    /// <param name="Cond">Expresion que representa la condicion</param>
    /// <param name="IfExp">Expresion que se ejecutara si se cumple la condicion</param>
    /// <param name="ElseExp">Expresion que se ejecutara si no se cumple la condicion</param>
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
    /// <summary>
    /// Funcion que retorna el valor resultante de evaluar la expresion
    /// </summary>
    /// <param name="Cond">Expresion que representa la condicion</param>
    /// <param name="IfExp">Expresion que se ejecutara si se cumple la condicion</param>
    /// <param name="ElseExp">Expresion que se ejecutara si no se cumple la condicion</param>
    /// <param name="execute">Valor booleano que indic si se ejecutaran instrucciones o solo se devolveran valores</param>
    /// <returns>Valor de retorno de la expresion condicional</returns>
    /// <exception cref="SemanticError"></exception>
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
    /// <summary>
    /// Expresion que representa la condicion
    /// </summary>
    public HulkExpression Condition { get; protected set; }
    /// <summary>
    /// Expresion que se ejecutara si se cumple la condicion
    /// </summary>
    public HulkExpression IfExpression { get; protected set; }
    /// <summary>
    /// Expresion que se ejecutara si no se cumple la condicion
    /// </summary>
    public HulkExpression ElseExpression { get; protected set; }
    #endregion
}
/// <summary>
/// Representa a las expresiones let-in. Expresiones del tipo let [declaraciones de variables sepaaradas por coma] in [expresion]
/// </summary>
public class LetInStatement : HulkExpression
{
    /// <summary>
    /// Construye una expresion let-in
    /// </summary>
    /// <param name="Variables">Variables locales que se declaran en la expresion</param>
    public LetInStatement(Dictionary<string, Variable> Variables) => StoredVariables = Variables;
    #region Methods
    public override object GetValue(bool execute)
    {
        CheckValues();
        return Body.GetValue(execute);
    }
    private void CheckValues()
    {
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
    /// <summary>
    /// Asigna el cuerpo de la expresion let-in
    /// </summary>
    /// <param name="Definition">Cuerpo de la expresion let-in</param>
    public void Define(HulkExpression Definition) => Body = Definition;
    #endregion
    #region Properties
    /// <summary>
    /// Variables del contexto de la expresion let-in
    /// </summary>
    public Dictionary<string, Variable> StoredVariables { get; private set; }
    /// <summary>
    /// Cuerpo de la expresion let-in 
    /// </summary>
    public HulkExpression Body { get; private set; }
    #endregion
}