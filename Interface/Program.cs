using Hulk;
namespace Interface;

internal class Program
{
    static void Main()
    {
        void PrintFunc(object a)
        {
            string output = a.ToString();
            if (a is double d)
            {
                if (double.IsPositiveInfinity(d))
                    output = "Infinity";
                else if (double.IsNegativeInfinity(d))
                    output = "-Infinity";
                else
                    output = d.ToString();
            }
            Console.WriteLine(output);
        }
        Kompiler kompiler = new(Console.WriteLine);
        while (true)
        {
            Console.Write(">");
            kompiler.Compile(Console.ReadLine());
        }
    }
}