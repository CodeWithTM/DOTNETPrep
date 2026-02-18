using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSPersonsApp
{

    // Lets write some custom logic to log method calls
    // We will use DispatchProxy to create a proxy class that intercepts method calls
    internal class LoggerApp
    {
        public static void Main1()
        {
            ICalculator calc = LoggingProxy<ICalculator>.Create(new Calculator());

            Console.WriteLine(calc.Add(2, 3));        // logs
            Console.WriteLine(calc.Multiply(4, 5));   // does NOT log
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class LoggerAttribute : Attribute
    {
    }

    public interface ICalculator
    {
        [Logger]
        int Add(int a, int b);

        int Multiply(int a, int b);
    }

    public class Calculator : ICalculator
    {
        public int Add(int a, int b) => a + b;
        public int Multiply(int a, int b) => a * b;
    }

    public class LoggingProxy<T> : DispatchProxy
    {
        private T? _decorated;

        public static T Create(T decorated)
        {
            object proxy = Create<T, LoggingProxy<T>>();
            ((LoggingProxy<T>)proxy)._decorated = decorated;
            return (T)proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            // Check if the method has [Logger]
            bool hasLogger = targetMethod.GetCustomAttribute<LoggerAttribute>() != null;

            if (hasLogger)
            {
                // This is the "caller info" best we can do in managed code
                var caller = new StackTrace().GetFrame(2)?.GetMethod();

                Console.WriteLine($"[LOG] Calling: {targetMethod.DeclaringType.Name}.{targetMethod.Name}");
                Console.WriteLine($"[LOG] Args: {string.Join(", ", args.Select(a => a?.ToString() ?? "null"))}");

                if (caller != null)
                {
                    Console.WriteLine($"[LOG] Called from: {caller.DeclaringType?.Name}.{caller.Name}");
                }
            }

            // Call the real method
            var result = targetMethod.Invoke(_decorated, args);

            if (hasLogger)
            {
                Console.WriteLine($"[LOG] Result: {result}");
                Console.WriteLine();
            }

            return result;
        }
    }
}
