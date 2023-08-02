using System.Globalization;

namespace Hulk
{
	public abstract class HulkException : Exception
	{
		public override string Message { get => MessageStart + MessageDefinition + "."; }
		public string MessageStart { get; protected set; }
		public string MessageDefinition { get; protected set; }
	}
	public class InstrucctionError: HulkException
	{
		public InstrucctionError(HulkException ex, int instrucctionNumber, int instrucctionsCount)
		{
			MessageStart = ex.MessageStart;
			string messageEnd = instrucctionsCount > 1 ? $" (on instrucction {instrucctionNumber})" : ""; 
			MessageDefinition = ex.MessageDefinition + messageEnd;
		}
	}
	public class DefaultError : HulkException
	{
		public DefaultError(string message)
		{
			MessageStart = "! ERROR : ";
			MessageDefinition = message;
		}
		public DefaultError(string message, string errorEspecification)
		{
			errorEspecification = errorEspecification.ToUpper(new CultureInfo("en-US"));
			MessageStart = $"! {errorEspecification} ERROR :";
			MessageDefinition = message;
		}
	}
	public class LexicalError : HulkException
	{
		public LexicalError(string invalidToken)
		{
			MessageStart = "! LEXICAL ERROR : ";
			InvalidToken = invalidToken;
			ExpectedToken = "token";
			MessageDefinition = $"`{InvalidToken}` is not a valid {ExpectedToken}";

		}
		public LexicalError(string invalidToken, string expectedToken)
		{
			MessageStart = "! LEXICAL ERROR : ";
			InvalidToken = invalidToken;
			ExpectedToken = expectedToken;
			MessageDefinition = $"`{InvalidToken}` is not a valid {ExpectedToken}";
		}
		public string InvalidToken { get; }
		public string ExpectedToken { get; }
	}
	public class SyntaxError : HulkException
	{
		public SyntaxError(string missingPart, string place)
		{
			MessageStart = "! SYNTAX ERROR : ";
			MissingPart = missingPart;
			MissingPlace = place;
			MessageDefinition = $"Missing {MissingPart} in {MissingPlace}";
		}
		public string MissingPart { get; }
		public string MissingPlace { get; }

	}
	public class SemanticError : HulkException
	{
		public SemanticError(string expression, string expected, string received)
		{
			MessageStart = "! SEMANTIC ERROR : ";
			Expression = expression;
			ExpressionReceived = received;
			ExpressionExpected = expected;
			MessageDefinition = $"{Expression} receives {expected}, not {received}";
		}
		public string Expression { get; }
		public string ExpressionReceived { get; }
		public string ExpressionExpected { get; }
	}
	public class OperationSemanticError : HulkException
	{
		public OperationSemanticError(string operation, string leftArg, string rightArg, string expected)
		{
			MessageStart = "! SEMANTIC ERROR : ";
			Operation = operation;
			LeftArgument = leftArg;
			RightArgument = rightArg;
			Expected = expected;
			MessageDefinition = $"Operation `{Operation}` cannot be used between `{leftArg}` and `{rightArg}`";
		}
		public string Operation { get; }
		public string LeftArgument { get; }
		public string RightArgument { get; }
		public string Expected { get; }
	}
}
