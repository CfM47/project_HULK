using Hulk;

namespace Interface
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Memory Memoria = new Memory();
            Tokenizer tokenizer = new Tokenizer(Memoria);
            while (true)
            {
                Console.Write(">");
                var input = Console.ReadLine();
                string[] s = tokenizer.GetTokens(input);
                if (input != null)
                {
                    try
                    {
                        HulkExpression exp = tokenizer.Parse(s);
                        if (exp is VariableDeclaration)
                        {
                            VariableDeclaration Vars = (VariableDeclaration)exp;
                            foreach (string name in Vars.Names)
                            {
                                Memoria.AddNewVariable(name, new Variable(name, Vars.GetValue(), Vars.Type));
                            }
                        }
                        else if (exp is FunctionDeclaration)
                        {
                            FunctionDeclaration Function = (FunctionDeclaration)exp;
                            Memoria.AddNewFunction(Function.FunctionName, Function);
                        }
                        else
                        {
                            try
                            {
                                exp.GetValue();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
    public static class ExtString
    {
        public static string algo(this String arg)
        {
            return "el blue label de jhonny walker";
        }
    }
}