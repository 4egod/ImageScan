namespace GitLab.Api.Providers
{
    public interface ITokenProvider
    {
        string? GetToken();
    }
}