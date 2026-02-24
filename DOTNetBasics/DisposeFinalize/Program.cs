namespace DisposeFinalize
{
    internal class Program
    {
        static void Main(string[] args)
        {

            GCDemo.MainGC(args);
            IDispoPattern.MainDisp();

            Console.ReadLine();
        }
    }
}
