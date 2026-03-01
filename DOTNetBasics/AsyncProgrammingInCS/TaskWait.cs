using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyncProgrammingInCS
{
    internal class TaskWait
    {

        // GetAwaiter
        // GetResult

        // Task await -- Task
        // TaskAwaiter

        // Is the actual thread pulled when we await or when we execute Task.Run

        /*
            // What you write
            await Task.Delay(1000);

            // What compiler generates behind the scenes
            TaskAwaiter awaiter = Task.Delay(1000).GetAwaiter();
            awaiter.OnCompleted(() => {
                // resume here when done
            });
            ```

            `GetAwaiter()` is just the compiler's way of saying:

            > *"Give me an object that knows HOW to wait for you"*

            ---

            **Think of it like this:**
            ```
            Task         →  the job
            GetAwaiter() →  the manager who knows how to track that job
                            knows when it started
                            knows when it finished
                            knows where to resume         
         */

        public static async Task MainWait()
        {

            Task t1 = Task.Run(() => {
                Console.WriteLine($"Working on Thread {Thread.CurrentThread.ManagedThreadId}");
            });

            Console.WriteLine($"before await {Thread.CurrentThread.ManagedThreadId}");

            await t1.ConfigureAwait(true);

            // t1 is ALREADY running here, before await
            // thread was already picked above

            
            //await t1;  // just waiting, no new thread picked here

            Console.WriteLine($"after await {Thread.CurrentThread.ManagedThreadId}");

            Task<string> washCloths = Task.Run(async () => {

                await Task.Delay(5000);

                return "wet cloths";
            });



            //string taskResult = await washCloths;
            // 1. release thread
            // 2. wait for completion
            // 3. resume after done


            TaskAwaiter<string> washClothsAwaiter = washCloths.GetAwaiter();
            // PART 1 — GetAwaiter()
            // This just gets the "manager/tracker" object
            // NON blocking, nothing waits here


            string result = washClothsAwaiter.GetResult();  // ❌ blocking
            // PART 2 — GetResult()
            // THIS is the blocking part
            // Thread frozen until task completes

            washClothsAwaiter.OnCompleted(() =>
            {
                // this code only runs AFTER washCloths is completed
                // so GetResult() doesn't block — result is already there
                string tResult = washClothsAwaiter.GetResult();  // ✅ safe, not blocking
            });

            Task boilWater = new Task(() => { Console.WriteLine("boling water"); });

            boilWater.Start();
            await boilWater;

            // await doesnt work directly on the Task, it calls GetAwaiter() behind the scenes.

            // what compiler does behind the scenes is below

            TaskAwaiter boilWaterTaskAwaiter = boilWater.GetAwaiter();

            boilWaterTaskAwaiter.OnCompleted(async () => { 
                // resume here when done..
            });

            // So GetAwaiter() is - compiler way of saying - Give me an object that knows HOW to wait for you
            // taskawaiter is that object


            TaskAwaiter taskAwaiter = boilWater.GetAwaiter();

            // GetAwiater - is a

            await boilWater;
        }
    }
}
