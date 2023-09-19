namespace Hulk
{
    public abstract class HulkExpression
    {
        public abstract object GetValue(bool execute);
        public abstract Types CheckType();
        public bool IsDependent { get; protected set; }
    }
    public delegate void Print(object input);
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
    public enum Types { Void, number, boolean, hstring, dynamic }
}