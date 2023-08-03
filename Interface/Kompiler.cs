using Hulk;

namespace Interface
{
	public class Kompiler
	{
		
		public Kompiler()
		{
			Memory = new();
			Parser = new(Memory);
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
                Console.WriteLine(ex.Message);
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
                        if (exp is HulkDeclaration Dec)
                        {
                            Dec.AddToMemory(Memory);
                        }
                        else
                        {
                            exp.GetValue(false);
                            exp.GetValue(true);
                        }
                    }
                    catch (HulkException ex)
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
        public void Clear()
        {
            Memory = new();
        }
		public HulkMemory Memory { get; private set; }
		HulkParser Parser;

	}
}
