using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelPrograminginCS
{

    // Parallel class is a static class in the System.Threading.Tasks namespace that provides methods for parallel programming in C#. It includes methods for executing loops and other operations in parallel, as well as methods for managing tasks and handling exceptions. The Parallel class is part of the Task Parallel Library (TPL) and can be used to improve the performance of applications by leveraging multiple processors or cores to execute tasks simultaneously.

    // Parallel.For: Executes a for loop in which iterations may run in parallel.
    // Parallel.ForEach: Executes a foreach loop in which iterations may run in parallel.
    // Parallel.Invoke: Executes a set of actions in parallel.

    internal class ParallelClass
    {

        public static void MainP()
        {

            RunNormalForEach();

            RunParallelForEach();
        }

        public static void RunParallelFor()
        {
            Parallel.For(0, 10, i =>
            {
                Console.WriteLine($"Iteration {i} is running on thread {Thread.CurrentThread.ManagedThreadId}");
            });
        }

        public static void RunNormalForEach()
        {
            var items = Enumerable.Range(0, 10);

            foreach (var item in items)
            {
                Console.WriteLine($"Processing item {item} on thread {Thread.CurrentThread.ManagedThreadId}");
            }
        }
        public static void RunParallelForEach()
        {
            var items = Enumerable.Range(0, 10);

            ParallelLoopResult result =  Parallel.ForEach(items, item =>
            {
                Console.WriteLine($"Processing item {item} on thread {Thread.CurrentThread.ManagedThreadId}");
            });

            
        }

        public static void RunParallelInvoke()
        {
            Parallel.Invoke(
                () => Console.WriteLine($"Action 1 is running on thread {Thread.CurrentThread.ManagedThreadId}"),
                () => Console.WriteLine($"Action 2 is running on thread {Thread.CurrentThread.ManagedThreadId}"),
                () => Console.WriteLine($"Action 3 is running on thread {Thread.CurrentThread.ManagedThreadId}")
            );

            // Real life example of arranging a dinner party and getting everything ready

            Parallel.Invoke(
                () => Console.WriteLine("Preparing the main course"),
                () => Console.WriteLine("Setting the table"),
                () => Console.WriteLine("Making the dessert")
            );

            Parallel.Invoke(new ParallelOptions { MaxDegreeOfParallelism = 2 },
                new Action[]
                { 
                    PrepareMainCourse,
                    SetTable,
                    MakeDessert
                }
            );
        }

        public static void PrepareMainCourse()
        {
            Console.WriteLine("Preparing the main course");
        }

        public static void SetTable()
        {
            Console.WriteLine("Setting the table");
        }

        public static void MakeDessert()
        {
            Console.WriteLine("Making the dessert");
        }
    }
}
