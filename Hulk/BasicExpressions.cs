namespace Hulk;

/// <summary>
/// Clase que representa la expresion de asignación destructiva. Expresiones de la forma [variable] = [expresion]
/// </summary>
public class Asignment : HulkExpression
{
    /// <summary>
    /// Constructor de un objeto que representa a la operacion de asignación destructiva
    /// </summary>
    /// <param name="Vars">Lista de variables  al las que se les van a cambiar los valores</param>
    /// <param name="ValueExp">Expresion que será asignada</param>
    public Asignment(List<Variable> Vars, HulkExpression ValueExp)
    {
        Variables = Vars;
        CheckValue(ValueExp);
        ValueExpression = ValueExp;
    }
    #region Methods
    /// <summary>
    /// Chequea si los valores de la expresion son del mismo tipo que las variables
    /// </summary>
    /// <param name="ValueExp">Valor de la expresion a asignar</param>
    /// <exception cref="DefaultError"></exception>
    private void CheckValue(HulkExpression ValueExp)
    {
        HulkTypes type = ValueExp.CheckType();
        foreach (Variable v in Variables)
        {
            if (v.Type != type)
                throw new DefaultError($"Cannot asign value of type `{type}` to `{v.Type}` variable", "asignment");
        }
    }
    /// <summary>
    /// Cambia los valores de las variables
    /// </summary>
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
    public override HulkTypes CheckType() => ValueExpression.CheckType();
    #endregion
    #region Propierties
    /// <summary>
    /// Variables a las que se les esta cambiando el valor
    /// </summary>
    public List<Variable> Variables { get; protected set; }
    /// <summary>
    /// Expresion del valor que se va a cambiar
    /// </summary>
    public HulkExpression ValueExpression { get; protected set; }
    #endregion
}
/// <summary>
/// Representa los llamados a funciones espresiones de la forma [id] ([expresion], [expresion]...)
/// </summary>
public class FunctionCall : HulkExpression
{
    /// <summary>
    /// Constructor de una expresion que representa un llamado a funcion 
    /// </summary>
    /// <param name="name">Nombre de la funcion que se esta llamando</param>
    /// <param name="Args">Lista de los argumentos de la funcion</param>
    /// <param name="Def">Referencia al lugar en memoria donde se define la funcion</param>
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
    /// <summary>
    /// Ejecuta el chequeo de tipos sobre las expresiones que representan los argumentos de la funcion
    /// </summary>
    /// <param name="Args">Lista de argumentos de la funcion</param>
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
    public override HulkTypes CheckType() => Definition.ReturnedType;
    #endregion
    #region Properties
    /// <summary>
    /// Nombre de la funcion a la que sea llama
    /// </summary>
    public string Name { get; protected set; }
    /// <summary>
    /// Lista de expresiones que representan a los argumentos
    /// </summary>
    public List<HulkExpression> Arguments { get; protected set; }
    /// <summary>
    /// Definicion de la funcion
    /// </summary>
    public FunctionDeclaration Definition { get; protected set; }
    #endregion
}
/// <summary>
/// Clase que representa a la funcion de imprimir en consola. Expresion del tipo print([expresion])
/// </summary>
public class PrintFunc : HulkExpression
{
    /// <summary>
    /// Constructor de un objeto que representa un llamado a la funcion print()
    /// </summary>
    /// <param name="Arg">Argumento de la funcion</param>
    /// <param name="printHandler">Funcion encargada de mostrar la salida en la interfaz</param>
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
    public override HulkTypes CheckType() => Argument.CheckType();
    /// <summary>
    /// Funcion encargada de mostrar la salida en la interfaz
    /// </summary>
    Print PrintHandler;
    /// <summary>
    /// Expresion a imprimir
    /// </summary>
    public HulkExpression Argument { get; }

}
/// <summary>
/// Clase que representa a una variable o literal
/// </summary>
public class Variable : HulkExpression
{
    #region Constructors
    ///<sumary>
    ///Constructor para literales
    ///<\sumary>
    public Variable(object value)
    {
        Value = value;
        Options = VariableOptions.Value;
        SetType();
    }
    /// <summary>
    /// Constructor para una variable declarada sin tipo
    /// </summary>
    /// <param name="name">Nombre de la variable</param>
    /// <param name="value">Valor de la variable</param>
    public Variable(string name, object value)
    {
        Name = name;
        Value = value;
        Options = VariableOptions.InitializedVariable;
        SetType();
    }
    /// <summary>
    /// Constructor para las variables declaradas explicitamente con el tipo
    /// </summary>
    /// <param name="name">Nombre de la variable</param>
    /// <param name="value">Valor de la vriable</param>
    /// <param name="type">Tipo de la variable</param>
    /// <param name="options">Opciones para la declaracion de la variable</param>
    /// <exception cref="SemanticError"></exception>
    public Variable(string name, object value, HulkTypes type, VariableOptions options)
    {
        Name = name;
        Options = options;
        if (options is VariableOptions.Dependent or VariableOptions.FunctionArgument)
            IsDependent = true;
        HulkTypes enteredType;
        if (value is HulkExpression expression)
        {
            enteredType = expression.CheckType();
        }
        else if (value is double)
            enteredType = HulkTypes.number;
        else if (value is bool)
            enteredType = HulkTypes.number;
        else if (value is string)
            enteredType = HulkTypes.hstring;
        else
            enteredType = HulkTypes.Undetermined;
        if(type == enteredType || type == HulkTypes.Undetermined || enteredType == HulkTypes.Undetermined)
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
    /// <summary>
    /// Desgina el tipo de la variable a partir de su valor
    /// </summary>
    private void SetType()
    {
        if (Value is double)
            Type = HulkTypes.number;
        else if (Value is bool)
            Type = HulkTypes.boolean;
        else if (Value is string)
            Type = HulkTypes.hstring;
        //else
        //    Type = null;
    }
    public override HulkTypes CheckType() => Type;
    #endregion
    #region Properties
    /// <summary>
    /// Nombre de la variable
    /// </summary>
    public string? Name { get; protected set; }
    /// <summary>
    /// Tipo de la variable
    /// </summary>
    public HulkTypes Type { get; protected set; }
    /// <summary>
    /// Opciones para establecer caracteristicas de las variable
    /// </summary>
    public enum VariableOptions { Value, InitializedVariable, NonInitialized, FunctionArgument, Dependent }
    /// <summary>
    /// Caracteristicas de la variable
    /// </summary>
    public VariableOptions Options { get; set; }
    /// <summary>
    /// Valor de la variable
    /// </summary>
    public object Value { get; set; }
    #endregion
}
