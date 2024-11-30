using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

    class HttpService
    {
        private readonly HttpClient _httpClient;


        public HttpService()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            _httpClient = new HttpClient(clientHandler);
            _httpClient.Timeout = TimeSpan.FromSeconds(5);
        }


        // GET Request
        public async Task<string?> GetAsync(string url)
        {
            try
            {
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
        public async Task<HttpResponseMessage> PostAsync(string url, string jsonContent)
        {
            try
            {
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return response;
            }
            catch (TaskCanceledException)
            {
                // Handles timeout scenarios
                return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.RequestTimeout }; // Return a valid response
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"An Error occurred {e.Message}");
                return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.InternalServerError }; // Return a valid response
            }
        }



    }
}
