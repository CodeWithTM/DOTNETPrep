using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionHandling
{
    public class OrderingApp
    {
        public static void Main4()
        {
            Console.WriteLine("=== App Started ===");

            // TOP-LEVEL TRY/CATCH:
            // This is the correct place to catch exceptions so:
            // 1) App doesn't crash
            // 2) We can show a clean message
            // 3) Main can continue executing other code
            try
            {
                int orderId = PlaceOrder();
                Console.WriteLine($"Order placed successfully. OrderId = {orderId}");
            }
            catch (Exception ex)
            {
                // If order fails, we catch here and Main continues.
                Console.WriteLine("Order failed!");
                PrintException(ex);
            }

            // This code should execute EVEN IF order fails.
            Console.WriteLine("Main continues doing other work...");
            Console.WriteLine("=== App Ended ===");
        }

        static int PlaceOrder()
        {
            Console.WriteLine("\n--- PlaceOrder Started ---");

            // Step 1 (CRITICAL):
            // If validation fails, the order must stop.
            // We do NOT catch here because we cannot recover.
            ValidateCart();

            // Step 2 (DEPENDENCY + FALLBACK):
            // Discount is useful but not worth failing the whole order.
            // If discount service fails, fallback to 0.
            decimal discount = 0;
            try
            {
                discount = CalculateDiscount();
            }
            catch (Exception ex)
            {
                // We catch ONLY because we have a fallback plan.
                Console.WriteLine("Discount service failed. Using discount = 0.");
                Console.WriteLine("Reason: " + ex.Message);
                discount = 0;
            }

            // Step 3 (CRITICAL + DEPENDS ON discount):
            // If payment fails, order must stop.
            // We do NOT catch here.
            string paymentId = ChargePayment(discount);

            // Step 4 (CRITICAL):
            // If saving fails, order must stop.
            // We do NOT catch here.
            int orderId = SaveOrderToDatabase(paymentId);

            // Step 5 (OPTIONAL):
            // Email failing should NOT cancel the order.
            // So we catch here and continue.
            try
            {
                SendConfirmationEmail(orderId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email failed but order is placed. Continuing...");
                Console.WriteLine("Reason: " + ex.Message);
            }

            // Step 6 (OPTIONAL):
            // Analytics failing should NOT cancel the order.
            // So we catch and ignore.
            try
            {
                LogAnalytics(orderId);
            }
            catch
            {
                Console.WriteLine("Analytics failed. Ignoring...");
            }

            Console.WriteLine("--- PlaceOrder Finished ---\n");
            return orderId;
        }

        static void ValidateCart()
        {
            Console.WriteLine("Validating cart...");

            // Simulating cart validation success.
            // If you want to test failure, uncomment this line:
            // throw new Exception("Cart is empty.");

            Console.WriteLine("Cart validation OK.");
        }

        static decimal CalculateDiscount()
        {
            Console.WriteLine("Calculating discount...");

            // Simulating a discount service failure.
            // This is a NON-CRITICAL failure (we fallback to 0).
            //throw new Exception("Discount API timeout.");

            return 10; // example discount
        }

        static string ChargePayment(decimal discount)
        {
            Console.WriteLine($"Charging payment (discount = {discount})...");

            try
            {
                // Simulating payment gateway failure.
                // This is CRITICAL (order must fail).
                throw new InvalidOperationException("Payment gateway rejected transaction.");

                //return Guid.NewGuid().ToString(); // example paymentId
            }
            catch (Exception ex)
            {
                // IMPORTANT:
                // We are NOT swallowing the exception.
                // We are wrapping it to add business meaning.
                // InnerException keeps the original root cause.
                throw new Exception("Payment failed while placing the order.", ex);
            }
        }

        static int SaveOrderToDatabase(string paymentId)
        {
            Console.WriteLine($"Saving order to database (paymentId = {paymentId})...");

            // Simulating success.
            return new Random().Next(1000, 9999);
        }

        static void SendConfirmationEmail(int orderId)
        {
            Console.WriteLine($"Sending confirmation email for order {orderId}...");

            // Simulating email failure (OPTIONAL step).
            throw new Exception("SMTP server not reachable.");
        }

        static void LogAnalytics(int orderId)
        {
            Console.WriteLine($"Logging analytics for order {orderId}...");

            // Simulating analytics success.
            // If you want to test failure:
            // throw new Exception("Analytics service down.");
        }

        static void PrintException(Exception ex)
        {
            Console.WriteLine("\n--- Exception Details ---");
            Console.WriteLine("Message: " + ex.Message);
            Console.WriteLine("Type: " + ex.GetType().Name);

            // Shows InnerException concept:
            // If we wrapped exceptions properly, we will see the real root cause here.
            if (ex.InnerException != null)
            {
                Console.WriteLine("\nInner Exception:");
                Console.WriteLine("Message: " + ex.InnerException.Message);
                Console.WriteLine("Type: " + ex.InnerException.GetType().Name);
            }
            else
            {
                Console.WriteLine("\nInner Exception: NULL");
            }

            Console.WriteLine("\nStack Trace:");
            Console.WriteLine(ex.StackTrace);

            Console.WriteLine("--------------------------\n");
        }
    }
}



