using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncProgrammingInCS
{

    // Working with Thread class constructors and delegates
    // Task.WhenAll vs Task.WaitAll: Non-blocking vs blocking waits for tasks

    internal class ThreadClassCtor
    {

        public static async Task NonBlockingWait()
        {
            Task t1 = Task.Run(async () => { 
                await Task.Delay(5000);
                Console.WriteLine("From task 1");  
            });
            Task t2 = Task.Run(async () => { 
                await Task.Delay(3000);
                Console.WriteLine("From task 2");  
            });
            
            await Task.WhenAll(t1, t2);  // This is the non-blocking way to wait for tasks (recommended in async code)
            // wait for both tasks WITHOUT blocking threads

            Task.WaitAll(t1, t2);  // This is the synchronous (blocking) way to wait for tasks (not recommended in async code)
            // does it block the main thread? Yes, it does! It will block the thread until both t1 and t2 complete, which is inefficient in an async context.

            /*
             await Task.WhenAll(t1, t2)
                         ↓
                .NET Runtime registers a CALLBACK:                  --> its like saying "Wake me up when BOTH are done"
                "when t1 AND t2 both complete → resume the code after this line"
                         ↓
                Main thread is released to thread pool
                         ↓
                t2 finishes at 3000ms  → Runtime checks: is t1 done? No → keep waiting
                t1 finishes at 5000ms  → Runtime checks: is t2 done? Yes → RESUME!      --> its like saying "Hey, t1 and t2 are done, go back and continue"
                         ↓
                A thread is picked from pool
                Console.WriteLine("in main") executes
             */
        }

        /*
            Task.WaitAll(t1, t2);        
            // main thread FROZEN, sitting here doing nothing
            // thread is wasted just waiting
            ```

            ---

            **Visual difference:**
            ```
            Task.WhenAll:

            Main Thread
                │
                │ await Task.WhenAll(t1,t2)
                │ [thread RELEASED]────────────────────────────► free to do other work
                │                                        both done ✓
                │ ◄──────────────────────────────────────────────
                │ continues...


            Task.WaitAll:

            Main Thread
                │
                │ Task.WaitAll(t1,t2)
                │ [thread FROZEN]━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓
                │                                        both done ✓┃
                │ ◄━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛
                │ continues...         
         */
        public static void MainTClass()
        {
            // ParameterizedThreadStart is a delegate that accepts one object parameter.
            // Use this when you want to pass data (like a number, string, or object) to your thread.
            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart((obj) =>
            {
                // 'obj' is the parameter passed when calling thread1.Start(10)
                Console.WriteLine($"Thread started with parameter: {obj}");
            });

            // Create a new thread using the parameterized delegate
            Thread thread1 = new Thread(parameterizedThreadStart);

            // Start() begins execution of thread1 and passes 10 as the parameter to the delegate above
            thread1.Start(10);


            // ThreadStart is a delegate that takes NO parameters.
            // Use this when your thread doesn't need to receive any data.
            ThreadStart threadStart = new ThreadStart(() =>
            {
                Console.WriteLine("Thread started without parameters.");
            });

            // Create another thread using the parameterless delegate
            Thread thread2 = new Thread(threadStart);

            // Start() begins execution of thread2 (no arguments passed)
            thread2.Start();

            // =============================================================================
            // SECTION 3: Thread.Join() - Synchronous Waiting
            // =============================================================================
            // IMPORTANT: If you DO NOT call Join(), the main thread may finish and exit
            // BEFORE thread1 and thread2 have completed their work!
            //
            // This is a RACE CONDITION:
            //   - Main thread (Thread 0) reaches end and program exits → process terminates
            //   - thread1 and thread2 may still be scheduled to run, but too late!
            //   - Result: You never see the output "Thread started with parameter: 10"
            //
            // Join() is the synchronous (blocking) way to wait for threads:
            //   Main Thread:  ────────── thread1.Join() ── BLOCKED ─ [thread1 completes] ── continue ────
            //   thread1:                       ▲ waiting here              │ finishes
            //                                   └─────────────────────────┘
            //
            // Uncomment the lines below to ensure threads complete before main exits:

            //thread1.Join();  // Main thread waits (blocks) until thread1 completes
            //thread2.Join();  // Main thread waits (blocks) until thread2 completes

            // After both Join() calls return, you are guaranteed both threads finished.

            // =============================================================================
            // ASYNC ALTERNATIVE: Task + await
            // =============================================================================
            // This synchronous Thread + Join() pattern is old (pre-.NET 4.5).
            // Modern C# uses async/await with Task for cleaner, non-blocking waits:
            //
            //   Task t1 = Task.Run(() => Console.WriteLine("From task 1"));
            //   Task t2 = Task.Run(() => Console.WriteLine("From task 2"));
            //   await Task.WhenAll(t1, t2);  // wait for both tasks WITHOUT blocking threads
            //
            // Key difference:
            //   Thread.Join()  → blocks a thread (wastes resources)
            //   await Task    → releases the thread while waiting (efficient)

        }

        // Retriving data from Thread using callback method

        public delegate void CallbackDelegate(string result);

        public static async Task ThreadWithReturn()
        {
            // Thread has no return type — you cannot do this
            Thread thread = new Thread(() =>
            {
                //return 42; // ❌ compile error, Thread doesn't support return values
            });

            //We can fix this by using a callback delegate to pass the result back to the main thread.

            Thread threadWithCallback = new Thread(() =>
            {
                // Simulate some work
                Thread.Sleep(2000);
                int result = 42; // The result we want to return
                OnWorkComplete(result); // Call the callback method with the result
            });

            threadWithCallback.Start();

        }

        // Step 1 — define the callback method
        static void OnWorkComplete(int result)
        {
            Console.WriteLine($"Callback called! Result = {result}");
        }
    }
}
