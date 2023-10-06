using System.Text.RegularExpressions;

namespace Hulk;

public static class Tokenizer
{
    public static string[] GetTokens(string entry)
    {
        Regex pattern = new(@";|(\u0022([^\u0022\\]|\\.)*\u0022)|@{2}|@|\(|\+|-|\*|/|%|,|(=>)|(<=)|(>=)|(<)|(>)|:=|={2}|=|(!=)|\^|&|\||!|\)|[^@\(\)\+\-\*/\^%<>:=!&\|,;\s]+");
        MatchCollection collection = pattern.Matches(entry);
        string[] tokens = new string[collection.Count];
        for (int i = 0; i < tokens.Length; i++)
            tokens[i] = collection[i].Value;
        return tokens;
    }
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
