namespace Hulk;

/// <summary>
/// Representa la declaracion de variables. Expresion del tipo [tipo?] [id] = [expresion]
/// </summary>
public class VariableDeclaration : HulkExpression
{
    #region Constructors
    /// <summary>
    /// Constructor para una declaracion de variable con tipo explicito
    /// </summary>
    /// <param name="names">Nombre de las variables que se van a declarar</param>
    /// <param name="type">Tipo de las variables que se van a declarar</param>
    /// <param name="ValueExp">Valor que tomaran las variables</param>
    /// <exception cref="SemanticError"></exception>
    public VariableDeclaration(List<string> names, string type, HulkExpression ValueExp)
    {
        Names = names;
        SetType(type);
        Types enteredType = ValueExp.CheckType();
        if(!(Type == enteredType || enteredType == Types.dynamic))
            throw new SemanticError("Variable declaration", Type.ToString(), enteredType.ToString());
        ValueExpression = ValueExp;
    }
    /// <summary>
    /// Constructor para una declaracion de variable con tipo explicito
    /// </summary>
    /// <param name="names">Nombre de las variables que se van a declarar</param>
    /// <param name="ValueExp">Valor que tomaran las variables</param>
    public VariableDeclaration(List<string> names, HulkExpression ValueExp)
    {
        Names = names;
        Type = ValueExp.CheckType();
        ValueExpression = ValueExp;
    }
    #endregion
    #region Methods
    /// <summary>
    /// Define el tipo de las variables que se declaran
    /// </summary>
    /// <param name="type">Tipo de las variables declaradas</param>
    /// <exception cref="DefaultError"></exception>
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
    /// <summary>
    /// Nombres de las variables
    /// </summary>
    public List<string> Names { get; }
    /// <summary>
    /// Expresion que define el valor de las variables
    /// </summary>
    public HulkExpression ValueExpression { get; private set; }
    /// <summary>
    /// Valor de las variables
    /// </summary>
    public object Value { get => ValueExpression?.GetValue(false); set { } }
    /// <summary>
    /// Tipo de las variables que se declaran
    /// </summary>
    public Types Type { get; private set; }
    #endregion
}
/// <summary>
/// Representa una declaraion de funcion inline. Expresion del tipo function [id] ([argumentos]) => [definicion] 
/// </summary>
public class FunctionDeclaration : HulkExpression
{
    /// <summary>
    /// Construye un objeto que representa una declaracion de funcion
    /// </summary>
    /// <param name="name">Nombre de la funcion que se declara</param>
    /// <param name="argNames">Nombres de los argumentos</param>
    public FunctionDeclaration(string name, List<string> argNames)
    {
        FunctionName = name;
        ArgumentNames = argNames;
        Arguments = new Dictionary<string, Variable>();
        SetArgs(ArgumentNames);
        ReturnedType = Types.dynamic;
    }
    #region Methods
    /// <summary>
    /// Numero de veces que se ha llamado la funcion actualmente
    /// </summary>
    int stackNumber = 0;
    /// <summary>
    /// Evalua la funcion
    /// </summary>
    /// <param name="Args">Lista de las expresiones que dan valores a los argumentos</param>
    /// <param name="execute"></param>
    /// <returns></returns>
    /// <exception cref="SemanticError"></exception>
    /// <exception cref="OverFlowError"></exception>
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
    /// <summary>
    /// Asigna la expresion que define al cuerpo de la funcion
    /// </summary>
    /// <param name="definition"></param>
    public void Define(HulkExpression definition)
    {
        Definition = definition;
        ReturnedType = CheckDefinition();
    }
    /// <summary>
    /// Aplica el chequeo de tipos a la expresion que define la funcion
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DefaultError"></exception>
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
    /// <summary>
    /// Crea objetos que representan las variables de los argumentos
    /// </summary>
    /// <param name="argNames">Lista de los nombres de los argumentos de la funcion</param>
    /// <exception cref="DefaultError"></exception>
    private void SetArgs(List<string> argNames)
    {
        foreach (string arg in argNames)
        {
            object val = default;
            if (!Arguments.TryAdd(arg, new Variable(arg, val, Types.dynamic, Variable.VariableOptions.FunctionArgument)))
                throw new DefaultError("Function arguments must have diferent names", "declaration");
        }
    }
    /// <summary>
    /// Áñade la funcion a una memoria
    /// </summary>
    /// <param name="Memoria"></param>
    public void AddToMemory(HulkMemory Memoria) => Memoria.AddNewFunction(FunctionName, this);
    public override object GetValue(bool execute) => new EmptyReturn();
    public override Types CheckType() => Types.Void;
    #endregion
    #region Properties
    /// <summary>
    /// Lista de nombres de los argumentos
    /// </summary>
    public List<string> ArgumentNames { get; }
    /// <summary>
    /// Nombre de la funcion que se declara
    /// </summary>
    public string FunctionName { get; private set; }
    /// <summary>
    /// Lugar donde se guardan las variables que representan los argumentos de la funcion
    /// </summary>
    public Dictionary<string, Variable> Arguments { get; private set; }
    /// <summary>
    /// Expresion que define la funcion
    /// </summary>
    public HulkExpression Definition { get; private set; }
    /// <summary>
    /// Tipo de retorno de la funcion
    /// </summary>
    public Types ReturnedType { get; private set; }
    #endregion
}