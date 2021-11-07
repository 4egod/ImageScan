using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refit;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace GitLab.Api.Tests
{
    using GitLab.Api.Providers;
    using Microsoft.Extensions.Configuration;
    using Models;

    [TestClass]
    public class ApiClientTests
    {
        public const string BaseAddress = "https://gitlab.com/api/v4";

        private readonly IApiClient _client;

        private readonly long _projectId;
        private readonly long _registryId;

        public ApiClientTests()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<ApiClientTests>();

            var _cfg = builder.Build();

            var token = _cfg["Token"];
            _projectId = long.Parse(_cfg["ProjectId"]);
            _registryId = long.Parse(_cfg["RegistryId"]);

            TokenProvider tokenProvider = new(token);

            var handler = new TokenHandler(tokenProvider)
            {
                InnerHandler = new HttpClientHandler()
            };

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(BaseAddress)
            };

            _client = RestService.For<IApiClient>(httpClient);
        }

        [TestMethod]
        public async Task GetContainerTagsTest()
        {
            var tags = await _client.GetContainersTagsAsync(_projectId, _registryId);

            Debug.WriteLine($"Tag name: {tags?[0].Name}");
            Debug.WriteLine($"Container URI: {tags?[0].Location}");
        }

        [TestMethod]
        public async Task GetFileTest()
        {
            var rawContent = await _client.GetFileAsync(_projectId, "deploy.yaml", "deploy");

            Debug.WriteLine(rawContent);
        }

        [TestMethod]
        public async Task GetRegistriesTest()
        {
            var registries = await _client.GetRegistries(_projectId);

            Debug.WriteLine(registries[0].Id);
        }

        [TestMethod]
        public async Task GetTagTest()
        {
            var tag = await _client.GetTagAsync(_projectId, _registryId, "latest");

            Debug.WriteLine(tag.Path);
        }

        [TestMethod]
        public async Task UpdateFileTest()
        {
            var request = new UpdateFileRequest()
            {
                Branch = "deploy",
                CommitMessage = "update deployment",
                Content = $"{DateTime.Now}"
            };

            var rawContent = await _client.UpdateFileAsync(_projectId, "deploy.yaml", request);

            Debug.WriteLine(rawContent);
        }
    }
}