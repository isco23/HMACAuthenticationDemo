using ClientApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HMACClient
{
    class Program
    {
        private static HttpClient httpClient = null;
        public static HttpClient GetInstance
        {
            get
            {
                if (httpClient == null)
                    httpClient = new HttpClient();
                return httpClient;
            }
        }
        static void Main(string[] args)
        {
            RunAsync().Wait();
            Console.ReadLine();
        }


        static async Task RunAsync()
        {
            Console.WriteLine("Calling Back-End API");
            string apiAddress = "http://localhost:52459/";
            HMACDelegatingHandler customDelegatingHandler = new HMACDelegatingHandler();
            HttpClient client = HttpClientFactory.Create(customDelegatingHandler);
            var order = new Order
            {
                OrderID = 1,
                CustomerName = "Isco",
                CustomerAddress = "Spain",
                ContactNumber = "12345",
                IsShipped = true            
            };

            //HttpResponseMessage response = await client.PostAsJsonAsync(apiAddress + "api/orders", order);
            HttpResponseMessage response = await client.GetAsync(apiAddress + "api/orders");
            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                Console.WriteLine("HTTP Status: {0}, Reason {1}, Press Enter to exit", response.StatusCode, response.ReasonPhrase);
            }
            else
            {
                Console.WriteLine("Failed to call the API. HTTP Status: {0}, Reason {1}", response.StatusCode, response.ReasonPhrase);
            }
        }
    }
}
