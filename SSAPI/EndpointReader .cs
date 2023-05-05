using SSAPI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SSAPI
{
    public class EndpointReader : IEndpointReader
    {
        private readonly AppSettings _appSettings;

        public EndpointReader(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task ReadAsync()
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_appSettings.AuthUser}:{_appSettings.AuthPass}")));

            using HttpResponseMessage response = await httpClient.GetAsync(_appSettings.EndpointUrl, HttpCompletionOption.ResponseHeadersRead);

            if (response.IsSuccessStatusCode && response.Content.Headers.ContentType.MediaType.Equals("multipart/x-mixed-replace"))
            {
                using Stream stream = await response.Content.ReadAsStreamAsync();
                using StreamReader reader = new StreamReader(stream);

                while (!reader.EndOfStream)
                {
                    string line = await reader.ReadLineAsync();
                    Console.WriteLine(line);
                }
            }
            else
            {
                Console.WriteLine("Failed to connect to the endpoint.");
            }
        }
    }
}
