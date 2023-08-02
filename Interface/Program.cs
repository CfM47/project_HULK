using Hulk;

namespace Interface
{
    internal class Program
    {
        static void Main()
        {
            Memory Memoria = new();
            Parser tokenizer = new(Memoria);
            while (true)
            {
                Console.Write(">");
                var input = Console.ReadLine();
                if (input.Length == 0)
                    continue;
                string[] s = Tokenizer.GetTokens(input);
                List<string[]> Instructions;
                try
                {
                    Instructions = Tokenizer.GetInstructions(s);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
                for (int i = 0; i < Instructions.Count; i++)
                {
                    string[] instruction = Instructions[i];
                    if (instruction.Length == 0)
                        continue;
                    try
                    {
                        try
                        {
                            HulkExpression exp = tokenizer.Parse(instruction);
                            if (exp is HulkDeclaration Dec)
                            {
                                Dec.AddToMemory(Memoria);
                            }
                            else
                            {
                                exp.GetValue(false);
                                exp.GetValue(true);
                            }
                        }
                        catch(HulkException ex)
                        {
                            throw new InstrucctionError(ex, i + 1, Instructions.Count);
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
}