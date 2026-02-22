using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisposeFinalize
{

    public class FileManager
    {
        private FileStream _file;

        public FileManager()
        {
            _file = new FileStream("log.txt", FileMode.Open);
            Console.WriteLine("File opened");
        }

        // Finalizer — GC calls this before collecting the object
        ~FileManager()
        {
            _file?.Close();
            Console.WriteLine("Finalizer called — file closed");
        }
    }

    /*
        // GC sees this
        FileStream _file; // a managed object on the heap ✅ GC knows about this

        // GC does NOT see this
        // the actual OS file handle that _file wraps internally ❌ GC has no idea
        ```

        GC cleans up the `FileStream` wrapper object — but the underlying OS handle needs to be explicitly released by calling `Close()` or `Dispose()`. GC cannot do that for you.

        ---

        ## Step 5 — GC Generations (why finalizers are slow)

        GC organizes objects into three generations:
        ```
        Generation 0 — new, short-lived objects (collected most frequently)
        Generation 1 — survived one GC collection
        Generation 2 — long-lived objects (collected least frequently)   
    

        // without finalizer — object collected quickly in Gen 0
        var simple = new Employee();

        // with finalizer — object lives longer, survives extra GC cycles
        // before finalizer is called and memory is eventually freed
        var managed = new FileManager();
     */
    internal class DispoFinalizer
    {
    }

    public class EmployeeRepository : IDisposable
    {
        private SqlConnection _connection;
        private bool _disposed = false; // tracks if we already cleaned up

        public EmployeeRepository(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        public void GetEmployee(int id)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(EmployeeRepository));

            using SqlCommand cmd = new SqlCommand("SELECT * FROM Employees WHERE Id = @id", _connection);
            cmd.Parameters.AddWithValue("@id", id);

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine($"Employee: {reader["Name"]}");
            }
        }

        // ============================================
        // PUBLIC Dispose — called by consumer via
        // using block or manually calling .Dispose()
        // ============================================
        public void Dispose()
        {
            Dispose(true);

            // tells GC — "don't bother calling the finalizer
            // we already cleaned up manually"
            GC.SuppressFinalize(this);
        }

        // ============================================
        // PROTECTED Dispose(bool) — actual cleanup logic
        // disposing = true  → called by us (Dispose method)
        // disposing = false → called by GC (finalizer)
        // ============================================
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return; // already cleaned up — don't do it twice

            if (disposing)
            {
                // cleanup managed resources that implement IDisposable
                // we know _connection is managed wrapper so clean it here
                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }
            }

            // if you had unmanaged resources (raw OS handles etc)
            // you would release them here — outside the if(disposing) block
            // because GC calling finalizer also needs to clean unmanaged stuff

            _disposed = true;
        }

        // ============================================
        // FINALIZER — safety net
        // only runs if consumer forgot to call Dispose
        // GC calls this as last resort
        // ============================================
        ~EmployeeRepository()
        {
            Dispose(false); // false = called by GC, not by us
        }
    }
}
