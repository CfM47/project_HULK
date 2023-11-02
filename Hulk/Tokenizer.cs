using System.Text.RegularExpressions;

namespace Hulk;

/// <summary>
/// Clase estatica que contiene metodos utiles para el tratamiento de tokens
/// </summary>
public static class Tokenizer
{
    /// <summary>
    /// Realiza el analisis lexico de una cadena
    /// </summary>
    /// <param name="entry">Cadena a la que se le va a hacer el analisis lexico</param>
    /// <returns>Arreglo de tokens necesarios para que la expresion tenga significado</returns>
    public static string[] GetTokens(string entry)
    {
        Regex pattern = new(@";|(\u0022([^\u0022\\]|\\.)*\u0022)|@{2}|@|\(|\+|-|\*|/|%|,|(=>)|(<=)|(>=)|(<)|(>)|:=|={2}|=|(!=)|\^|&|\||!|\)|[^@\(\)\+\-\*/\^%<>:=!&\|,;\s]+");
        MatchCollection collection = pattern.Matches(entry);
        string[] tokens = new string[collection.Count];
        for (int i = 0; i < tokens.Length; i++)
            tokens[i] = collection[i].Value;
        return tokens;
    }
    /// <summary>
    /// Obtiene una lista de las instrucciones separadas por ';' de un arreglo de tokens
    /// </summary>
    /// <param name="inputTokens">Arreglo de tokens a tratar</param>
    /// <returns>Lista de arreglos de tokens de expresiones</returns>
    /// <exception cref="DefaultError"></exception>
    public static List<string[]> GetInstructions(string[] inputTokens)
    {
        List<string[]> result = new();
        if (inputTokens[^1] != ";")
            throw new DefaultError("code lines must end with a semicolon");
        int start = 0;
        for (int i = 0; i < inputTokens.Length; i++)
        {
            if (inputTokens[i] == ";")
            {
                string[] instruction = new string[i - start];
                for (int j = 0; j < i - start; j++)
                {
                    instruction[j] = inputTokens[j + start];
                }
                result.Add(instruction);
                start = i + 1;
            }
        }
        return result;
    }
    /// <summary>
    /// Devuelve los tokens separados por comas de un arreglo de tokens
    /// </summary>
    /// <param name="tokens">Arreglo de tokens</param>
    /// <param name="start">Puntero que indica el inicio</param>
    /// <param name="end">Puntero que indica el final</param>
    /// <returns>Lista de tokens</returns>
    /// <exception cref="DefaultError"></exception>
    /// <exception cref="SyntaxError"></exception>
    public static List<string> GetCommaSeparatedTokens(string[] tokens, int start, int end)
    {
        List<string> result = new();
        if (tokens[start] == "," || tokens[end] == ",")
            throw new DefaultError("incorrect comma separation");
        for (int i = start; i <= end; i++)
        {
            if (tokens[i] != ",")
            {
                if (i % 2 == 1 || start == end)
                    result.Add(tokens[i]);
                else
                    throw new SyntaxError(",", "expression");
            }
        }
        return result;
    }
    /// <summary>
    /// Devuelve el indice anterior al de un token especificado
    /// </summary>
    /// <param name="tokens">Arreglo de tokens</param>
    /// <param name="start">Puntero que indica el inicio</param>
    /// <param name="end">Puntero que indica el final</param>
    /// <param name="delimiter">Token que especifica que se llego al limite</param>
    /// <returns>Indice del limite de una expresion</returns>
    public static int GetNameLimit(string[] tokens, int start, int end, string delimiter)
    {
        int result = start;
        for (int i = start; i < end; i++)
        {
            if (tokens[i + 1] == delimiter)
                break;
            result++;
        }
        return result;
    }
    /// <summary>
    /// Devuelve el indice de un parentesis que cierra correspondiente al parentesis que abre en un indice especificado
    /// </summary>
    /// <param name="index">Indice del parentesis que abre</param>
    /// <param name="end">Puntero que indica el final</param>
    /// <param name="tokens">Arreglo de tokens</param>
    /// <returns>Posicion del parentesis cerrado correspondiente</returns>
    /// <exception cref="SyntaxError"></exception>
    public static int GoToNextParenthesis(int index, int end, string[] tokens)
    {
        int parenthesisCount = 0;
        for (int i = index; i <= end; i++)
        {
            switch (tokens[i])
            {
                case "(":
                    parenthesisCount++;
                    break;
                case ")":
                    parenthesisCount--;
                    if (parenthesisCount == 0)
                    {
                        return i;
                    }
                    break;
            }
        }
        throw new SyntaxError(")", "expression");
    }
    /// <summary>
    /// Devuelve el indice de un parentesis que abre correspondiente al parentesis que cierra en un indice especificado
    /// </summary>
    /// <param name="index">Indice del parentesis que cierra</param>
    /// <param name="start">Puntero que indica el inicio</param>
    /// <param name="tokens">Arreglo de tokens</param>
    /// <returns>Posicion del parentesis abierto correspondiente</returns>
    /// <exception cref="SyntaxError"></exception>
    public static int GoToPreviousParenthesis(int index, int start, string[] tokens)
    {
        int parenthesisCount = 0;
        for (int i = index; i >= start; i--)
        {
            switch (tokens[i])
            {
                case ")":
                    parenthesisCount++;
                    break;
                case "(":
                    parenthesisCount--;
                    if (parenthesisCount == 0)
                    {
                        return i;
                    }
                    break;
            }
        }
        throw new SyntaxError("(", "expression");
    }
}
