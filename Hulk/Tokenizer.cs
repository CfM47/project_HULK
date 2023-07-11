using System.Text.RegularExpressions;

namespace Hulk
{
    public class Tokenizer
    {
        HulkExpression ParsingExp;
        public Dictionary<string, HulkExpression> Memory { get; }
        public Tokenizer(Dictionary<string, HulkExpression> pila)
        {
            Memory = pila;
        }
        public string[] GetTokens(string entry)
        {
            Regex pattern = new Regex(@"((\u0022(.)*\u0022)|\(|\+|-|\*|/|%|,|(=>)|(<=)|(>=)|(<)|(>)|={2}|={1}|(!=)|\^|&{2}|\|{2}|!|\)|[^\(\)\+\-\*/\^%<>=!&\|,\s]+)");
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
            if (tokens[start] == "function")
                expr = ParseFunctionDeclaration(tokens, start, end);
            else if (tokens[start] == "if")
                expr = ParseIfElseStatement(tokens, start, end);
            else if (tokens[start] == "number" || tokens[start] == "boolean" || tokens[start] == "string")
                expr = ParseVarDeclaration(tokens, start, end);
            if (expr == null)
                expr = ParseBoolean(tokens, start, end);
            if (expr == null)
                expr = ParseArithmetic(tokens, start, end);
            if (expr == null)
                expr = TryFunctionCall(tokens, start, end);
            return expr;
        }
        #region Variable declaration Parsing
        private HulkExpression ParseVarDeclaration(string[] tokens, int start, int end)
        {
            HulkExpression result = null;
            if (tokens[start] != "number" && tokens[start] != "boolean" && tokens[start] != "string")
                throw new Exception();
            else
            {
                string type = tokens[start];
                int declarationEnd = GetNameLimit(tokens, start + 1, end, "=");
                List<string> names = GetCommaSeparatedTokens(tokens, start + 1, declarationEnd);
                HulkExpression ValueExp = null;
                if (declarationEnd < end)
                    ValueExp = Parse(tokens, declarationEnd + 2, end);
                result = new VariableDeclaration(names, type, ValueExp);
            }
            return result;
        }
        #endregion
        #region Function Declaration Parsing
        public HulkExpression ParseFunctionDeclaration(string[] tokens, int start, int end)
        {
            HulkExpression result = null;
            int declarationEnd = GetNameLimit(tokens, start, end, "=>");
            if (tokens[start] != "function" || declarationEnd >= end - 1)
                throw new Exception();
            else
            {
                string funcName = tokens[start + 1];
                if (tokens[start + 2] != "(")
                    throw new Exception();
                else
                {
                    List<string> ArgNames = GetCommaSeparatedTokens(tokens, start + 3, GoToNextParenthesis(start + 3, tokens) - 1);
                    result = new FunctionDeclaration(funcName, ArgNames);
                    ParsingExp = result;
                    HulkExpression DefExpression = Parse(tokens, declarationEnd + 2, end);
                    ParsingExp = null;
                    ((FunctionDeclaration)result).Define(DefExpression);
                }
            }
            return result;
        }
        #endregion
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
                        return result;
                    }
                case "!":
                    {
                        result = start != end ? new Negation(ParseBoolean(tokens, start + 1, end))
                                        : throw new Exception();
                        return result;
                    }                
            }
            if (start == end)
            {

                if (tokens[start] != "true" && tokens[start] != "false")
                {
                    double maybeNum;
                    if (double.TryParse(tokens[start], out maybeNum))
                        return null;
                    try
                    {
                        result = Memory[tokens[start]];
                    }
                    catch
                    {
                        if (ParsingExp is FunctionDeclaration)
                        {
                            var Exp = ParsingExp as FunctionDeclaration;
                            result = Exp.Arguments[tokens[start]];
                        }
                        else
                            throw new Exception();
                    }
                    return result;
                }
                else
                {
                    result = new Variable(bool.Parse(tokens[start]));
                    return result;
                }
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
                                Regex regex = new Regex(@"(\(|\+|-|\*|/|%|=|\^|,)");
                                left = !regex.Match(tokens[i - 1]).Success ? ParseArithmetic(tokens, start, i - 1) : null;
                            }
                            else
                                left = new Variable(0);
                            HulkExpression right = i != end ? ParseArithmetic(tokens, i + 1, end) : throw new Exception();
                            if (left != null && right != null)
                                result = new Addition(left, right);
                            return result;
                        }
                    case "-":
                        {
                            HulkExpression left = null;
                            if (i != start)
                            {
                                Regex regex = new Regex(@"(\(|\+|-|\*|/|%|=|\^|,)");
                                left = !regex.Match(tokens[i - 1]).Success ? ParseArithmetic(tokens, start, i - 1) : null;
                            }
                            else
                                left = new Variable(0);
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
                case "log":
                    {
                        if (tokens[start + 1] != "(" || tokens[end] != ")")
                            throw new Exception();
                        else
                        {
                            List<HulkExpression> Args = GetComaSeparatedExpressions(tokens, start + 2, end - 1);
                            if (Args.Count != 2)
                                throw new Exception();
                            else
                            {
                                HulkExpression left = Args[0].GetValue() is double ? Args[0] : throw new Exception();
                                HulkExpression right = Args[1].GetValue() is double ? Args[1] : throw new Exception();
                                result = new Logarithm(left, right);
                            }
                        }
                        return result;
                    }
            }
            if (start == end)
            {
                double x;
                bool isNumber = double.TryParse(tokens[start], out x);
                if (!isNumber)
                {
                    try
                    {
                        result = Memory[tokens[start]];
                    }
                    catch
                    {
                        if (ParsingExp is FunctionDeclaration)
                        {
                            var Exp = ParsingExp as FunctionDeclaration;
                            result = Exp.Arguments[tokens[start]];
                        }
                        else
                            throw new Exception();
                    }
                    return result;
                }
                else
                {
                    result = new Variable(x);
                    return result;
                }
            }
            return result;
        }
        #endregion
        public HulkExpression TryFunctionCall(string[] tokens, int start, int end)
        {
            HulkExpression result = null ;
            if (tokens[start + 1] == "(")
            {
                try
                {
                    //la siguiente linea tiene cara de que me van a romper el programa
                    FunctionDeclaration Definition;
                    if (Memory.ContainsKey(tokens[start]))
                        Definition = Memory[tokens[start]] as FunctionDeclaration;
                    else
                        Definition = ParsingExp as FunctionDeclaration;
                    if (Definition == null)
                        throw new Exception();
                    string name = tokens[start];
                    var Args = GetComaSeparatedExpressions(tokens, start + 2, GoToNextParenthesis(start + 2, tokens) - 1);
                    result = new FunctionCall(name, Args, Definition);
                }
                catch
                {
                    throw new Exception();
                }
            }
            return result;
        }
        private List<HulkExpression> GetComaSeparatedExpressions(string[] tokens, int start, int end)
        {
            List<HulkExpression> result = new List<HulkExpression>();
            if (tokens[start] == "," || tokens[end] == ",")
                throw new Exception();
            int argStart = start;
            for (int i = start; i <= end; i++)
            {
                if (tokens[i] == ",")
                {
                    result.Add(Parse(tokens, argStart, i - 1));
                    argStart = i + 1;
                }
                else if (i == end)
                {
                    result.Add(Parse(tokens, argStart, i));
                    argStart = i + 1;
                }
            }
            return result;
        }
        private List<string> GetCommaSeparatedTokens(string[] tokens, int start, int end)
        {
            List<string> result = new List<string>();
            if (tokens[start] == "," || tokens[end] == ",")
                throw new Exception();
            for (int i = start; i <= end; i++)
            {
                if (tokens[i] != ",") 
                {
                    if (i % 2 == 1)
                        result.Add(tokens[i]);
                    else
                        throw new Exception();
                }
            }
            return result;
        }
        private int GetNameLimit(string[] tokens, int start, int end, string delimiter)
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
}