
namespace AsyncProgrammingInCS
{
    // TAP
    // Task-based Asynchronous Programming
    internal class TAPBasedProg
    {
        public static async Task MainTAP()
        {
            // Use Task<string> to capture the return value from FileDownload
            // The lambda () => FileDownload("document.pdf") is a Func<string>
            Task<string> downloadFile = new Task<string>(() => FileDownload("document.pdf"));

            // Start the task on a thread pool thread
            downloadFile.Start();

            // Await the task and capture the result
            string result = await downloadFile;
            Console.WriteLine(result);

            // Pass parameters through the lambda closure
            int num1 = 15;
            int num2 = 25;

            Task<int> summationTask = new Task<int>(() => Add(num1, num2));
            summationTask.Start();

            int sum = await summationTask;
            Console.WriteLine($"Sum of {num1} + {num2} = {sum}");


            Task<int> summa = Task.Run(() => Add(num2 , num1));

            int s = await summa;

        }

        public static string FileDownload(string file)
        {
            Thread.Sleep(5000);

            return $"{file} downloaded successfully";
        }

        // Helper method for summation
        public static int Add(int a, int b)
        {
            return a + b;
        }

        // ============================================================================
        // ALTERNATIVE: Using Task.Run (Modern and Cleaner Approach)
        // ============================================================================

        // Task.Run is a convenient wrapper around new Task() + Start()
        // It automatically:
        //   1. Creates the task
        //   2. Starts it on a thread pool thread
        //   3. Returns the running task immediately
        // This is the preferred modern way to create background tasks
        public static async Task DemonstrateTaskRun()
        {
            Console.WriteLine("--- Using Task.Run ---");

            // Task.Run starts the task immediately, no need to call .Start()
            Task<string> downloadFile = Task.Run(() => FileDownload("image.png"));

            // Await the result
            string result = await downloadFile;
            Console.WriteLine(result);
        }

        /*
         new Task<T>(Func<T>)     →  Create task (not started), must call .Start()
         └─ More verbose, explicit control

         Task.Run(Func<T>)        →  Create + Start in one line
         └─ Cleaner, modern approach (recommended)
         */
    }
}
