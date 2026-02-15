namespace ExceptionHandling
{
    internal class Program
    {
        static void Main(string[] args)
        {

            try
            {
                // This is unhanled exception and app will crashhhh!
                BaseMethod(4);

                AnotherImpMethodThatneedsToBeExecuted();

                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

        }

        private static int BaseMethod(int index)
        {
            //return ChildMethod(index);

            //add exception handling in child method and see the difference
            try
            {
                

                int result = ChildMethodWithExceptionHandling(index);

                int.Parse("a");

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("--------------------------------------------------------------");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);


                //throw; // Rethrow the exception to be handled by the caller if needed



                //throw vs throw ex; => throw will preserve the original stack trace,
                //while throw ex; will reset the stack trace to the point where the exception is rethrown.
                //It's generally recommended to use throw without specifying the exception variable to maintain the original stack trace for better debugging.

                //then why throw ex exists? 
                //Because C# allows you to throw any exception object, not only the one you just caught.
                //e.g. Throwing a different exception object - as below

                throw new InvalidOperationException("Invalid operation!", ex); //So in this case "Index out of range" exception will be the inner exception of "Invalid operation!" exception.
                                                                               //This way you can provide more context about the error while still preserving the original exception details.
                //return -1;
            }

        }

        private static int ChildMethod(int index)
        {
            int[] arr = { 1, 2, 3, 4, 5 };

            return arr[index];
        }

        private static void AnotherImpMethodThatneedsToBeExecuted()
        {
            Console.WriteLine("This is another important method that needs to be executed.");
        }


        private static int ChildMethodWithExceptionHandling(int index)
        {
            //try
            //{
                int[] arr = { 1, 2, 3, 4, 5 };
                return arr[index];
            //}
            //catch (IndexOutOfRangeException ex)
            //{
            //    Console.WriteLine("Index is out of range. Please provide a valid index.");
            //    Console.WriteLine($"Error details: {ex.Message}");
            //    //return -1; // Return a default value or handle as needed
            //
            //    throw; // Rethrow the exception to be handled by the caller if needed
            //
            //
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("An unexpected error occurred.");
            //    Console.WriteLine($"Error details: {ex.Message}");
            //    return -1; // Return a default value or handle as needed
            //}
        }












    }
}
