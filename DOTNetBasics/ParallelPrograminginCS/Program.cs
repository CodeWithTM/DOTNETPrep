namespace ParallelPrograminginCS
{

    // Parallel programming is a programming paradigm that allows for the execution of multiple tasks simultaneously,
    // which can lead to improved performance and efficiency. In C#, you can use the Task Parallel Library (TPL) to easily implement parallel programming. The TPL provides a set of APIs for creating and managing tasks, which can be used to perform parallel operations.

    //Parallel programming is subset of multithreading. Multithreading allows for multiple threads to run concurrently, while parallel programming focuses on breaking down a task into smaller sub-tasks that can be executed simultaneously across multiple threads or processors.

    // Multicore is a type of computer architecture that has multiple processing units (cores) on a single chip. Each core can execute instructions independently, allowing for improved performance and efficiency when running multiple tasks simultaneously. Parallel programming can take advantage of multicore processors to further enhance performance by distributing tasks across multiple cores.
    // Multi processor is a computer architecture that has multiple physical processors (CPUs) installed in the system. Each processor can execute instructions independently, allowing for improved performance and efficiency when running multiple tasks simultaneously. Parallel programming can also take advantage of multi-processor systems to further enhance performance by distributing tasks across multiple processors.

    // How to leverage parallel programming in C#:
    // 1. Use the Task Parallel Library (TPL) to create and manage tasks.
    // 2. Use the Parallel class to execute loops and other operations in parallel.
    // 3. Use the async and await keywords to create asynchronous methods that can run in parallel.
    // 4. Use the Concurrent collections to safely share data between tasks.

    internal class Program
    {

        [ThreadStatic]
        static int counter = 0; // Each thread will have its own copy of the counter variable, and changes made to it in one thread will not affect the value in other threads.
        static void Main(string[] args)
        {

            ParallelClass.MainP();

            // PLINQ (Parallel LINQ) is a parallel implementation of LINQ (Language Integrated Query) that allows for the execution of LINQ queries in parallel. PLINQ can be used to improve the performance of LINQ queries by leveraging multiple processors or cores to execute the query in parallel.

            string str = "parallel programming in c#";

            var upperSTR = str.Select(x => x.ToString().ToUpper());

            var parallelUpperSTR = str.AsParallel().AsOrdered().Select(s => char.ToUpper(s));

            foreach (var item in parallelUpperSTR)
            {
                Console.Write(item);
            }
            // 3 steps internally:
            // 1. Partitioning: The input data is divided into smaller chunks that can be processed in parallel. break down in chunks
            // 2. Execution: Each chunk of data is processed in parallel across multiple threads or processors.
            // 3. Merging: The results from each parallel operation are combined to produce the final output.


            // Linq queries in parallel

            Console.ReadLine();
        }

        public static void IncrementCounter()
        {
            for (int i = 0; i < 1000; i++)
            {
                counter++;
            }
        }

        public static void RunParallelTasks()
        {
            Task task1 = Task.Run(() => IncrementCounter());
            Task task2 = Task.Run(() => IncrementCounter());
            Task.WaitAll(task1, task2);
            Console.WriteLine($"Counter value: {counter}"); // The counter value will be 0, as each thread has its own copy of the counter variable.
        }

        public static void WorkingWithStatic()
        {
            ThreadLocal<int> threadLocalCounter = new ThreadLocal<int>(() => 0); // Each thread will have its own instance of the counter variable, initialized to 0.
            Task task1 = Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    threadLocalCounter.Value++;
                }
                Console.WriteLine($"Task 1 counter value: {threadLocalCounter.Value}");
            });
        }
    }
}
