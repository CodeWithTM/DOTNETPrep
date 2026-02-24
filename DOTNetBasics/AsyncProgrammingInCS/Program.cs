using System.ComponentModel;
using System.Diagnostics;

namespace AsyncProgrammingInCS
{

    // Basics of Threads and Tasks

    // Operting system
    // In windows OS we run multiple apps at the same time(Notepad, browser, VS), so these multipl apps run on a process, and each process has multiple threads,
    // and each thread can run a task, so we can say that a process is a container for threads, and a thread is a unit of execution, and a task is a unit of work that can be executed by a thread.

    // APP → PROCESS
    // An App is the software you install.Launching it creates one or more Processes. Chrome spawns a separate process per tab for isolation.

    // PROCESS → THREAD
    // A Process is a container.Every process has at least one thread.

    // THREAD ↔ TASK
    // A Task is a unit of work scheduled onto a Thread.Multiple Tasks can share one Thread pool, making async code efficient without blocking.
    internal class Program
    {
        static async Task Main(string[] args)
        {

            await TaskRun.MainRun();

            Console.WriteLine("Before calling delay");

            Console.ReadLine();

            await Task.Delay(500);

            Console.WriteLine("Resume execution..");


            Thread thread = new Thread(() =>
            {

            });

            thread.Start();

            ThreadStart threadStart = Method;

            Thread thread1 = new Thread(threadStart);

            thread1.Start();

            Console.ReadLine();
        }

        public static void Method()
        {
            Console.WriteLine("Going to sleep..");

            Thread.Sleep(1000);

            Console.WriteLine("sleep over!");
        }

        public static void ThreadSleep()
        {
            // Thread.Sleep — same thread before and after
            Console.WriteLine($"Before: Thread {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000);
            Console.WriteLine($"After:  Thread {Thread.CurrentThread.ManagedThreadId}");
            // Output:
            // Before: Thread 1
            // After:  Thread 1  ← always same
        }

        public static async Task TaskDelay()
        {
            // Task.Delay — may be different thread after await
            Console.WriteLine($"Before: Thread {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(2000);
            Console.WriteLine($"After:  Thread {Thread.CurrentThread.ManagedThreadId}");
            // Output:
            // Before: Thread 1
            // After:  Thread 4  ← could be any thread from pool
        }

        /*

        Thread 1:  ──── Task1 START ──── [freed] ·····················
        OS Timer:                         ←——— 500ms countdown ———→ 🔔[OS Timer]
        Thread 4:                                                    ──── Task1 END ────


        When you call await Task.Delay(500), what actually happens is:

            .NET registers a timer with the OS
            The OS kernel tracks that timer completely independently — no thread involved here
            Thread 1 is freed
            After 500ms, the OS fires a hardware interrupt
            .NET's scheduler sees it, picks an available thread (e.g. Thread 4) from the pool, and resumes your code

         */
    }
}
