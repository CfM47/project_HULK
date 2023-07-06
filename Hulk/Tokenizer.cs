using Hulk;
using System.Text.RegularExpressions;

public class Tokenizer
{
    public Tokenizer()
    {
    }
    public string[] GetTokens(string entry)
    {
        Regex pattern = new Regex(@"(\(|\+|-|\*|/|%|(<=)|(>=)|(<)|(>)|={2}|={1}|(!=)|\^|&{2}|\|{2}|!|\)|[^\(\)\+\-\*/\^%<>=!&|\s]+)");
        MatchCollection collection = pattern.Matches(entry);
        string[] tokens = new string[collection.Count];
        for (int i = 0; i < tokens.Length; i++)
            tokens[i] = collection[i].Value;
        return tokens;
    }
    public HulkExpression Parse(string[] tokens, int start, int end)
    {
        if (tokens.Length == 0)
            return null;
        HulkExpression expr = null;
        if (tokens[start] == "if")
            expr = ParseIfElseStatement(tokens, start, end);
        if (expr == null)
            expr = ParseBoolean(tokens, start, end);
        if (expr == null)
            expr = ParseArithmetic(tokens, start, end);
        return expr;
    }
    #region If-Else parsing
    private HulkExpression ParseIfElseStatement(string[] tokens, int start, int end)
    {
        HulkExpression result = null;
        if (tokens[start] != "if")
            throw new Exception();
        else
        {
            HulkExpression condition = null;
            HulkExpression IfDo = null;
            HulkExpression ElseDo = null;
            int conditionEnd;
            if (tokens[start + 1] != "(")
                throw new Exception();
            else
            {
                conditionEnd = GoToNextParenthesis(start + 2, tokens);
                if (conditionEnd == end)
                    throw new Exception();
                condition = Parse(tokens, start + 1, conditionEnd);
                int ifDoEnd = GetIfDoEnd(tokens, start, end);
                if (ifDoEnd == end - 1)
                    throw new Exception();
                IfDo = Parse(tokens, conditionEnd + 1, ifDoEnd);
                if (ifDoEnd < end - 1)
                    ElseDo = Parse(tokens, ifDoEnd + 2, end);
                result = new IfElseStatement(condition, IfDo, ElseDo);
            }
        }
        return result;
    }
    private int GetIfDoEnd(string[] tokens, int start, int end)
    {
        for (int i = start; i <= end; i++)
        {
            if (tokens[i] == "else")
                return i - 1;
        }
        return end;
    }
    #endregion
    #region Boolean Expressions Parsing
    public HulkExpression ParseBoolean(string[] tokens, int start, int end)
    {
        HulkExpression result = null;
        result = TryBinaryFirstLevelBoolExpr(tokens, start, end);
        if (result == null)
            result = TryUnaryBooleanExpression(tokens, start, end);
        if (result == null)
            result = TryBinarySecondLevelExpr(tokens, start, end);
        return result;
    }
    private HulkExpression? TryBinaryFirstLevelBoolExpr(string[] tokens, int start, int end)
    {
        HulkExpression result = null;
        for (int i = start; i <= end; i++)
        {
            switch (tokens[i])
            {
                case "(":
                    {
                        i = GoToNextParenthesis(i + 1, tokens);
                        break;
                    }
                case "&&":
                    {
                        HulkExpression left = i != start ? ParseBoolean(tokens, start, i - 1) : throw new Exception();
                        HulkExpression right = i != end ? ParseBoolean(tokens, i + 1, end) : throw new Exception();
                        result = new Conjunction(left, right);
                        return result;
                    }
                case "||":
                    {
                        HulkExpression left = i != start ? ParseBoolean(tokens, start, i - 1) : throw new Exception();
                        HulkExpression right = i != end ? ParseBoolean(tokens, i + 1, end) : throw new Exception();
                        result = new Disjunction(left, right);
                        return result;
                    }
                case "==":
                    {
                        HulkExpression left = i != start ? Parse(tokens, start, i - 1) : throw new Exception();
                        HulkExpression right = i != end ? Parse(tokens, i + 1, end) : throw new Exception();
                        result = new Equal(left, right);
                        return result;
                    }
                case "!=":
                    {
                        HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1) : throw new Exception();
                        HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end) : throw new Exception();
                        result = new UnEqual(left, right);
                        return result;
                    }
                    
            }
        }
        return null;
    }
    private HulkExpression? TryBinarySecondLevelExpr(string[] tokens, int start, int end)
    {
        HulkExpression result = null;
        for (int i = start; i <= end; i++)
        {
            switch (tokens[i])
            {
                case "(":
                    i = GoToNextParenthesis(i + 1, tokens);
                    break;
                case "<":
                    {

                        HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                            : throw new Exception();
                        HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                            : throw new Exception();
                        result = new LowerThan(left, right);
                        return result;
                    }

                case ">":
                    {
                        HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                            : throw new Exception();
                        HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                            : throw new Exception();
                        result = new GreaterThan(left, right);
                        return result;
                    }

                case "<=":
                    {

                        HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                            : throw new Exception();
                        HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                            : throw new Exception();
                        result = new LowerOrEqualThan(left, right);
                        return result;
                    }

                case ">=":
                    {
                        HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                            : throw new Exception();
                        HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                            : throw new Exception();
                        result = new GreaterOrEqualThan(left, right);
                        return result;
                    }
            }
        }
        return null;
    }
    private HulkExpression? TryUnaryBooleanExpression(string[] tokens, int start, int end)
    {
        HulkExpression result = null;
        switch (tokens[start])
        {
            case "(":
                {
                    result = start != end - 1 ? ParseBoolean(tokens, start + 1, end - 1)
                            : throw new Exception();
                    break;
                }
            case "!":
                { 
                    result = start != end ? new Negation(ParseBoolean(tokens, start + 1, end))
                                    : throw new Exception();
                    break;
                }
            default:
                if (start == end)
                {

                    if (tokens[start] != "true" && tokens[start] != "false")
                    {
                        double maybeNum = 0;
                        if (double.TryParse(tokens[start], out maybeNum))
                            return null;
                        result = new BooleanVariable(true);
                        return result;
                    }
                    else
                    {
                        result = new BooleanVariable(bool.Parse(tokens[start]));
                        return result;
                    }
                }
                break;
        }
        return result;
    }
    #endregion
    #region Aritmethic Expression Parsing
    public HulkExpression ParseArithmetic(string[] tokens, int start, int end)
    {
        HulkExpression result = null;
        result = TryFirstLevelArithmeticExpr(tokens, start, end);
        if (result == null)
            result = TrySecondLevelArithmeticExpr(tokens, start, end);
        if (result == null)
            result = TryThirdLevelArithmeticExpr(tokens, start, end);
        if (result == null)
            result = TryArithmeticFunc(tokens, start, end);
        return result;
    }
    public HulkExpression? TryFirstLevelArithmeticExpr(string[] tokens, int start, int end)
    {
        HulkExpression result = null;
        for (int i = end; i >= start; i--)
        {
            switch (tokens[i])
            {
                case ")":
                    {
                        i = GoToPreviousParenthesis(i - 1, tokens);
                        break;
                    }
                case "+":
                    {
                        HulkExpression left = null;
                        if (i != start)
                        {
                            Regex regex = new Regex(@"(\(|\+|-|\*|/|%|=|\^)");
                            left = !regex.Match(tokens[i - 1]).Success ? ParseArithmetic(tokens, start, i - 1) : null;
                        }
                        else
                            left = new NumVariable(0);
                        HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end) : throw new Exception();
                        if(left != null && right != null)
                            result = new Addition(left, right);
                        return result;
                    }
                case "-":
                    {
                        HulkExpression left = null;
                        if (i != start)
                        {
                            Regex regex = new Regex(@"(\(|\+|-|\*|/|%|=|\^|)");
                            left = !regex.Match(tokens[i - 1]).Success ? ParseArithmetic(tokens, start, i - 1) : null;
                        }
                        else
                            left = new NumVariable(0);
                        HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end) : throw new Exception();
                        if (left != null && right != null)
                            result = new Subtraction(left, right);
                        return result;
                    }
            }
        }
        return null;
    }
    public HulkExpression? TrySecondLevelArithmeticExpr(string[] tokens, int start, int end)
    {
        HulkExpression result = null;
        for (int i = end; i >= start; i--)
        {
            switch (tokens[i])
            {
                case ")":
                    {
                        i = GoToPreviousParenthesis(i - 1, tokens);
                        break;
                    }
                case "*":
                    {
                        HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1) : throw new Exception();
                        HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end) : throw new Exception();
                        result = new Multiplication(left, right);
                        return result;
                    }
                case "/":
                    {
                        HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1) : throw new Exception();
                        HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end) : throw new Exception();
                        result = new Division(left, right);
                        return result;
                    }                    
                case "%":
                    {
                        HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1) : throw new Exception();
                        HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end) : throw new Exception();
                        result = new Module(left, right);
                        return result;
                    }                    
            }
        }
        return null;
    }
    public HulkExpression? TryThirdLevelArithmeticExpr(string[] tokens, int start, int end)
    {
        HulkExpression result = null;
        for (int i = start; i <= end; i++)
        {
            switch (tokens[i])
            {
                case "(":
                    {
                        i = GoToNextParenthesis(i + 1, tokens);
                        break;
                    }
                case "^":
                    {

                        HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                            : throw new Exception();
                        HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                            : throw new Exception();
                        result = new Power(left, right);
                        return result;
                    }
            }
        }
        return null;
    }
    public HulkExpression? TryArithmeticFunc(string[] tokens, int start, int end)
    {
        HulkExpression result = null;
        switch (tokens[start])
        {
            case "(":
                {
                    result = start != end - 1 ? ParseArithmetic(tokens, start + 1, end - 1) : throw new Exception();
                    return result;
                }                
            case "sqrt":
                {
                    if (tokens[start + 1] != "(" || tokens[end] != ")")
                        throw new Exception();
                    else
                        result = new SquaredRoot(ParseArithmetic(tokens, start + 1, end));
                    return result;
                }                
            case "sin":
                {
                    if (tokens[start + 1] != "(" || tokens[end] != ")")
                        throw new Exception();
                    else
                        result = new Sine(ParseArithmetic(tokens, start + 1, end));
                    return result;
                }                
            case "cos":
                {
                    if (tokens[start + 1] != "(" || tokens[end] != ")")
                        throw new Exception();
                    else
                        result = new Cosine(ParseArithmetic(tokens, start + 1, end));
                    return result;
                }                
            case "exp":
                {
                    if (tokens[start + 1] != "(" || tokens[end] != ")")
                        throw new Exception();
                    else
                        result = new ERaised(ParseArithmetic(tokens, start + 1, end));
                    return result;
                }                
        }
        if (start == end)
        {
            double x;
            bool isNumber = double.TryParse(tokens[start], out x);
            if (!isNumber)
            {
                result = new NumVariable(0d);
                return result;
            }
            else
            {
                result = new NumVariable(x);
                return result;
            }
        }
        return result;
    }
    #endregion
    private int GoToNextParenthesis(int index, string[] tokens)
    {
        for (int i = index; i < tokens.Length; i++)
        {
            switch (tokens[i])
            {
                case "(":
                    i = GoToNextParenthesis(i + 1, tokens);
                    break;
                case ")":
                    return i;
            }
        }
        throw new Exception("\")\" expected");
    }
    private int GoToPreviousParenthesis(int index, string[] tokens)
    {
        for (int i = index; i >= 0; i--)
        {
            switch (tokens[i])
            {
                case ")":
                    i = GoToPreviousParenthesis(i - 1, tokens);
                    break;
                case "(":
                    return i;
            }
        }
        throw new Exception("\"(\" expected");
    }
}
