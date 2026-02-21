namespace InterfaceWithDefaultMethods
{

    // Default interface methods allow you to provide a default implementation for a method in an interface. This means that if a class implements the interface but does not provide an implementation for the method, the default implementation will be used.
    // it is available from C# 8.0 and later versions, but in C# 12.0 it has been enhanced with more features like static abstract members, which allow you to define static methods in interfaces that can be implemented by classes.


    // before c# 8.0, if you wanted to add a new method to an existing interface, you would have to modify all the classes that implement that interface to provide an implementation for the new method. With default interface methods, you can add new methods to an interface without breaking existing implementations.


    // this helps us to add new functionality to an interface without breaking existing implementations, and it also allows us to provide a default implementation for a method in an interface, which can be useful in certain scenarios.

    internal class Program
    {
        static void Main(string[] args)
        {

            IMyInterface myClass = new MyClass();
            myClass.Method1(); // This will call the implementation in MyClass
            myClass.Method2(); // This will call the default implementation in IMyInterface

            Console.WriteLine("Hello, World!");

            IMyInterface myClass2 = new MyAnotherClass();
            myClass2.Method3();
        }
    }

    internal interface IMyInterface
    {
        void Method1();
        void Method2()
        {
            Console.WriteLine("This is a default implementation of Method2");
        }

        // now lets see if we want to add 1 more method to contract
        //void Method3(); // then it breaks all the existing consumers of this interface

        // so with default implementation we can provide implementation to this Method3 w/o breaking anyone

        void Method3() { }
    }

    public class MyClass : IMyInterface
    {
        public void Method1()
        {
            Console.WriteLine("This is the implementation of Method1");
        }
    }

    public class MyAnotherClass : IMyInterface
    {
        public void Method1() { 
            Console.WriteLine(); 
        }

        public void Method3() { 
            Console.WriteLine("another class can provide its own implementation.."); 
        }
    }
}
