namespace DI.PaymentApp
{
    public interface IApiClient
    {
        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request, Dictionary<string, string>? headers = null);
    }
}