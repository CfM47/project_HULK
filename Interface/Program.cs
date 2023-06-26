using Hulk;

namespace Interface
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //((!pasd) && aaweew) || !afas
            Tokenizer tokenizer = new Tokenizer();
            string[] s = tokenizer.GetTokens(Console.ReadLine());
            HulkExpression exp = tokenizer.ParseBoolean(s);
            
            Console.WriteLine(exp.Value);
        }
    }
}