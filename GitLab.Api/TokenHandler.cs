namespace GitLab.Api
{
    using Providers;

    public class TokenHandler : DelegatingHandler
    {
        private readonly ITokenProvider _tokenProvider;

        public TokenHandler(ITokenProvider tokenProvider) : base()
        {
            _tokenProvider = tokenProvider; 
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("PRIVATE-TOKEN", _tokenProvider.GetToken());

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}
