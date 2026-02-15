using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionHandling
{

    //Inner Exception in C#
    /*
    An inner exception is the real/original exception that caused a new exception.

    You usually wrap exceptions when:

    you want to add more context

    but still keep the original root cause
    */

    internal class InnerExceptionExample
    {
        public static void Main2()
        {
            try
            {
                Case1_InnerNull(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("===== CASE 1 =====");
                PrintException(ex);
            }

            Console.WriteLine();

            try
            {
                Case2_InnerFilled(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("===== CASE 2 =====");
                PrintException(ex);
            }
        }

        static void Case1_InnerNull(int number)
        {
            try
            {
                int result = 10 / number;
            }
            catch (Exception ex)
            {
                throw new Exception("Case1: Failed while dividing"); // in this case inner = null
            }
        }

        static void Case2_InnerFilled(int number)
        {
            try
            {
                int result = 10 / number;
            }
            catch (Exception ex)
            {
                throw new Exception("Case2: Failed while dividing", ex); // inner = ex (i.e. attempt to divide by zero)
            }
        }

        static void PrintException(Exception ex)
        {
            Console.WriteLine("Message: " + ex.Message);
            Console.WriteLine("Type: " + ex.GetType().Name);

            if (ex.InnerException == null)
            {
                Console.WriteLine("InnerException: NULL");
            }
            else
            {
                Console.WriteLine("InnerException Message: " + ex.InnerException.Message);
                Console.WriteLine("InnerException Type: " + ex.InnerException.GetType().Name);
            }
        }
    }
}
