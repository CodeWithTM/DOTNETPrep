
namespace DI.PaymentApp
{
    public class RazorpayPaymentRequest
    {
        public int Amount { get; set; } // paise
        public string Currency { get; set; } = "";
        public string Receipt { get; set; } = "";
    }
    public class RazorpayPaymentResponse
    {
        public string Id { get; set; } = "";
        public string Status { get; set; } = "";
    }

    public class RazorpayPaymentProvider : IPaymentProvider
    {
        private readonly IApiClient _apiClient;

        public RazorpayPaymentProvider(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public string Name => "Razorpay";

        public async Task<PaymentResult> CreatePaymentAsync(PaymentRequest request)
        {
            var razorReq = new RazorpayPaymentRequest
            {
                Amount = (int)(request.Amount * 100), // paise
                Currency = request.Currency,
                Receipt = $"rcpt_{Guid.NewGuid():N}"
            };

            var headers = new Dictionary<string, string>
        {
            { "x-api-key", "reqres_f0d21f61eca24255ac6f31d8c780e6a9" }
        };

            RazorpayPaymentResponse razorRes =
                await _apiClient.PostAsync<RazorpayPaymentRequest, RazorpayPaymentResponse>(
                    "https://reqres.in/api/users",
                    razorReq,
                    headers);

            return new PaymentResult
            {
                Success = razorRes.Status == "created",
                TransactionId = razorRes.Id
            };
        }
    }
}
