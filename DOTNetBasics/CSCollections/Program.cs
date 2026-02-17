using System.Collections;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace CSCollections
{
    public class Student : IComparable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Student otherStudent = obj as Student;
            if (otherStudent != null)
            {
                return this.Id.CompareTo(otherStudent.Id);
            }
            else
            {
                throw new ArgumentException("Object is not a Student");
            }
        }
    }
    internal class Program
    {

        //Generic vs non generic vs concurrent collections

        static void Main(string[] args)
        {

            List<int> intList = new List<int>() { 10, 20, 30, 40, 50 };

            // ForEach not available as Extension method, rather it is instance member
            intList.ForEach(i => Console.WriteLine(i));

            // Find vs FindAll vs Where
            // Find and FindAll works with List<T> only
            // Find, FindAll --> IMMEDIATE execution and NOT deferred like WHERE

            // Where -> return IEnumerable and deferred/lazy

            var firstEle = intList.First(i => i > 50); // Return type will be int and NOT IEnumerable . Note* Throws exception if no matching records found

            var greaterThan20 = intList.FindAll(i => i > 20);

            var greatherThanEnumerable = intList.Where(i => i > 20);

            

            //intList.FindAll

            //We can write our own extension method
            intList.ForEachEx(i => Console.WriteLine(i));

            Tricks.main2();

            string userId = "user-123";

            try
            {
                Parallel.For(0, 1_000_000, i =>
                {
                    RegisterHit_Dictionary(userId);
                });

                Console.WriteLine($"Dictionary result: {hits[userId]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Dictionary crashed!");
                Console.WriteLine(ex.GetType().Name + ": " + ex.Message);
            }

            /*
            GenericCollections();

            UseDictionary();

            WorkingWithHashTable();

            RemoveDuplicatesFromList();

            GetEvenNumbers();
            */
            //SimulateConcurrentCalls();
            Console.ReadLine();
        }

        static Dictionary<string, int> hits = new();

        static ConcurrentDictionary<string, int> hitsConn = new();

        // Replace this with concurrent dictionary

        private static void SimulateConcurrentCalls()
        {
            string userId = "user-123";

            try
            {
                Parallel.For(0, 1_000_000, i =>
                {
                    RegisterHit_Dictionary(userId);
                });

                Console.WriteLine($"Dictionary result: {hits[userId]}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Dictionary crashed!");
                Console.WriteLine(ex.GetType().Name + ": " + ex.Message);
            }
        }


        static void RegisterHit_Dictionary(string userId)
        {
            if (!hits.ContainsKey(userId))
                hits[userId] = 0;

            hits[userId]++;
        }

        public static void GenericCollections()
        {

            HashSet<int> list = new HashSet<int>();
            //stores only unique values, so if we try to add duplicate value, it will not be added to the collection

            LinkedList<string> linkedList = new LinkedList<string>();
            linkedList.AddLast("Home");
            linkedList.AddFirst("Welcome");
            linkedList.AddAfter(linkedList.First, "to");
            linkedList.AddBefore(linkedList.Last, "my");

            // --> stored as "Welcome to my Home"

            SortedList<int, string> students = new SortedList<int, string>();
            students.Add(3, "C");
            students.Add(1, "A");
            students.Add(2, "B");

            Console.WriteLine(students[1]); // A

            //Stored as 1,A 2,B 3,C
            //because sorted list stores the elements in sorted order based on the keys. In this case, the keys are 1, 2, and 3, so the elements are stored in ascending order of the keys.

            SortedList<string, int> sortedStr = new SortedList<string, int>();
            sortedStr.Add("3", 3);
            sortedStr.Add("1", 1);
            sortedStr.Add("2", 2);
            //sortedStr.Add("1", 4);  // This will throw an exception as keys must be unique in a sorted list


            // It sorts based on the lexicographical order of the keys. In this case, the keys are "1", "2", and "3", so the elements are stored in ascending order of the keys as "1", "2", and "3". Therefore, the sorted list will be stored as:


            SortedList<Student, int> keyValuePairs = new SortedList<Student, int>();

            keyValuePairs.Add(new Student { Id = 2, Name = "B" }, 2);
            keyValuePairs.Add(new Student { Id = 1, Name = "A" }, 1);
            keyValuePairs.Add(new Student { Id = 3, Name = "C" }, 3);
            // This will throw an exception as keys must be unique in a sorted list and also the Student class does not implement IComparable interface, so it cannot be compared to determine the order of the keys in the sorted list. To use a custom class as a key in a sorted list, you need to implement the IComparable interface and provide a comparison method that defines how the keys should be compared and sorted.


            List<int> intList = new List<int>() { 10, 20, 30, 40, 50 };


            intList.ForEach(i => Console.WriteLine(i));

            intList.ForEachEx(i => Console.WriteLine(i));
        }

        public static void RemoveDuplicatesFromList()
        {
            List<int> numbers = new List<int> { 6, 1, 2, 3, 2, 4, 1, 5, 7 };
            // Using HashSet to remove duplicates
            HashSet<int> uniqueNumbers = new HashSet<int>(numbers);
            // Convert back to list if needed
            List<int> result = uniqueNumbers.ToList();
            Console.WriteLine(string.Join(", ", result)); // Output: 1, 2, 3, 4, 5


            // This removes only first occurance of given element and NOT all
            numbers.Remove(2);
            numbers.Remove(2);
            numbers.Remove(2); // This will not throw any exception

            numbers.RemoveAll(n => n == 2); // In order to remove all occurances of 2 from the list, we can use RemoveAll method and pass a predicate that checks for the value to be removed.
        }

        public static void WorkingWithHashTable()
        {
            Hashtable ht = new Hashtable(); // non generic equivalent of dictionary
            ht.Add("a", 1);
            ht.Add("b", 2);
            //ht.Add("a", 3);       // NOT allowed as keys must be unique in a hashtable, this will throw an exception

            //ht.Add("b", 3);
        }

        public static void GetEvenNumbers()
        {
            List<int> numbers = new List<int> { 6, 1, 2, 3, 2, 4, 1, 5, 7 };

            var filteredList = numbers.Where(n => n % 2 == 0);

            numbers.Add(8);

            foreach (int item in filteredList)
            {
                Console.WriteLine(item);
            }

        }

        public static void UseDictionary()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            dict.Add("apple", 1);
            dict.Add("banana", 2);
            // Updating an item in the dictionary
            dict["apple"] = 3;
            // Removing an item from the dictionary
            dict.Remove("banana");
            // Accessing items in the dictionary
            if (dict.TryGetValue("apple", out int value))
            {
                Console.WriteLine($"Value for 'apple': {value}");
            }

            dict["mango"] = 10;     // if the key does not exist, it will be added to the dictionary with the specified value. If the key already exists, its value will be updated to the new value provided.
        }

        public static void UseConcurrentDictionary()
        {
            ConcurrentDictionary<string, int> concurrentDict = new ConcurrentDictionary<string, int>();
            // Adding items to the concurrent dictionary
            concurrentDict.TryAdd("apple", 1);
            concurrentDict.TryAdd("banana", 2);
            // Updating an item in the concurrent dictionary
            concurrentDict.AddOrUpdate("apple", 1, (key, oldValue) => oldValue + 1);
            // Removing an item from the concurrent dictionary
            concurrentDict.TryRemove("banana", out _);
            // Accessing items in the concurrent dictionary
            if (concurrentDict.TryGetValue("apple", out int value))
            {
                Console.WriteLine($"Value for 'apple': {value}");
            }
        }
    }

    public static class ListExtension
    {
        public static void ForEachEx(this List<int> collection, Action<int> actions)
        {

            if (actions == null)
                throw new ArgumentNullException();

            for (int i = 0; i < collection.Count; i++)
            {
                actions(collection[i]);
            }
        }
    }
}
