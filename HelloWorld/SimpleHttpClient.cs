using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    public static class SimpleHttpClient
    {
        // string resJson = SimpleHttpClient.GetAsync("http://localhost:5000/api/todo").Result;
        public static async Task<string> GetAsync(string uri)
        {
            try
            {
                using (var httpClient = new HttpClient())
                // deadlock ConfigureAwait(false)
                using (var response = await httpClient.GetAsync(uri).ConfigureAwait(false))
                {
                    // will throw an exception if not successful
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException hre)
            {
                return hre.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static async Task<string> PostAsync(string uri, string data)
        {
            try
            {
                using (var httpClient = new HttpClient())
                // deadlock ConfigureAwait(false)
                using (var response = await httpClient.PostAsync(
                    uri,
                    new StringContent(data == null ? string.Empty : data, Encoding.UTF8, "application/json")
                    ).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException hre)
            {
                return hre.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
