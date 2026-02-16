using System.Collections;

namespace CSGenerics
{
    internal class NonGenerics
    {

        /*
         
        ✅ ArrayList + int → boxing → heap
        ❌ ArrayList + string → no boxing
        ❌ ArrayList + custom class → no boxing



            ArrayList a = new ArrayList();
            a.Add(10);
            a.Add("Hi");

                    STACK                                  HEAP
            -----                                  ----
            a  ───────────────▶            [ ArrayList object ]
                                             |
                                             v
                                          [ object[] ]
                                           |       |
                                           v       v
                                     [ boxed 10 ]  [ "Hi" ]

         */
        public void Main2()
        {

            ArrayList arrayList = new ArrayList();

            arrayList.Add(1);

            arrayList.Add(true);

            arrayList.Add("TM"); // no boxing occurs here for reference type 'string'

            arrayList.Add(new Person("Alice", 30)); // no boxing occurs here for reference type 'Person'


            // UNBOXING

            int firstVal = (int)arrayList[0]; // Unboxing occurs here for value type 'int', and it can lead to runtime errors if the object is not actually an int.
            string? name = (string?)arrayList[2]; // need to convert the object back to string, and it can lead to runtime errors if the object is not actually a string.



            string str = null;
            arrayList.Add(str); // No boxing occurs here for reference type 'string', but it can lead to runtime errors if str is null or not a string when accessed later.


            List<int> list = new List<int>();
            list.Add(1); //type safety is maintained, no boxing occurs for value type '

            int nonnullable = 0;
            //nonnullable = null;


            int? nullable = 0;
            nullable = null; // this is valid because nullable int can hold null values
        }

    }

    /*
        Memory allocation in case of List<T>

        Actual objects (class instances) → stored on the heap

        Local variables that hold references to those objects → stored on the stack

        STACK                     HEAP
        -----                     ----
        obj  ───────────────▶   [ MyClass object ]

        so on the similar lines:

            STACK                               HEAP
        -----                               ----
        list  (reference)  ───────────▶   [ List<int> object ]



        List<int> list = new List<int>();
        list.Add(10);
        list.Add(20);

            STACK                               HEAP
        -----                               ----
        list ───────────────▶        [ List<int> object ]
                                       |
                                       | has field: _items              class List<T> { internal T[] _items; }
                                       v
                                    [ int[] array ]
                                    [ 10, 20, ... ]


     */

    public class Person
    {
        public string? Name { get; set; }
        public int Age { get; set; }
        public Person()
        {

        }
        public Person(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public override string ToString()
        {
            return $"Person(Name={Name.ToString()}, Age={Age})";
        }
    }
}
