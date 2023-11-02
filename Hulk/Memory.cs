namespace Hulk;

/// <summary>
/// Representa un memoria de HULK. Instancias de esta clase se encargaran de guardar las funciones
/// </summary>
public class HulkMemory
{
    /// <summary>
    /// Construye un memoria de HULK
    /// </summary>
    public HulkMemory() => FunctionsStorage = new Dictionary<string, FunctionDeclaration>();
    /// <summary>
    /// Agrega una nueva funcion a la memoria
    /// </summary>
    /// <param name="key">Nombre de la funcion</param>
    /// <param name="Val">Declaracion de la funcion</param>
    /// <exception cref="DefaultError"></exception>
    public void AddNewFunction(string key, FunctionDeclaration Val)
    {
        if (!FunctionsStorage.TryAdd(key, Val))
            throw new DefaultError($"Function {key} already exist");
    }
    /// <summary>
    /// Lugar de la memoria donde se guardan las funciones
    /// </summary>
    public Dictionary<string, FunctionDeclaration> FunctionsStorage { get; }
}