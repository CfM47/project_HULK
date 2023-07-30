using System.Globalization;
using System.Text.RegularExpressions;

namespace Hulk
{
    public class Tokenizer
    {
        Stack<HulkExpression> ParsingExp;
        public Memory Memoria { get; }
        public Tokenizer(Memory Mem)
        {
            Memoria = Mem;
            ParsingExp = new Stack<HulkExpression>();
        }
        public string[] GetTokens(string entry)
        {
            Regex pattern = new Regex(@"(\u0022([^\u0022\\]|\\.)*\u0022)|\(|\+|-|\*|/|%|,|(=>)|(<=)|(>=)|(<)|(>)|={2}|={1}|(!=)|\^|&{2}|\|{2}|!|\)|[^\(\)\+\-\*/\^%<>=!&\|,\s]+");
            MatchCollection collection = pattern.Matches(entry);
            string[] tokens = new string[collection.Count];
            for (int i = 0; i < tokens.Length; i++)
                tokens[i] = collection[i].Value;
            return tokens;
        }
        public HulkExpression Parse(string[] tokens)
        {
            try
            {
                var result = ParseInternal(tokens, 0, tokens.Length - 1);
                return result;
            }
            catch (Exception)
            {
                ParsingExp.Clear();
                throw;
            }
        }
        private HulkExpression ParseInternal(string[] tokens, int start, int end)
        {
            if (tokens.Length == 0)
                return null;
            HulkExpression expr = null;
            if (tokens[start] == "function")
                expr = ParseFunctionDeclaration(tokens, start, end);
            else if (tokens[start] == "let")
                expr = ParseLetInExpression(tokens, start, end);
            else if (tokens[start] == "if")
                expr = ParseIfElseStatement(tokens, start, end);
            else if (tokens[start] == "number" || tokens[start] == "boolean" || tokens[start] == "string")
                expr = ParseVarDeclaration(tokens, start, end);
            if (expr == null)
                expr = TryAsignment(tokens, start, end);
            if (expr == null)
                expr = TryConditionalOr(tokens, start, end);
            if (expr == null)
                expr = TryConditionalAnd(tokens, start, end);
            if (expr == null)
                expr = TryEquality(tokens, start, end);
            if (expr == null)
                expr = TryRelational(tokens, start, end);
            if (expr == null)
                expr = TryAdditive(tokens, start, end);
            if (expr == null)
                expr = TryMultiplicative(tokens, start, end);
            if (expr == null)
                expr = TryPower(tokens, start, end);
            if (expr == null)
                expr = TryUnary(tokens, start, end);
            if (expr == null)
                expr = TryPrincipal(tokens, start, end);
            if (expr == null)
                throw new Exception();
            return expr;
        }
        #region Parsing(new)
        //Info:
        //en esta region se implementaran los metodos para parasear los operadores. Estos tienen un nivel de prioridad,
        //me estoy guiando por la prioridad de C#. Para ver la lista detallada del orden de las operaciones ir a
        //https://learn.microsoft.com/es-es/dotnet/csharp/language-reference/operators/ 
        private HulkExpression TryAsignment(string[] tokens, int start, int end)
        {
            HulkExpression result = null;
            for (int i = start; i <= end; i++)
            {
                if (tokens[i] == "(")
                    i = GoToNextParenthesis(i, end, tokens);
                else if (tokens[i] == "=")
                {
                    List<HulkExpression> left = i != start ? GetComaSeparatedExpressions(tokens, start, i - 1) : throw new Exception();
                    HulkExpression right = i != end ? ParseInternal(tokens, i + 1, end) : throw new Exception();
                    List<Variable> Vars = new List<Variable>();
                    foreach (HulkExpression exp in left)
                    {
                        if (exp is not Variable)
                            throw new Exception();
                        else
                            Vars.Add(exp as Variable);
                    }
                    result = new Asignment(Vars, right);
                }
            }
            return result;
        }
        private HulkExpression TryConditionalOr(string[] tokens, int start, int end)
        {
            for (int i = end; i >= start; i--)
            {
                switch (tokens[i])
                {
                    case ")":
                        {
                            i = GoToPreviousParenthesis(i, start, tokens);
                            break;
                        }
                    case "||":
                        return BinaryFunctionMaker(tokens, start, end, i, typeof(Disjunction));
                }
            }
            return null;
        }
        private HulkExpression TryConditionalAnd(string[] tokens, int start, int end)
        {
            for (int i = end; i >= start; i--)
            {
                switch (tokens[i])
                {
                    case ")":
                        {
                            i = GoToPreviousParenthesis(i, start, tokens);
                            break;
                        }
                    case "&&":
                        return BinaryFunctionMaker(tokens, start, end, i, typeof(Conjunction));               
                }
            }
            return null;
        }
        private HulkExpression TryEquality(string[] tokens, int start, int end)
        {
            for (int i = end; i >= start; i--)
            {
                switch (tokens[i])
                {
                    case ")":
                        {
                            i = GoToPreviousParenthesis(i, start, tokens);
                            break;
                        }
                    case "==":
                        return BinaryFunctionMaker(tokens, start, end, i, typeof(Equal));
                    case "!=":
                        return BinaryFunctionMaker(tokens, start, end, i, typeof(UnEqual));
                }
            }
            return null;
        }
        private HulkExpression TryRelational(string[] tokens, int start, int end)
        {
            for (int i = end; i >= start; i--)
            {
                switch (tokens[i])
                {
                    case ")":
                        {
                            i = GoToPreviousParenthesis(i, start, tokens);
                            break;
                        }
                    case "<":
                        return BinaryFunctionMaker(tokens, start, end, i, typeof(LowerThan));
                    case ">":
                        return BinaryFunctionMaker(tokens, start, end, i, typeof(GreaterThan));
                    case "<=":
                        return BinaryFunctionMaker(tokens, start, end, i, typeof(LowerEqualThan));
                    case ">=":
                        return BinaryFunctionMaker(tokens, start, end, i, typeof(GreaterEqualThan));
                }
            }
            return null;
        }
        private HulkExpression TryAdditive(string[] tokens, int start, int end)
        {
            for (int i = end; i >= start; i--)
            {
                switch (tokens[i])
                {
                    case ")":
                        {
                            i = GoToPreviousParenthesis(i, start, tokens);
                            break;
                        }
                    case "+":
                        return i == start ? null : BinaryFunctionMaker(tokens, start, end, i, typeof(Addition));
                       
                    case "-":
                        return i == start? null : BinaryFunctionMaker(tokens, start, end, i, typeof(Subtraction));
                       
                }
            }
            return null;
        }
        private HulkExpression TryMultiplicative(string[] tokens, int start, int end)
        {
            for (int i = end; i >= start; i--)
            {
                switch (tokens[i])
                {
                    case ")":
                        {
                            i = GoToPreviousParenthesis(i, start, tokens);
                            break;
                        }
                    case "*":
                        return BinaryFunctionMaker(tokens, start, end, i, typeof(Multiplication));
                    case "/":
                        return BinaryFunctionMaker(tokens, start, end, i, typeof(Division));
                    case "%":
                        return BinaryFunctionMaker(tokens, start, end, i, typeof(Module));
                }
            }
            return null;
        }
        private HulkExpression TryPower(string[] tokens, int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                switch (tokens[i])
                {
                    case "(":
                        {
                            i = GoToNextParenthesis(i, end, tokens);
                            break;
                        }
                    case "^":
                        return BinaryFunctionMaker(tokens, start, end, i, typeof(Power));
                }
            }
            return null;
        }
        private HulkExpression TryUnary(string[] tokens, int start, int end)
        {
            switch (tokens[start])
            {
                case "(":
                    {
                        int i = GoToNextParenthesis(start, end, tokens);
                        if (i != end)
                            return null;
                        else
                            return start != end - 1 ? ParseInternal(tokens, start + 1, end - 1) : throw new Exception();
                    }
                case "!":
                    return UnaryFunctionMaker(tokens, start, end, start, typeof(Negation));
                case "+":
                    return UnaryFunctionMaker(tokens, start, end, start, typeof(Positive));
                case "-":
                    return UnaryFunctionMaker(tokens, start, end, start, typeof(Negative));
            }
            return null;
        }
        private HulkExpression TryPrincipal(string[] tokens, int start, int end)
        {
            if (start == end)
            {

                if (tokens[start] == "true" || tokens[start] == "false")
                    return new Variable(bool.Parse(tokens[start]));
                double maybeNum;
                if (double.TryParse(tokens[start], NumberStyles.Any, new CultureInfo("en-US"), out maybeNum))
                    return new Variable(maybeNum);
                if (Regex.Match(tokens[start], @"\u0022(.)*\u0022").Success) 
                {
                    string arg = tokens[start].Replace("\\", "");
                    arg = arg.Substring(1, arg.Length - 2);
                    return new Variable(arg);
                }
                return TryVariable(tokens[start]);
            }
            switch (tokens[start])
            {
                case "(":
                {
                    return start != end - 1 ? ParseInternal(tokens, start + 1, end - 1) : throw new Exception();
                }
                case "sqrt":
                    return FunctionCallMaker(tokens, start, end, start, typeof(SquaredRoot));
                case "sin":
                    return FunctionCallMaker(tokens, start, end, start, typeof(Sine));
                case "cos":
                    return FunctionCallMaker(tokens, start, end, start, typeof(Cosine));
                case "exp":
                    return FunctionCallMaker(tokens, start, end, start, typeof(SquaredRoot));
                case "log":
                    return FunctionCallMaker(tokens, start, end, start, typeof(Logarithm));
                case "print":
                    return FunctionCallMaker(tokens, start, end, start, typeof(PrintFunc));
                default:
                    return TryFunctionCall(tokens, start, end);
            }
        }
        #endregion
        #region Statements Parsing
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
                foreach (string name in names)
                {
                    double x = 0;
                    if (HulkInfo.KeyWords.Contains(name) || double.TryParse(name, out x))
                        throw new Exception();
                }
                HulkExpression ValueExp = null;
                if (declarationEnd < end)
                    ValueExp = ParseInternal(tokens, declarationEnd + 2, end);
                result = new VariableDeclaration(names, type, ValueExp);
            }
            return result;
        }
        private HulkExpression ParseVarDeclaration(string[] tokens, int start, int end, VarDecParseOptions parsingOptions)
        {
            if (parsingOptions == VarDecParseOptions.Usual)
                return ParseVarDeclaration(tokens, start, end);
            else
            {
                VariableDeclaration result;
                string type = null;
                string name;
                // la siguiente linea se puede poner dentro del bloque if
                int declarationEnd = GetNameLimit(tokens, start + 1, end, "=");
                if (tokens[start] == "number" || tokens[start] == "boolean" || tokens[start] == "string")
                {
                    type = tokens[start];
                    name = tokens[start + 1];
                }
                else
                {
                    name = tokens[start];
                    declarationEnd = GetNameLimit(tokens, start, end, "=");
                }
                List<string> VariableName = new List<string>();

                double x = 0;
                if (HulkInfo.KeyWords.Contains(name) || double.TryParse(name, out x))
                    throw new Exception();

                VariableName.Add(name);
                HulkExpression ValueExp = null;
                if (declarationEnd < end)
                    ValueExp = ParseInternal(tokens, declarationEnd + 2, end);
                return new VariableDeclaration(VariableName, ValueExp);
            }
        }
        private enum VarDecParseOptions { Usual, LetInArgs }
        public HulkExpression ParseFunctionDeclaration(string[] tokens, int start, int end)
        {
            FunctionDeclaration result = null;
            int declarationEnd = GetNameLimit(tokens, start, end, "=>");
            if (tokens[start] != "function" || declarationEnd >= end - 1)
                throw new Exception();
            else
            {
                string funcName = tokens[start + 1];
                double x = 0;
                if (HulkInfo.KeyWords.Contains(funcName) || double.TryParse(funcName, out x))
                    throw new Exception();
                if (tokens[start + 2] != "(")
                    throw new Exception();
                if (tokens[declarationEnd] != ")")
                    throw new Exception();
                List<string> ArgNames = GetCommaSeparatedTokens(tokens, start + 3, declarationEnd - 1);
                foreach (string name in ArgNames)
                {
                    if (HulkInfo.KeyWords.Contains(name) || double.TryParse(name, out x))
                        throw new Exception();
                }
                result = new FunctionDeclaration(funcName, ArgNames);
                ParsingExp.Push(result);
                HulkExpression DefExpression = ParseInternal(tokens, declarationEnd + 2, end);
                ParsingExp.Pop();
                result.Define(DefExpression);
            }
            return result;
        }
        private HulkExpression ParseLetInExpression(string[] tokens, int start, int end)
        {
            LetInStatement result;
            if (tokens[start] != "let")
                return null;
            int declarationEnd = GetNameLimit(tokens, start, end, "in");
            if (declarationEnd >= end - 1)
                throw new Exception();
            List<HulkExpression> Args = GetComaSeparatedDeclarations(tokens, start + 1, declarationEnd, VarDecParseOptions.LetInArgs);
            Dictionary<string, Variable> LayerVariables = new Dictionary<string, Variable>();
            foreach (HulkExpression arg in Args)
            {
                if (arg is not VariableDeclaration)
                    throw new Exception();
                var Vars = arg as VariableDeclaration;
                //if (Vars.ValueExpression is Variable)
                //{
                //    var var = (Variable)Vars.ValueExpression;
                //    foreach (string name in Vars.Names)
                //        LayerVariables.Add(name, var);
                //}
                //else
                //{
                foreach (string name in Vars.Names)
                {
                    if (Vars.ValueExpression is null)
                        throw new Exception("let-in statements args must be initialized");
                    var LetVariable = new Variable(name, Vars.ValueExpression.GetValue(false), Vars.Type);
                    if (Vars.ValueExpression.IsDependent)
                        LetVariable = new Variable(name, Vars.ValueExpression, Vars.Type, Variable.VariableOptions.Dependent);
                    LayerVariables.Add(name, LetVariable);
                }
                //}
            }
            result = new LetInStatement(LayerVariables);
            ParsingExp.Push(result);
            HulkExpression DefExpression = ParseInternal(tokens, declarationEnd + 2, end);
            ParsingExp.Pop();
            result.Define(DefExpression);
            return result;
        }
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
                    conditionEnd = GoToNextParenthesis(start + 1, end, tokens);
                    if (conditionEnd == end)
                        throw new Exception();
                    condition = ParseInternal(tokens, start + 1, conditionEnd);
                    int ifDoEnd = GetNameLimit(tokens, start, end, "else");
                    if (ifDoEnd == end - 1)
                        throw new Exception();
                    IfDo = ParseInternal(tokens, conditionEnd + 1, ifDoEnd);
                    if (ifDoEnd < end - 1)
                        ElseDo = ParseInternal(tokens, ifDoEnd + 2, end);
                    result = new IfElseStatement(condition, IfDo, ElseDo);
                }
            }
            return result;
        }
        #endregion
        #region Operands Parsing templates
        private HulkExpression TryFunctionCall(string[] tokens, int start, int end)
        {
            HulkExpression result = null;
            if (tokens[start + 1] == "(")
            {
                if (tokens[end] != ")")
                    throw new Exception();
                try
                {
                    //la siguiente linea tiene cara de que me van a romper el programa
                    FunctionDeclaration Definition;
                    HulkExpression PExp;
                    ParsingExp.TryPeek(out PExp);
                    if (Memoria.FunctionsStorage.ContainsKey(tokens[start]))
                        Definition = Memoria.FunctionsStorage[tokens[start]];
                    else if (tokens[start] == ((FunctionDeclaration)PExp).FunctionName)
                        Definition = PExp as FunctionDeclaration;
                    else
                        throw new Exception();
                    string name = tokens[start];
                    //var Args = GetComaSeparatedExpressions(tokens, start + 2, GoToNextParenthesis(start + 2, tokens) - 1);
                    var Args = GetComaSeparatedExpressions(tokens, start + 2, end - 1);
                    result = new FunctionCall(name, Args, Definition);
                }
                catch
                {
                    throw new Exception();
                }
            }
            return result;
        }
        private HulkExpression TryVariable(string varName)
        {
            switch (varName) 
            {
                case "PI":
                    return new Variable(Math.PI);
                case "E":
                    return new Variable(Math.E);
            }
            Stack<HulkExpression> PosibleLocations = new Stack<HulkExpression>(new Stack<HulkExpression>(ParsingExp));
            HulkExpression exp;
            Dictionary<string, Variable> Location = new Dictionary<string, Variable>();
            while (PosibleLocations.TryPop(out exp))
            {
                if (exp is FunctionDeclaration)
                    Location = ((FunctionDeclaration)exp).Arguments;
                else if (exp is LetInStatement)
                    Location = ((LetInStatement)exp).StoredVariables;

                if (Location.ContainsKey(varName))
                    return Location[varName];
            }
            Location = Memoria.VariablesStorage;
            if (Location.ContainsKey(varName))
                return Location[varName];
            else
                throw new Exception("no se encontro la variable");
        }
        private HulkExpression BinaryFunctionMaker (string[] tokens, int start, int end, int opPos, Type type) 
        {
            HulkExpression left = opPos != start ? ParseInternal(tokens, start, opPos - 1) : throw new Exception();
            HulkExpression right = opPos != end ? ParseInternal(tokens, opPos + 1, end) : throw new Exception();

            //aqui hay que hacerle algo pa ver si el tipo es correcto
            object[] args = new object[] { left, right };
            return (HulkExpression)Activator.CreateInstance(type, args);            
            //return null;
        }
        private HulkExpression UnaryFunctionMaker(string[] tokens, int start, int end, int opPos, Type type)
        {
            HulkExpression argument = start != end ?ParseInternal(tokens, start + 1, end) : throw new Exception();
            //aqui hay que hacerle algo pa ver si el tipo es correcto
            object[] args = new object[] { argument };
            return (HulkExpression)Activator.CreateInstance(type, args);
            //return null;
        }
        private HulkExpression FunctionCallMaker(string[] tokens, int start, int end, int opPos, Type type)
        {
            if (tokens[start + 1] != "(" || tokens[end] != ")")
                throw new Exception();
            else
            {
                List<HulkExpression> Args = GetComaSeparatedExpressions(tokens, start + 2, end - 1);
                object[] args = Args.ToArray();
                return (HulkExpression)Activator.CreateInstance(type, args);
            }
        }
        #endregion
        private List<HulkExpression> GetComaSeparatedExpressions(string[] tokens, int start, int end)
        {
            List<HulkExpression> result = new List<HulkExpression>();
            if (tokens[start] == "," || tokens[end] == ",")
                throw new Exception();
            int argStart = start;
            for (int i = start; i <= end; i++)
            {
                if (tokens[i] == "(")
                {
                    i = GoToNextParenthesis(i, end, tokens);          
                    //continue;
                }

                if (tokens[i] == ",")
                {
                    var exp = ParseInternal(tokens, argStart, i - 1);
                    if (exp == null)
                        throw new Exception();
                    result.Add(exp);
                    argStart = i + 1;
                }
                else if (i == end)
                {
                    var exp = ParseInternal(tokens, argStart, i);
                    if (exp == null)
                        throw new Exception();
                    result.Add(exp);
                }
            }
            return result;
        }
        private List<HulkExpression> GetComaSeparatedDeclarations(string[] tokens, int start, int end, VarDecParseOptions options)
        {
            List<HulkExpression> result = new List<HulkExpression>();
            if (tokens[start] == "," || tokens[end] == ",")
                throw new Exception();
            int argStart = start;
            for (int i = start; i <= end; i++)
            {
                if (tokens[i] == "(")
                {
                    i = GoToNextParenthesis(i, end, tokens);
                    //continue;
                }

                if (tokens[i] == ",")
                {
                    
                    var exp = ParseVarDeclaration(tokens, argStart, i - 1, options);
                    if (exp == null || exp is not VariableDeclaration)
                        throw new Exception();
                    result.Add(exp);
                    argStart = i + 1;
                }
                else if (i == end)
                {
                    var exp = ParseVarDeclaration(tokens, argStart, i, options);
                    if (exp == null || exp is not VariableDeclaration)
                        throw new Exception();
                    result.Add(exp);
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
                    if (i % 2 == 1 || start == end)
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
        private int GoToNextParenthesis(int index, int end, string[] tokens)
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
            throw new Exception("\")\" expected");
        }
        private int GoToPreviousParenthesis(int index, int start, string[] tokens)
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
            throw new Exception("\"(\" expected");
        }
    }
}