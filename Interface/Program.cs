using Hulk;

namespace Interface
{
    internal class Program
    {
        static void Main()
        {
            Memory Memoria = new();
            Tokenizer tokenizer = new(Memoria);
            while (true)
            {
                Console.Write(">");
                var input = Console.ReadLine();
                if (input.Length == 0)
                    continue;
                string[] s = tokenizer.GetTokens(input);
                List<string[]> Instructions;
                try
                {
                    Instructions = GetInstructions(s);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                foreach (string[] instruction in Instructions)
                {
                    if (instruction.Length == 0)
                        continue;
                    try
                    {
                        HulkExpression exp = tokenizer.Parse(instruction);
                        if (exp is VariableDeclaration Vars)
                        {
                            foreach (string name in Vars.Names)
                            {
                                var options = Variable.VariableOptions.InitializedVariable;
                                Variable newVar;
                                if (Vars.ValueExpression == null)
                                {
                                    options = Variable.VariableOptions.NonInitialized;
                                    newVar = new Variable(name, null, Vars.Type, options);

                                }
                                else
                                    newVar = new Variable(name, Vars.ValueExpression.GetValue(false), Vars.Type, options);
                                Memoria.AddNewVariable(name, newVar);
                            }
                        }
                        else if (exp is FunctionDeclaration Function)
                        {
                            Memoria.AddNewFunction(Function.FunctionName, Function);
                        }
                        else
                        {
                            exp.GetValue(false);
                            exp.GetValue(true);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }



        public static List<string[]> GetInstructions(string[] inputTokens)
        {
            List<string[]> result = new();
            if (inputTokens[^1] != ";")
                throw new DefaultError("code lines must end with a semicolon");
            int start = 0;
            for (int i = 0; i < inputTokens.Length; i++)
            {
                if (inputTokens[i] == ";")
                {
                    string[] instruction = new string[i - start];
                    for (int j = 0; j < i - start; j++)
                    {
                        instruction[j] = inputTokens[j + start];
                    }
                    result.Add(instruction);
                    start = i + 1;
                }
            }
            return result;
        }
    }
}