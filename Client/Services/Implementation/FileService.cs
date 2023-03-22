using Client.Services.Interfaces;
using Client.Models;
using Newtonsoft.Json;

namespace Client.Services.Implementation
{
    public sealed class FileService : IFileService
    {
        private readonly IHttpClientService _httpService;

        public FileService(IHttpClientService httpService)
        {
            _httpService = httpService;
        }

        public async Task DivideAndSendFile()
        {
            List<NodesMetadata> activeNodes = new List<NodesMetadata>();

            var response = await _httpService.GetActiveNodesAsync();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                activeNodes = JsonConvert.DeserializeObject<List<NodesMetadata>>(json);

                string sourceFilePath = @"D:\VisualStudioProjects\MapReduce\names.csv";
                int lineCount = 0;
                using (StreamReader reader = new StreamReader(sourceFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        reader.ReadLine();
                        lineCount++;
                    }
                }
                
                int linesPerFile = 1000; 
                int fileCount = 0;
                string header = null;

                using (StreamReader reader = new StreamReader(sourceFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        if (header == null)
                        {
                            header = reader.ReadLine();
                        }

                        string text = header+"\n";
                        for (int i = 0; i < linesPerFile; i++)
                        {
                            if (reader.EndOfStream)
                            {
                                break;
                            }

                            string line = reader.ReadLine();
                            text += line +"\n";
                        }
                        fileCount++;
                        while(!activeNodes.Any(n=>n.NodeId == fileCount))
                        {
                            fileCount++;
                        }
                        

                        FileModel file = new FileModel { NodeId = fileCount, FileName = Path.GetFileName(sourceFilePath), Text = text };
                        NodesMetadata node = activeNodes.Where(n => n.NodeId == fileCount).FirstOrDefault();

                        if(node is not null && file is not null)
                        {
                            _httpService.SendAsync(node, file);
                        }


                        if (fileCount == activeNodes.Count)
                        {
                            fileCount = 0;
                        }
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
