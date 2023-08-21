namespace Hulk
{
    public class Kompiler
    {
        public Kompiler(Print print)
        {
            Memory = new();
            Parser = new(Memory, print);
            Handler = print;
        }
        public void Compile(string input)
        {
            string[] s = Tokenizer.GetTokens(input);
            List<string[]> Instructions;
            try
            {
                Instructions = Tokenizer.GetInstructions(s);
            }
            catch (Exception ex)
            {
                Handler(ex.Message);
                return;
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
                        HulkExpression exp = Parser.Parse(instruction);
                        if (exp is FunctionDeclaration Dec)
                        {
                            Dec.AddToMemory(Memory);
                        }
                        else if(exp is PrintFunc print)
                        {
                            print.GetValue(false);
                            print.GetValue(true);
                        }
                        else
                        {
                            Handler(exp.GetValue(false));
                        }
                    }
                    catch (HulkException ex)
                    {
                        throw new InstrucctionError(ex, i + 1, Instructions.Count);
                    }
                }
                catch (Exception ex)
                {
                    Handler(ex.Message);
                }
            }
        }
        public void Clear()
        {
            Memory = new();
            Parser = new(Memory, Handler);
        }
        public HulkMemory Memory { get; private set; }
        Print Handler;
        HulkParser Parser;

	}
}
