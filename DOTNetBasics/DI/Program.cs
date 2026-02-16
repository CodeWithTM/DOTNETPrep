using DI.PaymentApp;

namespace DI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PaymentClient paymentClient = new PaymentClient();
                paymentClient.Main5();

            Console.ReadLine();
        }
    }
}
