using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionHandling
{

    //A method either returns a value OR throws an exception.
    //But the compiler still wants proof that every possible path either returns or throws.
    internal class ExceptionExamples
    {
        // basic example of exception handling
        public static void Divide(int a, int b)
        {
            try
            {
                int result = a / b;
                Console.WriteLine($"Result: {result}");

                GetUserName(1234);

                if(TryGetUsername(1234, out string name))
                {
                    Console.WriteLine(name);
                }


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

        public static string GetUserName(int userId)
        {
            try
            {
                if (userId == 1)
                    return "Alice";

                throw new Exception($"User with id {userId} not found");
            }
            catch (Exception ex)
            {
                Console.WriteLine("DB error");


                //return null; // this is a return but it doesn't make sense to return null here because we don't know if userId was valid or if there was a DB error. We should throw an exception instead.

                throw new Exception("Failed to load user from DB!", ex);
            }
        }

        public static bool TryGetUsername(int userId, out string name)
        {
            try
            {
                if (userId == 1)
                {
                    name = "Alice";
                    return true;
                }

                name = null;
                return false;

            }
            catch (Exception ex)
            {
                name = null;
                return false;
            }
        }
    }
}
