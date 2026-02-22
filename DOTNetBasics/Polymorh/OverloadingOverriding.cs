using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polymorh
{

    // return type in method signature

    internal class OverloadingOverriding
    {
        public static void MainOO()
        {
            Process();


            // overloading with implicit type promotion
            Display(10); // calls long version

            ProcessMethod(1); // calls one param version
            ProcessMethod(1, 2); // calls two param version
        }

        /*
         Overloading is NOT valid when you only change:

            Return type
            Parameter names

         */
        public static int Process() => 1;
        //public string Process() => "hello"; // ❌ compiler error

        public static int Process(int i) => 1;

        public static void Display(long value) => Console.WriteLine("long version");
        public static void Display(double value) => Console.WriteLine("double version");


        public static void ProcessMethod(int a, int b = 0) => Console.WriteLine("two param");
        public static void ProcessMethod(int a) => Console.WriteLine("one param");
    }
}
