using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main()
        {
            //HttpClient client = new HttpClient(new LoggingHandler(new HttpClientHandler()));

            var client = new HttpClient();
            client.BaseAddress= new Uri("https://sandbox-api.codabox.com/v2/delivery/download/3b16ddcf-5ff4-45eb-a476-64202d2d3b58/cod");
            var request = new HttpRequestMessage();
            var byteArray = Encoding.ASCII.GetBytes("GF-4e2cee89-e8df-4a1d-b285-f7c:XyJn6NQYrm");

            request.Headers.Add("X-Software-Company", "641088c3-8fcb-47a3-8cef-de8197f5172c");
            request.Headers.Authorization=new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
           
            client.BaseAddress=new Uri("https://sandbox-api.codabox.com/v2/delivery/download/3b16ddcf-5ff4-45eb-a476-64202d2d3b58/cod");
            Client.DefaultRequestHeaders.Authorization =
               new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            Client.DefaultRequestHeaders.Add("X-Software-Company", "641088c3-8fcb-47a3-8cef-de8197f5172c");
            
         
            HttpResponseMessage response = client.SendAsync(request).Result;
            Task t = new Task(MainGet);
            t.Start();
            Console.ReadLine();
        }
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient Client = new HttpClient();

        static async void MainGet()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions
            try
            {
                var byteArray = Encoding.UTF8.GetBytes("GF-4e2cee89-e8df-4a1d-b285-f7c:XyJn6NQYrm");
                Client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
               
              
                Client.DefaultRequestHeaders.Add("X-Software-Company", "641088c3-8fcb-47a3-8cef-de8197f5172c");
                HttpResponseMessage response = await Client.GetAsync("https://sandbox-api.codabox.com/v2/delivery/download/3b16ddcf-5ff4-45eb-a476-64202d2d3b58/cod/");
                string responseBody = await response.Content.ReadAsStringAsync();

                response.EnsureSuccessStatusCode();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        private static  void HTTP_GET()
        {
            var request = WebRequest.Create("https://sandbox-api.codabox.com/v2/delivery/download/3b16ddcf-5ff4-45eb-a476-64202d2d3b58/cod");
            string authInfo = "GF-4e2cee89-e8df-4a1d-b285-f7c" + ":" + "XyJn6NQYrm";
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

            //like this:
            request.Headers["Authorization"] = "Basic " + authInfo;
            request.Headers.Add("X-Software-Company", "641088c3-8fcb-47a3-8cef-de8197f5172c");
            var response = request.GetResponse();

        }

        static void GetHttp()
        {
            WebRequest req = WebRequest.Create(@"https://sandbox-api.codabox.com/v2/delivery/download/3b16ddcf-5ff4-45eb-a476-64202d2d3b58/cod");
            req.Method = "GET";
            req.Headers.Add("X-Software-Company", "641088c3-8fcb-47a3-8cef-de8197f5172c");
            req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("GF-4e2cee89-e8df-4a1d-b285-f7c:XyJn6NQYrm"));
            //req.Credentials = new NetworkCredential("username", "password");
            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
        }
    }
}
