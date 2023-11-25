namespace Hulk;

/// <summary>
/// Representa todas las expresiones del lenguaje HULK
/// </summary>
public abstract class HulkExpression
{
    /// <summary>
    /// Metodo que devuelve el valor de la expresion
    /// </summary>
    /// <param name="execute">Indica si se debe ejecutar o no una accion al devolver el valor, como immprimir algo o cambiar algun valor</param>
    /// <returns>Valor de la expresion</returns>
    public abstract object GetValue(bool execute);
    /// <summary>
    /// Chequea que no existan errores semanticos en la expresion y devuelve el tipo de retorno de la misma
    /// </summary>
    /// <returns>Tipo de retorno de la expresion</returns>
    public abstract HulkTypes CheckType();
    /// <summary>
    /// Especificacion de si la expresion depende de un parametro (argumento de funcion) o no
    /// </summary>
    public bool IsDependent { get; protected set; }
}
/// <summary>
/// Funcion delegada que se encargara de imprimir en consola
/// </summary>
/// <param name="input"></param>
public delegate void Print(object input);
/// <summary>
/// Metodo que deduce el tipo que tendria en HULK un objeto
/// </summary>
public static class ExtObject
{
    public static string GetHulkTypeAsString(this object arg)
    {
        Type type = arg.GetType();
        if (type == typeof(double))
            return "number";
        else if (type == typeof(bool))
            return "boolean";
        else if (type == typeof(string))
            return "string";
        return type == typeof(EmptyReturn) ? "void" : "type";
    }
}
/// <summary>
/// Objeto que representa un retorno vacio
/// </summary>
public class EmptyReturn
{
    public EmptyReturn() { }
}
/// <summary>
/// Tipos de los valores que se pueden devolver en HULK
/// </summary>
public enum HulkTypes { Void, number, boolean, hstring, Undetermined }