using System.Text.Json.Serialization;

namespace GitLab.Api.Models
{
    public class UpdateFileRequest
    {
        public string Branch { get; set; }

        public string Content { get; set; }

        [JsonPropertyName("author_email")]
        public string AuthorEmail { get; set; }

        [JsonPropertyName("author_name")]
        public string AuthorName { get; set; }

        [JsonPropertyName("commit_message")]
        public string CommitMessage { get; set; }
    }
}
