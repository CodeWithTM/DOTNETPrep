using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSCollections
{
    internal class Tricks
    {
        public static void main2() { 
        
        
            List<int> list = new List<int>() { 1,3,5,6,3,4,6,7,85,3,4,5};

            // help me create a list of integer with 100 elements

            var filteredList = list.Where(x =>
            {
                return x > 0;
            });


            //Multiple enumerations of filteredList will cause performance issues

            Console.WriteLine( filteredList.Count() );

            Console.WriteLine( filteredList.First() );

            // This will iterate the collection twice

            var filteredListNew = list.Where(x => x>0).ToList();

            Console.WriteLine(filteredListNew.Count);
            Console.WriteLine(filteredListNew[0]);

            //modifying collection while iterating over

            foreach (var item in list.ToList()) // just by changing list --> list.ToList() we can avoid the exception because we are iterating over a copy of the list, not the original list that we are modifying.
            {
                if(item > 80)
                    list.Remove(item);
            }

            // Another FIX: We can iterate in reverse order to safely remove items from the list while iterating.
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] > 80)
                    list.RemoveAt(i);
            }

            Console.WriteLine(list.Count); // BEST as its property on List<T> and does not require iteration to calculate the count. O(1)

            Console.WriteLine(list.Count()); // Extension method on IEnumerable.  if list if of type IEnumerable<T>, Count() will iterate through the collection to count the elements, which can be O(n) if the underlying collection does not have a Count property.


            //.Length vs .Count vs Count() .Length is a property of arrays and some collections, while Count is a property of collections that implement ICollection<T>. Count() is an extension method for IEnumerable<T> that counts the number of elements in the collection. Length is O(1) for arrays, Count is O(1) for collections that implement ICollection<T>, and Count() can be O(n) for collections that do not have a Count property.

            


            var peoples = new List<Person>()
            {
                new Person() { Id = 1, Name = "Alice" },
                new Person() { Id = 2, Name = "Bob" },
                new Person() { Id = 3, Name = "Charlie" }
             };

            peoples[1].Name = "Bobby"; // As it holds reference to the object, we can modify the properties of the object even if the list itself is not modified.

            foreach (var item in peoples)
            {
                Console.WriteLine( item.Name);
            }


            ImmutableList<Person> immutablePersons = ImmutableList.CreateRange(peoples);
            immutablePersons[1].Name = "Bobby"; // This will throw an exception because the list is immutable


            //IEnumerable, ICollection, IList are interfaces in C# that represent different levels of collection functionality.
            //IEnumerable allows for iteration over a collection,
            //ICollection allows for modification of the collection,
            //and IList allows for both iteration and modification, as well as access by index.
            //The choice of which interface to use depends on the specific requirements of your application and the level of functionality you need from the collection.
            /*
             IEnumerable → iterate

            ICollection → iterate + modify

            IList → iterate + modify + index
             */

        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    } 
}
