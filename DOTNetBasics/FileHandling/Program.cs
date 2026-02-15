namespace FileHandling
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string rootPath = @"C:\Users\tukar\OneDrive\Pictures";

            IEnumerable<string> dirs = Directory.EnumerateDirectories(rootPath);

            foreach (string dir in dirs) { 
            
            }

            string[] files = Directory.GetFiles(rootPath, "*.png", SearchOption.AllDirectories);


            //Path class provides methods for working with file and directory paths. It is a static class that cannot be instantiated. The methods in the Path class are used to manipulate strings that represent file paths, such as combining paths, getting file extensions, and retrieving directory names.
            Path.GetFullPath(files[0]);

             
            // The FileInfo class provides properties and instance methods for the creation, copying, deletion, moving, and opening of files, and aids in the creation of FileStream objects. It is used to perform operations on files and provides more detailed information about the file compared to the static File class.
            FileInfo fileInfo = new FileInfo(files[0]);



            Console.ReadLine();
        }
    }
}
