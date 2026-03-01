using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncProgrammingInCS
{

    // Cancellation Token
    // CancellationTokenSource is the "cancel button" that you can press to signal cancellation.
    // CancellationToken is the "listener" that tasks can check to see if cancellation has been requested.
    // Tasks can periodically check the CancellationToken to see if they should stop doing work and exit early.
    // This allows you to gracefully stop long-running tasks without abruptly killing threads, which can lead to resource leaks or inconsistent state.
    // CancellationTokenSource and CancellationToken work together to provide a cooperative cancellation mechanism for tasks, allowing you to signal cancellation and have tasks respond to it in a controlled manner.
    internal class CToken
    {
        public static async Task MainCToken()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task longRunningTask = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancellation requested, stopping task.");
                        return;
                    }
                    Console.WriteLine($"Working... {i}");
                    Thread.Sleep(1000);
                }
            }, token);

            Console.WriteLine("Press any key to cancel...");
            Console.ReadKey();
            cts.Cancel();

            await longRunningTask;
        }

        public static async Task AnotherExampleAsync()
        {
            // Step 1 — create the source (cancel button)
            CancellationTokenSource cts = new CancellationTokenSource();

            // Step 2 — pass token to task (listener)
            Task t = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    // Step 3 — task checks if cancelled
                    cts.Token.ThrowIfCancellationRequested(); // this is same as checking if (token.IsCancellationRequested) and then throwing the exception manually, but this is a cleaner way to do it

                    Console.WriteLine($"Working... {i}");
                    Thread.Sleep(500);
                }
            }, cts.Token);

            // Step 4 — cancel after 2 seconds
            await Task.Delay(2000);
            cts.Cancel();  // pressing the cancel button

            try
            {
                await t;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Task was cancelled!");
            }
        }
    }
}
