using Hulk;
namespace Interface;

internal class Program
{
    static void Main()
    {
        Kompiler kompiler = new(Console.WriteLine);
        while (true)
        {
            Console.Write(">");
            kompiler.Compile(Console.ReadLine());
        }
    }
}