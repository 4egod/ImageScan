using System.Text.Json.Serialization;

namespace GitLab.Api.Models
{
    public class Tag
    {
        public string Name { get; set; } = string.Empty;

        public string Path { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string? Revision { get; set; }

        [JsonPropertyName("short_revision")]
        public string? ShortRevision { get; set; }

        public string? Digest { get; set; }

        [JsonPropertyName("total_size")]
        public long? Size { get; set; }
    }
}
