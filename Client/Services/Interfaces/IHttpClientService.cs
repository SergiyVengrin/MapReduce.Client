using Client.Models;

namespace Client.Services.Interfaces
{
    public interface IHttpClientService
    {
        Task<HttpResponseMessage> GetActiveNodesAsync();
        Task SendAsync(NodesMetadata node, FileModel file);
    }
}
