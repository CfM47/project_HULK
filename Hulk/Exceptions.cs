using System;
using static LexicalError;

public abstract class HulkException : Exception
{
	public override string Message { get => MessageStart + MessageDefinition; }
	public string MessageStart { get; protected set; }
	public string MessageDefinition { get; protected set; }
}
public class LexicalError : HulkException
{
	public LexicalError(string invalidToken)
	{
		MessageStart = "! LEXICAL ERROR :";
		InvalidToken = invalidToken;
		

    }
	public LexicalError(string invalidToken, string expectedToken) 
	{
        MessageStart = "! LEXICAL ERROR :";
        InvalidToken = invalidToken;
        ExpectedToken = expectedToken;
        MessageDefinition = $"{InvalidToken} is not a valid {ExpectedToken}";
    }
	public string InvalidToken { get; }
	public string ExpectedToken { get; }
}
public class SyntaxError : HulkException
{
	public SyntaxError(string missingPart, string place)
	{
        MessageStart = "! SYNTAX ERROR :";
		MissingPart = missingPart;
		MissingPlace = place;
		MessageDefinition = $"Missing {MissingPart} in {MissingPlace}";
	}
    public string MissingPart { get; }
	public string MissingPlace { get; }

}
public class SemanticError: HulkException
{
	public SemanticError(string expression, string expected, string received )
	{
		MessageStart = "! SEMANTIC ERROR :";
		Expression = expression;
		ExpressionReceived = received;
		ExpressionExpected = expected;
		MessageDefinition = $"{Expression} receives {expected}, not {received}";
	}
	public string Expression { get; }
	public string ExpressionReceived { get; }
	public string ExpressionExpected { get; }
}