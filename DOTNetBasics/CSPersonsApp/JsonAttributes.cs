using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSPersonsApp
{

    // Real life example of C# attributes is JsonPropertyNameAttribute
    // JsonPropertyNameAttribute is used to specify the name of a property when serializing and deserializing JSON.
    // It is defined in the System.Text.Json.Serialization namespace and is used with the System.Text.Json.JsonSerializer class.
    internal class JsonAttributes
    {
        public static void Main2(string[] args)
        {
            // Example usage of JsonPropertyName attribute
            Student student = new Student { Id = 1, Name = "John Doe", EmailAdd = "j.d@ms.com" };
            string json = System.Text.Json.JsonSerializer.Serialize(student);

            // on .Serialize it doesn something as below --

            Type t = typeof(Student);

            PropertyInfo[] props = t.GetProperties();

            foreach (PropertyInfo prop in props)
            {
                var attr = prop.GetCustomAttribute<JsonPropertyNameAttribute>();

                // my extension method
                JsonPropertyNameAttribute? j = prop.GetMyCustomAttribute<JsonPropertyNameAttribute>();
                
            }
            

            Console.WriteLine(json); // Output: {"FullName":"John Doe","Id":1}
        }
    }

    //Extension method example 

    public static class PropInfoExtensions
    {
        public static T? GetMyCustomAttribute<T>(this PropertyInfo prop) where T : Attribute
        {
            // This method is used to get the custom attribute of type T from the property info
            return prop.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }
    }

    public class Student
    {
        
        public int Id { get; set; }

        [JsonPropertyName("FullName")]      // Compliler just stores metadata. METADATA:  “Property Person.Name has attribute JsonPropertyNameAttribute("FullName")”
        public string? Name { get; set; }

        //JSON serializer library reads that metadata at runtime and changes the behavior according
        //e.g. in this case change the property name from "Name" to "FullName" when serializing/deserializing

        [JsonIgnore]
        public string? EmailAdd { get; set; }

    }

    // so if we see all there attributes like JsonPropertyName, JsonIgnore, etc. are coming from library System.Text.Json.Serialization
    /*
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class JsonPropertyNameAttribute : JsonAttribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="JsonPropertyNameAttribute"/> with the specified property name.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        public JsonPropertyNameAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The name of the property.
        /// </summary>
        public string Name { get; }
    }
    */
}
