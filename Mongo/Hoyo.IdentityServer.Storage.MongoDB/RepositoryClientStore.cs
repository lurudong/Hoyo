using Duende.IdentityServer.Models;
using Duende.IdentityServer.Stores;

namespace Hoyo.IdentityServer.Storage.MongoDB;
internal class RepositoryClientStore : IClientStore
{
    protected IRepository Repository;

    public RepositoryClientStore(IRepository repository) => Repository = repository;
    public Task<Client> FindClientByIdAsync(string clientId) => Task.FromResult(Repository.Single<Client>(c => c.ClientId == clientId));
}
