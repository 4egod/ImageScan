namespace GitLab.Api.Providers
{
    public class TokenProvider : ITokenProvider
    {
        private string? _token;

        public TokenProvider()
        {
        }

        public TokenProvider(string token)
        {
            _token = token;
        }

        public string GetToken()
        {
            if (_token is not null)
            {
                return _token;
            }

            _token = Environment.GetEnvironmentVariable("TOKEN");

            if (_token is null)
            {
                throw new ArgumentNullException(nameof(_token));
            }

            return _token;
        }
    }
}
