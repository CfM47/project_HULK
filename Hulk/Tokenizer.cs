using Hulk;
using System;
using System.Text.RegularExpressions;

public class Tokenizer
{
	public Tokenizer()
	{
	}
	public string[] GetTokens(string entry)
	{
		Regex pattern = new Regex(@"(\(|\+|-|\*|%|(log)|(sqrt)|(sin)|(cos)|(exp)|(<=)|(>=)|(<)|(>)|={2}|(!=)|/|\^|&{2}|\|{2}|!|\)|(true)|(false)|[^\(\)\+\-\*%<>=!&|\s]+)");
		MatchCollection collection = pattern.Matches(entry);
		string[] tokens = new string[collection.Count];
		for(int i = 0; i < tokens.Length; i++)
			tokens[i] = collection[i].Value;
		return tokens;
	}
    public HulkExpression Parse(string[] tokens, int start, int end)
    {
        HulkExpression expr = null;
        expr = ParseBoolean(tokens, start, end);
        if (expr == null)
            expr = ParseArithmetic(tokens, start, end);
        return expr;
    }
	public HulkExpression ParseBoolean(string[] tokens, int start, int end)
	{
		HulkExpression result = null;
        result = TryBinaryFirstLevelBoolExpr(tokens, start, end);
        if(result == null)
            result = TryUnaryBooleanExpression(tokens, start, end);
		if(result == null)
            result = TryBinarySecondLevelExpr(tokens, start, end);
		return result;
	}
	private HulkExpression? TryBinaryFirstLevelBoolExpr(string[] tokens, int start, int end)
	{
		HulkExpression result = null;
        for (int i = start; i <= end; i++)
        {
            if (tokens[i] == "(")
            {
                i = GoToNetParenthesis(i + 1, tokens);
            }
            else if (tokens[i] == "&&")
            {

                HulkExpression left = i != start ? ParseBoolean(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? ParseBoolean(tokens, i + 1, end)
                    : throw new Exception();
                result = new Conjunction(tokens[i], left, right);
                return result;
            }
            else if (tokens[i] == "||")
            {
                HulkExpression left = i != start ? ParseBoolean(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? ParseBoolean(tokens, i + 1, end)
                    : throw new Exception();
                result = new Disjunction(tokens[i], left, right);
                return result;
            }
            else if (tokens[i] == "==")
            {

                HulkExpression left = i != start ? Parse(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? Parse(tokens, i + 1, end)
                    : throw new Exception();
                result = new Equal(tokens[i], left, right);
                return result;
            }
            else if (tokens[i] == "!=")
            {
                HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                    : throw new Exception();
                result = new UnEqual(tokens[i], left, right);
                return result;
            }
        }
		return null;
    }
	private HulkExpression? TryBinarySecondLevelExpr(string[] tokens, int start, int end)
	{
        HulkExpression result = null;
        for (int i = start; i <= end; i++)
        {
            if (tokens[i] == "(")
            {
                i = GoToNetParenthesis(i + 1, tokens);
            }
            else if (tokens[i] == "<")
            {

                HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                    : throw new Exception();
                result = new LowerThan(tokens[i], left, right);
                return result;
            }
            else if (tokens[i] == ">")
            {
                HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                    : throw new Exception();
                result = new GreaterThan(tokens[i], left, right);
                return result;
            }
            else if (tokens[i] == "<=")
            {

                HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                    : throw new Exception();
                result = new LowerOrEqualThan(tokens[i], left, right);
                return result;
            }
            else if (tokens[i] == ">=")
            {
                HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                    : throw new Exception();
                result = new GreaterOrEqualThan(tokens[i], left, right);
                return result;
            }
        }
        return null;
    }

	private HulkExpression? TryUnaryBooleanExpression(string[] tokens, int start, int end)
	{
        HulkExpression result = null;
        if (tokens[start] == "(")
			result = start != end - 1 ? ParseBoolean(tokens, start + 1, end - 1)
                : throw new Exception();
        else if (tokens[start] == "!")
            result = start != end ? new Negation(tokens[start], ParseBoolean(tokens, start + 1, end))
                : throw new Exception();
        else if (start == end)
        {
            if (tokens[start] != "true" && tokens[start] != "false")
            {
                result = new BooleanVariable(tokens[start], true);
                return result;
            }
            else
            {
                result = new BooleanVariable(tokens[start], bool.Parse(tokens[start]));
                return result;
            }
        }
        //else
        //    throw new Exception("Boolean operation expected");
        return result;
    }
	public HulkExpression ParseArithmetic(string[] tokens, int start, int end)
	{
        HulkExpression result = null;
        result = TryFirstLevelArithmeticExpr(tokens, start, end);
        if (result == null)
            result = TrySecondLevelArithmeticExpr(tokens, start, end);
        if (result == null)
            result = TryThirdLevelArithmeticExpr(tokens, start, end);
        if(result == null)
            result = TryArithmeticFunc(tokens, start, end);
        return result;
    }
    public HulkExpression? TryFirstLevelArithmeticExpr(string[] tokens, int start, int end)
    {
        HulkExpression result = null;
        for (int i = start; i <= end; i++)
        {
            if (tokens[i] == "(")
            {
                i = GoToNetParenthesis(i + 1, tokens);
            }
            else if (tokens[i] == "+")
            {

                HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                    : throw new Exception();
                result = new Addition(tokens[i], left, right);
                return result;
            }
            else if (tokens[i] == "-")
            {
                HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                    : throw new Exception();
                result = new Subtraction(tokens[i], left, right);
                return result;
            }
        }
        return null;
    }
    public HulkExpression? TrySecondLevelArithmeticExpr(string[] tokens,int start, int end)
    {
        HulkExpression result = null;
        for (int i = start; i <= end; i++)
        {
            if (tokens[i] == "(")
            {
                i = GoToNetParenthesis(i + 1, tokens);
            }
            else if (tokens[i] == "*")
            {

                HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                    : throw new Exception();
                result = new Multiplication(tokens[i], left, right);
                return result;
            }
            else if (tokens[i] == "/")
            {
                HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                    : throw new Exception();
                result = new Division(tokens[i], left, right);
                return result;
            }
            else if (tokens[i] == "%")
            {
                HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                    : throw new Exception();
                result = new Module(tokens[i], left, right);
                return result;
            }
        }
        return null;
    }
    public HulkExpression? TryThirdLevelArithmeticExpr(string[] tokens, int start, int end)
    {
        HulkExpression result = null;
        for (int i = start; i <= end; i++)
        {
            if (tokens[i] == "(")
            {
                i = GoToNetParenthesis(i + 1, tokens);
            }
            else if (tokens[i] == "^")
            {

                HulkExpression left = i != start ? ParseArithmetic(tokens, start, i - 1)
                    : throw new Exception();
                HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end)
                    : throw new Exception();
                result = new Power(tokens[i], left, right);
                return result;
            }
        }
        return null;
    }
    public HulkExpression? TryArithmeticFunc(string[] tokens, int start, int end)
    {
        HulkExpression result = null;
        if (tokens[start] == "(")
            result = start != end - 1 ? ParseArithmetic(tokens, start + 1, end - 1)
                : throw new Exception();
        else if (tokens[start] == "sqrt")
        {
            if (tokens[start + 1] != "(" || tokens[end] != ")")
                throw new Exception();
            else
                result = new SquaredRoot(tokens[start], ParseArithmetic(tokens, start + 1, end));
        }
        else if (tokens[start] == "sin")
        {
            if (tokens[start + 1] != "(" || tokens[end] != ")")
                throw new Exception();
            else
                result = new Sine(tokens[start], ParseArithmetic(tokens, start + 1, end));
        }
        else if (tokens[start] == "cos")
        {
            if (tokens[start + 1] != "(" || tokens[end] != ")")
                throw new Exception();
            else
                result = new Cosine(tokens[start], ParseArithmetic(tokens, start + 1, end));
        }
        else if (tokens[start] == "exp")
        {
            if (tokens[start + 1] != "(" || tokens[end] != ")")
                throw new Exception();
            else
                result = new SquaredRoot(tokens[start], ParseArithmetic(tokens, start + 1, end));
        }
        else if (tokens[start] == "+")
        {
            result = new Addition(tokens[start], new NumVariable("0", 0), ParseArithmetic(tokens, start + 1, end));
            return result;
        }
        else if (tokens[start] == "-")
        {
            result = new Subtraction(tokens[start], new NumVariable("0", 0), ParseArithmetic(tokens, start + 1, end));
            return result;
        }
        else if (start == end)
        {
            double x;
            bool isNumber = double.TryParse(tokens[start], out x);
            if (!isNumber)
            {
                result = new NumVariable(tokens[start], 0d);
                return result;
            }
            else
            {
                result = new NumVariable(tokens[start], x);
                return result;
            }
        }
        return result;
    }
    private int GoToNetParenthesis(int index, string[] tokens)
	{
		for(int i = index;i < tokens.Length; i++)
		{
			if (tokens[i] == "(")
			{
				i = GoToNetParenthesis(i+1, tokens);
			}
			else if (tokens[i] == ")")
			{
				return i;
			}
		}
		throw new Exception("\")\" expected");
	}
}
