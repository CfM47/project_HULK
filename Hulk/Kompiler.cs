namespace Hulk;

/// <summary>
/// Representa un compilador de HULK. Instancias de esta clase se encargaran juntar los distintos procesos de compilacion
/// </summary>
public class Kompiler
{
    /// <summary>
    /// Construye un compilador de HULK
    /// </summary>
    /// <param name="print">Funcion que se encargara de imprimir</param>
    public Kompiler(Print print)
    {
        Memory = new();
        Parser = new(Memory, print);
        Handler = print;
    }
    /// <summary>
    /// Ejecuta una entrada escrita en HULK
    /// </summary>
    /// <param name="input">Entrada escrita en HULK</param>
    /// <exception cref="InstrucctionError"></exception>
    public void Compile(string input)
    {
        if (input == null || input.Length == 0)
            return;
        string[] s = Tokenizer.GetTokens(input);
        List<string[]> Instructions;
        try
        {
            Instructions = Tokenizer.GetInstructions(s);
        }
        catch (Exception ex)
        {
            Handler(ex.Message);
            return;
        }
        for (int i = 0; i < Instructions.Count; i++)
        {
            string[] instruction = Instructions[i];
            if (instruction.Length == 0)
                continue;
            try
            {
                try
                {
                    HulkExpression exp = Parser.Parse(instruction);
                    if (exp is FunctionDeclaration Dec)
                    {
                        Dec.AddToMemory(Memory);
                    }
                    else
                    {
                        exp.CheckType();
                        exp.GetValue(true);
                    }
                }
                catch (HulkException ex)
                {
                    throw new InstrucctionError(ex, i + 1, Instructions.Count);
                }
            }
            catch (Exception ex)
            {
                Handler(ex.Message);
            }
        }
    }
    /// <summary>
    /// Limpia la memoria de funciones 
    /// </summary>
    public void Clear()
    {
        Memory = new();
        Parser = new(Memory, Handler);
    }
    /// <summary>
    /// Memoria donde se guardaran las funciones 
    /// </summary>
    public HulkMemory Memory { get; private set; }
    /// <summary>
    /// Funcion que se encargara de imprimir en consola
    /// </summary>
    Print Handler;
    /// <summary>
    /// Objeto que se encaargara de parsear las instrucciones
    /// </summary>
    HulkParser Parser;

}
