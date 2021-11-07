using GitLab.Api;
using GitLab.Api.Models;
using ImageScan.Providers;

namespace ImageScan
{
    public class DeploymentWorker : BackgroundService
    {
        private const string BranchName = "deploy";

        private const string LatestTagName = "latest";

        private readonly ILogger<DeploymentWorker> _logger;

        private readonly IHostApplicationLifetime _applicationLifetime;

        private readonly IApiClient _client;

        private readonly IProjectIdProvider _projectIdProvider;

        public DeploymentWorker(ILogger<DeploymentWorker> logger, IHostApplicationLifetime hostApplicationLifetime, IApiClient apiClient, IProjectIdProvider projectIdProvider)
        {
            _logger = logger;
            _applicationLifetime = hostApplicationLifetime;
            _client = apiClient;
            _projectIdProvider = projectIdProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            long projectId = _projectIdProvider.GetProjectId();

            _logger.LogInformation(message: $"Using project: {projectId}");

            var registryId = (await _client.GetRegistries(projectId))[0].Id;

            _logger.LogInformation($"Found registry: {registryId}");

            var tags = await _client.GetContainersTagsAsync(projectId, registryId);

            _logger.LogInformation($"Found {tags?.Count} tags in {registryId} registry");

            var latestTag = await _client.GetTagAsync(projectId, registryId, LatestTagName);

            string latestTagName = LatestTagName;

            foreach (var item in tags)
            {
                var tag = await _client.GetTagAsync(projectId, registryId, item.Name);

                if (tag.Digest  == latestTag.Digest && tag.Name != LatestTagName)
                {
                    _logger.LogInformation($"Found last image: {item.Name}");

                    latestTagName = item.Name;

                    break;
                }
            }

            var rawContent = await _client.GetFileAsync(projectId, $"{BranchName}.yaml", BranchName);

            var oldTag = TagHelper.GetTag(rawContent);

            if (latestTagName != oldTag)
            {
                _logger.LogInformation($"Updating deployment ({oldTag} => {latestTagName}).");

                rawContent = TagHelper.ChangeTag(rawContent, latestTagName);

                var request = new UpdateFileRequest()
                {
                    Branch = BranchName,
                    CommitMessage = "Update deployment",
                    Content = rawContent,
                    AuthorName = "Deployment Bot",
                    AuthorEmail = "deployment@bot"
                };

                await _client.UpdateFileAsync(projectId, $"{BranchName}.yaml", request);

                _logger.LogInformation($"Deployment seccessfully updated ({latestTagName}).");
            }
            else _logger.LogInformation($"Same tags ({oldTag}).");

            _applicationLifetime.StopApplication();
        }
    }
}