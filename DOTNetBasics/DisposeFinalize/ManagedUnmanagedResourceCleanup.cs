using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DisposeFinalize
{

    // https://www.youtube.com/watch?v=nQCoy9csNk4&t=859s


    public class ConnectionAndMemory : IDisposable
    {
        private SqlConnection? _connection;
        private IntPtr _unmanagedMemoryChunk; // Pointer to unmanaged memory
        private bool _isDisposed = false; // To ensure Dispose is idempotent

        public ConnectionAndMemory(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            Console.WriteLine("Connection opened.");

            // Allocate unmanaged memory (dummy representation)
            _unmanagedMemoryChunk = Marshal.AllocHGlobal(1024); // Allocate 1KB
            Console.WriteLine("Unmanaged memory allocated.");
        }


        // Public Dispose method
        public void Dispose()
        {
            Console.WriteLine("Public Dispose called.");
            // Call the protected Dispose method with isDisposing = true
            Dispose(true);
            // Suppress finalization to prevent the finalizer from being called
            GC.SuppressFinalize(this);
        }

        // Protected virtual Dispose method to handle both managed and unmanaged resources
        protected virtual void Dispose(bool isDisposing)
        {
            // Check if Dispose has already been called
            if (_isDisposed)
            {
                Console.WriteLine("Dispose already called, returning.");
                return;
            }

            Console.WriteLine($"Protected Dispose called (isDisposing: {isDisposing}).");

            if (isDisposing)
            {
                // Free managed resources
                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                    Console.WriteLine("Managed connection closed and disposed.");
                }
            }

            // Free unmanaged resources
            if (_unmanagedMemoryChunk != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(_unmanagedMemoryChunk);
                _unmanagedMemoryChunk = IntPtr.Zero;
                Console.WriteLine("Unmanaged memory freed.");
            }

            _isDisposed = true; // Mark as disposed
        }

        // Dummy method to represent work with resources
        public void DoWork()
        {
            // Check if the object has been disposed
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ConnectionAndMemory));
            }
            Console.WriteLine("Doing some work with resources...");
            // In a real application, you would use _connection and _unmanagedMemoryChunk here
        }

        // Finalizer (destructor)
        ~ConnectionAndMemory()
        {
            Console.WriteLine("Finalizer called.");
            // Call the protected Dispose method with isDisposing = false
            Dispose(false);
        }
    }

    internal class ManagedUnmanagedResourceCleanup
    {
        public static void MainClient(string[] args)
        {
            string connectionString = "Server=RUTVI\\MSSQLSERVER01;Database=EMSDB;Trusted_Connection=True;TrustServerCertificate=True;"; // Replace with your actual connection string

            // Example of proper consumption using 'using' statement
            Console.WriteLine("n--- Running with 'using' statement ---");
            try
            {
                using (ConnectionAndMemory obj1 = new ConnectionAndMemory(connectionString))
                {
                    obj1.DoWork();
                } // Dispose() is automatically called here
                Console.WriteLine("Object 1's resources should be freed.");
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"Caught expected exception: {ex.Message}");
            }

            // Example of 'using' declaration (C# 8 and later)
            Console.WriteLine("n--- Running with 'using' declaration ---");
            {
                using ConnectionAndMemory obj2 = new ConnectionAndMemory(connectionString);
                obj2.DoWork();
            } // Dispose() is automatically called here at the end of the scope
            Console.WriteLine("Object 2's resources should be freed.");

            // Example of improper consumption (no dispose call)
            Console.WriteLine("n--- Running with no dispose call (expecting finalizer eventually) ---");
            ConnectionAndMemory obj3 = new ConnectionAndMemory(connectionString);
            obj3.DoWork();
            Console.WriteLine("Object 3's resources will only be freed by the finalizer (non-deterministic).");
            // For demonstration, force GC and finalization to see finalizer effect
            Console.WriteLine("Forcing Garbage Collection and waiting for finalizers...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            Console.WriteLine("Forced GC completed.");
        }
    }
}

/*
IDisposable (1:14): Deterministic Resource Management

Purpose: IDisposable is a .NET pattern for deterministic resource management (1:16). It allows developers to explicitly free up resources immediately, rather than waiting for the non-deterministic garbage collector (1:22).
Problem Demonstration: The video demonstrates a console application that crashes due to exhausting the SQL Server connection pool (4:27), illustrating the critical need for IDisposable. The application fails because new database connections are fetched (5:16) but not immediately released (5:33).
Solution: Calling Dispose(): The SqlConnection class implements IDisposable, which has a single Dispose() method (6:04). Calling Dispose() on the connection (6:32) immediately frees the database connection, resolving the resource exhaustion issue (6:38).
Consumer's Burden: The video stresses that it is the consumer's responsibility to call Dispose() on IDisposable objects; the garbage collector does not handle this (7:14).
Better Options: using Statement and Declaration (7:50):
The using statement (8:14) ensures that Dispose() is called automatically when the associated code block is exited, even if exceptions occur, adding exception handling (8:05).
The newer C# 8 using declaration (8:59) provides a more concise way to achieve the same, calling Dispose() at the end of the variable's scope. Both only work on IDisposable classes (9:30).
Finalizers (1:43): Non-Deterministic Safety Net

Purpose: Finalizers are a non-deterministic feature that provides a safety net for freeing unmanaged resources (1:43, 11:18) in cases where the developer failed to call Dispose(). The garbage collector itself does not know about or free unmanaged resources (11:01).
Implementation: A finalizer is simply a method named after the class, preceded by a tilde (~) (11:37). It must be parameterless with no return type (11:46).
Drawbacks: Finalizers add extra overhead to the garbage collection process and can cause objects to remain in memory much longer than they otherwise would (10:38). Generally, they should be avoided unless a class owns unmanaged resources (10:47).
Demonstration: The video shows a class owning both a managed (database connection) and unmanaged (memory chunk) resource. Initially, without IDisposable or a finalizer, resources are not freed (12:54). Implementing a finalizer demonstrates that unmanaged memory is eventually freed, but it's non-deterministic, requiring forced garbage collection to observe immediately (13:51).
Implementing the Standard IDisposable Pattern (14:56):

The video details the standard, often confusing, pattern for implementing IDisposable with finalizer interactions.
Protected Virtual Dispose(bool isDisposing) Method (15:30): This is the core of the pattern.
It centralizes the logic for freeing both managed and unmanaged resources (15:42).
It's virtual (16:02) to support class hierarchies, allowing derived classes to override it and clean up their own resources while calling the base implementation.
The isDisposing boolean parameter (16:45) is crucial:
If true (called from the public Dispose method), both managed and unmanaged resources can be safely freed.
If false (called from the finalizer), only unmanaged resources should be freed, as managed resources might already be garbage collected (17:21).
Public Dispose() Method (17:54): This method calls the protected Dispose(true) (18:01) to indicate explicit disposal. Crucially, it then calls GC.SuppressFinalize(this) (18:07). This prevents the finalizer from being executed for the object, improving performance by removing it from the finalization queue.
Best Practice: Avoiding Finalizer Calls (19:14): The video emphasizes that the ideal scenario is to always call Dispose() and suppress finalization, making the finalizer a rarely invoked safety net.
Refinements and Considerations:

Idempotency (19:43): Dispose() calls should be itempotent, meaning calling it multiple times should have no side effects. This is achieved by using a boolean flag (_isDisposed) to ensure resource freeing logic runs only once (20:06).
ObjectDisposedException (20:21): The video mentions the recommendation to protect public properties and methods with a check for disposal, throwing an ObjectDisposedException if called on a disposed object. However, it notes this often leads to significant boilerplate code and suggests tools like Fody Janitor (21:12) to automate such tedious implementations.
Conclusion (21:39):
The video concludes by summarizing that IDisposable allows for deterministic resource freeing, finalizers provide a safety net for unmanaged resources, and proper Dispose() implementation should suppress finalization. While acknowledging that the IDisposable pattern can seem complex and places a burden on developers (22:12), the speaker accepts it as a minor "wart" in an otherwise well-engineered .NET environment. 
 */




