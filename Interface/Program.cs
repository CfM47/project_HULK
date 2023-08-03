using Hulk;

namespace Interface
{
    internal class Program
    {
        static void Main()
        {
            Kompiler kompiler = new();
            while (true)
            {
                Console.Write(">");
                kompiler.Compile(Console.ReadLine());
            }
        }
    }
}