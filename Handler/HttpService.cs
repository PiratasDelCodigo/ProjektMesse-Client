using Messe_Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Messe_Client.Handler
{

    public enum ResponseCodes
    {
        TimeOut,    // 0
        RequestException,   // 1
        March,      // 2
        April,      // 3
        May,        // 4
        June,       // 5
        July        // 6
    }

    public class PostResult<T>
    {
        public HttpResponseMessage HttpResponse { get; set; }
        public T Data { get; set; }
    }

    class HttpService
    {
        private readonly HttpClient _httpClient;


        public HttpService()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _httpClient = new HttpClient(clientHandler);
            _httpClient.Timeout = TimeSpan.FromSeconds(2);
        }


        // GET Request
        public async Task<string?> GetAsync(string url)
        {
            try
            {
                bool isReachable = await IsApiReachableAsync(url);
                if (!isReachable)
                {
                    return null;
                }
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (TaskCanceledException)
            {
                // Handles timeout scenarios
                return null;
            }
            catch (HttpRequestException e)
            {
                return null;
            }
        }

        // POST Request
        public async Task<PostResult<T>> PostAsync<T>(string url, string jsonContent)
        {
            try
            {
                // Check API reachability
                bool isReachable = await IsApiReachableAsync(url);
                if (!isReachable)
                {
                    return new PostResult<T>
                    {
                        HttpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.ServiceUnavailable)
                    };
                }

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var createdObject = JsonConvert.DeserializeObject<T>(responseContent);

                return new PostResult<T>
                {
                    HttpResponse = response,
                    Data = createdObject
                };
            }
            catch (TaskCanceledException)
            {
                return new PostResult<T>
                {
                    HttpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.RequestTimeout)
                };
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An Error occurred {e.Message}");
                return new PostResult<T>
                {
                    HttpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                };
            }
        }

        private async Task<bool> IsApiReachableAsync(string url)
        {
            try
            {
                // Use GET instead of HEAD
                HttpResponseMessage response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Is Success: {response.IsSuccessStatusCode}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reachability Check Failed: {ex.GetType().Name}");
                Console.WriteLine($"Error Message: {ex.Message}");
                return false;
            }
        }



    }
}
