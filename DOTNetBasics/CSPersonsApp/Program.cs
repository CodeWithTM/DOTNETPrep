using System.Reflection;

namespace CSPersonsApp
{
    internal class Program
    {

        // C# Attributes
        // Attributes are a way to add metadata to your code, which can be used by the compiler or at runtime.
        // Attributes by themselves do not change the behavior of your code, but they can be used to provide additional information or to modify the behavior of your code in some way.
        static void Main(string[] args)
        {

            JsonAttributes.Main2(args);
            LoggerApp.Main1();

            var type = typeof(Person);

            var attr = type.GetCustomAttribute<MyAttribute>();

            if (attr != null)
            {
                Console.WriteLine("Attribute found! Name = " + attr.Name);
                // "execute" something:
                Console.WriteLine("Doing something because of the attribute...");
            }
            
            Console.ReadLine();

        }
    }

    [My("TM")]
    public class Person { }

    // When this code is compiled, the compiler will generate metadata for the Person class that includes the MyAttribute attribute.

    // At runtime, you can use reflection to read this metadata and determine if the MyAttribute attribute is present on the Person class.
    // also .net runtime reads some metadata like [GetHttp] or some thrid party library reads metadata like [JsonPropertyName] etc.
    public class  MyAttribute : Attribute
    {

        public string Name { get; set; }
        public MyAttribute(string name) {
            Name = name;
        }

        public void LogMsg(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
