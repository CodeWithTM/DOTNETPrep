namespace ExceptionHandling
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");


        }

        // basic example of exception handling
        public static void Divide(int a, int b)
        {
            try
            {
                int result = a / b;
                Console.WriteLine($"Result: {result}");
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine("Cannot divide by zero. Please provide a non-zero denominator.");
                Console.WriteLine($"Error details: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred.");
                Console.WriteLine($"Error details: {ex.Message}");
            }
        }

        // i want it to nested function calls with exception handling
        public static void OuterFunction()
        {
            try
            {
                Console.WriteLine("Entering OuterFunction.");
                InnerFunction();
                Console.WriteLine("Exiting OuterFunction.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred in OuterFunction.");
                Console.WriteLine($"Error details: {ex.Message}");
            }
        }
        public static void InnerFunction()
        {
            try
            {
                Console.WriteLine("Entering InnerFunction.");
                // Simulate an error
                throw new InvalidOperationException("Something went wrong in InnerFunction.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred in InnerFunction.");
                Console.WriteLine($"Error details: {ex.Message}");
                // Rethrow the exception to be handled by the outer function
                throw;
            }
        }

    }
}
