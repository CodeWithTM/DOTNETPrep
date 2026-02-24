using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisposeFinalize
{
    internal class GCDemo
    {
        // Simple demo: large for-loop that allocates many small arrays on the heap.
        // Run, attach Diagnostic Tools, then press Enter to start the allocations.
        public static void MainGC(string[] args)
        {
            Console.WriteLine("Attach Diagnostic Tools (Memory/GC). Press Enter to start allocating.");
            Console.ReadLine();

            var keepAlive = new List<byte[]>();

            // Adjust 'iterations' and 'size' to increase/decrease pressure.
            int iterations = 300_000; // many objects -> Gen0/1/2 activity
            int size = 1024;          // 1 KB each

            for (int i = 0; i < iterations; i++)
            {
                keepAlive.Add(new byte[size]); // allocate on heap
                if ((i + 1) % 50 == 0)
                {
                    Console.WriteLine($"Allocated {i + 1} objects (~{((long)(i + 1) * size) / 1024 / 1024} MB).");
                    Thread.Sleep(200); // give Diagnostic Tools time to update
                }
            }

            Console.WriteLine("Allocation loop finished. Press Enter to force a GC.");
            Console.ReadLine();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            Console.WriteLine($"GC counts: Gen0={GC.CollectionCount(0)}, Gen1={GC.CollectionCount(1)}, Gen2={GC.CollectionCount(2)}");
            Console.WriteLine("Inspect the heap now. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
