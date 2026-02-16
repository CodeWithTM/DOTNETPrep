using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGenerics.PaymentApp
{
    public class PaymentService
    {
        private readonly Dictionary<string, IPaymentProvider> _providers;

        public PaymentService(IEnumerable<IPaymentProvider> providers)
        {
            _providers = providers.ToDictionary(p => p.Name, p => p);
        }

        public async Task<PaymentResult> PayAsync(string providerName, PaymentRequest request)
        {
            if (!_providers.TryGetValue(providerName, out var provider))
                throw new Exception($"Provider '{providerName}' not registered.");

            return await provider.CreatePaymentAsync(request);
        }
    }
}
