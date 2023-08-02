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
                        if(exp is HulkDeclaration Dec)
                        {
                            Dec.AddToMemory(Memoria);
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