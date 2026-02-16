using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IContainer = Autofac.IContainer;

namespace DI.PaymentApp
{
    internal class PaymentClient
    {
        public async void Main5()
        {
            var services = new ServiceCollection();
            services.AddHttpClient();

            var builder = new ContainerBuilder();

            builder.Populate(services);

            builder.RegisterType<ApiClient>().As<IApiClient>().SingleInstance();

            builder.RegisterType<RazorpayPaymentProvider>().As<IPaymentProvider>().SingleInstance();

            builder.RegisterType<PaymentService>().AsSelf().SingleInstance();

            IContainer container = builder.Build();

            var paymentService = container.Resolve<PaymentService>();


            /*
             Alternatively this can be done using ::
            
                var host = Host.CreateDefaultBuilder().UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((_, services) =>
                {
                    services.AddHttpClient();
                }).ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterType<ApiClient>().As<IApiClient>().SingleInstance();
                    builder.RegisterType<RazorpayPaymentProvider>().As<IPaymentProvider>().SingleInstance();
                    builder.RegisterType<PaymentService>().AsSelf().SingleInstance();
                }).Build();

                var paymentServ = host.Services.GetRequiredService<PaymentService>();
             
             */

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
