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
                string[] s = tokenizer.GetTokens(Console.ReadLine());
                try
                {
                    HulkExpression exp = tokenizer.Parse(s, 0, s.Length - 1);
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