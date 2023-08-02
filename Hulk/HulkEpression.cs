using System.Runtime.CompilerServices;

namespace Hulk
{
    public abstract class HulkExpression
    {
        public abstract object GetValue(bool execute);
        public bool IsDependent { get; protected set; }
    }
    public static class ExtObject
    {
        public static string GetHulkTypeAsString (this Object arg)
        {
            var type = arg.GetType();
            if (type == typeof(double))
                return "number";
            else if (type == typeof(bool))
                return "boolean";
            else if (type == typeof(string))
                return "string";
            else if (type == typeof(EmptyReturn))
                return "void";
            else
                return "type";
        }
    }
    public class EmptyReturn 
    {
        public EmptyReturn() { }
    }
    public abstract class BinaryFunction : HulkExpression
    {
        public BinaryFunction(HulkExpression leftArgument, HulkExpression rightArgument)
        {
            if (leftArgument.IsDependent || rightArgument.IsDependent)
                IsDependent = true;
            LeftArgument = leftArgument;
            RightArgument = rightArgument;
        }
        public override object GetValue(bool execute)
        {
            return Evaluate(LeftArgument.GetValue(execute), RightArgument.GetValue(execute));
        }
        protected bool ArgsOk(object left, object right, List<Type> AdmitedTypesName)
        {
            foreach(var type in AdmitedTypesName)
            {
                if (left == null || right == null) 
                {
                    if(left == null && right != null)
                    {
                        if (right.GetType() == type)
                            return true;
                    }
                    else if(right == null && left != null)
                    {
                        if (left.GetType() == type)
                            return true;
                    }
                    return true;                        
                }
                if(left.GetType()==right.GetType() && left.GetType()==type)
                    return true;
            }
            return false;
        }
        public HulkExpression LeftArgument { get; protected set; }
        public HulkExpression RightArgument { get; protected set; }
        public abstract object Evaluate(object left, object right);
    }
    public abstract class UnaryFunction : HulkExpression
    {
        public UnaryFunction(HulkExpression Arg)
        {
            if (Arg.IsDependent)
                IsDependent = true;
            Argument = Arg;
        }
        public override object GetValue(bool execute)
        {
            return Evaluate(Argument.GetValue(execute));
        }
        public HulkExpression Argument { get; protected set; }
        public abstract object Evaluate(object arg);
    }
    public class Asignment: HulkExpression
    {
        public Asignment(List<Variable> Vars, HulkExpression ValueExp)
        {
            Variables = Vars;
            CheckValue(ValueExp);
            ValueExpression = ValueExp;
        }
        private void CheckValue(HulkExpression ValueExp)
        {
            Types type = default;
            var val = ValueExp.GetValue(false);
            if (val is double)
                type = Types.number;
            else if (val is bool)
                type = Types.boolean;
            else if (val is string)
                type = Types.hstring;
            else if (val is EmptyReturn)
                type = Types.Void;
            foreach (Variable v in Variables)
            {
                if (v.Type != type)
                    throw new Exception("Cannot asign value of type " + type + " to "+ v.Type + " variable");
            }
        }
        private void ChangeValues()
        {
            var val = ValueExpression.GetValue(false);
            foreach (Variable v in Variables)
            {
                v.Value = val;
                v.Options = Variable.VariableOptions.InitializedVariable;
            }
        }
        public override object GetValue(bool execute)
        {
            if(execute)
                ChangeValues();
            return new EmptyReturn();
        }
        public List<Variable> Variables{ get; protected set; }
        public HulkExpression ValueExpression { get; protected set; }
    }
    public class Variable : HulkExpression
    {
        //constructor para cuando te encuentras numeros, valores de verdad o strings en lo salvaje
        public Variable(object value)
        {
            Value = value;
            Options = VariableOptions.Value;
            SetType();
        }
        public Variable(string name, object value, Types type)
        {
            Name = name;
            Value = value;
            Options = VariableOptions.InitializedVariable;
            SetType();
        }
        //constructor para las declaraciones de variable con tipo
        public Variable(string name, object value, Types type, VariableOptions options)
        {
            Name = name;
            //Value = value;
            //SetType();
            Options = options;
            if (options == VariableOptions.Dependent || options == VariableOptions.FunctionArgument)
                IsDependent = true;
            object valueToCheck = value;
            if (value is HulkExpression)
                valueToCheck = ((HulkExpression)value).GetValue(false);
            bool matchNumber = valueToCheck is double && type == Types.number;
            bool matchBool = valueToCheck is bool && type == Types.boolean;
            bool matchString = valueToCheck is string && type == Types.hstring;            
            if (matchNumber || matchBool || matchString || value == null || type == Types.dynamic)
            {
                Value = value;
                Type = type;
            }
            else 
                throw new SemanticError($"Variable `{Name}`", $"{Type}", value.GetHulkTypeAsString());

        }
        public VariableOptions Options { get;  set; }
        public object Value { get; set; }
        public override object GetValue(bool execute)
        {            
            switch (Options)
            {
                case VariableOptions.NonInitialized:
                    throw new Exception("Use of unasigned variable");
                case VariableOptions.FunctionArgument:
                    return Value;
            }
            if (IsDependent)
                return ((HulkExpression)Value).GetValue(execute);
            return Value;
        }
        public string? Name { get; protected set; }
        private void SetType()
        {
            if (Value is double)
                Type = Types.number;
            else if (Value is bool)
                Type = Types.boolean;
            else if (Value is string)
                Type = Types.hstring;
            //else
            //    Type = null;
        }
        public Types Type { get; protected set; }
        public enum VariableOptions { Value, InitializedVariable, NonInitialized, FunctionArgument, Dependent}
    }
    public class FunctionCall : HulkExpression
    {
        public FunctionCall(string name, List<HulkExpression> Args, FunctionDeclaration Def)
        {
            foreach(var arg in Args)
            {
                if (arg.IsDependent)
                    IsDependent = true;
            }
            Name = name;
            Arguments = Args;
            Definition = Def;
        }
        public override object GetValue(bool execute)
        {
            try
            {
                return Definition.Evaluate(Arguments, execute);
            }
            catch (SemanticError ex)
            {
                throw new SemanticError($"Function `{Name}`", ex.ExpressionExpected, ex.ExpressionReceived);
            }
        }
        public string Name { get; protected set; }
        public List<HulkExpression> Arguments { get; protected set; }
        public FunctionDeclaration Definition { get; protected set; }
    }
    public class PrintFunc : HulkExpression
    {
        public PrintFunc(HulkExpression Arg)
        {
            Argument = Arg;                
        }
        public HulkExpression Argument { get; }
        public override object GetValue(bool execute)
        {
            if(execute)
                Console.WriteLine(Argument.GetValue(false));
            return new EmptyReturn();
        }
    }
}