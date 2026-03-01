using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyncProgrammingInCS
{

    // Task.Run is a method in the Task Parallel Library (TPL) that allows you to execute a piece of code asynchronously on a thread pool thread.
    // It is commonly used to offload CPU-bound work to a background thread, allowing the main thread to remain responsive.

    //Task.Run is specifically for CPU-heavy work that would freeze your main thread

    /*
         Task.Run(() => DoHeavyWork());
        //  │        │
        //  │        └─ the job you're handing off
        //  │
        //  └─ "hey thread pool, give me a free worker for this"
     */

    // Return type of Task.Run is Task, that is a receipt for the work you handed off, and you can await it to know when the work is done, or you can check its status to see if it's still running, completed, or faulted.

    /*
     Task t = Task.Run(() => DoWork());
        //      └─ "I promise this work will complete at some point"
        ```

        A promise has 3 possible states:
        ```
        Pending   → still running
        Completed → finished successfully  
        Faulted   → something went wrong


        Task t = Task.Run(() => DoWork());

        Console.WriteLine(t.Status);  // Pending? Completed? Faulted?

        await t;  // "wait here until the promise is fulfilled"
     */
    internal class TaskRun
    {
        public static async Task MainRun()
        {
            await DownloadAsync();

            await TaskAsReturn();

            /*
            Task work = Task.Run(() =>
            {
                // Simulate some CPU-bound work

            });

            if (work.Status == TaskStatus.RanToCompletion)
            {

            }

            await work;
            */

        }

        //  Task<T>   = a promise that something will finish AND give you a value of type T

        public static async Task<int> TaskWithValue()
        {
            int receiptVal = await Task.Run(() => { return 42; });

            return receiptVal;
        }

        public static async Task DownloadAsync()
        {
            Task download = Task.Run(() => DownloadTheFile());

            while (!download.IsCompleted)
            {
                Console.WriteLine("Still downloading...");
                await Task.Delay(500);
            }

            await download; // collect result / catch any errors

            // now i am sure here that my promise is fulfilled, so i can do something with the result or just continue with the next steps

            Console.WriteLine("download completed!");
        }

        private static async Task DownloadTheFile()
        {
            // Use Task.Delay instead of Thread.Sleep for true async waiting
            await Task.Delay(10000); // simulate 5 second download
        }

        /*
            Task download = Task.Run(() => DownloadFile());

            // show progress to user while downloading
            while (!download.IsCompleted)
            {
                Console.WriteLine("Still downloading...");
                await Task.Delay(500);
            }

            await download; // collect result / catch any errors
            ```

            ---

            **So the rule is simple:**
            ```
            await t  →  you STOP and wait right here until done
            t.Status →  you just PEEK without stopping         
         */


        /*
            Task - is a unit of work that can be scheduled and executed asynchronously.
            Task t = something, this task is a promise that something will finish at some point in the future which you can await on

            So task is both, lets take an example of microwave

            The microwave = Task

            Microwave doing the heating   = work being executed
            Microwave display/timer       = you can check status, wait for it


            Task t = Task.Run(() => DoCooking());
            ```

            Think of it exactly like:
            ```
            Task.Run(...)  →  pressing START on the microwave
            Task t         →  the microwave itself, now running
         */


        //If you return void, the caller has no way to know when your method is done
        //It's truly fire and forget

        public static async void TaskAsVoidReturn()
        {
            await Task.Delay(1000);
        }
        // --->>
        public static async Task TaskAsReturn()
        {
            await Task.Delay(1000);
            // no return statement needed
            // but caller can still await this method
            // its about the "ability to track completion", not about returning a value
        }

        /*
            Think of it like:
            ```
            void        →  "I'll do the work, don't bother tracking me"
            Task        →  "I'll do the work, here's a way to know when I'm done"
            Task<int>   →  "I'll do the work, here's a way to know when I'm done AND here's a value"
            ```         
         */
    }
}
