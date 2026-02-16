using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGenerics.PaymentApp
{
    public interface IPaymentProvider
    {
        string Name { get; }

        Task<PaymentResult> CreatePaymentAsync(PaymentRequest request);
    }
}
