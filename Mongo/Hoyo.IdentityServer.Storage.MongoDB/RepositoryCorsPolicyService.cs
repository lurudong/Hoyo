using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

namespace Hoyo.IdentityServer.Storage.MongoDB;

public class RepositoryCorsPolicyService : ICorsPolicyService
{
    private readonly string[] _allowedOrigins;

    public RepositoryCorsPolicyService(IRepository repository)
    {
        _allowedOrigins = repository.All<Client>().SelectMany(x => x.AllowedCorsOrigins).ToArray();
    }

    public Task<bool> IsOriginAllowedAsync(string origin) => Task.FromResult(_allowedOrigins.Contains(origin));
}
