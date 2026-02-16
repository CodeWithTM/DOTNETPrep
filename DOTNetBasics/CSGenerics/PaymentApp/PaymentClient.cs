using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGenerics.PaymentApp
{
    internal class PaymentClient
    {
        public async void Main5()
        {
            // HttpClient (shared)
            HttpClient httpClient = new HttpClient();

            // Generic API client
            ApiClient apiClient = new ApiClient(httpClient);

            // Providers
            IPaymentProvider razorpayProvider = new RazorpayPaymentProvider(apiClient);

            // Payment service
            PaymentService paymentService = new PaymentService(new[]
            {
                razorpayProvider
            });

            // --------------------
            // Call Razorpay
            // --------------------
            try
            {
                PaymentResult razorResult = await paymentService.PayAsync("Razorpay", new PaymentRequest
                {
                    Amount = 750,
                    Currency = "INR"
                });

                Console.WriteLine("Razorpay Result:");
                Console.WriteLine($"Success: {razorResult.Success}");
                Console.WriteLine($"TxnId: {razorResult.TransactionId}");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Razorpay call failed:");
                Console.WriteLine(ex.Message);
                Console.WriteLine();
            }
        }
    }
}
