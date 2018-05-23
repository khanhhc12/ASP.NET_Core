using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HelloMvc.Models
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
                string message = hre.Message;
                if (hre.InnerException != null && !string.IsNullOrEmpty(hre.InnerException.Message))
                    message += " >> " + hre.InnerException.Message;
                return message;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                    message += " >> " + ex.InnerException.Message;
                return message;
            }
        }

        public static async Task<string> PostAsync(string uri, string data, string contentType = "application/json")
        {
            try
            {
                using (var httpClient = new HttpClient())
                // deadlock ConfigureAwait(false)
                using (var response = await httpClient.PostAsync(
                    uri,
                    new StringContent(data == null ? string.Empty : data, Encoding.UTF8, contentType)
                    ).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException hre)
            {
                string message = hre.Message;
                if (hre.InnerException != null && !string.IsNullOrEmpty(hre.InnerException.Message))
                    message += " >> " + hre.InnerException.Message;
                return message;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message))
                    message += " >> " + ex.InnerException.Message;
                return message;
            }
        }
    }
}