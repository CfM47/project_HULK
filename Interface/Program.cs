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
                if (input.Length == 0)
                    continue;
                string[] s = tokenizer.GetTokens(input);
                List<string[]> Instructions = new List<string[]>();
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
                        if (exp is VariableDeclaration)
                        {
                            VariableDeclaration Vars = (VariableDeclaration)exp;
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
                        else if (exp is FunctionDeclaration)
                        {
                            FunctionDeclaration Function = (FunctionDeclaration)exp;
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
            List<string[]> result = new List<string[]>();
            if (inputTokens[0] == ";")
                throw new Exception("code Lines cannot start with a semicolon");
            if (inputTokens[inputTokens.Length - 1] != ";")
                throw new Exception("code lines must end with a semicolon");
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
                    //if(i != inputTokens.Length - 1)
                    start = i + 1;
                }
            }
            return result;
        }
    }
}