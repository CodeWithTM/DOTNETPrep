using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CSGenerics
{
    // Generics in C# allow you to define classes, methods, and data structures with a placeholder for the type of data they store or use.
    // This enables you to create reusable code that can work with any data type while maintaining type safety.
    // Generics are particularly useful for collections, such as lists and dictionaries,
    // where you want to specify the type of elements they contain without sacrificing performance or safety.
    // In C#, you can define a generic class or method using angle brackets (<>) to specify the type parameter.


    //Generics vs Non-Generics:
    //List<T> vs ArrayList:
    //List<T> is a generic collection that can store elements of a specific type, while ArrayList is a non-generic collection
    //that can store elements of any type as objects. List<T> provides better performance and type safety compared to ArrayList,
    //which requires boxing and unboxing for value types and can lead to runtime errors if the wrong type is accessed.


    internal class Program
    {
        static void Main(string[] args)
        {

            NonGenerics nonGenerics = new NonGenerics();    

            nonGenerics.Main2();

            GenericRepositoryExample example = new GenericRepositoryExample();
            example.Main4();

            Console.WriteLine("Hello, World!");

            int x = 5; int y = 15; int z = 0;
            Swap<int>(ref x, ref y); // Example of using the generic swap method with integers


            string s1 = new string('A', 3); // "AAA"
            string s2 = new string('B', 3); // "BBB"

            Console.WriteLine("Before Swap:");
            Print("s1", s1);
            Print("s2", s2);

            Swap(ref s1, ref s2);

            Console.WriteLine("\nAfter Swap:");
            Print("s1", s1);
            Print("s2", s2);

            List<int> list = new List<int>();
            list.Add(x); //type safety is maintained, no boxing occurs for value type 'int'

            ArrayList arrayList = new ArrayList();
            arrayList.Add(z); // Boxing occurs here for value type 'int'
            arrayList.Add("y"); // No boxing occurs here for reference type 'string'

            int? firstVal = (int?)arrayList[0]; // Unboxing occurs here for value type 'int', and it can lead to runtime errors if the object is not actually an int.

            string? str = null;
            arrayList.Add(str); // No boxing occurs here for reference type 'string', but it can lead to runtime errors if str is null or not a string when accessed later.

        }

        public static void GenericMethod<T>(T parameter)
        {
            Console.WriteLine($"The type of the parameter is: {typeof(T)}");
            Console.WriteLine($"The value of the parameter is: {parameter}");
        }

        //Example of a generic swap method that can swap two values of any type:
        public static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        static void Print(string label, string value)
        {
            // RuntimeHelpers.GetHashCode gives identity-based hashcode,
            // meaning it does NOT use string.GetHashCode() (which is value-based).
            Console.WriteLine($"{label} -> Value={value}, Identity={RuntimeHelpers.GetHashCode(value)}");
        }

    }

    //create a generic class that can hold a value of any type and provide a method to display that value:
    public class GenericHolder<T>
    {
        private T value;
        public GenericHolder(T value)
        {
            this.value = value;
        }
        public void DisplayValue()
        {
            Console.WriteLine($"The value is: {value}");
        }
    }

    // create another generic class lets say Store which can be used to store Products like Book / Mobile. so Product being abstract class
    // and Book and Mobile being concrete classes inheriting from Product. Store class should be able to store any type of Product and provide a method to display the details of the stored product.

    public abstract class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
        public abstract void DisplayDetails();
    }

    public class Book : Product
    {
        public string Author { get; set; }
        public Book(string name, decimal price, string author) : base(name, price)
        {
            Author = author;
        }
        public override void DisplayDetails()
        {
            Console.WriteLine($"Book: {Name}, Price: {Price}, Author: {Author}");
        }
    }

    public class Mobile : Product
    {
        public string Brand { get; set; }
        public Mobile(string name, decimal price, string brand) : base(name, price)
        {
            Brand = brand;
        }
        public override void DisplayDetails()
        {
            Console.WriteLine($"Mobile: {Name}, Price: {Price}, Brand: {Brand}");
        }
    }

    public class Store<T> where T : Product
    {
        private T product;
        public Store(T product)
        {
            this.product = product;
        }
        public void DisplayProductDetails()
        {
            product.DisplayDetails();
        }
    }
}
