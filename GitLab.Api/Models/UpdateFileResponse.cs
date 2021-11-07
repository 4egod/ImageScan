using System.Text.Json.Serialization;

namespace GitLab.Api.Models
{
    public class UpdateFileResponse
    {
        [JsonPropertyName("file_path")]
        public string FilePath { get; set; }

        public string Branch { get; set; }
    }
}
