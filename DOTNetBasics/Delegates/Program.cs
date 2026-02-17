namespace Delegates
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Func<int, int, int> addMethod = addition;

            Func<int, int> inlineFunc = (a) => a + 1;


            Func<int, bool> predicate = (a) => a > 0;


            List<int> list = new List<int>() { 1,2,3,4,5};

            list.Where(predicate);

            Console.WriteLine($"{addMethod(1,2)}");

            Console.ReadLine();
        }


        private static int addition(int a, int b) {  
            return a + b; 
        }
    }

    
}
