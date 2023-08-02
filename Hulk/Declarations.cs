namespace Hulk
{
    public abstract class HulkDeclaration : HulkExpression
    {
        public abstract void AddToMemory(Memory Mem);
        public override object GetValue(bool execute)
        {
            return new EmptyReturn();
        }
    }
    public class VariableDeclaration : HulkDeclaration
    {
        #region Constructors
        public VariableDeclaration(List<string> names, string type, HulkExpression ValueExp)
        {
            Names = names;
            SetType(type);
            bool valueOk = ValueExp == null || IsOkValue(ValueExp);
            if (!valueOk)
                throw new SemanticError("Variable declaration", type, ValueExp.GetValue(false).GetHulkTypeAsString());
            ValueExpression = ValueExp;
        }
        public VariableDeclaration(List<string> names, HulkExpression ValueExp)
        {
            Names = names;
            SetType(ValueExp.GetValue(false));
            ValueExpression = ValueExp;
        }
        #endregion
        #region Methods
        private void SetType(string type)
        {
            Type = type switch
            {
                "number" => Types.number,
                "boolean" => Types.boolean,
                "string" => Types.hstring,
                _ => throw new Exception(),
            };
        }
        private void SetType(object value)
        {
            if (value is double)
                Type = Types.number;
            else if (value is bool)
                Type = Types.boolean;
            else if (value is string)
                Type = Types.hstring;
            else if (value == null)
                Type = Types.dynamic;
            else
                throw new Exception();
        }
        private bool IsOkValue(HulkExpression ValueExp)
        {
            var val = ValueExp.GetValue(false);
            bool matchExp = false;
            if (val == null)
            {
                if (ValueExp is Addition && (Type == Types.number || Type == Types.hstring))
                    matchExp = true;
                else if (Value is Variable)
                    matchExp = true;
            }
            bool okNumber = (val is double) && (Type == Types.number);
            bool okBoolean = (val is bool) && (Type == Types.boolean);
            bool okString = (val is string) && (Type == Types.hstring);
            if (okNumber || okBoolean || okString || matchExp)
                return true;
            return false;
        }
        public override void AddToMemory(Memory Memoria)
        {
            foreach (string name in Names)
            {
                var options = Variable.VariableOptions.InitializedVariable;
                Variable newVar;
                if (ValueExpression == null)
                {
                    options = Variable.VariableOptions.NonInitialized;
                    newVar = new Variable(name, null, Type, options);

                }
                else
                    newVar = new Variable(name, ValueExpression.GetValue(false), Type, options);
                Memoria.AddNewVariable(name, newVar);
            }
        }
        #endregion
        #region Properties
        public List<string> Names { get; }
        public HulkExpression ValueExpression { get; private set; }
        public object Value { get => ValueExpression?.GetValue(false); set { } }
        public Types Type { get; private set; }
        #endregion
    }
    public class FunctionDeclaration : HulkDeclaration
    {
        public FunctionDeclaration(string name, List<string> argNames)
        {
            FunctionName = name;
            ArgumentNames = argNames;
            Arguments = new Dictionary<string, Variable>();
            SetArgs(ArgumentNames);
        }
        #region Methods
        public object Evaluate(List<HulkExpression> Args, bool execute)
        {
            if (Args.Count != Arguments.Count)
                throw new SemanticError($"Function `{FunctionName}`", $"{Arguments.Count} argument(s)", $"{Args.Count} argument(s)");
            else
            {
                List<object> OldValues = new();
                for (int i = 0; i < Args.Count; i++)
                {
                    string key = ArgumentNames[i];
                    Variable v = Arguments[key];
                    OldValues.Add(v.Value);
                    v.Value = Args[i].GetValue(false);
                }
                var result = Definition.GetValue(execute);
                for (int i = 0; i < OldValues.Count; i++)
                {
                    string key = ArgumentNames[i];
                    Variable v = Arguments[key];
                    v.Value = OldValues[i];
                }
                return result;
            }
        }
        public void Define(HulkExpression definition)
        {
            Definition = definition;
            CheckDefinition();
        }
        public void CheckDefinition()
        {
            try
            {
                Definition.GetValue(false);
            }
            catch (SemanticError ex)
            {
                throw ex;
            }
        }
        private void SetArgs(List<string> argNames)
        {
            foreach (string arg in argNames)
            {
                object val = default;
                Arguments.Add(arg, new Variable(arg, val, Types.dynamic, Variable.VariableOptions.FunctionArgument));
            }
        }
        public override void AddToMemory(Memory Memoria)
        {
            Memoria.AddNewFunction(this.FunctionName, this);
        }
        #endregion
        #region Properties
        public List<string> ArgumentNames { get; }
        public string FunctionName { get; private set; }
        public Dictionary<string, Variable> Arguments { get; private set; }
        public HulkExpression Definition { get; private set; }
        #endregion
    }
}