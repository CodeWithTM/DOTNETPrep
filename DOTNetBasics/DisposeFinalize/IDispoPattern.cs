using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisposeFinalize
{

    // Managed resources vs Unmanaged resources
    // Managed resources are those that are handled by the .NET runtime's garbage collector. These include objects created using .NET classes,
    // such as strings, lists, and other data structures. The garbage collector automatically frees the memory used by these objects when they are no longer needed.
    // Unmanaged resources are those that are not handled by the garbage collector and require explicit cleanup. These include resources such as file handles,
    // database connections, network connections, and memory allocated using unmanaged code (e.g., using the Windows API or C++ code).
    // Unmanaged resources can lead to memory leaks and other issues if not properly released.
    internal class IDispoPattern
    {
        public static void MainDisp()
        {

            // another example
            using (DBConnection dbConnection = new DBConnection())
            {
                dbConnection.Open();
                // Use the database connection here
            } // Dispose will be called automatically at the end of this block, which will close the connection


            //MyResourceConsumer consumer = new MyResourceConsumer();
            //consumer.UseResource();

            DBConnector();
            
        }

        public static void DBConnector()
        {
            string connectionString = "Server=RUTVI\\MSSQLSERVER01;Database=EMSDB;Trusted_Connection=True;TrustServerCertificate=True;Max Pool Size=5;";

            Console.WriteLine("Opening connections without closing them...\n");

            List<SqlConnection> leakedConnections = new();

            try
            {
                for (int i = 1; i <= 10; i++)
                {
                    Console.WriteLine($"Opening connection {i}...");

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        //conn.Dispose(); // I am done using sql connection, just free up NOW

                        leakedConnections.Add(conn); // intentionally NOT closing
                        Console.WriteLine($"Connection {i} opened successfully");
                    } // <- this will make sure Dispose is called.. so need to call it explicitly
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ FAILED — Pool exhausted!");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void NetworkConnection()
        {
            HttpClient httpClient = new HttpClient();

            httpClient.Dispose();   // I am done using network connection, just damn free this up NOW!
        }
    }

    public class MyResource : IDisposable
    {
        private bool disposed = false; // To detect redundant calls
        // This method is called to release unmanaged resources
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Prevent finalizer from being called
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                }
                // Free any unmanaged resources here.
                disposed = true;
            }
        }
        ~MyResource()
        {
            Dispose(false); // Finalizer calls Dispose with false
        }
    }

    public class MyResourceConsumer
    {
        public void UseResource()
        {
            using (MyResource resource = new MyResource())
            {
                // Use the resource here
            } // Dispose will be called automatically at the end of this block
        }
    }

    // ------------------- another example -------------------


    // IDisposible is a deteministic way to release unmanaged resources, it provides a mechanism for releasing unmanaged resources explicitly,
    // rather than relying on the garbage collector to do it automatically. This is important because the garbage collector may not run immediately when an object is no longer needed, and if the object holds onto unmanaged resources, it can lead to memory leaks and other issues.
    
    
    public class DBConnection : IDisposable // Free them (i.e. obj of DBConnection) RIGHT NOW, dont wait for GC to do it, because it is holding some unmanaged resource (i.e. connection to database)
    {
        private bool disposed = false;
        public void Open()
        {
            Console.WriteLine("Opening database connection...");
        }
        public void Close()
        {
            Console.WriteLine("Closing database connection...");
        }
        public void Dispose()
        {
            Dispose(true);
            // If you have done yourself then dont call Garbage Collector to call finalizer
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Free any other managed objects here.
                    Close();
                }
                // Free any unmanaged resources here.
                disposed = true;
            }
        }

        // Finalizer (destructor) is called by the garbage collector when it determines that the object is no longer accessible.
        // It is used to release unmanaged resources if the Dispose method was not called explicitly. However, relying on the finalizer is not recommended because it may not run immediately, and it can lead to resource leaks if the object holds onto unmanaged resources for an extended period of time.
        // Finalizers are non-deterministic, meaning that you cannot predict when they will be called. They are typically used as a safety net to ensure that unmanaged resources are released if the Dispose method is not called explicitly.
        ~DBConnection() // DON'T know when it will be called, so it is non-deterministic, it is called by GC when it determines that the object is no longer accessible
        {
            // As GC calls finalizer when it determines that the object is no longer accessible, it is possible that the object is still holding onto unmanaged resources for an extended period of time, which can lead to resource leaks.
            // Therefore, it is recommended to call Dispose explicitly to release unmanaged resources in a timely manner.
            Dispose(false);
        }
    }
}
