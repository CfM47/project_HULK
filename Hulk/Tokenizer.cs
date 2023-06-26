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
		
		Regex pattern = new Regex(@"(\({1}|&{2}|\|{2}|!{1}|\){1}|(true)|(false))|([^&!\|\(\)\s]+)");
		MatchCollection collection = pattern.Matches(entry);
		string[] tokens = new string[collection.Count];
		for(int i = 0; i < tokens.Length; i++)
			tokens[i] = collection[i].Value;
		return tokens;
	}
	public HulkExpression ParseBoolean(string[] tokens)
	{
		return InternParseBoolean(tokens, 0, tokens.Length - 1);
	}
	private HulkExpression InternParseBoolean(string[] tokens, int start, int end)
	{
		HulkExpression result = null;
		for(int i = start; i <= end; i++)
		{
			if (tokens[i] == "(")
			{
				i = GoToNetParenthesis(i + 1, tokens);
			}
			else if (tokens[i] == "&&")
			{

				HulkExpression left = i != start? InternParseBoolean(tokens, start, i - 1) 
					: throw new Exception("&& has no left argument");
				HulkExpression right = i != end ? InternParseBoolean(tokens, i + 1, end) 
					: throw new Exception("&& has no right argument");
				result = new Conjunction(tokens[i], left, right);
                return result;
            }
			else if(tokens[i] == "||")
			{
                HulkExpression left = i != start ? InternParseBoolean(tokens, start, i - 1) 
					: throw new Exception("&& has no left argument");
                HulkExpression right = i != end ? InternParseBoolean(tokens, i + 1, end) 
					: throw new Exception("&& has no right argument");
                result = new Disjunction(tokens[i], left, right);
                return result;
            }
		}
		if(result == null)
		{
			if (tokens[start] == "(")
			{
				result = start != end - 1 ? InternParseBoolean(tokens, start + 1, end - 1)
					: throw new Exception("there is no argument given to ()");
			}
			else if (tokens[start] == "!")
				result = start != end ? new Negation(tokens[start], InternParseBoolean(tokens, start + 1, end))
					: throw new Exception("! has no argument");
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
			else
				throw new Exception("Boolean operation expected");
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
