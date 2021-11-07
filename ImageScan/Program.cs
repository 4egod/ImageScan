using GitLab.Api;
using GitLab.Api.Providers;
using ImageScan;
using ImageScan.Providers;
using Refit;

Uri gitLabUri = new("https://gitlab.com/api/v4");

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, builder) =>
    {
        if (hostContext.HostingEnvironment.IsDevelopment())
        {
            builder.AddUserSecrets<Program>();
        }
    })
    .ConfigureServices((hostContext, services) =>
    {
        if (hostContext.HostingEnvironment.IsDevelopment())
        {
            var cfg = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            TokenProvider tokenProvider = new(cfg["Token"]);
            ProjectIdProvider projectIdProvider = new(long.Parse(cfg["ProjectId"]));

            services.AddSingleton<ITokenProvider>(tokenProvider);
            services.AddSingleton<IProjectIdProvider>(projectIdProvider);
        }
        else
        {
            services.AddSingleton<ITokenProvider, TokenProvider>();
            services.AddSingleton<IProjectIdProvider, ProjectIdProvider>();
        }

        services.AddTransient<TokenHandler>();

        services.AddRefitClient<IApiClient>()
            .ConfigureHttpClient(c => c.BaseAddress = gitLabUri)
            .AddHttpMessageHandler<TokenHandler>();

        services.AddHostedService<DeploymentWorker>();
    })
    .Build();

await host.RunAsync();
