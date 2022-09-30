using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Hoyo.IdentityServer4.Storage.MongoDB;
internal class RepositoryClientStore : IClientStore
{
    protected IRepository Repository;

    public RepositoryClientStore(IRepository repository) => Repository = repository;
    public Task<Client> FindClientByIdAsync(string clientId) => Task.FromResult(Repository.Single<Client>(c => c.ClientId == clientId));
}
