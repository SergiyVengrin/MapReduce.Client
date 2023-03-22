namespace Client.Models
{
    public class NodesMetadata
    {
        public int NodeId { get; set; }
        public string NodeUrl { get; set; } = string.Empty;
        public string NodeFolder { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
