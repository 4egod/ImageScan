using Refit;

namespace GitLab.Api
{
    using Models;

    public interface IApiClient
    {
        [Get("/projects/{projectId}/registry/repositories/{repositoryId}/tags")]
        public Task<IList<Tag>> GetContainersTagsAsync([AliasAs("projectId")] long projectId, [AliasAs("repositoryId")] long repositoryId);

        [Get("/projects/{projectId}/repository/files/{filePath}/raw?ref={branch}")]
        public Task<string> GetFileAsync([AliasAs("projectId")] long projectId, [AliasAs("filePath")] string filePath, [AliasAs("branch")] string branch = "main");

        [Put("/projects/{projectId}/repository/files/{filePath}")]
        public Task<string> UpdateFileAsync([AliasAs("projectId")] long projectId, [AliasAs("filePath")] string filePath, [Body] UpdateFileRequest request);

        [Get("/projects/{projectId}/registry/repositories")]
        public Task<IList<Registry>> GetRegistries([AliasAs("projectId")] long projectId);

        [Get("/projects/{projectId}/registry/repositories/{repositoryId}/tags/{tag}")]
        public Task<Tag> GetTagAsync([AliasAs("projectId")] long projectId, [AliasAs("repositoryId")] long repositoryId, [AliasAs("tag")] string tag);
    }
}
