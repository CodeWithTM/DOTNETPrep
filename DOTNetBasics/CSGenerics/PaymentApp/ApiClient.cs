using System.Text;
using System.Text.Json;


namespace CSGenerics.PaymentApp
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(
            string url,
            TRequest request,
            Dictionary<string, string>? headers = null)
        {
            string json = JsonSerializer.Serialize(request, _jsonOptions);

            using var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
            httpRequest.Content = new StringContent(json, Encoding.UTF8, "application/json");

            if (headers != null)
            {
                foreach (var h in headers)
                    httpRequest.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }

            using var response = await _httpClient.SendAsync(httpRequest);
            string responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"API Error: {response.StatusCode}, Body: {responseJson}");

            return JsonSerializer.Deserialize<TResponse>(responseJson, _jsonOptions)
                   ?? throw new Exception("Failed to deserialize response JSON.");
        }
    }
}





