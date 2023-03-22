using Client.Models;
using Client.Services.Interfaces;
using System.Text.Json;
using System.Text;
using Client.DTOs;

namespace Client.Services.Implementation
{
    public sealed class HttpClientService : IHttpClientService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> GetActiveNodesAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var httpResponse = await httpClient.GetAsync($"https://localhost:7262/api/Nodes/GetActiveNodes");
                return httpResponse;
            }
            catch { throw; }
        }

        public async Task SendAsync(NodesMetadata node, FileModel file)
        {
            FileDTO fileDTO = new FileDTO
            {
                NodeFolder = node.NodeFolder+"\\"+file.FileName,
                Text = file.Text
            };

            try
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, node.NodeUrl+"SaveFile")
                {
                    Content = new StringContent(JsonSerializer.Serialize(fileDTO), Encoding.UTF8, "application/json")
                };

                var httpClient = _httpClientFactory.CreateClient();
                var httpResponse = await httpClient.SendAsync(httpRequest);
            }
            catch { throw; }
        }
}
}
